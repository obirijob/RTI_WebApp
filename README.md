# Rapid Inventory — Stage 3 Practical

A small inventory and order-management system used for the take-home practical assessment. Task instructions and deliverables are in the brief sent to you with this project.

## Requirements

- Docker Desktop with at least 6 GB of memory available
- Ports `1433`, `4200`, and `5080` free

## Start

```bash
docker compose up --build
```

The first start pulls the SQL Server image and seeds 2,000 products plus 250,000 historical orders, so it takes longer than later starts.

- Web app: http://localhost:4200
- API: http://localhost:5080
- Health check: http://localhost:5080/health

## Reset

Reset the database and rebuild all services:

```bash
./seed/reset.sh
```

## Project Map

```text
backend/   ASP.NET Core API, EF Core, SignalR
frontend/  Angular web app
seed/      reset and load-generation scripts
```

The main user flows are:

- Search and inspect product stock
- Place a single-product order
- Restock a product
- Watch live stock changes and low-stock alerts in another browser
- Query recent orders through `GET /api/orders/recent`

## Useful Commands

```bash
# Build the API and its projects
dotnet build Inventory.sln

# Build the Angular app
npm --prefix frontend run build

# Send concurrent orders (defaults to 40 requests for product 1)
./seed/stress-orders.sh
```

Change load-generator inputs with environment variables:

```bash
PRODUCT_ID=10 REQUESTS=20 QUANTITY=2 ./seed/stress-orders.sh
```

## Troubleshooting

- If startup hangs, run `docker compose ps` and wait for `db` to report healthy.
- If a port is occupied, stop the process using it before restarting Compose.
- To discard all database state, use `docker compose down -v` and start again.
