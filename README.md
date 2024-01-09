# Uniqgram-TransactionService.Scheduler
scheduler for transaction service

# How to run on Docker
type this follow command
1. docker build -t transaction-service-scheduler:{version} .
2. docker tag transaction-service-scheduler:{version} transaction-service-scheduler:latest
3. docker run -d -e TZ=Asia/Bangkok --name transaction-service-scheduler transaction-service-scheduler:{version}
