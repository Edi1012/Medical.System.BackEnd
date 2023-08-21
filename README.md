
# The Medical Systems Management API

## Overview
The Medical Systems Management API is a .NET 7 Core application designed to manage various aspects of medical systems. It employs a robust architecture with best practices and design patterns such as Repository, Unit of Work, and Service, ensuring a highly maintainable and scalable system..

## Running the Application
You have two methods to get the application up and running:

### 1. Using Docker
The application is containerized and readily available from Docker Hub. You can pull and run the Docker image using the following command:

```bash
docker run -e "Jwt_Key=TuClaveSecretaAqui" -e "JwtIssuer=TuIssuer" -e "Jwt_Audience=TuAudience" -p 8080:80 -p 8443:443 -d edi10/edi-medical-system:tagname
```

You can find more details on the [Docker Hub repository page](https://hub.docker.com/repository/docker/edi10/edi-medical-system/general).

### 2. Building from Source
If you prefer to build and run the application from the source code, follow these steps:

#### Installation
1. **Clone the repository**:
   ```
   git clone [Repository URL]
   ```

2. **Navigate to the project folder**:
   ```
   cd [Project Folder]
   ```

3. **Restore the project**:
   ```
   dotnet restore
   ```

4. **Build the application**:
   ```
   dotnet build
   ```

5. **Start the application**:
   ```
   dotnet run
   ```

## Support and Documentation
## Project Structure

```
Medical.System.Core:
  Enums: "Contains the enumerations used in the system such as Gender, Marital Status, and User Roles."
  Exceptions: "Houses the custom exception classes and middleware for error handling."
  DTOs: "Defines the Data Transfer Objects for interacting with the system."
  Entities: "Defines the core entity models used in the application."
  Repositories: "Contains the repository classes for data access."
  Services: "Contains the service classes for business logic."
  Interfaces: "Contains the interface definitions for repositories and services."
  UnitOfWork: "Includes the implementation and interface for the Unit of Work pattern."
  Validators: "Houses the validator classes for validating data."
  Files:
    Enums:
      - Gender.cs
      - MaritalStatus.cs
      - UserRoleEnum.cs
    Exceptions:
      - NotFoundException.cs
      - ValidationException.cs
      - ErrorDetail.cs
      - ErrorResponse.cs
      - ErrorHandlingMiddleware.cs
    DTOs:
      - AddressDTO.cs
      - CreateSupplierDto.cs
      - ...
    Entities:
      - Address.cs
      - EmergencyContact.cs
      - ...
    Repositories:
      - GenericRepository.cs
      - SupplierRepository.cs
      - ...
    Services:
      - DatabaseResolverService.cs
      - SupplierService.cs
      - ...
    Interfaces:
      - IGenericRepository.cs
      - ISupplierRepository.cs
      - ...
    UnitOfWork:
      - IUnitOfWork.cs
      - UnitOfWork.cs
    Validators:
      - CreateTokenValidator.cs
      - CreateUserValidator.cs
```



For additional support, documentation, or information about contributions, please refer to the project's documentation or contact the development team.

---



