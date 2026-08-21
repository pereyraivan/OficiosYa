/*
  Agrega FK PlanSuscripcionId en Suscripciones (normalización con PlanesSuscripcion).
  Ejecutá en la base OficiosYa_Database (o la que uses).

  Tras correr, los nuevos registros de suscripción guardan el Id del plan;
  el backfill opcional enlaza filas viejas por nombre en TipoSuscripcion.
*/

IF COL_LENGTH(N'dbo.Suscripciones', N'PlanSuscripcionId') IS NULL
BEGIN
    ALTER TABLE dbo.Suscripciones ADD PlanSuscripcionId INT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_Suscripciones_PlanesSuscripcion'
      AND parent_object_id = OBJECT_ID(N'dbo.Suscripciones', N'U')
)
BEGIN
    ALTER TABLE dbo.Suscripciones
    ADD CONSTRAINT FK_Suscripciones_PlanesSuscripcion
    FOREIGN KEY (PlanSuscripcionId) REFERENCES dbo.PlanesSuscripcion (Id);
END
GO

/* Opcional: enlazar suscripciones existentes por nombre de plan */
UPDATE s
SET PlanSuscripcionId = p.Id
FROM dbo.Suscripciones AS s
INNER JOIN dbo.PlanesSuscripcion AS p
  ON LOWER(LTRIM(RTRIM(s.TipoSuscripcion))) = LOWER(LTRIM(RTRIM(p.Nombre)))
WHERE s.PlanSuscripcionId IS NULL;
GO
