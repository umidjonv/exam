# Database Initialization Scripts

This directory contains SQL scripts that will be automatically executed when the PostgreSQL container is first initialized.

## Usage

1. Place your `.sql` or `.sh` files in this directory
2. Files are executed in alphabetical order
3. Scripts only run on the first container initialization (when the database volume is empty)

## Example

Create a file `01-init.sql`:

```sql
-- Example initialization script
CREATE TABLE IF NOT EXISTS example (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);
```

## Notes

- Scripts are executed as the `postgres` user
- If you need to re-run initialization scripts, you must remove the database volume:
  ```bash
  docker-compose down -v
  docker-compose up -d
  ```
- The database connection string is configured in the docker-compose.yml file
- Entity Framework migrations should be run separately from the application

## Entity Framework Migrations

Instead of using these init scripts, you may prefer to use EF Core migrations:

```bash
# From inside the ex-api container
docker-compose exec ex-api dotnet ef database update

# Or from your local machine (if .NET SDK is installed)
cd ../src/EX.Api
dotnet ef database update
```
