# UsersApi

A secure RESTful API for user management built with ASP.NET Core 8.0, featuring JWT authentication and PostgreSQL database integration.

## Features

- User authentication and authorization using JWT tokens
- User management (CRUD operations)
- PostgreSQL database integration
- Swagger UI for API documentation
- Secure password policies
- Entity Framework Core for data access

## Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL database
- Visual Studio 2022 or later / VS Code with C# extensions

## Project Structure

```
UsersApi/
├── Controllers/     # API endpoints
├── Data/           # Database context and configurations
├── Models/         # Data models
├── Migrations/     # Database migrations
├── Properties/     # Launch settings
└── Program.cs      # Application entry point and configuration
```

## Getting Started

1. Clone the repository
2. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=your_host;Database=your_database;Username=your_username;Password=your_password"
   }
   ```
3. Configure JWT settings in `appsettings.json`:
   ```json
   "Jwt": {
     "Key": "your_secret_key",
     "Issuer": "your_issuer",
     "Audience": "your_audience"
   }
   ```
4. Run the following commands in the terminal:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

## API Documentation

Once the application is running, you can access the Swagger UI documentation at:
```
https://localhost:5001/swagger
```

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. To access protected endpoints:

1. Register a new user
2. Login to receive a JWT token
3. Include the token in the Authorization header:
   ```
   Authorization: Bearer your_jwt_token
   ```

## Password Requirements

- Minimum length: 6 characters
- No special requirements for:
  - Uppercase letters
  - Lowercase letters
  - Numbers
  - Special characters

## Dependencies

- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.8)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.8)
- Microsoft.EntityFrameworkCore (9.0.3)
- Npgsql.EntityFrameworkCore.PostgreSQL (9.0.4)
- Swashbuckle.AspNetCore (6.4.0)

## Development

To add new migrations after model changes:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

## Security Considerations

- JWT tokens are used for stateless authentication
- Passwords are hashed using ASP.NET Core Identity
- HTTPS is enabled by default
- CORS policies should be configured based on your requirements

## License

This project is licensed under the MIT License - see the LICENSE file for details. 