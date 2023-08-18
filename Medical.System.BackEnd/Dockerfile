#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Medical.System.BackEnd/Medical.System.BackEnd.csproj", "Medical.System.BackEnd/"]
RUN dotnet restore "Medical.System.BackEnd/Medical.System.BackEnd.csproj"
COPY . .
WORKDIR "/src/Medical.System.BackEnd"
RUN dotnet build "Medical.System.BackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Medical.System.BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Medical.System.BackEnd.dll"]