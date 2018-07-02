FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY src/api/Titan.UFC.Users.WebAPI.csproj src/api/
RUN dotnet restore src/api/Titan.UFC.Users.WebAPI.csproj
COPY . .
WORKDIR /src/src/api
RUN dotnet build Titan.UFC.Users.WebAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Titan.UFC.Users.WebAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Titan.UFC.Users.WebAPI.dll"]
