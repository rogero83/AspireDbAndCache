# .NET Aspire Project with PostgreSQL and Redis

# Aspire
Install .net Aspire CLI tool globally
```bash
dotnet tool install -g Aspire.Cli --prerelease
```

From the root of the project the following command run to discover and run the project:
```bash
aspire run
```

To create docker image and docker-compose file run:
```bash
aspire publish -o docker-file
```	

After edit .env file with password, run:
```bash
docker -compose up -d
```