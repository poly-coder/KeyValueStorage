#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["samples/KeyValueStorage.SampleApp/KeyValueStorage.SampleApp.csproj", "samples/KeyValueStorage.SampleApp/"]
RUN dotnet restore "samples/KeyValueStorage.SampleApp/KeyValueStorage.SampleApp.csproj"
COPY . .
WORKDIR "/src/samples/KeyValueStorage.SampleApp"
RUN dotnet build "KeyValueStorage.SampleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeyValueStorage.SampleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeyValueStorage.SampleApp.dll"]