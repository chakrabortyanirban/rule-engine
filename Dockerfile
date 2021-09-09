FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /home/app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . /
#RUN ls -la /
RUN dotnet restore "/src/RuleEngine/RuleEngine.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "/src/RuleEngine/RuleEngine.csproj" -c Release -o /home/app/build

FROM build AS publish
RUN dotnet publish "/src/RuleEngine/RuleEngine.csproj" -c Release -o /home/app/publish

FROM base AS final
WORKDIR /home/app
COPY --from=publish /home/app/publish .

ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RuleEngine.dll"]