FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

WORKDIR /src

COPY . .

RUN dotnet restore Identity.API.csproj
RUN dotnet publish Identity.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Identity.API.dll"]