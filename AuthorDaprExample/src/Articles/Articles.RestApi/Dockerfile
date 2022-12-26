FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Articles/Articles.RestApi/Articles.RestApi.csproj", "src/Articles/Articles.RestApi/"]
RUN dotnet restore "src/Articles/Articles.RestApi/Articles.RestApi.csproj"
COPY . .
WORKDIR "/src/src/Articles/Articles.RestApi"
RUN dotnet build "Articles.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Articles.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Articles.RestApi.dll"]
