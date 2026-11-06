# Full-Stack Application

This repository contains both the backend API and frontend client applications, configured with Docker containerization and automated via GitHub Actions.

## 📁 Project Structure

```text
├── src/
│   ├── MyProject.API/        # .NET 8 Web API (Controllers, Interfaces, Dependencies)
│   └── myproject-ui/         # React Frontend Application
├── .github/workflows/        # GitHub Actions CI/CD pipelines
├── Dockerfile                # Docker configuration for deployment
└── README.md                 # Project documentation
```

## 🛠️ Tech Stack

- **Backend:** .NET 8 Web API, C#
- **Frontend:** React (TypeScript / JavaScript)
- **DevOps:** Docker, GitHub Actions, Microsoft Azure

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (LTS version)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Local Development Setup

#### 1. Running the Backend API
```bash
cd src/MyProject.API
dotnet restore
dotnet run
```
*The API will typically be accessible at `https://localhost:7001` or `http://localhost:5001`.*

#### 2. Running the Frontend UI
```bash
cd src/myproject-ui
npm install
npm start
```
*The web application will open at `http://localhost:3000`.*

## 🐳 Docker Deployment

To build and run the entire solution using Docker:

```bash
# Build the Docker image
docker build -t fullstack-app .

# Run the container
docker run -p 8080:80 fullstack-app
```

## 🤖 CI/CD Pipeline

This repository is integrated with **GitHub Actions**. The workflow triggers automatically on:
- Pushes to the `main` branch
- Pull requests to the `main` branch

The pipeline handles code linting, automated testing, Docker build verification, and deployment staging to Azure.
