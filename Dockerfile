FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY [".", "./"]
RUN dotnet restore "TransactionService.Scheduler\TransactionService.Scheduler.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TransactionService.Scheduler\TransactionService.Scheduler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionService.Scheduler\TransactionService.Scheduler.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "TransactionService.Scheduler.dll"]
