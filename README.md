# CORS Demo API

A .NET 10.0 Web API demonstrating Cross-Origin Resource Sharing (CORS) configurations and behaviors.

## Overview

This project showcases different CORS policies and their effects on cross-origin requests. It includes examples of:

- Simple requests (no preflight)
- Preflighted requests
- Credentialed requests
- Preflight caching

## CORS Policies

| Policy             | Description                                          |
| ------------------ | ---------------------------------------------------- |
| `AllowPostMethod`  | Allows POST requests from `http://127.0.0.1:5500`    |
| `AllowCredentials` | Allows POST with credentials (cookies, auth headers) |
| `PreflightCache`   | Caches preflight responses for 10 minutes            |

## API Endpoints

| Method | Endpoint                          | CORS Policy      | Description                                |
| ------ | --------------------------------- | ---------------- | ------------------------------------------ |
| GET    | `/simple-get`                     | None             | Simple GET request                         |
| POST   | `/preflighted-post-no-cors`       | None             | POST without CORS (will fail cross-origin) |
| POST   | `/preflighted-post-with-cors`     | AllowPostMethod  | POST with CORS enabled                     |
| POST   | `/credentialed-post-with-no-cors` | None             | Credentialed POST without CORS             |
| POST   | `/credentialed-post-with-cors`    | AllowCredentials | Credentialed POST with CORS                |
| POST   | `/submit-form`                    | None             | Form submission (no preflight needed)      |
| POST   | `/preflighted-cached`             | PreflightCache   | POST with preflight caching                |

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

## Getting Started

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd CORS
   ```
3. Run the API:
   ```bash
   dotnet run --project APIs
   ```

The API will be available at the URLs specified in `launchSettings.json`.
