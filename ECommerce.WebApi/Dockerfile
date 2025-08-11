
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["ECommerce.WebApi/ECommerce.WebApi.csproj", "ECommerce.WebApi/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]


RUN dotnet restore "ECommerce.WebApi/ECommerce.WebApi.csproj"


COPY . .

WORKDIR /src/ECommerce.WebApi
RUN dotnet publish "ECommerce.WebApi.csproj" -c Release -o /app /p:UseAppHost=false



FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .


ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.WebApi.dll"]
