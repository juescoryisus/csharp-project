# ------------------------------------------------------------------
# Dockerfile para SQL Server 2022 (Developer Edition)
# ------------------------------------------------------------------
# Descarga la imagen oficial de Microsoft.
FROM mcr.microsoft.com/mssql/server:2022-latest

# Parametros de configuracion del motor.
# (Pueden sobreescribirse desde docker-compose.yml o el comando run.)
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer
ENV MSSQL_SA_PASSWORD=Passw0rd!2024

# Puerto por defecto de SQL Server.
EXPOSE 1433

# La imagen base ya define el ENTRYPOINT que arranca sqlservr.
