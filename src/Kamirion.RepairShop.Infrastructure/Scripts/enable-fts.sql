-- Script de verificación y habilitación de Full-Text Search
-- Requiere SQL Server Developer Edition o superior (no Express)
-- La migración AddSearchIndex ya crea el catálogo y el FTS index automáticamente.
-- Este script es para verificación manual o setup en entornos existentes.

-- Verificar si FTS está instalado en la instancia
SELECT FULLTEXTSERVICEPROPERTY('IsFulltextInstalled') AS IsInstalled;

-- Crear catálogo si no existe
IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = 'RepairShopCatalog')
BEGIN
    CREATE FULLTEXT CATALOG RepairShopCatalog AS DEFAULT;
    PRINT 'Catálogo RepairShopCatalog creado.';
END
ELSE
BEGIN
    PRINT 'Catálogo RepairShopCatalog ya existe.';
END

-- Verificar estado del catálogo
SELECT name, is_default, population_status
FROM sys.fulltext_catalogs
WHERE name = 'RepairShopCatalog';

-- Verificar que el FTS index existe sobre SearchIndexEntries
SELECT
    t.name AS TableName,
    c.name AS ColumnName,
    fi.change_tracking_state_desc AS ChangeTracking
FROM sys.fulltext_indexes fi
JOIN sys.tables t ON fi.object_id = t.object_id
JOIN sys.fulltext_index_columns fic ON fic.object_id = fi.object_id
JOIN sys.columns c ON c.object_id = fic.object_id AND c.column_id = fic.column_id
WHERE t.name = 'SearchIndexEntries';
