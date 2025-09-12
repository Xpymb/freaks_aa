#!/bin/bash

# Exit on any error
set -e

# Set working directory to script location
cd "$(dirname "$0")"

# Check if .env file exists
if [ ! -f .env ]; then
    echo "Error: .env file not found. Please run setup-env.sh first."
    exit 1
fi

# Load environment variables
export $(cat .env | grep -v '^#' | xargs)

# Set Docker BuildKit
export DOCKER_BUILDKIT=0

echo "Starting backend services..."
docker-compose -f ./docker-compose-local-back.yml -p freaks-back up --build -d --remove-orphans

echo "Backend services started successfully!"
echo "You can check logs with: docker-compose -f ./docker-compose-local-back.yml -p freaks-back logs -f"