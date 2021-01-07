#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["QuickFrame.Web/QuickFrame.Web.csproj", "QuickFrame.Web/"]
COPY ["QuickFrame.Controllers/QuickFrame.Controllers.csproj", "QuickFrame.Controllers/"]
COPY ["QuickFrame.IServices/QuickFrame.IServices.csproj", "QuickFrame.IServices/"]
COPY ["QuickFrame.Models/QuickFrame.Models.csproj", "QuickFrame.Models/"]
COPY ["QuickFrame.Common/QuickFrame.Common.csproj", "QuickFrame.Common/"]
COPY ["QuickFrame.Services/QuickFrame.Services.csproj", "QuickFrame.Services/"]
COPY ["QuickFrame.Repositorys/QuickFrame.Repositorys.csproj", "QuickFrame.Repositorys/"]
COPY ["QuickFrame.Extensions/QuickFrame.Extensions.csproj", "QuickFrame.Extensions/"]
COPY ["QuickFrame.EventBus/QuickFrame.EventBus.csproj", "QuickFrame.EventBus/"]
RUN dotnet restore "QuickFrame.Web/QuickFrame.Web.csproj"
COPY . .
WORKDIR "/src/QuickFrame.Web"
RUN dotnet build "QuickFrame.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuickFrame.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickFrame.Web.dll"]