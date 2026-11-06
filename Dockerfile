# ==========================================
# STAGE 1: Build the ASP.NET Core MVC Application
# ==========================================
FROM ://microsoft.com AS build-env
WORKDIR /app

# Copy solution and project files for restoration optimization
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore

# Copy all remaining source files
COPY . ./

# Build and publish release binaries
RUN dotnet publish AgriStreamWeb.csproj -c Release -o out

# ==========================================
# STAGE 2: Lightweight Runtime Environment
# ==========================================
FROM ://microsoft.com
WORKDIR /app
COPY --from=build-env /app/out .

# Expose ports commonly used by Azure App Services
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "AgriStreamWeb.dll"]
