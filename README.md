TODO : Generate Opening template paragraph

## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [Microsoft SQL Server](https://azure.microsoft.com/en-us/services/cosmos-db/)
* [Swashbuckle (Swagger Implementation)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Serilog (Logging implementation)](https://github.com/serilog/serilog-aspnetcore)
* [XUnit (Unit Test implementation)](https://xunit.net/docs/getting-started/netcore/cmdline)
* [FluentAssertions (Unit Test Extension Methods)](https://fluentassertions.com/)

## Getting Started

### Software Prerequisites
In order to run this solution, you will need the following software prerequisites installed on your development machine.

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)\
Downloading and installing will also install needed .NET CORE 6 SDK.\
Any edition will suffice but 'Enterprise' will provide the most robust experience.
* [Microsoft SQL Server 2019 Developers Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)\
Any edition of MSSQL will suffice (Developers Edition, Express, etc).

### Clone Solution
Clone the following solution to your development machine.

### Secrets Configuration

For development, the 'Secrets Manager' is utilized for managing sensitive data.\
[More information about Secrets Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

To create and access your individual development secrets file, right click on the 'Api' project and select 'Manage User Secrets' from the context menu.\
Paste the following code into your secrets file and update your connection string.\

```javascript
{
  "ConnectionStrings": {
    //This will be your localhost database connection string. If applicable, modify.
    "NorthwindContext": "Server=localhost;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Database Migrations

In order to prepopulate your configured localhost database, you must execute Database Migrations against it.\
Open up your 'Package Manager' (In VS, go to 'View' --> 'Other Windows' --> 'Package Manager')\
and execute the following snippet.

```
update-database -Context NorthwindContext
```
After you successfully run the above snippet, your configured localhost database should be seeded with a subset of the 'Northwind' database.

## Solution Architecture Overview

### Domain

This will contain all database entities, enumerations, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application business logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within 'infrastructure' layer.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### Api

This layer is an API based on Restful Principals. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure by way of the *services.AddMyInfrastructure(Configuration);* method.\
If you need to add more services, you can add the service to the *Infrastructure.DependencyInjection.cs* file.

### Templator

This layer is an html creator using standard MVC (Model/View/Controller) conventions. It uses domain objects as the 'Model' and .cshtml files as the 'View' to create html templates for emails or other html needed services.

## Support

If you are having problems, please let us know by [To Do : Add contact email or link].

## License

This project is licensed with the [MIT license](LICENSE).
