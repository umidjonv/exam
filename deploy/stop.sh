#!/bin/bash

# EX Application Stop Script

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ $1${NC}"
}

echo "========================================="
echo "  Stopping EX Application"
echo "========================================="
echo ""

print_info "Stopping all services..."
docker-compose down

print_success "All services stopped"

echo ""
print_info "To remove volumes as well, run:"
echo "  docker-compose down -v"
