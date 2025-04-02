# BlogNotes

This is an API for managing a blog of notes, developed in .NET 8. The application allows to perform CRUD operations (create, read, update and delete) on notes, and is organized in several layers to facilitate its maintainability and scalability. In addition, a mapper is implemented to transform entities to DTOs that can be easily consumed from the frontend, as it will be added in the future.

**FEATURES**
 - CRUD Operations: Allows to create, read, update and delete notes.
 - Data Mapping: Includes a mapper to convert entities to transfer objects (DTOs) for the frontend.
 - Database: Uses Entity Framework Core with Azure SQL Database.
 - Automated CI/CD: Pipeline configured with GitHub Actions to compile, test and deploy the application to Azure Web Apps.
 - Interactive Documentation: Integrated Swagger to explore and test API endpoints.

**TECHNOLOGIES**
 - .NET 8
 - Entity Framework Core
 - Azure SQL Database
 - GitHub Actions
 - Swagger

**PROJECT STRUCTURE**
The project follows a layered architecture:
 - Controllers: expose the REST endpoints of the API.
 - Services: Contain the business logic and validations.
 - Repositories: In charge of interacting with the database through EF Core.
 - Interfaces: Define contracts to facilitate decoupling and dependency injection.
 - Mapper: Tool to transform entities into DTOs for communication with the frontend.

