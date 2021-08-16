FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["testPwnedPasswords/testPwnedPasswords.csproj", "testPwnedPasswords/"]
RUN dotnet restore "testPwnedPasswords/testPwnedPasswords.csproj"
COPY . .
WORKDIR "/src/testPwnedPasswords"
RUN dotnet build "testPwnedPasswords.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "testPwnedPasswords.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testPwnedPasswords.dll"]
