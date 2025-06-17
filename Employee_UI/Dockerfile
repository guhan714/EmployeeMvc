FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Employee_UI/Employee.UI.csproj", "Employee_UI/"]
COPY ["Employee.DataAccess/Employee.DataAccess.csproj", "Employee.DataAccess/"]
COPY ["Employee.BusinessLogic/Employee.BusinessLogic.csproj", "Employee.BusinessLogic/"]
COPY ["Employee.Domain/Employee.Domain.csproj", "Employee.Domain/"]
RUN dotnet restore "Employee_UI/Employee.UI.csproj"
COPY . .
WORKDIR "/src/Employee_UI"
RUN dotnet build "Employee.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Employee.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.UI.dll"]
