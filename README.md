
# 📋 Staff Admin - Web API for Managing Staff

## 📖 Overview

**Staff Admin** is a comprehensive C# Web API project designed to manage staff operations. Developed using the latest **.NET 8** framework, the project utilizes modern design patterns and technologies to ensure scalability, maintainability, and ease of use.

## 📂 Project Structure

The project follows a microservices architecture pattern with the following key services:

- **VnvcStaffAdmin.Gateway**: Manages API routing and request aggregation using Ocelot.
- **VnvcStaffAdmin.Authen**: Manages user authentication and authorization.
- **VnvcStaffAdmin.WebApi**: Core web API service for staff management operations.

## ✨ Features

- **Design Patterns**: 
  - Implements **Unit of Work** and **Repository** patterns for clear abstraction and efficient data management.
- **Scalability**: 
  - Easily deployable with **Docker** for creating scalable and isolated environments.
- **Multi-Database Support**: 
  - Supports multiple `DbContext` connections with **MongoDB** for flexible data management.
- **Authentication & Authorization**: 
  - Secure user authentication using **Identity**.
- **API Gateway**: 
  - Uses **Ocelot** for API Gateway management, routing, and aggregation.
- **Caching**: 
  - **Redis** integration (in-progress) to enhance performance.
- **Full-Text Search**: 
  - **ElasticSearch** integration (in-progress) for advanced search capabilities.
- **Logging**: 
  - Structured logging with **Serilog** (in-progress) for better monitoring and diagnostics.
- **CI/CD**: 
  - Automated build, test, and deployment using **GitHub Actions**.

## 📋 Prerequisites

Before running this project, ensure you have the following installed:

- [Docker](https://docs.docker.com/get-docker/) - Version 20.10 or higher
- [Docker Compose](https://docs.docker.com/compose/install/) - Version 1.29 or higher
- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

## 🚀 Getting Started

Follow these steps to set up and run the project:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/{username}/{repository}.git
   cd {repository}
   ```

2. **Build Docker Images**:
   ```bash
   docker-compose build
   ```

3. **Start the Services**:
   ```bash
   docker-compose up
   ```

4. **Access the API Gateway**:
   Open your browser and navigate to the following URL to view the Swagger UI:
   ```plaintext
   http://localhost:{port}/swagger/index.html
   ```
   Replace `{port}` with the port number defined in your `docker-compose.yml`.