# Functional Background
CAG-WareHouseManagement System, is designed to enable 
•	Real-time ingestion via RESTful APIs for Products, Purchase Orders (POs) & Sales Orders (SOs).
•	Scheduled polling of legacy data files and ingesting data for Products, Purchase Orders (POs) & Sales Orders (SOs).

# Flow Diagram
![image](https://github.com/user-attachments/assets/c8d976fb-8c02-479d-8d00-5c86dcf0eb19)

# ReadMe per service
Refer ReadMe files in each project to understand how to run & use the service

https://github.com/lekha-sahdev/CAG.WarehouseManagementSystem/tree/main/CAG.WarehouseManagementSystem - https://github.com/lekha-sahdev/CAG.WarehouseManagementSystem/blob/main/CAG.LegacyWareHouseMngntSystem/README.md
https://github.com/lekha-sahdev/CAG.WarehouseManagementSystem/tree/main/CAG.LegacyWareHouseMngntSystem - https://github.com/lekha-sahdev/CAG.WarehouseManagementSystem/blob/main/CAG.WarehouseManagementSystem/README.md

# Technical Patterns / Designs Implemented

## CAG.WareHouseManagement Service

### REST API Design Pattern
Microservices design implemented with **REST Controller pattern** and **Repository pattern**.

### Features and Design Strategies

| Feature                          | Design Strategy |
|----------------------------------|------------------|
| **Microservice Design Pattern**  | 1. Created two microservices:<br> &nbsp;&nbsp;&nbsp;a) `CAG.WareHouseManagementService` - for REST API calls<br> &nbsp;&nbsp;&nbsp;b) `CAG.WareHouseLegacyMngntService` - to manage legacy file processing |
| **Dependency Injection - Autofac** | 1. Custom strategy in `Program.cs`<br>2. Classes inherit `IScoped` / `ITransient` interfaces for auto-registration |
| **Repository Pattern - Generic** | `IRepository<T>` supports regular CRUD operations across entities |
| **Configurable Data Source**     | 1. Dev testing uses **InMemory SQLite**<br>2. Real-world uses SQL Server<br>3. Configurable via `DatabaseType` in `appsettings.json` |
| **Swagger Integration - Swashbuckle** | Swagger UI enabled for dev testing |
| **Centralised Exception Handling - Filters** | `CagExceptionFilter` and custom business exceptions used |
| **Logging**                      | Configured via `appsettings.json` using `Microsoft.Extensions.Logging` |
| **DTOs & AutoMapper**            | Simplifies mapping logic between entities and DTOs |
| **Eager Loading**                | Used to link `PurchaseOrder` to related orders |

---

## CAG.WareHouseLegacyMngnt Service

### Scheduled Poller over File System
- Periodically polls a configurable folder
- Reads and parses files
- Ingests data by calling the REST APIs from `CAG.WareHouseManagementService`

### File Parsing Strategy - Extensible
- Implemented using **Strategy + Factory Pattern**
- Autofac key-based registration in `Program.cs`
- To support a new file format (e.g., JSON), implement `JsonParser : IFileParser`

### Entity Type Strategy – Extensible
- Entity inferred from file name (e.g., `Customer_1.xml`)
- Implemented using **Strategy + Factory Pattern** via `Dictionary`
- To support a new entity:
  1. Create a corresponding DTO class
  2. Register it in the dictionary in `FileProcessService`

---

### Features and Design Strategies

| Feature                        | Design Strategy |
|--------------------------------|------------------|
| **Cron Scheduler - Quartz**    | `FilePollingJob` scheduled using cron expression from `appsettings.json` |
| **Retry - Polly**              | File operations are retryable using **Polly** |
| **Exception Handling & Logging** | Centralized logging via `appsettings.json` |
| **File Archival**              | Processed files are archived to prevent reprocessing |
| **HttpClient**                 | Used to call the REST APIs for ingestion |
