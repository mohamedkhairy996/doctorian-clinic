
using clinic.Domain.Repositories;
using clinic.Infrastructure;
using clinic.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using clinic.Infrastructure.DatabaseInitializer;
using clinic.Domain.models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace clinic.PresentationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<applicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<applicationContext>().AddDefaultTokenProviders();



            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
            });
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<applicationContext>();

            ////////////////////*********************************************////////////////////
            ///for the cookies

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Identity.Application";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.LoginPath = "/login";
            });


            ///
            /////////////////////*********************************************//////////////////

            // Register services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            //builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            //builder.Services.AddScoped<IPatientRepository, PatientRepository>();

            // Identity setup (if using Identity)
            //builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<applicationContext>();
            builder.Services.AddOpenApi("internal"); // Document name is internal

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAngular", policy =>
            //    {
            //        policy.WithOrigins("http://localhost:4200")
            //        .AllowCredentials() //for cookies
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //    });
            //});

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", policy =>
            //    {
            //        policy.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //    });
            //});
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowConfiguredOrigins", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });




            var app = builder.Build();
            seedDb();

            app.UseCors("AllowConfiguredOrigins");
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Help");
                c.RoutePrefix = "help"; // Optional: loads Swagger UI at root
            });
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }


            app.MapGet("/", () => "Welcome TO Doctorian Clinic");
            

            //app.UseCors("AllowAll");


            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                await next();
            });


            app.UseHttpsRedirection();


            app.UseAuthentication(); 

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            void seedDb()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var dbInitializer = services.GetRequiredService<IDbInitializer>();
                        dbInitializer.Initialize();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }
        }
    }
}
