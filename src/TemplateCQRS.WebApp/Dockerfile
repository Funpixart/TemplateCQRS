#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TemplateCQRS.WebApp/TemplateCQRS.WebApp.csproj", "src/TemplateCQRS.WebApp/"]
COPY ["src/TemplateCQRS.Application/TemplateCQRS.Application.csproj", "src/TemplateCQRS.Application/"]
COPY ["src/TemplateCQRS.Domain/TemplateCQRS.Domain.csproj", "src/TemplateCQRS.Domain/"]
COPY ["src/TemplateCQRS.Infrastructure/TemplateCQRS.Infrastructure.csproj", "src/TemplateCQRS.Infrastructure/"]
RUN dotnet restore "src/TemplateCQRS.WebApp/TemplateCQRS.WebApp.csproj"
COPY . .
WORKDIR "/src/src/TemplateCQRS.WebApp"
RUN dotnet build "TemplateCQRS.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TemplateCQRS.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TemplateCQRS.WebApp.dll"]