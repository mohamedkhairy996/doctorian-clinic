using clinic.Domain.models;
using clinic.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace clinic.PresentationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public StripeController(IUnitOfWork unitOfWork)
        {
            StripeConfiguration.ApiKey = "sk_test_51RhuyvHGAZFtsgHbF7alWjDztAtQlamNbXBbyofc7uOn4aMR8if22ADVhl3VGHCFY48hS4cDoLwc86srEy44O4fz003IRDg0GB";
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { success = true, data = "done" });
        }
        [HttpPost]
        public IActionResult PostPayment(Payment request)
        {
            var options = new ChargeCreateOptions
            {
                Amount = request.Amount, // in cents
                Currency = "usd",
                Description = "Clinic Payment",
                Source = request.Token
            };

            var service = new ChargeService();
            try
            {
                Charge charge = service.Create(options);
                var appointment = _unitOfWork.Appointment.GetBy(app=>app.Id==request.AppointmentID);
                if (appointment != null)
                {
                    appointment.Status = "Approved";
                    appointment.PaymentId = charge.Id;
                    _unitOfWork.Appointment.Update(appointment);
                    _unitOfWork.Complete();
                    return Ok(new { success = true, chargeId = charge.Id });
                }
                else
                {
                    return BadRequest("Appointment id isnot correct");
                }
            }
            catch (StripeException e)
            {
                return BadRequest(new { success = false, error = e.Message });
            }
        }
    }
}
