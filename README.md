# GBC Travel Booking Application (Group 125)

![License](https://img.shields.io/github/license/kabamehmetali/Assignment_Group_125) 

## Project Description
**GBC Travel** is a web-based travel booking platform built with **ASP.NET Core MVC** and **C#**. It was developed as a group assignment (Group 125) to simulate a travel agency application. The platform allows users to browse and book travel services such as flights, hotels, and car rentals. It implements a role-based system using ASP.NET Core Identity for **authentication and authorization**, so that *travelers* (regular users) can search and book trips, while *admins* can manage listings and bookings. The project emphasizes a layered MVC architecture, secure user account management, and a responsive web interface for a smooth booking experience.

## Installation Instructions
**Prerequisites:** Ensure you have **.NET 6.0 or higher** SDK installed (the project targets .NET 6+ and ASP.NET Core). You will also need a SQL Server instance (e.g., **SQL Server Express LocalDB** or SQL Server) for the application’s database.

**Steps to set up the project:**
1. **Clone the Repository:** Download or clone this repository to your local machine using:  
   ```bash
   git clone https://github.com/kabamehmetali/Assignment_Group_125.git
   ```
2. **Open the Solution:** Navigate to the project folder and open the `GBC_Travel_Group_125.sln` solution file in **Visual Studio 2022** (or later). Alternatively, you can use VS Code or the .NET CLI.
3. **Restore Dependencies:** Visual Studio will automatically restore NuGet packages on opening the solution. If using CLI, run `dotnet restore` to install all dependencies.
4. **Database Configuration:** Update the database connection string in `appsettings.json` if needed. By default, the project may use a LocalDB connection (e.g., `Server=(localdb)\MSSQLLocalDB;Database=GBCTravelDB;...`). Ensure SQL Express LocalDB is installed or modify the connection to point to your SQL Server.  
   *Optional:* If using Entity Framework Core Code-First Migrations, run `Update-Database` (in the Package Manager Console or via CLI with `dotnet ef database update`) to create the database and tables.
5. **Build and Run:** Build the solution and run the web application:  
   - In Visual Studio, press **F5** or click **Start** to launch the project.  
   - With .NET CLI, navigate to the project folder and run `dotnet run`.  
6. **Access the App:** Once running, the application will be accessible in your web browser at the URL displayed in the console (by default something like `https://localhost:5001` or `http://localhost:5000` for ASP.NET Core). 

## Usage
After launching the **GBC Travel** application, you can begin using the travel booking features through your web browser.

- **User Registration & Login:** Sign up for a new account or log in with an existing one. During registration, you can register as a **Traveler (regular user)** or an **Admin** (administrative user) as per the application’s interface. The system uses **ASP.NET Identity**, so account verification (e.g., email confirmation) and secure password practices are in place. Once registered and logged in, your user role (Traveler or Admin) will determine what features you can access.
- **Traveler Functionality:** As a traveler, you can browse available travel services:
  - **View Listings:** Browse or search for flights, hotels, and car rentals. Listings include details like destination, dates, prices, etc.
  - **Booking:** Select a travel option (flight, hotel, etc.) and make a booking/reservation. The application will guide you through providing necessary details and confirming the booking. You can view your past bookings in your account dashboard.
  - **Reviews:** After completing a booking, you may be able to leave a review or rating for the service (if this feature is enabled in the application).
- **Admin Functionality:** As an admin, you have access to management features:
  - **Manage Listings:** Create, edit, or delete listings for flights, hotels, and car rentals.
  - **Manage Bookings:** View all bookings made by users. Admins can see booking details and may perform administrative actions on bookings if required.
  - **User Management:** Admins might have additional user management capabilities. For example, viewing the list of registered users or assigning roles.
- **Navigation:** Use the site’s navigation menu to switch between pages.
- **Example Workflow:** A traveler searches for a flight by destination and date, finds a suitable option, and books it. An admin can add new flight listings or review bookings.

## Contributors
**Group 125 Team:** This project was developed by **Mehmet Ali KABA** (GitHub: @kabamehmetali). If other team members contributed, their details can be added here.

## License
This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for full license details.

## Badges
- ![License](https://img.shields.io/badge/License-MIT-green.svg) 

## Additional Information
- **Technology Stack:** ASP.NET Core MVC, .NET 6+, Entity Framework Core, ASP.NET Identity.
- **Purpose:** Created for educational purposes as a college coursework (COMP2139 assignment).
- **How to Contribute:** Fork the repository and open a pull request.
- **Contact:** Create an issue on the repository.
