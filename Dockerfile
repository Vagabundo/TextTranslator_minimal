FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ENV ASPNETCORE_URLS=http://+:5000
# ENV ASPNETCORE_HTTP_PORTS=5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["/src", "."]
RUN dotnet restore "MinimalTranslator.Api/MinimalTranslator.Api.csproj"
COPY . .
WORKDIR "/src/MinimalTranslator.Api"
RUN dotnet build "MinimalTranslator.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MinimalTranslator.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalTranslator.Api.dll"]
