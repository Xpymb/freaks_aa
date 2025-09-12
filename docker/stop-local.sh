#!/bin/bash

# Exit on any error
set -e

# Set working directory to script location
cd "$(dirname "$0")"

echo "🛑 Stopping Freaks Local Development Environment"
echo "================================================"

echo "📦 Stopping backend services..."
docker-compose -f ./docker-compose-local-back.yml -p freaks-back down

echo "📦 Stopping database services..."
docker-compose -f ./docker-compose-local-db.yml -p freaks down

echo ""
echo "✅ All services stopped successfully!"
echo ""
echo "💡 To clean up volumes and data, run:"
echo "   docker-compose -f ./docker-compose-local-db.yml -p freaks down -v"
echo "   docker-compose -f ./docker-compose-local-back.yml -p freaks-back down -v"
