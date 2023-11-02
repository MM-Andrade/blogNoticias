#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY "*.sln", "."
COPY ["BlogPetNews.API.csproj", "BlogPetNews.API/"]
COPY ["BlogPetNews.Unit.Tests/BlogPetNews.Unit.Tests.csproj", "BlogPetNews.Unit.Tests/"]
COPY ["BlogPetNews.Tests.Common/BlogPetNews.Tests.Common.csproj", "BlogPetNews.Tests.Common/"]

RUN dotnet restore "BlogPetNews.API/BlogPetNews.API.csproj"
RUN dotnet restore "BlogPetNews.Tests.Common/BlogPetNews.Unit.Tests.csproj"
RUN dotnet restore "BlogPetNews.Tests.Common/BlogPetNews.Tests.Common.csproj"
COPY . .

RUN dotnet build "BlogPetNews.API/BlogPetNews.API.csproj" -c Release -o /app 
RUN dotnet build "BlogPetNews.Tests.Common/BlogPetNews.Unit.Tests.csproj" -c Release -o /app 
RUN dotnet build "BlogPetNews.Tests.Common/BlogPetNews.Tests.Common.csproj" -c Release -o /app 

RUN dotnet test "BlogPetNews.Tests.Common/BlogPetNews.Unit.Tests.csproj" --logger "trx;LogFileName=webapplication1.trx" 


#publish the app
FROM build AS publish
RUN dotnet publish "BlogPetNews.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

#run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogPetNews.API.dll"]
