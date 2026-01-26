# FinTrack - Personal Finance Management App

FinTrack is a comprehensive personal finance management application designed to help you track your income and expenses, gain insights into your spending habits, and manage your financial health effectively. Built with .NET 8, Blazor, and a Web API backend, it offers a modern and responsive user experience.

## Features

*   **Secure Authentication:** JWT-based authentication for user sign-up and sign-in.
*   **Transaction Management:** Create, view, and manage different types of financial transactions.
*   **Financial Operations:** Log detailed income and expense operations.
*   **Data Summaries:**
    *   View a summary of financial activities for a specific day.
    *   Generate reports for a custom date range.
*   **Monobank Integration:** Seamlessly import your transaction history from your Monobank account.
*   **Responsive UI:** A clean and intuitive user interface built with Blazor.

## Tech Stack

*   **Frontend:** Blazor Server (.NET 8)
*   **Backend:** ASP.NET Core Web API (.NET 8)
*   **Database:** Microsoft SQL Server
*   **Authentication:** JSON Web Tokens (JWT)
*   **API Documentation:** Swagger (OpenAPI)

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the ASP.NET and web development workload
*   Microsoft SQL Server (e.g., Express, Developer, or LocalDB)

### Installation & Configuration

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/Miclasher/Fintrack.git
    cd Fintrack
    ```

2.  **Configure Backend (`Fintrack.WebAPI`):**
    The backend requires a database connection string and JWT settings.

    *   Right-click the `Fintrack.WebAPI` project in Visual Studio and select **Manage User Secrets**.
    *   Add the following configuration to the `secrets.json` file:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Your_SQL_Server_Connection_String"
      },
      "Jwt": {
        "Issuer": "YourIssuer",
        "Audience": "YourAudience",
        "Key": "YourSuperSecretAndLongEnoughKeyForSigning"
      }
    }
    ```

3.  **Configure Frontend (`Fintrack.Frontend`):**
    The frontend needs to know the base URL of the backend API.

    *   Open the `appsettings.json` file in the `Fintrack.Frontend` project.
    *   Set the `ApiBaseUrl` to the address of your running backend API. For local development, this is typically the HTTPS address from the `launchSettings.json` of the WebAPI project.

    ```json
    {
      "ApiBaseUrl": "https://localhost:7123"
    }
    ```

4.  **Apply Database Migrations:**
    *   Open the Package Manager Console in Visual Studio (__View > Other Windows > Package Manager Console__).
    *   Set the `Fintrack.Infrastructure` project as the default project.
    *   Run the `Update-Database` command to create the database schema.

### Running the Application

1.  Set the `Fintrack.WebAPI` project as the startup project and run it (F5 or Ctrl+F5). This will launch the backend API.
2.  Set the `Fintrack.Frontend` project as the startup project and run it.
3.  Alternatively, you can configure the solution to start both projects simultaneously:
    *   Right-click the solution in Solution Explorer and select **Properties**.
    *   Go to **Common Properties > Startup Project**.
    *   Select **Multiple startup projects** and set the Action for both `Fintrack.WebAPI` and `Fintrack.Frontend` to **Start**.

## API Documentation

Once the `Fintrack.WebAPI` project is running, you can access the Swagger UI for interactive API documentation and testing. Navigate to `/swagger` in your browser (e.g., `https://localhost:7123/swagger`).
