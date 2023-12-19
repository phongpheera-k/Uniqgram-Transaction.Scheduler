# ใช้ .NET SDK เป็นฐานสำหรับการ build
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy โค้ดและรัน dotnet restore เพื่อติดตั้ง dependencies
COPY [".", "./"]
RUN dotnet restore "TransactionService.Scheduler\TransactionService.Scheduler.csproj"

# Copy โค้ดทั้งหมดและ build application
COPY . .
WORKDIR "/src/"
RUN dotnet build "TransactionService.Scheduler\TransactionService.Scheduler.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "TransactionService.Scheduler\TransactionService.Scheduler.csproj" -c Release -o /app/publish


# ใช้ .NET Runtime เป็นฐานสำหรับการรันแอปพลิเคชัน
FROM base AS final
WORKDIR /app

# Copy เฉพาะ output จากขั้นตอน build
COPY --from=publish /app/publish .

# รันแอปพลิเคชัน .NET Console
ENTRYPOINT ["dotnet", "TransactionService.Scheduler.dll"]

