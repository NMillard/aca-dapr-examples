# Run locally
Have a running Postgres instance.  
`docker run --name basic-postgres --rm -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 25432:5432 -d postgres:15-alpine`


## Articles API
Build the articles API container using:  
`docker build -f src/Articles/Articles.RestApi/Dockerfile -t articles-api .`


## Dapr
Run application with dapr sidecar.  
`dapr run -a articles-api -p 5102 -d ./.dapr/components -- dotnet run --project src/Articles/Articles.RestApi`