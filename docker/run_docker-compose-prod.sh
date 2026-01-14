docker compose -f ./docker-compose-back-base.yml build
docker compose -f ./docker-compose-prod.yml up --build -d --remove-orphans
pause