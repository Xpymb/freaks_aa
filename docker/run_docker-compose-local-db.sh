export DOCKER_BUILDKIT=0
docker-compose -f ./docker-compose-local-db.yml -p freaks up --build -d --remove-orphans