#!/usr/bin/env bash

# Start the users service in dapr mode.

dapr run -a users-api -p 5088 -d ./.dapr/components -- dotnet run --project src/Users/Users.Api