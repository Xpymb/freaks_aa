#!/bin/bash

echo "Copying .env.local to .env..."
cp .env.local .env

echo "Please enter the value for KEYCLOAK_ADMIN_CLIENT_SECRET:"
read -r secret

echo "Updating .env file..."

# Create temporary file
temp_file=$(mktemp)

# Process .env file
while IFS= read -r line; do
    if [[ $line == KEYCLOAK_ADMIN_CLIENT_SECRET=* ]]; then
        echo "KEYCLOAK_ADMIN_CLIENT_SECRET=$secret" >> "$temp_file"
    else
        echo "$line" >> "$temp_file"
    fi
done < .env

# Replace original file
mv "$temp_file" .env

echo "Done! .env file is updated."
