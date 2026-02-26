
# King Price Assessment

This project was developed as a technical assessment for **King Price**.

It is an ASP.NET Core web application using:
-   **SQLite** database
-   **Entity Framework Core**
-   **Swagger** for API documentation (`/swagger`)
-   **IIS** hosting
-   **Razor Views** (no frontend framework)
    

----------

## 🛠 Tech Stack

-   ASP.NET Core
    
-   Entity Framework Core
    
-   SQLite
  

----------

## 📦 Project Structure Overview

-   `Controllers/` – MVC Controllers for API and Pages
    
-   `Views/` – Razor Views
    
-   `Models/` – Domain models and DbContext
    
-   `Services/` – Logic layer 
    
-   `Migrations/` – Entity Framework migrations

-   `tests/King_Price_Assessment.Tests/ –` Unit test project
    

----------

## 🚀 Getting Started

### 1️⃣ Prerequisites

Make sure you have the following installed:

-   .NET SDK 10
    
-   IIS (if hosting locally via IIS)
    
-   Entity Framework CLI tools
    

Install EF tools if not already installed:

    dotnet tool install --global dotnet-ef

----------

### 2️⃣ Configure Database

The project uses **SQLite**.

Database is stored in `%appdata%`

----------

### 3️⃣ Apply Entity Framework Migrations

Before running the application, apply the database migrations.

Run the following commands in the project root:

    dotnet ef database update

If migrations do not exist and need to be created:

    dotnet ef migrations add InitialCreate  
    dotnet ef database update

This will:

-   Create the SQLite database file
    
-   Apply all schema changes
    

----------

### 4️⃣ Run the Application

#### Using .NET CLI:

dotnet run

#### Using IIS:

-   Publish the application:
    

dotnet publish -c Release

-   Point IIS to the published folder
    
-   Configure the application pool to use **No Managed Code**
    
-   Ensure the site has write permissions to the folder (for SQLite database file creation)
 ----------
 
## 🧪 Running Tests

The solution includes a test project:

    King_Price_Assessment.Tests

Located at:

    tests/King_Price_Assessment.Tests

To execute the tests, run the following command from the solution root:

    dotnet test tests/King_Price_Assessment.Tests

This will:

-   Build the solution
    
-   Execute all unit tests
    
-   Output results to the console
----------

## 📘 API Documentation

Swagger UI is available at:

    /swagger

Example:

https://localhost:7052/swagger

This provides:

-   Full API endpoint documentation
    
-   Request/response schemas
    
-   Interactive testing interface
    

----------

## 🗄 Database

-   Provider: **SQLite**
    
-   ORM: **Entity Framework Core**
    
-   Migrations: Code-first approach
    

The SQLite database file will be created automatically after running:

    dotnet ef database update

----------

## ✅ Notes

- Access between API Controllers and Page Controller might seem weird but they are to simulate typical API access.
    

----------

## 👨‍💻 Author

Stefan Delport