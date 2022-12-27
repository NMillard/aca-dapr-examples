#!/usr/bin/env bash

# Start the articles service in dapr mode.

dapr run -a articles-api -p 5102 -d ./.dapr/components -- dotnet run --project src/Articles/Articles.RestApi