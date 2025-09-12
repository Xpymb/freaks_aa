#!/bin/bash

# Exit on any error
set -e

# Set working directory to script location
cd "$(dirname "$0")"

echo "🚀 Starting Freaks Local Development Environment"
echo "================================================"

# Check if .env file exists
if [ ! -f .env ]; then
    echo "❌ .env file not found. Running setup-env.sh first..."
    ./setup-env.sh
    if [ $? -ne 0 ]; then
        echo "❌ Failed to setup environment. Exiting."
        exit 1
    fi
fi

echo "📦 Starting database services..."
./run_docker-compose-local-db.sh

echo "⏳ Waiting for database to be ready..."
sleep 10

echo "🔧 Starting backend services..."
./run_docker-compose-local-back.sh

echo ""
echo "✅ All services started successfully!"
echo ""
echo "📊 Service Status:"
echo "=================="
echo "Database services:"
docker-compose -f ./docker-compose-local-db.yml -p freaks ps

echo ""
echo "Backend services:"
docker-compose -f ./docker-compose-local-back.yml -p freaks-back ps

echo ""
echo "🔗 Useful commands:"
echo "==================="
echo "View database logs: docker-compose -f ./docker-compose-local-db.yml -p freaks logs -f"
echo "View backend logs:  docker-compose -f ./docker-compose-local-back.yml -p freaks-back logs -f"
echo "Stop all services:  ./stop-local.sh"
echo ""
echo "🌐 Services should be available at:"
echo "- Backend API: http://localhost:5100"
echo "- Database: localhost:5432"
echo "- Redis: localhost:6379"
echo "- MinIO: http://localhost:9000"
