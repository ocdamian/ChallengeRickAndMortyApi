
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln ./
COPY ChallengeRickAndMortyApi.Application/*.csproj ChallengeRickAndMortyApi.Application/
COPY ChallengeRickAndMortyApi.Domain/*.csproj ChallengeRickAndMortyApi.Domain/
COPY ChallengeRickAndMortyApi.Infrastructure/*.csproj ChallengeRickAndMortyApi.Infrastructure/
COPY ChallengeRickAndMortyApi/*.csproj ChallengeRickAndMortyApi/
COPY ChallengeRickAndMortyApi.Tests/*.csproj ChallengeRickAndMortyApi.Tests/

RUN dotnet restore

COPY . .
RUN dotnet publish ChallengeRickAndMortyApi/ChallengeRickAndMortyApi.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /out .

EXPOSE 8090

ENTRYPOINT ["dotnet", "ChallengeRickAndMortyApi.dll"]