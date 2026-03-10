# Avrang Project - E-Commerce Management System

## Overview
Avrang is a complete e-commerce management platform built with ASP.NET Core, Blazor WebAssembly, PostgreSQL, and Bootstrap.

## Project Structure

### Backend (webApi)
- **Port**: 5094
- **Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL
- **Features**:
  - JWT Authentication
  - RESTful API endpoints
  - Swagger/OpenAPI documentation
  - Product, Category, Brand management
  - Order and installment system
  - Email notifications

### Admin Panel (Admin)
- **Port**: 5001
- **Framework**: Blazor WebAssembly
- **Features**:
  - User authentication
  - Dashboard with statistics
  - Product management (CRUD)
  - Category management
  - Brand management
  - Installment plans management
  - Responsive design with Bootstrap

### Architecture Layers
- **API Layer**: webApi (Controllers, DTOs)
- **Application Layer**: Application (Services, Filters)
- **Infrastructure Layer**: Infrastructure (Repositories, Data)
- **Domain Layer**: Domain (Models, Entities)

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- PostgreSQL 12+
- Node.js (optional, for frontend tools)

### Setup Instructions

1. **Clone/Setup Database**
   ```bash
   # Database connection string is configured in appsettings.json
   # Default: Host=localhost;Port=5432;Database=ProjectAvrang;Username=postgres;Password=1234;
   ```

2. **Run Database Migrations**
   ```bash
   cd Infrastructure
   dotnet ef database update --startup-project ../webApi
   ```

3. **Start Backend API**
   ```bash
   cd webApi
   dotnet run
   ```
   API will be available at: `http://localhost:5094`

4. **Start Admin Panel**
   ```bash
   cd Admin
   dotnet run
   ```
   Admin panel will be available at: `http://localhost:5001`

## Default Credentials

### Admin Account
- **Email**: admin@gmail.com
- **Password**: Admin123

### Available Endpoints

#### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token
- `GET /api/auth/me` - Get current user info

#### Products
- `GET /api/products` - List all products (with pagination)
- `GET /api/products/{id}` - Get product details
- `POST /api/products` - Create new product (Admin)
- `PUT /api/products/{id}` - Update product (Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

#### Categories
- `GET /api/categories` - List categories
- `POST /api/categories` - Create category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

#### Brands
- `GET /api/brands` - List brands
- `POST /api/brands` - Create brand
- `PUT /api/brands/{id}` - Update brand
- `DELETE /api/brands/{id}` - Delete brand

#### Installments
- `GET /api/installments` - List installment plans
- `POST /api/installments` - Create installment plan
- `DELETE /api/installments/{id}` - Delete installment

## Testing

### Via Swagger UI
Visit `http://localhost:5094/swagger/index.html` to test API endpoints interactively.

### Admin Panel
1. Navigate to `http://localhost:5001`
2. Default redirect to login page
3. Enter admin credentials: admin@gmail.com / Admin123
4. Access dashboard with management options

### Manual API Testing (PowerShell)
```powershell
# Get products
$response = Invoke-WebRequest -Uri "http://localhost:5094/api/products?PageSize=10"
$response.Content | ConvertFrom-Json | Select-Object Items | Format-Table

# Login
$body = @{email = "admin@gmail.com"; password = "Admin123"} | ConvertTo-Json
$response = Invoke-WebRequest -Uri "http://localhost:5094/api/auth/login" -Method Post -ContentType "application/json" -Body $body
$response.Content | ConvertFrom-Json
```

## Features Implemented

### Admin Panel
- ✅ Dashboard with statistics
- ✅ Product management (Create, Read, Update, Delete)
- ✅ Category management
- ✅ Brand management
- ✅ Installment plans management
- ✅ User authentication & logout
- ✅ Responsive navbar with navigation
- ✅ Modal dialogs for CRUD operations
- ✅ Form validation
- ✅ UI/UX improvements with hovers and styling

### API Features
- ✅ JWT Authentication
- ✅ Role-based authorization
- ✅ Pagination support
- ✅ Filtering support
- ✅ CORS enabled
- ✅ Email notifications
- ✅ Database seeding
- ✅ Error handling
- ✅ Logging

## Seed Data

The application includes seed data for easy testing:
- **5+ Products**: iPhones, Samsung devices, MacBooks, etc.
- **5 Categories**: Smartphones, Laptops, Audio, Appliances, Wearables
- **5 Brands**: Apple, Samsung, Sony, Dell, Bose
- **4 Installment Plans**: 3, 6, 12, and 24 months

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ProjectAvrang;Username=postgres;Password=1234;"
  },
  "Jwt": {
    "Issuer": "MyApi",
    "Audience": "MyApiClient",
    "Key": "SUPER_LONG_SECRET_KEY_AT_LEAST_32_CHARS"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

## Troubleshooting

### Database Connection Issues
- Ensure PostgreSQL is running
- Check connection string in appsettings.json
- Run migrations: `dotnet ef database update`

### Admin Panel Not Loading
- Clear browser cache
- Check if port 5001 is available
- Ensure webApi is running on port 5094

### API Not Responding
- Check if port 5094 is available
- Verify PostgreSQL connection
- Check logs in console output

## Future Enhancements
- Customer/Frontend website
- Order management system
- Payment gateway integration
- Advanced filtering and search
- Analytics dashboard
- Mobile app support
- Notification system
- Review and rating system

## Technologies Used
- **Backend**: ASP.NET Core 9.0, Entity Framework Core, PostgreSQL
- **Frontend**: Blazor WebAssembly, Bootstrap 5
- **Authentication**: JWT (JSON Web Tokens)
- **Database**: PostgreSQL with migrations
- **Tools**: Visual Studio Code, DotNet CLI

## License
This project is for educational purposes.

## Support
For issues or questions, please check the logs and ensure all prerequisites are properly installed.
