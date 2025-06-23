# WiSave Portal

A .NET 8 API Gateway using YARP (Yet Another Reverse Proxy) for microservices routing, authentication, and authorization.

## Features

- **JWT Authentication** - Login, registration, token refresh, and logout
- **Role-based Authorization** - Three-tier system (User, Moderator, Admin) with policy-based access control
- **User Management** - Full CRUD operations with role assignment and account activation/deactivation
- **YARP Reverse Proxy** - Routes requests to downstream microservices with authorization policies
- **Health Checks** - Service monitoring with database connectivity checks
- **Data Protection** - Built-in key management and encryption for sensitive data
- **Identity Framework** - ASP.NET Core Identity with customizable password policies
- **Swagger/OpenAPI** - Interactive API documentation with JWT authentication
- **PostgreSQL** - Database integration with Entity Framework Core
- **Docker** - Containerized deployment with HTTPS support



## Quick Start

**Using Docker Compose:**
```bash
# Clone and start
git clone <repository-url>
cd WiSave.Portal
docker-compose up -d
```

**Access:**
- API: `https://localhost:5003` (HTTPS) or `http://localhost:5002` (HTTP)
- Swagger UI: `https://localhost:5003/swagger`
- Database: `localhost:5433`

## Configuration

**Environment Variables:**
```bash
# Required
POSTGRES_USER=wisave
POSTGRES_PASSWORD=your-password
ASPNETCORE_Kestrel__Certificates__Default__Password=cert-password

# JWT Configuration (in appsettings.json)
"Jwt": {
  "Key": "your-super-secret-jwt-key-that-is-at-least-32-characters-long",
  "Issuer": "WiSave.Portal",
  "Audience": "WiSave.Portal.Users"
}

# Optional
ASPNETCORE_ENVIRONMENT=Development
```

**Password Policy:**
- Minimum 6 characters
- Requires uppercase, lowercase, and digit
- No special characters required
- Unique email addresses enforced

## API Usage

### Authentication
```bash
# Register
POST /api/auth/register
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

# Login
POST /api/auth/login
{
  "email": "user@example.com", 
  "password": "SecurePassword123!"
}

# Use token in headers
Authorization: Bearer <your-jwt-token>
```

### Available Endpoints

**Authentication** (`/api/auth/`)
- `POST /register` - Register new user
- `POST /login` - User login
- `POST /refresh` - Refresh JWT token
- `POST /change-password` - Change password (requires auth)
- `GET /me` - Get current user info (requires auth)
- `POST /logout` - Logout and invalidate tokens (requires auth)

**User Management** (`/api/users/`) - Admin/Moderator only
- `GET /` - Get all users (Admin only)
- `GET /{id}` - Get user by ID (Moderator+)
- `POST /{id}/assign-role` - Assign role to user (Admin only)
- `DELETE /{id}/remove-role` - Remove role from user (Admin only)
- `PATCH /{id}/deactivate` - Deactivate user (Admin only)
- `PATCH /{id}/activate` - Activate user (Admin only)

**Health Checks**
- `GET /health` - Basic health status
- `GET /health/ready` - Readiness probe with database check

**Proxied Routes**
- `/api/subscriptions/*` → Routes to Subscriptions microservice

## Development

### Database Migrations
```bash
# Add migration
dotnet ef migrations add MigrationName --project WiSave.Core

# Update database  
dotnet ef database update --project WiSave.Core
```

### Adding Microservice Routes
1. **Configure route in `appsettings.json`:**
```json
"ReverseProxy": {
  "Routes": {
    "my-service-route": {
      "ClusterId": "my-service-cluster",
      "AuthorizationPolicy": "UserOrAbove",
      "Match": {
        "Path": "/api/my-service/{**catch-all}"
      }
    }
  },
  "Clusters": {
    "my-service-cluster": {
      "Destinations": {
        "destination1": {
          "Address": "https://my-service:5001/"
        }
      }
    }
  }
}
```

2. **Add service to Docker Compose networks**

## Technology Stack

- **.NET 8** with minimal APIs and dependency injection
- **ASP.NET Core Identity** for user management and authentication
- **JWT Bearer** authentication with configurable token validation
- **YARP** for high-performance reverse proxy
- **Entity Framework Core** + PostgreSQL with Npgsql provider
- **Data Protection API** for encryption and key management
- **Authorization Policies** - AdminOnly, ModeratorOrAdmin, UserOrAbove
- **Swagger/OpenAPI** documentation with security definitions
- **Docker** with HTTPS certificates and multi-service orchestration