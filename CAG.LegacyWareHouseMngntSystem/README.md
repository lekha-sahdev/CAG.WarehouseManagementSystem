# CAG.LegacyWareHouseMngntSystem

## Overview
CAG.LegacyWareHouseMngntSystem is a .NET 9-based application designed for automated polling, parsing, and processing of warehouse data files. It supports both local and SFTP file sources, processes files using configurable parsers, and dispatches data to external APIs.

## Architecture
- **.NET 9**: Modern, high-performance runtime.
- **Dependency Injection**: Managed via Autofac.
- **Quartz.NET**: For scheduled background jobs (e.g., file polling).
- **Polly**: For resilience and retry policies.
- **Modular Services**: Includes file parsers, dispatchers, and job services.
- **Logging**: Uses Microsoft.Extensions.Logging.

## Project Structure
CAG.LegacyWareHouseMngntSystem/ 
├── Jobs/                # Quartz job classes (e.g., FilePollingJob) 
├── Services/              # Business logic, file parsers, dispatchers 
├── Dtos/                  # Data transfer objects 
├── appsettings.json       # Configuration file 
├── Program.cs             # Application entry point 
├── Dockerfile             # Containerization instructions 
└── CAG.LegacyWareHouseMngntSystem.csproj

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- (Optional) Docker for containerized deployment
- (Optional) SFTP server for SFTP mode

## Setup Instructions
1. **Clone the repository**  
   `git clone <your-repo-url>`

2. **Configure settings**  
   Edit `appsettings.json` for API endpoints, polling mode, SFTP credentials, and local paths.

3. **Restore dependencies**  
   Navigate to the project directory and run:
   ```bash
   dotnet restore  CAG.LegacyWareHouseMngntSystem/CAG.LegacyWareHouseMngntSystem.csproj
   ```
4. **Build the project**   
 Run the following command:
   ```bash
   dotnet build CAG.LegacyWareHouseMngntSystem/CAG.LegacyWareHouseMngntSystem.csproj
   ```

5. **Run the application**  
1. For local execution, use:
   ```bash
   dotnet run --project CAG.LegacyWareHouseMngntSystem/CAG.LegacyWareHouseMngntSystem.csproj
   ```
## Running Tests
1. **Navigate to the test project directory**  
1. Run the tests using:
   ```bash
   dotnet test CAG.LegacyWareHouseMngntSystem.Tests/CAG.LegacyWareHouseMngntSystem.Tests.csproj
   ```
   