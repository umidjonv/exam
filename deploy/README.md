# EX Application - Docker Deployment Guide

This directory contains all the necessary files to deploy the EX application using Docker Compose.

## Architecture

The application consists of the following services:

- **postgres**: PostgreSQL 16 database
- **redis**: Redis cache server
- **ex-api**: Backend API service (ASP.NET Core 8.0)
- **ex-web**: Frontend web application (ASP.NET Core 8.0 MVC)

## Prerequisites

- Docker Engine 20.10+ or Docker Desktop
- Docker Compose 2.0+
- At least 4GB of available RAM
- Ports 5432, 6379, 10000, and 10001 available

## Quick Start

### 1. Clone the repository

```bash
cd /path/to/exam
```

### 2. Navigate to the deploy directory

```bash
cd deploy
```

### 3. Create environment file (optional)

```bash
cp .env.example .env
# Edit .env with your custom values if needed
```

### 4. Build and start all services

```bash
docker-compose up -d --build
```

### 5. Check service status

```bash
docker-compose ps
```

### 6. View logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f ex-web
docker-compose logs -f ex-api
```

## Service Access

After successful deployment:

- **Web Application**: http://localhost:10000
- **API**: http://localhost:10001
- **API Health Check**: http://localhost:10001/health
- **Web Health Check**: http://localhost:10000/health
- **PostgreSQL**: localhost:5432
- **Redis**: localhost:6379

## Database Initialization

The PostgreSQL database will be automatically initialized with:
- Database name: `EX`
- Username: `postgres`
- Password: `12345`

### Running Migrations

If you need to run Entity Framework migrations:

```bash
# From the ex-api container
docker-compose exec ex-api dotnet ef database update

# Or from your local machine (if you have .NET SDK installed)
cd ../src/EX.Api
dotnet ef database update
```

## Common Commands

### Start services

```bash
docker-compose up -d
```

### Stop services

```bash
docker-compose down
```

### Stop services and remove volumes (WARNING: This will delete all data)

```bash
docker-compose down -v
```

### Restart a specific service

```bash
docker-compose restart ex-web
docker-compose restart ex-api
```

### Rebuild a specific service

```bash
docker-compose up -d --build ex-api
docker-compose up -d --build ex-web
```

### View real-time logs

```bash
docker-compose logs -f
```

### Execute commands in a container

```bash
docker-compose exec ex-api bash
docker-compose exec postgres psql -U postgres -d EX
```

## Troubleshooting

### Service won't start

Check logs for errors:
```bash
docker-compose logs ex-api
docker-compose logs ex-web
```

### Database connection issues

1. Ensure PostgreSQL is healthy:
```bash
docker-compose ps postgres
```

2. Check database logs:
```bash
docker-compose logs postgres
```

3. Test connection manually:
```bash
docker-compose exec postgres psql -U postgres -d EX
```

### Port already in use

If you get "port already in use" errors, modify the port mappings in `docker-compose.yml`:

```yaml
ports:
  - "10000:10000"  # Change first port: "NEW_PORT:10000"
```

### Clear cache and rebuild

```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

## Production Deployment

For production deployment, consider:

1. **Change default passwords** in `.env` file
2. **Use secrets management** instead of plain environment variables
3. **Enable SSL/TLS** with a reverse proxy (nginx, traefik)
4. **Set up regular backups** for PostgreSQL data
5. **Configure log rotation** for application logs
6. **Use external volumes** for persistent data
7. **Set resource limits** in docker-compose.yml

### Example Production Changes

```yaml
services:
  ex-api:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 2G
        reservations:
          cpus: '1'
          memory: 1G
```

## Backup and Restore

### Backup PostgreSQL database

```bash
docker-compose exec postgres pg_dump -U postgres EX > backup_$(date +%Y%m%d_%H%M%S).sql
```

### Restore PostgreSQL database

```bash
cat backup.sql | docker-compose exec -T postgres psql -U postgres EX
```

### Backup volumes

```bash
docker run --rm -v deploy_postgres_data:/data -v $(pwd):/backup alpine tar czf /backup/postgres_backup.tar.gz -C /data .
```

## Monitoring

### Check service health

```bash
curl http://localhost:10001/health
curl http://localhost:10000/health
```

### Monitor resource usage

```bash
docker stats ex-api ex-web postgres redis
```

## Directory Structure

```
deploy/
├── docker-compose.yml       # Main orchestration file
├── Dockerfile.api          # API service Dockerfile
├── Dockerfile.web          # Web service Dockerfile
├── .env.example           # Environment variables template
├── README.md              # This file
├── init-scripts/          # Database initialization scripts
│   └── (place .sql files here)
└── logs/                  # Application logs (auto-created)
    ├── api/
    └── web/
```

## Environment Variables

See `.env.example` for all available environment variables and their descriptions.

## Support

For issues and questions:
- Check the logs: `docker-compose logs -f`
- Review the troubleshooting section above
- Ensure all prerequisites are met
- Verify port availability

## License

[Your License Here]
