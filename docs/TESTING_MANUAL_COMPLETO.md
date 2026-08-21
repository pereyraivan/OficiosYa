# OficiosYa - Testing Manual Completo (E2E)

Este documento define un plan de testing manual integral del sistema, con foco funcional, validaciones de negocio y verificacion en base de datos.

## 1) Objetivo

Validar que el flujo completo de OficiosYa funcione de punta a punta:

- Registro e inicio de sesion.
- Registro de profesional/oficiante (categorias, zonas, plan).
- Busqueda de profesionales y vista de perfil.
- Contacto y limites por plan.
- Reseñas.
- Empleos (publicar y buscar).
- Panel profesional.
- Consistencia de datos en BD.

## 2) Precondiciones

- Backend levantado (API).
- Frontend levantado (Vue).
- Base de datos accesible.
- Migraciones aplicadas.
- Datos base cargados (`TipoServicios`, `CategoriaServicios`, `PlanesSuscripcion`).

SQL rapido de sanidad inicial:

```sql
SELECT COUNT(*) AS TiposServicio FROM TipoServicios;
SELECT COUNT(*) AS CategoriasServicio FROM CategoriaServicios;
SELECT COUNT(*) AS PlanesActivos FROM PlanesSuscripcion WHERE Activo = 1;
SELECT TOP 20 * FROM __EFMigrationsHistory ORDER BY MigrationId DESC;
```

## 3) Usuarios de prueba sugeridos

Crear al menos:

- `cliente1@test.com` (rol cliente).
- `pro_free@test.com` (profesional plan Free).
- `pro_basica@test.com` (profesional plan Basica).
- `pro_premium@test.com` (profesional plan Premium).
- `admin@test.com` (usuario admin).

## 4) Tablas clave a monitorear

- `Usuarios`
- `Profesionales`
- `TipoServicios`
- `CategoriaServicios`
- `ProfesionalCategorias`
- `ZonasCobertura`
- `PlanesSuscripcion`
- `Suscripciones`
- `ContactosProfesional`
- `Resenas`
- `TrabajosPublicados`
- `TrabajoCategorias`
- `ImagenesTrabajo`
- `Certificados`
- `__EFMigrationsHistory`

## 5) Casos de prueba por modulo

## 5.1 Autenticacion y cuenta

### TC-AUTH-01 Registro de usuario cliente
- **Pasos**: ir a registro -> completar datos validos -> enviar.
- **Esperado**: alta exitosa; puede iniciar sesion.
- **BD revisar**:
  - `Usuarios`: nuevo registro con `Email` unico.
  - `Profesionales`: no debe crear perfil profesional para cliente puro.

```sql
SELECT * FROM Usuarios WHERE Email = 'cliente1@test.com';
SELECT * FROM Profesionales WHERE UsuarioId = (SELECT Id FROM Usuarios WHERE Email='cliente1@test.com');
```

### TC-AUTH-02 Validacion de email duplicado
- **Pasos**: intentar registrar con email existente.
- **Esperado**: mensaje de email ya registrado.
- **BD revisar**: no se crea nuevo usuario duplicado.

### TC-AUTH-03 Login correcto e incorrecto
- **Pasos**: login con credenciales validas y luego invalidas.
- **Esperado**: acceso en valido; error en invalido.
- **BD revisar**: sin cambios estructurales (solo lectura de credenciales).

## 5.2 Registro profesional/oficiante (wizard)

### TC-REGPRO-01 Registro profesional completo
- **Pasos**:
  1. Completar datos personales.
  2. Elegir tipo (Oficiante/Profesional).
  3. Elegir varias categorias.
  4. Elegir zonas de cobertura.
  5. Elegir plan, metodo de pago y finalizar.
- **Esperado**: alta de usuario y perfil profesional.
- **BD revisar**:
  - `Usuarios`: alta.
  - `Profesionales`: alta asociada al usuario.
  - `ProfesionalCategorias`: categorias activas.
  - `ZonasCobertura`: zonas guardadas.
  - `Suscripciones`: registro creado (estado y fechas).

```sql
SELECT u.Id, u.Email, p.Id AS ProfesionalId
FROM Usuarios u
LEFT JOIN Profesionales p ON p.UsuarioId = u.Id
WHERE u.Email = 'pro_free@test.com';

SELECT * FROM ProfesionalCategorias WHERE ProfesionalId = <ProfesionalId>;
SELECT * FROM ZonasCobertura WHERE ProfesionalId = <ProfesionalId> AND Activo = 1;
SELECT * FROM Suscripciones WHERE ProfesionalId = <ProfesionalId> ORDER BY FechaRegistro DESC;
```

