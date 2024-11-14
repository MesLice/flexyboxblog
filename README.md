# .NET Developer Job Application - Flexybox Blog

## Start Guide

To run the project locally, follow these steps:

### Prerequisites

- Install [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or use Docker to run SQL Server in a container
- Install [Docker Desktop](https://www.docker.com/products/docker-desktop/) for compatibility and easier management of containers (optional for Docker setup)

### Running the Application

#### Normal Setup

1. **Download the Repository**: You can either clone the Git repository to your local machine or download it as a zip file from [GitHub Repository](https://github.com/MesLice/flexyboxblog).

   - **Clone**: Run the following command to clone:
     ```
     git clone https://github.com/MesLice/flexyboxblog
     ```
   - **Download as Zip**: Alternatively, download the repository as a zip file from GitHub and extract it to your preferred location.

2. **Load the Database Backup**: Load up the database backup from the `mssql-init` folder.

3. **Edit Default Connection String**: Edit the `DefaultConnection` string in `FlexyboxBlog\appsettings.json` to point to your local SQL Server instance.

4. **Open the Project in Visual Studio**: Open the project in Visual Studio.

5. **Set Up Multiple Project Startup**: Set up multiple project startup by configuring `FlexyboxBlog` to Start and `FlexyboxBlogRazor` to Start.

6. **Start the Project**: Start the project in Visual Studio.


#### Docker Setup

1. **Install Docker Desktop**: Install [Docker Desktop](https://www.docker.com/products/docker-desktop/) for easier management of containers, or alternatively ensure that normal Docker is installed on your machine.

2. **Download the Repository**: You can either clone the Git repository to your local machine or download it as a zip file from [GitHub Repository](https://github.com/MesLice/flexyboxblog).

   - **Clone**: Run the following command to clone:
     ```
     git clone https://github.com/MesLice/flexyboxblog
     ```
   - **Download as Zip**: Alternatively, download the repository as a zip file from GitHub and extract it to your preferred location.

3. **Open PowerShell and Navigate to the Repository**: Open PowerShell and navigate to the directory containing the `docker-compose.yml` file.

4. **Run Docker Compose**: Run the following command to build and start the containers in detached mode:
   ```
   docker-compose up --build --detach
   ```

5. **(Optional) Install Certificates**: In the `certs` directory, there is a `dockercert.crt` certificate. Open it and follow the prompts to install the certificate.

6. **Access the Application**: Open your browser and navigate to [https://localhost:7212/](https://localhost:7212/).

## Technologies Used

- **Frontend**: Blazor with MudBlazor components (.NET 8)
- **Backend**: ASP.NET Core Web API (.NET 8)
- **Database**: SQL Server for blog posts and user identity management

## Frontend (UI)

- **Homepage**: Uses MudBlazor components to display all blog posts. There is also a search section to help users find posts quickly, and an about section to provide information about the project.
- **Logged-in Features**: When logged in, users see a button to create new blog posts and another to view their posts. They can individually edit or delete their posts from this section.

## Backend (API)

- **CRUD Endpoints**: Full CRUD operations for blog posts, along with cookie-based authentication.
- **Error Handling**: Errors are displayed using MudBlazor's Snackbar for user-friendly messages.
- **Security**: Includes cookie-based authentication, anti-forgery tokens, and CORS handling.

## Enhancements

- **Authentication**: Cookie-based login, registration, and logout.
- **Search Feature**: Added full search functionality for blog posts.
- **Dark Mode Switch**: Users can toggle between light and dark themes.
- **Docker Deployment**: Added Docker support for easy deployment, including a full implementation using Docker Compose.

## Time Tracking

- **Backend Development**: The API was the quickest part to implement, taking only a few hours of work.
- **Frontend Development**: The UI took a couple of days to complete, mostly due to challenges with integrating the new RenderModes in .NET 8.
- **Docker Setup**: The Docker setup, along with writing documentation for it, took around a day to finish.

## Challenges

- **Upgrading to .NET 8**: Moving to .NET 8 Blazor with the new RenderModes was challenging due to limited documentation, especially for MudBlazor. It required extensive experimentation to integrate properly.