## Project Introduction

**"Racket Speed Badminton Club"** is a Web application which has two main points: <br />
   ★ **Reservation System**  <br />
   ★ **Informational Platform**

## Racket Speed Badminton Club

website link: [to be announced] <br />

A web application for managing badminton club's content, players, coaches, etc. with additional functionality for users to book courts to play. <br />
The target audience is people that love badminton and want to be a part of a family of one of the biggest badminton clubs in Bulgaria.
## General Info
The application is Mobile, Tablet, Desktop Responsive.

The applicaiton can display information about the club such as: 
* [Coaches](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/CoachesPage.png)
* [Players](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/PlayersPage.png)
* [Trainings](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/TrainingSchedulePage.png)
* [Events](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/EventsPage.png)
* [News](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/NewsPage.png)
* [User's Bookings](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/MyReservationsPage.png)

The application provides functionality for the user such as:
* [Booking Courts](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/BookCourtPage.png)
* [Deposit](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/DepositPage.png) (unavailable)
* [Sign-in Kid](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/SignFormPage.png)

## Registration
[Registration](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/AuthenticationPages/RegisterPage.png) is only possible through email invitation and confirmation. <br />
A guest account has been set-up for free use. <br />

Username: **regularUser** <br />
Password: **regularUser123** <br />
website link: [todo] <br />

## Navbars and footers
* [2 footers](https://github.com/NikolovDaniel/SoftUniProject/tree/main/Documentation/Views/Footers)
* [4 navbars](https://github.com/NikolovDaniel/SoftUniProject/tree/main/Documentation/Views/Navbars) for Administrator User, Guest User, Regular User and Regular (Employee) User

## Resend Email Confirmation and Forgot Password
Users can request a new [Email Confirmation](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/AuthenticationPages/EmailConfirmationResendPage.png) link and [Forgot Password](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/AuthenticationPages/ForgotPasswordPage.png) link.

## Architecture
The application is made with ASP.NET MVC supporting the Model/View/Controller pattern.
There are 3 layers, which are: 
* [RacketSpeed](https://github.com/NikolovDaniel/SoftUniProject/tree/main/RacketSpeed/RacketSpeed) (Web/Root)
* [RacketSpeed.Core](https://github.com/NikolovDaniel/SoftUniProject/tree/main/RacketSpeed/RacketSpeed.Core) (Business Logic)
* [RacketSpeed.Infrastructure](https://github.com/NikolovDaniel/SoftUniProject/tree/main/RacketSpeed/RacketSpeed.Infrastructure) (Data Layer)
* [RacketSpeed.UnitTests](https://github.com/NikolovDaniel/SoftUniProject/tree/main/RacketSpeed/RacketSpeed.UnitTests) (Unit Tests)

## Technologies
* IDE - Visual Studio 2022, VS Code
* Framework - ASP.NET Core 6.0
* Version Control - Git, GitHub, GitHub Desktop
* Hosting and File Storage - Azure SQL Database, Azure Cloud

### Database 
* MS SQL Server
* Entity Framework Core 6.0, Scaffold, LINQ

### Backend
* C# .NET 6.0
* 1 Area, 10 Controllers, 7 Services
* Design Pattern - DI, MVC, Repository, SOLID
* Also build with:
  * SendGrid
    
### Client-side
* Razor, JavaScript
* Libraries - jQuery, Ajax, Bootstrap, Cookie
* SASS/CSS3, HTML5 (SVG, Canvas, localStorage)

### Testing
* xUnit
* Fluent Assertions
* Auto Fixture
* Moq, In-memory Database

### Security
 * ASP.NET Core
 * X-CSRF
 * Cross-site Scripting (XSS)
 * Antiforgery
 * SQL Injection
 * SSL

## Functionality 
### Email
 - Custom email [template](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/CustomEmails/EmailTemplate.png)
### Guest Users
 - Can view every [informational](https://github.com/NikolovDaniel/SoftUniProject/tree/main/Documentation/Views/GuestAndInformationalPages) based page
 - Can send forms to [sign a kid](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/GuestAndInformationalPages/SignFormPage.png) in the club
### Logged in Users without Role
 - All of the functionalities of Guest User
 - [Make Deposits](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/DepositPage.png)
 - [Book Court](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/BookCourtPage.png)
 - [Manage Reservations](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/RegularUserPages/MyReservationsPage.png)
 ### Users in role "Employee"
  - Users in this role can [approve, cancel and return the User's deposit](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/EmployeePages/ReservationsToday.png), based on whether the user came to the training or not
 ### Administrators
 - All the functionalities of the Guest, Regular and Regular (Employee) Users
 - [Create, Edit and Delete](https://github.com/NikolovDaniel/SoftUniProject/tree/main/Documentation/Views/AdministratorPages) content
 - Has [Administrator Panel](https://github.com/NikolovDaniel/SoftUniProject/blob/main/Documentation/Views/Navbars/AdministratorNavbar.png) (navbar)

## Documentation

[All Views](https://github.com/NikolovDaniel/SoftUniProject/tree/main/Documentation/Views) 

## Usage
In order to run the project successfully, you have to uncomment the seeding in the [ApplicationDbContext](https://github.com/NikolovDaniel/SoftUniProject/blob/main/RacketSpeed/RacketSpeed.Infrastructure/Data/ApplicationDbContext.cs) and update the appsettings.json with your own Connection String and SendGrid API Key.

## License
This project is licensed under the [MIT License](LICENSE)
  
## Contact
:boy: **Daniel Nikolov**

- LinkedIn: [@danielnikolov](https://www.linkedin.com/in/daniel-nikolov-1090aa233/)
- Github: [@danielnikolov](https://github.com/NikolovDaniel)

### :handshake: Contributing

Contributions, issues and feature requests are welcome!

### :man_astronaut: Show your support

Give a :star: if you like this project!
