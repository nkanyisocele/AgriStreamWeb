# Full-Stack Application

This repository contains both the backend API and frontend client applications, configured with Docker containerization and automated via GitHub Actions pipelines.

## 📁 Project Structure

```text
├── src/
│   ├── MyProject.API/        # .NET 8 Web API (Controllers, Interfaces, Dependencies)
│   └── myproject-ui/         # React Frontend Application
├── .github/workflows/        # GitHub Actions CI/CD pipelines
├── .gitignore                # Git exclusion rules
├── Dockerfile                # Multi-stage Docker configuration for deployment
└── README.md                 # Project documentation
```

## 🛠️ Tech Stack

- **Backend:** .NET 8 Web API, C#
- **Frontend:** React (TypeScript / JavaScript)
- **DevOps:** Docker, GitHub Actions, Microsoft Azure

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://microsoft.com)
- [Node.js](https://nodejs.org) (LTS version)
- [Docker Desktop](https://docker.com)

### 🔑 Environment Configuration

Do not commit actual secrets or credentials to source control. Create the following files locally in their respective project directories.

#### Backend (`src/MyProject.API/appsettings.Development.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
  },
  "Azure": {
    "StorageConnectionString": "YOUR_AZURE_STORAGE_CONNECTION"
  }
}
```

#### Frontend (`src/myproject-ui/.env.local`)
```env
REACT_APP_API_URL=https://localhost:7001
```

### Local Development Setup

#### 1. Database Migrations (Backend)
Navigate to the API folder and apply any outstanding Entity Framework Core migrations to your local instance:
```bash
cd src/MyProject.API
dotnet ef database update
```

#### 2. Running the Backend API
```bash
dotnet restore
dotnet run
```
*The API will be accessible at `https://localhost:7001` or `http://localhost:5001`.*

#### 3. Running the Frontend UI
```bash
cd ../myproject-ui
npm install
npm start
```
*The web application will open automatically at `http://http://localhost:3000`.*

## 🧪 Testing

### Run Backend Tests
```bash
cd src/MyProject.API
dotnet test
```

### Run Frontend Tests
```bash
cd src/myproject-ui
npm test
```

## 🐳 Docker Deployment

To build and run the entire solution using Docker locally:

```bash
# Build the multi-stage Docker image
docker build -t fullstack-app .

# Run the container
docker run -p 8080:80 --env-file ./src/myproject-ui/.env.local fullstack-app
```

## 🤖 CI/CD Pipeline

This repository is integrated with **GitHub Actions**. The pipeline handles code linting, automated testing, Docker build verification, and deployment staging to Azure on:
- Pushes to the `main` branch
- Pull requests to the `main` branch
