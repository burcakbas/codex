# Employees Demo

Minimal full-stack demo for a Sovereign Cloud style environment.

## Prerequisites

- Docker Desktop

## Getting Started

1. Copy `.env` with required settings:

```
SA_PASSWORD=Your_strong_password123
ALLOWED_ORIGINS=http://localhost:3000
NEXT_PUBLIC_API_BASE_URL=http://localhost:5080
ConnectionStrings__Default=Server=sql;Database=EmployeesDb;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True;
```

2. Build and run:

```
docker compose up --build
```

- UI: http://localhost:3000
- API: http://localhost:5080
- Health: http://localhost:5080/healthz

Seed data will appear on first run. Use the UI to create, edit and delete employees. Sorting and search are available.

### Running locally without Docker

**API**
```
cd employees-api
# ensure ConnectionStrings__Default and ALLOWED_ORIGINS are set
# dotnet restore
# dotnet run
```

**UI**
```
cd employees-ui
# npm install
# npm run dev
```

### Reset database

```
docker compose down -v
```
The next `docker compose up` will recreate the database and seed data.

### External SQL Server

Set `ConnectionStrings__Default` to point to an external SQL Server/Azure SQL instance and rebuild the API container or run the API locally with that connection string.
