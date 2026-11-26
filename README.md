<h1 align="center">AutoAppHoho</h1>
<p align="center">
  <b>Car dealership platform with web app and REST API</b><br/>
  ASP.NET Core application for a car dealership: catalog, appointment bookings, invoices, news, map view and user management, plus a JWT-secured API used by the HohoCarApp mobile app.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/ASP.NET%20Core-8%20MVC-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-language-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/EF%20Core-SQL%20Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-auth-000000?style=flat-square&logo=jsonwebtokens&logoColor=white" />
  <img src="https://img.shields.io/badge/Swagger-API%20docs-85EA2D?style=flat-square&logo=swagger&logoColor=black" />
  <img src="https://img.shields.io/badge/GitHub%20Actions-CI-2088FF?style=flat-square&logo=githubactions&logoColor=white" />
</p>

---

## Features

- **Car catalog** — cars with categories and fuel types.
- **Appointments** — customers book an appointment for a specific car.
- **Invoices** — invoices per car and customer.
- **Map** — map view of the cars in the catalog.
- **News** — news articles managed in the app.
- **Dashboard** — management overview.
- **Users and roles** — ASP.NET Core Identity with registration, email confirmation, 2FA support and user management.
- **Multilingual** — language selection.
- **REST API** — `/api/cars` and authentication endpoints secured with JWT, documented with Swagger.
- **Email** — transactional emails via MailKit.

## Architecture

```
Browser ──▶ MVC controllers + Razor views ─┐
                                           ├──▶ EF Core ──▶ SQL Server
HohoCarApp (MAUI) ──▶ REST API (JWT) ──────┘
```

The mobile client is [HohoCarApp](https://github.com/oiseaubleuc/HohoCarApp).

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| Data | Entity Framework Core, SQL Server (LocalDB) |
| Auth | ASP.NET Core Identity, JWT bearer tokens |
| API docs | Swashbuckle (Swagger) |
| Email | MailKit |
| CI | GitHub Actions |

## Getting started

Requirements: .NET 8 SDK, SQL Server LocalDB (or update the connection string).

```bash
git clone https://github.com/oiseaubleuc/AutoAppHoho.git
cd AutoAppHoho/AutoAppHoho
dotnet ef database update
dotnet run                       # https://localhost:7231
```

Swagger UI is available at `/swagger` in development.

---

<p align="center">Built by <a href="https://github.com/oiseaubleuc">Hoho</a> · Belgium</p>
