# UsersApi

A secure RESTful API for user management built with ASP.NET Core 8.0, featuring JWT authentication and PostgreSQL database integration.

## Features

- User authentication and authorization using JWT tokens
- User management (CRUD operations)
- PostgreSQL database integration
- Swagger UI for API documentation
- Secure password policies
- Entity Framework Core for data access
- Docker support for easy deployment
- Production-ready configuration with SSL support
- Azure deployment ready

## Prerequisites

- .NET 8.0 SDK or later (for local development)
- PostgreSQL database (optional if using Docker)
- Visual Studio 2022 or later / VS Code with C# extensions
- Docker and Docker Compose (for containerized deployment)
- Azure account (for cloud deployment)

## Project Structure

```
UsersApi/
├── Controllers/     # API endpoints
├── Data/           # Database context and configurations
├── Models/         # Data models
├── Migrations/     # Database migrations
├── Properties/     # Launch settings
├── Dockerfile      # Container configuration
├── docker-compose.yml # Container orchestration
├── azure-pipelines.yml # Azure CI/CD configuration
├── .env           # Environment variables (create this file)
└── Program.cs      # Application entry point and configuration
```

## Getting Started

### Option 1: Local Development

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

### Option 2: Docker Deployment

#### Local Development with Docker

1. Clone the repository
2. Create a `.env` file with your configuration:
   ```
   DB_PASSWORD=your_strong_password
   JWT_KEY=your_super_secret_key_here
   JWT_ISSUER=your_issuer
   JWT_AUDIENCE=your_audience
   ```
3. Run the following command to start the application:
   ```bash
   docker-compose up -d
   ```

The application will be available at:
- API: http://localhost:80
- Swagger UI: http://localhost:80/swagger
- PostgreSQL: localhost:5432

### Option 3: Azure Deployment

1. **Prepare Azure Resources**:
   - Create an Azure account if you don't have one
   - Install Azure CLI
   - Login to Azure:
     ```bash
     az login
     ```

2. **Create Azure Resources**:
   ```bash
   # Create a resource group
   az group create --name usersapi-rg --location eastus

   # Create Azure Container Registry
   az acr create --resource-group usersapi-rg --name usersapiregistry --sku Basic --admin-enabled true

   # Create Azure Database for PostgreSQL
   az postgres flexible-server create \
     --resource-group usersapi-rg \
     --name usersapi-db \
     --admin-user postgres \
     --admin-password <your-secure-password> \
     --sku-name Standard_B1ms

   # Create Azure App Service Plan
   az appservice plan create \
     --name usersapi-plan \
     --resource-group usersapi-rg \
     --sku B1 \
     --is-linux

   # Create Azure App Service
   az webapp create \
     --resource-group usersapi-rg \
     --plan usersapi-plan \
     --name usersapi-app \
     --deployment-container-image-name usersapiregistry.azurecr.io/usersapi:latest
   ```

3. **Configure Environment Variables**:
   ```bash
   # Get the database connection string
   DB_CONNECTION=$(az postgres flexible-server show-connection-string \
     --server-name usersapi-db \
     --database-name usersdb \
     --query connectionStrings.jdbc \
     --output tsv)

   # Set application settings
   az webapp config appsettings set \
     --resource-group usersapi-rg \
     --name usersapi-app \
     --settings \
     ASPNETCORE_ENVIRONMENT=Production \
     ConnectionStrings__DefaultConnection="$DB_CONNECTION" \
     Jwt__Key="your-secure-jwt-key" \
     Jwt__Issuer="https://usersapi-app.azurewebsites.net" \
     Jwt__Audience="https://usersapi-app.azurewebsites.net"
   ```

4. **Deploy the Application**:
   ```bash
   # Build and push the Docker image
   az acr build --registry usersapiregistry --image usersapi:latest .

   # Restart the web app to pull the new image
   az webapp restart --name usersapi-app --resource-group usersapi-rg
   ```

5. **Enable Swagger in Production**:
   ```bash
   az webapp config appsettings set \
     --resource-group usersapi-rg \
     --name usersapi-app \
     --settings ASPNETCORE_ENVIRONMENT=Development
   ```

Your application will be available at:
- API: https://usersapi-app.azurewebsites.net
- Swagger UI: https://usersapi-app.azurewebsites.net/swagger

#### Azure Commands

To view logs:
```bash
az webapp log tail --name usersapi-app --resource-group usersapi-rg
```

To restart the application:
```bash
az webapp restart --name usersapi-app --resource-group usersapi-rg
```

To update the application:
```bash
az acr build --registry usersapiregistry --image usersapi:latest .
az webapp restart --name usersapi-app --resource-group usersapi-rg
```

## API Documentation

Once the application is running, you can access the Swagger UI documentation at:
```
https://usersapi-app.azurewebsites.net/swagger
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
- HTTPS is enabled by default in production
- CORS policies should be configured based on your requirements
- When using Docker, ensure to use strong passwords and secure environment variables
- Keep your `.env` file secure and never commit it to version control
- Regularly update dependencies for security patches
- Use proper firewall rules to restrict access to necessary ports only
- Azure Key Vault can be used to store sensitive configuration in production

## License

This project is licensed under the MIT License - see the LICENSE file for details. 