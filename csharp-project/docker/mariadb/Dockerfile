# ------------------------------------------------------------------
# Dockerfile para MariaDB 11
# ------------------------------------------------------------------
# Descarga la imagen oficial de MariaDB.
FROM mariadb:11

# Parametros de configuracion del motor.
# (Pueden sobreescribirse desde docker-compose.yml o el comando run.)
ENV MARIADB_ROOT_PASSWORD=Passw0rd!2024
ENV MARIADB_DATABASE=TiendaDB
ENV MARIADB_USER=appuser
ENV MARIADB_PASSWORD=Passw0rd!2024

# Los archivos .sql copiados a este directorio se ejecutan
# automaticamente la primera vez que arranca el contenedor.
COPY ./init/ /docker-entrypoint-initdb.d/

# Puerto por defecto de MariaDB / MySQL.
EXPOSE 3306
