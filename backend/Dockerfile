FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /App

COPY . ./

RUN dotnet restore "backend/Choose-Your-Own-Adventure.csproj"

RUN dotnet publish "backend/Choose-Your-Own-Adventure.csproj" -o out


FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /App

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_HTTPS_PORTS=8001
ENV ASPNETCORE_HTTP_PORT=8000
ENV ASPNETCORE_URLS=http://+:8000

COPY --from=build /App/out .

ENTRYPOINT ["dotnet", "Choose-Your-Own-Adventure.dll"]