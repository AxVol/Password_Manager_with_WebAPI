#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY ["WebApi/Keys/keys.txt", "Keys/"]	
EXPOSE 80
EXPOSE 443
EXPOSE 7125

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["WebApi.DAL/WebApi.DAL.csproj", "WebApi.DAL/"]
COPY ["WebApi.Domain/WebApi.Domain.csproj", "WebApi.Domain/"]
COPY ["WebApi.Service/WebApi.Service.csproj", "WebApi.Service/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]