### TC-REGPRO-02 Regla de zonas para plan Free
- **Pasos**:
  1. En paso 2 seleccionar 2+ zonas.
  2. En paso 3 seleccionar plan Free.
  3. Intentar continuar/finalizar.
- **Esperado**:
  - Se muestra mensaje de advertencia.
  - No permite continuar hasta dejar solo 1 zona.
- **BD revisar**: al finalizar en Free, `ZonasCobertura` activas <= 1.

### TC-REGPRO-03 Plan Basica/Premium con multiples zonas
- **Pasos**: repetir caso anterior pero elegir Basica/Premium.
- **Esperado**: permite multiples zonas.
- **BD revisar**: `ZonasCobertura` activas > 1.

## 5.3 Planes y suscripciones

### TC-PLAN-01 Visualizacion de planes
- **Pasos**: abrir pagina Planes y dropdown Planes del nav.
- **Esperado**: muestra planes activos y descripcion de alcance (contactos, zonas, empleos).
- **BD revisar**: datos coherentes con `PlanesSuscripcion`.

```sql
SELECT Nombre, Descripcion, PrecioMensual, PrecioTrimestral, PrecioSemestral, PrecioAnual, Activo
FROM PlanesSuscripcion
ORDER BY PrecioMensual;
```

### TC-PLAN-02 Vencimiento de suscripcion
- **Pasos**: usar usuario con suscripcion vencida.
- **Esperado**: el sistema lo trata como Free.
- **BD revisar**:
  - `Suscripciones`: `FechaVencimiento` pasada.
  - Estado efectivo en API/panel equivalente a Free.

```sql
SELECT TOP 5 *
FROM Suscripciones
WHERE ProfesionalId = <ProfesionalId>
ORDER BY FechaVencimiento DESC;
```

## 5.4 Busqueda de profesionales

### TC-BUSQ-01 Filtrar por tipo y oficio/categoria
- **Pasos**: buscar por tipo oficiante/profesional + categoria.
- **Esperado**: resultados correctos y relevantes.
- **BD revisar**:
  - `ProfesionalCategorias` enlazada a `CategoriaServicios`.
  - `TipoServicios` correcto para esa categoria.

```sql
SELECT pc.ProfesionalId, cs.Nombre AS Categoria, ts.Nombre AS Tipo
FROM ProfesionalCategorias pc
JOIN CategoriaServicios cs ON cs.Id = pc.CategoriaServicioId
JOIN TipoServicios ts ON ts.Id = cs.TipoServicioId
WHERE pc.Activo = 1;
```

### TC-BUSQ-02 Filtrar por zona
- **Pasos**: buscar por localidad/provincia.
- **Esperado**: devuelve profesionales con cobertura en esa zona.
- **BD revisar**: `ZonasCobertura` activas.

## 5.5 Perfil profesional publico y contacto

### TC-PERF-01 Ver perfil publico
- **Pasos**: abrir perfil de un profesional desde resultados.
- **Esperado**: muestra nombre, categorias, zonas, descripcion, imagenes, estado contacto.
- **BD revisar**:
  - `Profesionales`
  - `ProfesionalCategorias`
  - `ZonasCobertura`
  - `ImagenesTrabajo`

### TC-PERF-02 Registrar contacto y limites
- **Pasos**: click en contactar (WhatsApp/email) varias veces.
- **Esperado**:
  - Se registra cada contacto.
  - Al alcanzar limite del plan, bloquea y muestra mensaje.
- **BD revisar**: `ContactosProfesional`.

```sql
SELECT ProfesionalId, COUNT(*) AS ContactosMes
FROM ContactosProfesional
WHERE FechaHora >= DATEFROMPARTS(YEAR(GETUTCDATE()), MONTH(GETUTCDATE()), 1)
GROUP BY ProfesionalId
ORDER BY ContactosMes DESC;
```

## 5.6 Reseñas

### TC-RES-01 Crear reseña como visitante/logueado
- **Pasos**: publicar reseña valida (1..5 y comentario).
- **Esperado**: reseña guardada y visible.
- **BD revisar**:
  - `Resenas`: nuevo registro.
  - `Profesionales`: `CalificacionPromedio` y `TotalResenas` actualizados (si aplica en flujo actual).

```sql
SELECT TOP 20 * FROM Resenas WHERE ProfesionalId = <ProfesionalId> ORDER BY FechaCreacion DESC;
SELECT Id, CalificacionPromedio, TotalResenas FROM Profesionales WHERE Id = <ProfesionalId>;
```

