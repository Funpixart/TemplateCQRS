![.NET 7](https://img.shields.io/badge/.NET-7.0-blue)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)]()


## Overview

This template adheres to the Command Query Responsibility Segregation (CQRS) pattern, enabling a separation of read and write responsibilities in the system. This enhances scalability and performance. Additionally, it incorporates Clean Architecture, making the project easily maintainable and testable.

To ensure code flexibility and reusability, the Generic Repository pattern is used in conjunction with the Unit of Work pattern. These patterns simplify the interaction with data resources and enable more efficient control of transactions and operations in the database.

## Front-end 

The project also implements a front-end design in Blazor with the most basic components to create a simple system from scratch while reusing dynamic components.

### Existing Components

- [x] Badge
- [x] Button
- [x] Container
- [x] DropDown
- [x] Input
- [x] Label
- [x] Modal
- [x] Spinner
- [x] Notifications
- [x] Pagination
- [x] AccessDenied
- [x] NotFound
- [x] RedirectToPage

### Planned Components

- [ ] Accordion
- [ ] Alerts
- [ ] Breadcrumb
- [ ] Card
- [ ] Carousel
- [ ] Collapse
- [ ] SidebarMenu
- [ ] NavbarMenu
- [ ] Placeholder
- [ ] Popovers
- [ ] Progress
- [ ] Tooltips
- [ ] Scheduler
- [ ] Dialog (a better modal service)
- [ ] Layouts
- [ ] ProfileMenu
- [ ] Steps
- [ ] Tabs
- [ ] DynamicForms
- [ ] TemplateForms
- [ ] Rating
- [ ] SelectBar
- [ ] Slider
- [ ] Upload
- [ ] Charts (Pies, Lines, Bars)

## Authentication

The project uses JWT authentication to authorize the endpoints, which are developed in MinimalAPI. All these are implemented in .NET 7 and with potential databases in MySQL or MSSQL.

## Testing

The project incorporates xUnit and Moq for unit testing and uses EF Core InMemory for integration testing.

## Getting Started

### Initial Setup

1. Clone the repository
2. Navigate to the project folder
3. Install the necessary dependencies

### Model and DTO Creation

4. Create models and DTOs in the following directories:
   - `TemplateCQRS.Domain > Models`
   - `TemplateCQRS.Domain > DTOs`

### Feature Setup

5. Each class will have its own folder in the `Features` directory in `TemplateCQRS.Application`. The typical structure is:
   - `FeatureName > Commands > Create...Command.cs`
   - `FeatureName > Handlers > Create...CommandHandler.cs`
   - `FeatureName > Handlers > Get...QueryHandler.cs`
   - `FeatureName > Queries > Get...Query.cs`
   - `FeatureName > Validators > [Dto or class]...Validator.cs`
   - `FeatureName > FeatureMappingProfile.cs`

## Utilization

Use this project as a template for new solutions that will be developed.