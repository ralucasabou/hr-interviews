FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HrInterviews/HrInterviews.csproj", "HrInterviews/"]

RUN dotnet restore "HrInterviews/HrInterviews.csproj"
COPY . .
WORKDIR "/src/HrInterviews"
RUN dotnet build "HrInterviews.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HrInterviews.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HrInterviews.dll"]