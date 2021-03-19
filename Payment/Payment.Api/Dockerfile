#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Payment/Payment.Api/Payment.Api.csproj", "Payment/Payment.Api/"]
RUN dotnet restore "Payment/Payment.Api/Payment.Api.csproj"
COPY . .
WORKDIR "/src/Payment/Payment.Api"
RUN dotnet build "Payment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Api.dll"]