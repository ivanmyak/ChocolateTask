
FROM bitnami/aspnet:8.0.8 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build
FROM bitnami/sdk:8.0.401 AS build
WORKDIR /src
COPY ChocolateTask.sln ./
COPY ChocolateAPI/ChocolateAPI.csproj ChocolateAPI/
RUN dotnet restore "ChocolateAPI/ChocolateAPI.csproj"
COPY . .
WORKDIR /src/ChocolateAPI
RUN dotnet publish "ChocolateAPI.csproj" -c Release -o /app/publish

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "ChocolateAPI.dll"]