### TC-RES-02 Validaciones
- **Casos**:
  - Calificacion fuera de rango.
  - Comentario vacio o > 1000 chars.
  - Intentar reseñarse a si mismo.
- **Esperado**: rechazo con mensaje correcto; sin insercion en BD.

## 5.7 Panel profesional

### TC-PANEL-01 Editar perfil, categorias y zonas
- **Pasos**: ingresar a `mi-panel`, modificar descripcion/categorias/zonas y guardar.
- **Esperado**: persistencia de cambios.
- **BD revisar**:
  - `Profesionales` (descripcion/disponible/foto).
  - `ProfesionalCategorias` (activos/inactivos).
  - `ZonasCobertura` (activos/inactivos).

### TC-PANEL-02 Restriccion de zonas por plan en panel
- **Pasos**: con plan Free intentar agregar >1 zona.
- **Esperado**: bloqueo por limite.
- **BD revisar**: `ZonasCobertura` activas <= 1 para ese profesional.

## 5.8 Empleos

### TC-EMP-01 Publicar empleo
- **Pasos**: crear empleo con datos validos y categorias requeridas.
- **Esperado**: empleo visible en listado/detalle.
- **BD revisar**:
  - `TrabajosPublicados`
  - `TrabajoCategorias`

```sql
SELECT TOP 20 * FROM TrabajosPublicados ORDER BY FechaPublicacion DESC;
SELECT TOP 50 * FROM TrabajoCategorias ORDER BY Id DESC;
```

### TC-EMP-02 Filtros de busqueda de empleos
- **Pasos**: aplicar filtros por categoria/zona/estado.
- **Esperado**: resultados acordes.
- **BD revisar**: consistencia con datos publicados.

## 5.9 Admin (oficios/categorias)

### TC-ADM-01 Gestion de categorias
- **Pasos**: crear/editar/desactivar categoria desde admin.
- **Esperado**:
  - Persiste cambios.
  - Se refleja en registro profesional y busquedas.
- **BD revisar**: `CategoriaServicios` y FK con `TipoServicios`.

### TC-ADM-02 Seguridad de rutas admin
- **Pasos**: intentar entrar a rutas admin sin login y con usuario no admin.
- **Esperado**: redireccion o bloqueo por permisos.

## 5.10 Archivos e imagenes

### TC-FILE-01 Foto de perfil e imagenes de trabajo
- **Pasos**: subir imagen valida y no valida.
- **Esperado**: acepta formatos permitidos y rechaza lo no permitido.
- **BD revisar**:
  - `Profesionales.FotoPerfil`
  - `ImagenesTrabajo`

### TC-FILE-02 Certificados
- **Pasos**: subir/gestionar certificados (si el flujo actual lo persiste en backend).
- **BD revisar**: `Certificados`.

## 6) Pruebas no funcionales minimas

- **Responsive**: Home, Login, Registro Profesional, Panel, Planes, Perfil.
- **Performance basica**: tiempos de carga de listados y busquedas.
- **Mensajes de error**: claros, accionables y sin stack trace al usuario.
- **Seguridad basica**:
  - Sin SQL injection en filtros de busqueda.
  - Rutas protegidas por rol/autenticacion.
  - No exponer datos de contacto cuando aplica limite de plan.

## 7) Matriz de regresion recomendada (por release)

Ejecutar siempre:

1. Registro + login cliente.
2. Registro profesional (Free y Premium).
3. Restriccion de zonas Free.
4. Busqueda por categoria y zona.
5. Perfil + contacto + limite mensual.
6. Crear reseña + ver promedio.
7. Publicar empleo + listar/detalle.
8. Panel profesional (guardar categorias/zonas).
9. Validacion de permisos admin.

## 8) Criterio de salida (Definition of Done de QA manual)

Se considera aprobado cuando:

- 100% de casos criticos y altos en verde.
- Sin errores bloqueantes (P0/P1).
- Sin inconsistencias de datos en BD en tablas clave.
- Evidencia capturada (screenshots + queries + resultados).

## 9) Plantilla de evidencia por caso

- **Caso**: TC-XXX
- **Build/fecha**:
- **Tester**:
- **Datos usados**:
- **Resultado esperado**:
- **Resultado obtenido**:
- **SQL de verificacion**:
- **Estado**: OK / FAIL
- **Observaciones**:

---

Si queres, en una segunda version te armo este mismo plan en formato "hoja de ejecucion" (checkbox por caso y columna PASS/FAIL) para que lo uses directamente en cada ciclo de prueba.
