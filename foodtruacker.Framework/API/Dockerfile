#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["foodtruacker.Framework/API/foodtruacker.API.csproj", "foodtruacker.Framework/API/"]
COPY ["foodtruacker.Framework/IdentityRepository/foodtruacker.IdentityRepository.csproj", "foodtruacker.Framework/IdentityRepository/"]
COPY ["foodtruacker.Application/foodtruacker.Application.csproj", "foodtruacker.Application/"]
COPY ["foodtruacker.Framework/EventsourcingRepository/foodtruacker.EventSourcingRepository.csproj", "foodtruacker.Framework/EventsourcingRepository/"]
COPY ["foodtruacker.SharedKernel/foodtruacker.SharedKernel.csproj", "foodtruacker.SharedKernel/"]
COPY ["foodtruacker.Domain/foodtruacker.Domain.csproj", "foodtruacker.Domain/"]
COPY ["foodtruacker.Framework/QueryRepository/foodtruacker.QueryRepository.csproj", "foodtruacker.Framework/QueryRepository/"]
RUN dotnet restore "foodtruacker.Framework/API/foodtruacker.API.csproj"
COPY . .
WORKDIR "/src/foodtruacker.Framework/API"
RUN dotnet build "foodtruacker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "foodtruacker.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "foodtruacker.API.dll"]