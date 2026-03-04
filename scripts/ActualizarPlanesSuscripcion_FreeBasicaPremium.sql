-- =============================================================================
-- Script: Actualizar tabla PlanesSuscripcion con planes Free, Básica y Premium
-- Base de datos: OficiosYa_Database
-- =============================================================================
-- Planes:
--   FREE:    Gratis. 2 contactos/mes. Ver empleos (no postularse).
--   BÁSICA:  7000/mes - 18000/3 meses (ahorro ~14,29%). 15 contactos/mes. Ver empleos (no postularse).
--   PREMIUM: 12000/mes - 30000/3 meses (ahorro ~16,67%). Contactos ilimitados. Acceso a empleos.
-- =============================================================================

USE [OficiosYa_Database];
GO

-- Desactivar plan "Pro" si existe (solo usamos Free, Básica, Premium)
UPDATE [dbo].[PlanesSuscripcion]
SET [Activo] = 0
WHERE [Nombre] = N'Pro';
GO

-- Actualizar plan Básica (precios y ahorro 3 meses)
-- Precio 3 meses sin oferta: 7000 * 3 = 21000. Con plan: 18000. Ahorro: (21000-18000)/21000 = 14,29%
UPDATE [dbo].[PlanesSuscripcion]
SET
  [Descripcion] = N'15 contactos mensuales. Podés ver empleos; para postularte necesitás plan Premium.',
  [PrecioMensual] = 7000.00,
  [PrecioTrimestral] = 18000.00,
  [PrecioSemestral] = 36000.00,   -- 2 x trimestral
  [PrecioAnual] = 72000.00,       -- 4 x trimestral
  [DescuentoTrimestral] = 14.29,  -- Ahorro contratando 3 meses
  [DescuentoSemestral] = 0.00,
  [DescuentoAnual] = 0.00,
  [Activo] = 1
WHERE [Nombre] = N'Basica';
GO

-- Actualizar plan Premium (precios y ahorro 3 meses)
-- Precio 3 meses sin oferta: 12000 * 3 = 36000. Con plan: 30000. Ahorro: (36000-30000)/36000 = 16,67%
UPDATE [dbo].[PlanesSuscripcion]
SET
  [Descripcion] = N'Contactos ilimitados y acceso a empleos (postularte a trabajos publicados).',
  [PrecioMensual] = 12000.00,
  [PrecioTrimestral] = 30000.00,
  [PrecioSemestral] = 60000.00,   -- 2 x trimestral
  [PrecioAnual] = 120000.00,     -- 4 x trimestral
  [DescuentoTrimestral] = 16.67,  -- Ahorro contratando 3 meses
  [DescuentoSemestral] = 0.00,
  [DescuentoAnual] = 0.00,
  [Activo] = 1
WHERE [Nombre] = N'Premium';
GO

-- Insertar plan Free si no existe
IF NOT EXISTS (SELECT 1 FROM [dbo].[PlanesSuscripcion] WHERE [Nombre] = N'Free')
BEGIN
  INSERT INTO [dbo].[PlanesSuscripcion]
    ( [Nombre], [Descripcion], [PrecioMensual], [PrecioTrimestral], [PrecioSemestral], [PrecioAnual],
      [DescuentoTrimestral], [DescuentoSemestral], [DescuentoAnual], [Activo], [FechaCreacion] )
  VALUES
    ( N'Free', N'2 contactos por mes. Podés ver empleos; para postularte necesitás plan Premium.', 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 1, GETUTCDATE() );
  PRINT 'Plan Free insertado.';
END
ELSE
  PRINT 'Plan Free ya existe.';
GO

-- Resumen: mostrar planes activos con ahorro en % (3 meses)
SELECT
  [Nombre],
  [PrecioMensual],
  [PrecioTrimestral],
  [DescuentoTrimestral] AS AhorroPorcTrimestral,
  [Activo]
FROM [dbo].[PlanesSuscripcion]
WHERE [Activo] = 1
ORDER BY CASE [Nombre] WHEN N'Free' THEN 1 WHEN N'Basica' THEN 2 WHEN N'Premium' THEN 3 ELSE 4 END;
GO

PRINT 'Script finalizado. Planes: Free (gratis), Básica (7000/mes - 18000/3 meses, ~14% ahorro), Premium (12000/mes - 30000/3 meses, ~17% ahorro).';
GO
