/*
  Deprecación tablas legacy Oficios / ProfesionalOficios
  Ejecutar en SSMS solo cuando confirmes que no las necesitás.
  La app ya usa CategoriaServicios + ProfesionalCategorias.

  Estado típico: ProfesionalOficios vacía (0 filas).
*/

USE OficiosYa_Database;
GO

-- Verificar antes de borrar
SELECT COUNT(*) AS FilasProfesionalOficios FROM ProfesionalOficios;
SELECT COUNT(*) AS FilasOficios FROM Oficios;
GO

/*
-- Descomentar para eliminar tablas legacy:

IF OBJECT_ID(N'dbo.ProfesionalOficios', N'U') IS NOT NULL
    DROP TABLE dbo.ProfesionalOficios;
GO

IF OBJECT_ID(N'dbo.Oficios', N'U') IS NOT NULL
    DROP TABLE dbo.Oficios;
GO
*/
