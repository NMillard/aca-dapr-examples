# External services
Required services to run locally:
- Postgres
- RabbitMQ

### Creating service docker containers
Postgres:  
`docker run --name basic-postgres --rm -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:15-alpine`

RabbitMQ:
`docker run --name basic-rabbitmq --rm -p 5672:5672 -p 15673:15672 -d rabbitmq:3-management-alpine`

## Running locally

## Docker compose with dapr sidecars
Run `docker-compose up`.  
This creates the required external services, builds the docker container images, and runs them.

Make sure the external services are configured correctly, e.g. ensure the postgres and rabbitmq ports are mapped to what's defined in the dapr [components files](.dapr/components). 

## Dapr
Remember to have the required services running before.  

Run application with dapr sidecar.  
Articles API:  
`dapr run -a articles-api -p 5102 -d ./.dapr/components -- dotnet run --project src/Articles/Articles.RestApi`

Users API:  
`dapr run -a users-api -p 5088 -d ./.dapr/components -- dotnet run --project src/Users/Users.Api`

## Building images

### Articles API
Build the articles API container using:  
`docker build -f src/Articles/Articles.RestApi/Dockerfile -t articles-api .`


## Notes
The applications have an appsetting `UseDapr`, and EF core is having some trouble with creating migrations if this setting is set to `true`. To create migrations the `UseDapr` must be `false`.