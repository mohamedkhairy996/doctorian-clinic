
using clinic.Application.DTOs;
using clinic.Application.SD;
using clinic.Domain.models;
using clinic.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace clinic.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(
            //IUserService userService,
            SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            //_userService = userService;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: api/users
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var users = await _userService.GetAllUsersExceptAsync(currentUserId);
            var users = await _signInManager.UserManager.Users
                .Where(u => u.Id != currentUserId)
                .ToListAsync();
            return Ok(new { success = true, data = users });
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ApplicationUserDTO user )
        {
            if (user == null)
                return BadRequest("User data is null.");
            if (string.IsNullOrEmpty(user.username) || string.IsNullOrEmpty(user.password))
                return BadRequest("Username and password are required.");
            var existingUser =  _unitOfWork.ApplicationUser.GetBy(u => u.UserName == user.username);
                if (existingUser != null)
                return BadRequest("Username already exists.");
            var existingEmail = _unitOfWork.ApplicationUser.GetBy(u => u.Email == user.email);
            if (existingEmail != null)
                return BadRequest("Email already exists.");
            var newUser = new ApplicationUser()
            {
                UserName = user.username,
                Email = user.email,
                PhoneNumber = user.phone,
                Name = user.name,
                Address = user.address,
                City = user.City,
                DOB = user.DOB,
                Age = (int)(DateTime.Now.Year - (user.DOB?.Year ?? 0)), // حساب العمر بناءً على تاريخ الميلاد
                ContactInfo = user.email + " , " + user.phone, // دمج الإيميل ورقم الهاتف في حقل واحد
                About = user.About,
                
                
            };
            
            
            // 👇 إضافة المستخدم + كلمة المرور
            var result = await _userManager.CreateAsync(newUser, user.password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (user.Role == Roles.PatientRole)
            {
                var patient = new Patient()
                {
                    Name = user.name,
                    Email = user.email,
                    PhoneNumber = user.phone,
                    Address = user.address,
                    DOB = user.DOB,
                    Age = (short)(DateTime.Now.Year - (user.DOB?.Year ?? 0)),
                    ApplicationUserId = newUser.Id
                };
                _unitOfWork.Patient.Add(patient);
            } else if (user.Role == Roles.DoctorRole)
            {
                var doctor = new Doctor()
                {
                    Name = user.name,
                    ContactInfo = user.email + " , " + user.phone,
                    WorkingHours = "9:00 AM - 5:00 PM", // مثال على ساعات العمل
                    Specialization = user.Specialization,
                    ApplicationUserId = newUser.Id
                };
                _unitOfWork.Doctor.Add(doctor);
            }

            // ممكن تضيفه لأدوار:
            await _userManager.AddToRoleAsync(newUser,user.Role);
            _unitOfWork.Complete();
            return Ok(new { success = true, data = newUser });
        }

        // POST: api/users/lock-unlock/{id}
        [HttpPut("lock-unlock/{id}")]
        public async Task<IActionResult> LockUnlockUser(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetBy(u => u.Id == id);
            if (user == null)
                return NotFound("User not found.");
            if (user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow - DateTimeOffset.UtcNow.Subtract(DateTime.Now);

            } else
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            }

            _unitOfWork.Complete();
            return Ok();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetBy(u => u.Id == id);
            if (user == null)
                return NotFound("User not found.");
            _unitOfWork.ApplicationUser.Delete(user);
            _unitOfWork.Complete();
            return Ok();
        }


        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(signinDTO user)
        {
            var existUser = _unitOfWork.ApplicationUser.GetBy(u => u.UserName == user.UserName)
                            ?? _unitOfWork.ApplicationUser.GetBy(u => u.Email == user.Email); // دعم تسجيل الدخول بالاسم أو الإيميل

            if (existUser == null)
            {
                return NotFound("User not found.");
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, user.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid login attempt.");
            }
            return Ok();

        }





        [HttpGet("issignedin/{id}")]
        public IActionResult IsUserSignedIn(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetBy(u => u.Id == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var isSignedIn = _signInManager.IsSignedIn(User);
            if (!isSignedIn)
            {
                return Unauthorized("User is not signed in.");
            }
            if (user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return BadRequest("User is locked out.");
            }
            // يمكن إضافة المزيد من التحقق هنا إذا لزم الأمر

            // إذا كان كل شيء على ما يرام، يمكن إرجاع بيانات المستخدم أو رسالة نجاح
            return Ok(new { success = true, data = user });

        }

        [Authorize]
        [HttpGet("getSignedInUser")]
        public async Task<IActionResult> getUserID()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var role = await _userManager.GetRolesAsync(user);
            
            if (role.Contains("Doctor"))
            {
                var userRole = _unitOfWork.Doctor.GetBy(d => d.ApplicationUserId == user.Id);
                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    name = user.Name,
                    doctorId = userRole.Id,
                });
            }else if (role.Contains("Patient"))
            {
                var userRole = _unitOfWork.Patient.GetBy(d => d.ApplicationUserId == user.Id);
                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    name = user.Name,
                    patientId = userRole.Id,
                });
            }
            else
            {
                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    name = user.Name,
                });
            }
        }


        [HttpPost("signout")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _signInManager.SignOutAsync();

            return Ok(new { success = true, data = "signed out successfully" });
        }




    }
}

