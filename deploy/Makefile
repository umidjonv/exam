.PHONY: help build up down restart logs logs-api logs-web logs-db status clean ps exec-api exec-web exec-db backup restore health

help:
	@echo "Available commands:"
	@echo "  make build       - Build all Docker images"
	@echo "  make up          - Start all services"
	@echo "  make down        - Stop all services"
	@echo "  make restart     - Restart all services"
	@echo "  make logs        - View logs from all services"
	@echo "  make logs-api    - View logs from API service"
	@echo "  make logs-web    - View logs from Web service"
	@echo "  make logs-db     - View logs from database"
	@echo "  make status      - Show status of all services"
	@echo "  make ps          - List running containers"
	@echo "  make clean       - Stop and remove all containers and volumes"
	@echo "  make exec-api    - Execute bash in API container"
	@echo "  make exec-web    - Execute bash in Web container"
	@echo "  make exec-db     - Execute psql in database container"
	@echo "  make backup      - Backup database"
	@echo "  make health      - Check health of all services"

build:
	docker-compose build

up:
	docker-compose up -d

down:
	docker-compose down

restart:
	docker-compose restart

logs:
	docker-compose logs -f

logs-api:
	docker-compose logs -f ex-api

logs-web:
	docker-compose logs -f ex-web

logs-db:
	docker-compose logs -f postgres

status:
	docker-compose ps

ps:
	docker-compose ps

clean:
	@echo "WARNING: This will remove all containers and volumes!"
	@read -p "Are you sure? [y/N] " -n 1 -r; \
	echo; \
	if [[ $$REPLY =~ ^[Yy]$$ ]]; then \
		docker-compose down -v; \
	fi

exec-api:
	docker-compose exec ex-api bash

exec-web:
	docker-compose exec ex-web bash

exec-db:
	docker-compose exec postgres psql -U postgres -d EX

backup:
	@mkdir -p backups
	docker-compose exec postgres pg_dump -U postgres EX > backups/backup_$$(date +%Y%m%d_%H%M%S).sql
	@echo "Backup created in backups/ directory"

restore:
	@echo "Available backups:"
	@ls -1 backups/*.sql 2>/dev/null || echo "No backups found"
	@read -p "Enter backup filename: " backup; \
	cat backups/$$backup | docker-compose exec -T postgres psql -U postgres EX

health:
	@echo "Checking health endpoints..."
	@curl -f http://localhost:10001/health && echo "✓ API is healthy" || echo "✗ API is unhealthy"
	@curl -f http://localhost:10000/health && echo "✓ Web is healthy" || echo "✗ Web is unhealthy"
