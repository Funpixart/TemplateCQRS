#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/TemplateCQRS.Api/TemplateCQRS.Api.csproj", "src/TemplateCQRS.Api/"]
COPY ["src/TemplateCQRS.Application/TemplateCQRS.Application.csproj", "src/TemplateCQRS.Application/"]
COPY ["src/TemplateCQRS.Domain/TemplateCQRS.Domain.csproj", "src/TemplateCQRS.Domain/"]
COPY ["src/TemplateCQRS.Infrastructure/TemplateCQRS.Infrastructure.csproj", "src/TemplateCQRS.Infrastructure/"]
RUN dotnet restore "src/TemplateCQRS.Api/TemplateCQRS.Api.csproj"
COPY . .
WORKDIR "/src/src/TemplateCQRS.Api"
RUN dotnet build "TemplateCQRS.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TemplateCQRS.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TemplateCQRS.Api.dll"]