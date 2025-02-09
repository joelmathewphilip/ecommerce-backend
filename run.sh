#to run mongo in a docker container
docker run -d -it -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=12345 --name mongo mongo

#run rabbit mq container
docker run -d -it -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management-alpine

#run postgres container
docker run -d -it -p 5431:5431 -p 5432:5432 -e POSTGRES_PASSWORD=12345 --name postgresdb postgres