# Kamirion.RepairShop

[![CI](https://github.com/hesuaknight/Kamirion.RepairShop/actions/workflows/ci.yml/badge.svg)](https://github.com/hesuaknight/Kamirion.RepairShop/actions/workflows/ci.yml)

Sistema de gestión de taller de reparación de dispositivos.

Stack: ASP.NET Core 10.0 · SQL Server · Hangfire · Serilog  
Arquitectura: Modular Monolith + Clean Architecture

## Levantar entorno local con Docker

1. Copiar el archivo de variables de entorno:
   ```bash
   cp .env.example .env
   ```
2. Editar `.env` con una contraseña segura para `SA_PASSWORD`.
3. Levantar todos los servicios:
   ```bash
   docker compose up
   ```
4. La aplicación estará disponible en `http://localhost:8080`.
5. SQL Server estará accesible en `localhost:1433`.

> El archivo `.env` está ignorado por git. Nunca commitear credenciales.

## Full-Text Search

El módulo de búsqueda usa SQL Server Full-Text Search (FTS).

> **SQL Server Express no soporta FTS.** Se requiere Developer Edition o superior.

El `docker-compose.yml` usa `MSSQL_PID=Developer` para habilitarlo automáticamente en el entorno local. La migración `AddSearchIndex` crea el catálogo `RepairShopCatalog` y el full-text index sobre `SearchIndexEntries.SearchableText` al iniciar la aplicación.

Para verificar el estado del catálogo en un entorno existente, ejecutar el script `src/Kamirion.RepairShop.Infrastructure/Scripts/enable-fts.sql`.
