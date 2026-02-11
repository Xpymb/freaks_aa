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

# === Grafana Alloy Configuration ===
echo ""
echo "=== Grafana Alloy Configuration ==="
echo ""

echo "--- Loki (logs) ---"
read -rp "Loki URL: " loki_url
read -rp "Loki username: " loki_username
read -rp "Loki password: " loki_password

echo ""
echo "--- Tempo (traces) ---"
read -rp "Tempo OTLP endpoint: " tempo_url
read -rp "Tempo username: " tempo_username
read -rp "Tempo password: " tempo_password

echo ""
echo "--- Prometheus (metrics) ---"
read -rp "Prometheus remote write URL: " prometheus_url
read -rp "Prometheus username: " prometheus_username
read -rp "Prometheus password: " prometheus_password

echo ""
echo "Generating GrafanaAlloy/config.alloy from template..."

sed -e "s|your_loki_url|$loki_url|g" \
    -e "s|your_loki_username|$loki_username|g" \
    -e "s|your_loki_password|$loki_password|g" \
    -e "s|your_otlp_url|$tempo_url|g" \
    -e "s|your_tempo_username|$tempo_username|g" \
    -e "s|your_tempo_password|$tempo_password|g" \
    -e "s|your_prometheus_url|$prometheus_url|g" \
    -e "s|your_prometheus_username|$prometheus_username|g" \
    -e "s|your_prometheus_password|$prometheus_password|g" \
    GrafanaAlloy/config.alloy.template > GrafanaAlloy/config.alloy

echo "Done! GrafanaAlloy/config.alloy is generated."
