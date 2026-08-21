/*
  Opcional: evita códigos de referencia duplicados en Suscripciones.CodigoPago.
  Ejecutá en la base OficiosYa (SQL Server) si querés reforzar unicidad a nivel BD.

  Si ya tenés filas con CodigoPago NULL o repetido, corregí antes o el CREATE puede fallar.

  Nota: el predicado del índice filtrado no puede usar LTRIM/RTRIM (error 10735).
*/

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'UX_Suscripciones_CodigoPago_NonEmpty'
      AND object_id = OBJECT_ID(N'dbo.Suscripciones', N'U')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_Suscripciones_CodigoPago_NonEmpty
    ON dbo.Suscripciones (CodigoPago)
    WHERE CodigoPago IS NOT NULL AND CodigoPago <> N'';
END
GO
