FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src
COPY Inventory.Api.csproj .
RUN dotnet restore
COPY . .
CMD ["dotnet", "watch", "run", "--no-launch-profile"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Inventory.Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Inventory.Api.dll"]
