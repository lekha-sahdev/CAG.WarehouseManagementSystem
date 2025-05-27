# CAG Warehouse Management System

## Overview

The CAG Warehouse Management System is a RESTful API built using ASP.NET Core. 
It manages customer records, purchase orders and sales orders, providing CRUD operations for all entities. 
The system is designed to be scalable and maintainable, following best practices in software architecture.

## Architecture

The project follows a microservices architecture pattern with RestAPI structure and Repository pattern for Data access.
- Microservices pattern RestAPIs 
- Repository Pattern for Data Access 
- Configurable Data Source - Can switch between InMemory/Actual DB via DatabaseType in app.settings.json
- Dependency Injection via custom AutoFac setup. No need to register every class - just inherit them from ITransient interface.
- 
- Unit testable components.

## Project Structure
The folder structure includes:
- **Entities**: Represent the data model for the application.
- **Repositories**: Handle data access and encapsulate the logic required to access data sources.
- **Services**: Contain business logic and interact with repositories.
- **Controllers**: Handle HTTP requests and responses, interacting with services to perform operations.

## Running the Project Locally

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup Instructions

1. **Clone the Repository**:
bash
git clone https://github.com/yourusername/CAG.WarehouseManagementSystem.git
cd CAG.WarehouseManagementSystem

2. **Configure the Database**:
   - Update the connection string in `appsettings.json` with your SQL Server credentials.
3. **Run the Application**:
bash
   dotnet run
4. **Access the API**:
   - The API will be available at `http://localhost:5004/api`.
   - The Swagger UI is at http://localhost:5044/swagger/index.html

### Running Tests

1. **Navigate to the Test Project**:
'''bash
cd CAG.WarehouseManagementSystem.Tests'''

2. **Run Tests**:
'''bash
dotnet test'''
