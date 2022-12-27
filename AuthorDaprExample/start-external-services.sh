#!/usr/bin/env bash

# Stop containers if they already exist and
# start postgres and rabbitMQ containers

docker stop basic-postgres
docker stop basic-rabbitmq

docker run --name basic-postgres --rm -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:15-alpine
docker run --name basic-rabbitmq --rm -p 5672:5672 -p 15673:15672 -d rabbitmq:3-management-alpine