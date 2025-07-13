#Doctorian Clinic
🩺 Clinic Management API Project — ASP.NET Core Web API
This project is a fully functional Clinic Management RESTful API developed using ASP.NET Core, Entity Framework Core, and Microsoft Identity. It is designed to serve as the backend for a clinic system that manages appointments, doctors, patients, and user roles with secure authentication and authorization.

🔧 Key Features:
✅ User Authentication & Authorization
Secure registration and login using ASP.NET Identity, with support for user roles such as Admin, Doctor, and Patient.

🗓️ Appointment Scheduling
Patients can book appointments with doctors. Admins and doctors can manage appointment status and update diagnoses.

👩‍⚕️ Doctor & Patient Management
Full CRUD operations for doctor and patient profiles, including medical details and contact info.

💳 Stripe Payment Integration
Integrated with Stripe for handling secure payments related to appointments and medical services.

🔐 Cookie-based Authentication
Supports cookie-based sessions for secure frontend/backend communication.

🌐 CORS Configuration
Properly configured CORS to allow communication with Angular frontend (development & production modes).

🧱 Database Seeding
Automatically seeds the database with essential roles, default admin user, and basic data on application start.

📑 Swagger/OpenAPI Support
Interactive Swagger UI is provided for testing endpoints and API documentation.

🧪 Tech Stack:
ASP.NET Core Web API

Entity Framework Core (Code-First)

SQL Server

Microsoft Identity

Stripe .NET SDK

Swagger / Swashbuckle

Angular (as frontend client)

Somee.com (for deployment)
