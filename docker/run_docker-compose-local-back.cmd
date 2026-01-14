set DOCKER_BUILDKIT=0
docker-compose -f ./docker-compose-back-base.yml build
docker-compose -f ./docker-compose-local-back.yml -p freaks-back up --build -d --remove-orphans
pause