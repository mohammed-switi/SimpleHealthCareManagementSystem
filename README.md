
# Simple Health Care Management System (ASP .NET)

A simple, yet powerful healthcare management system built using .NET Core, Entity Framework Core, and JWT-based authentication. This project aims to provide a streamlined and efficient solution for managing various aspects of a healthcare system.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
  - [API Endpoints](#api-endpoints)
  - [Authentication](#authentication)
  - [Roles and Authorization](#roles-and-authorization)
- [Logging](#logging)
- [License](#license)
- [Contact](#contact)

## Features

- User registration and authentication with JWT
- Role-based authorization (Admin and User roles)
- Manage patients, doctors, visits, and tests
- Logging with Serilog to console and database
- AutoMapper for object-object mapping
- Comprehensive error handling and logging

## Technologies

- [.NET Core](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [JWT (JSON Web Tokens)](https://jwt.io/)
- [AutoMapper](https://automapper.org/)
- [Serilog](https://serilog.net/)
- [Swagger](https://swagger.io/)

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [MySQL](https://www.mysql.com/) (or any other compatible database)
- [Git](https://git-scm.com/)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/USERNAME/SimpleHealthCareManagementSystem.git
   cd SimpleHealthCareManagementSystem
   ```

2. Set up the database:
   - Create a new MySQL database.
   - Update the connection string in `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "MySqlConnectionString": "Server=your_server;Database=your_database;User=your_user;Password=your_password;"
     }
     ```

3. Apply migrations:
   ```bash
   dotnet ef database update
   ```

### Configuration

Configure the `appsettings.Development.json` file with the necessary settings:

1. **Logging**: Set the appropriate log levels for your application.
2. **JWT Settings**: Configure the key, issuer, and audience for JWT authentication.
3. **WhatsApp Settings**: Add the access token for WhatsApp integration.
4. **Admin User**: Define the admin user's email and password.

Here is an example structure for the `appsettings.Development.json` file:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtSettings": {
    "Key": "your_jwt_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  },
  "WhatsAppSettings": {
    "AccessToken": "your_whatsapp_access_token"
  },
  "AdminUser": {
    "Email": "your_admin_email",
    "Password": "your_admin_password"
  }
}
```


### Running the Application

1. Build and run the application:
   ```bash
   dotnet build
   dotnet run
   ```

2. Open your browser and navigate to `https://localhost:5001/swagger` to access the Swagger UI.


## Usage

### API Endpoints

The API endpoints are documented using Swagger. You can access the API documentation at `https://localhost:5001/swagger`.

### Authentication

- Register a new user:
  - `POST /api/Account/register`

- Login to get a JWT token:
  - `POST /api/Account/login`

### Roles and Authorization

The system supports role-based authorization for Admin and User roles.

- Admin-only endpoints:
  - Accessible only to users with the Admin role.
  - Example: `GET /api/Admin`

- User-only endpoints:
  - Accessible only to users with the User role.
  - Example: `GET /api/User`

To assign roles to users, use the RoleService during user registration or via admin endpoints.

## Logging

The application uses Serilog for logging. Logs are written to both the console and the database.

### Database Logging

Logs are stored in the `LogEntries` table. The logging configuration can be found in `Program.cs`.


## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For any inquiries or support, please contact:

- **Your Name**
- **Email:** mosowity@gmail.com
- **GitHub:** [USERNAME](https://github.com/mohammed-switi)



### Summary

This `README.md` provides a comprehensive overview of your project, including features, technologies, setup instructions, usage guidelines, logging information, contribution guidelines, and contact details. Adjust the placeholders (`USERNAME`, `your_server`, `your_database`, `your_user`, `your_password`, etc.) with your actual information. This will make it easier for users and contributors to understand and work with your project.
