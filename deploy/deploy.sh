#!/bin/bash

# EX Application Deployment Script
# This script helps deploy the EX application using Docker Compose

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Functions
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}ℹ $1${NC}"
}

check_prerequisites() {
    print_info "Checking prerequisites..."

    if ! command -v docker &> /dev/null; then
        print_error "Docker is not installed. Please install Docker first."
        exit 1
    fi
    print_success "Docker is installed"

    if ! command -v docker-compose &> /dev/null && ! docker compose version &> /dev/null; then
        print_error "Docker Compose is not installed. Please install Docker Compose first."
        exit 1
    fi
    print_success "Docker Compose is installed"
}

check_ports() {
    print_info "Checking if required ports are available..."

    ports=(5432 6379 10000 10001)
    for port in "${ports[@]}"; do
        if lsof -Pi :$port -sTCP:LISTEN -t >/dev/null 2>&1 ; then
            print_warning "Port $port is already in use"
        else
            print_success "Port $port is available"
        fi
    done
}

create_env_file() {
    if [ ! -f .env ]; then
        print_info "Creating .env file from template..."
        cp .env.example .env
        print_success ".env file created. Please review and update with your values."
    else
        print_success ".env file already exists"
    fi
}

build_images() {
    print_info "Building Docker images..."
    docker-compose build
    print_success "Docker images built successfully"
}

start_services() {
    print_info "Starting services..."
    docker-compose up -d
    print_success "Services started"
}

wait_for_services() {
    print_info "Waiting for services to be healthy..."

    max_attempts=30
    attempt=0

    while [ $attempt -lt $max_attempts ]; do
        if docker-compose ps | grep -q "healthy"; then
            print_success "Services are healthy"
            return 0
        fi

        attempt=$((attempt + 1))
        echo -n "."
        sleep 2
    done

    print_warning "Services may not be fully healthy yet. Check logs with: docker-compose logs"
}

show_status() {
    print_info "Service status:"
    docker-compose ps

    echo ""
    print_info "Access URLs:"
    echo "  Web Application: http://localhost:10000"
    echo "  API: http://localhost:10001"
    echo "  API Health: http://localhost:10001/health"
    echo "  Web Health: http://localhost:10000/health"
    echo ""
    print_info "Useful commands:"
    echo "  View logs: docker-compose logs -f"
    echo "  Stop services: docker-compose down"
    echo "  Restart services: docker-compose restart"
}

# Main deployment flow
main() {
    echo "========================================="
    echo "  EX Application Deployment"
    echo "========================================="
    echo ""

    check_prerequisites
    check_ports
    create_env_file

    echo ""
    read -p "Do you want to proceed with deployment? (y/N) " -n 1 -r
    echo ""

    if [[ $REPLY =~ ^[Yy]$ ]]; then
        build_images
        start_services
        wait_for_services

        echo ""
        echo "========================================="
        print_success "Deployment completed!"
        echo "========================================="
        echo ""

        show_status
    else
        print_info "Deployment cancelled"
        exit 0
    fi
}

# Run main function
main
