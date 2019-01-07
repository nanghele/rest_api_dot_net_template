FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 52718
EXPOSE 44300

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MyAPI/MyAPI.csproj", "MyAPI/"]
RUN dotnet restore "MyAPI/MyAPI.csproj"
COPY . .
WORKDIR "/src/MyAPI"
RUN dotnet build "MyAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyAPI.dll"]