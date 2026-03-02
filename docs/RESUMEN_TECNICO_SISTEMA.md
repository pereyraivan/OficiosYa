# Resumen técnico – OficiosYa

## Cómo funciona el sistema

- **Backend**: API ASP.NET Core (.NET 10) con Entity Framework Core y SQL Server.
- **Frontend**: Vue 3 + Vite; consume la API por `/api` (proxy a `localhost:5230`).
- **Flujo**: El usuario (o oficiante/profesional) se registra → el profesional elige categorías/servicios y zonas de cobertura → quien busca filtra por oficio/categoría y zona y ve listados; puede ver perfil y (cuando esté implementado) contactar o dejar reseña.

---

## Dónde se guarda cada cosa (tablas principales)

| Tabla | Qué guarda |
|-------|------------|
| **Usuarios** | Datos de login y perfil: nombre, apellido, email, password hash, teléfono, dirección, ciudad, provincia, si es admin. |
| **Profesionales** | Un perfil “profesional” por usuario: descripción, foto, calificación promedio, total reseñas, disponible, activo. (1 Usuario → 1 Profesional.) |
| **TipoServicios** | Tipos de servicio: ej. “Oficiante” (plomero, electricista…) y “Profesional” (abogado, contador…). |
| **CategoriaServicios** | Categorías dentro de cada tipo: Plomero, Electricista, Abogado, etc. Tiene Grupo, Icono, y referencia a TipoServicioId. |
| **ProfesionalCategorias** | Relación Profesional ↔ CategoriaServicio: qué servicios ofrece cada profesional (y si está activo). |
| **ProfesionalOficios** | Relación legacy Profesional ↔ Oficio (tabla Oficios). En la búsqueda se usa sobre todo CategoriaServicio. |
| **ZonasCobertura** | Zonas donde atiende cada profesional: localidad, provincia, departamento, lat/long, código postal. |
| **PlanesSuscripcion** | Planes (ej. Basica, Premium, Pro) con precios mensual/trimestral/semestral/anual y descuentos. |
| **Suscripciones** | Suscripción activa/vencida de cada profesional: plan, fechas, estado, método de pago, si está confirmado. |
| **Resenas** | Calificación (1–5) y comentario de un usuario sobre un profesional; se usa para promediar y ordenar. |
| **Certificados** | Certificados del profesional (formación, cursos, etc.). |
| **Oficios** | Catálogo legacy de oficios (nombre, icono, color); la búsqueda actual usa sobre todo CategoriaServicios. |

La distinción **Oficiante vs Profesional** en la UI viene del **TipoServicio** al que pertenecen las categorías que tiene el profesional (ProfesionalCategorias → CategoriaServicio → TipoServicioId).

---

## Qué ve un profesional / oficiante

- **Registro**: formulario en pasos (datos personales, luego categorías/servicios y zonas de cobertura).
- **Después de registrarse**: hoy no hay panel propio; en el futuro debería ver:
  - Su perfil público (descripción, categorías, zonas, calificación).
  - Sus suscripciones (plan, vencimiento, renovar).
  - Sus reseñas y (opcional) certificados.
  - Posibles solicitudes o mensajes de clientes (si se implementa contacto/solicitudes).

---

## Qué ve quien busca (cliente)

- **Inicio**: selector Oficiantes / Profesionales, buscador por oficio o categoría y por zona; listado de “destacados” (ordenados por plan y calificación).
- **Búsqueda**: resultados filtrados por oficio y zona (API `buscar` con `oficio`, `zona`, `tipo`), en tarjetas con nombre, descripción, calificación, cantidad de reseñas.
- **Perfil del profesional**: detalle por `GET /api/profesionales/{id}` (nombre, descripción, calificación, reseñas total, oficios/categorías).  
  **Falta**: vista de detalle en el frontend (página de perfil público), listado de reseñas, botón de contacto (teléfono/email/chat).

---

## Qué faltaría hacer

1. **Perfil público del profesional**  
   Vista en el frontend que muestre toda la info del profesional (descripción, categorías, zonas, calificación, reseñas) y un botón de contacto.

2. **Contacto**  
   Decidir cómo se contacta (mostrar teléfono/email, formulario de mensaje, chat). Si es mensaje, puede implicar tabla de Mensajes o Solicitudes y notificaciones.

3. **Reseñas**  
   - API: crear/listar reseñas (y opcionalmente moderación).  
   - Frontend: listar reseñas en el perfil del profesional; formulario para que un usuario registrado deje reseña después de un trabajo (y definir “después de un trabajo” si hay solicitudes/contrataciones).

4. **Panel del profesional**  
   Área (dashboard) para que el profesional vea y edite su perfil, sus zonas y categorías, suscripción, reseñas recibidas y, si aplica, solicitudes o mensajes.

5. **Pagos / suscripciones**  
   Flujo completo: elegir plan, método de pago (transferencia, tarjeta, Rapipago, etc.), confirmación y actualización de estado en **Suscripciones**; renovación y recordatorios.

6. **Login y permisos**  
   Ya existe registro; falta uso consistente de login (JWT o cookies) y proteger rutas: solo el dueño puede editar su perfil; solo usuarios registrados pueden dejar reseña; admin para gestionar oficios/categorías (ya hay vistas admin).

7. **Mejoras de búsqueda**  
   Filtros extra (precio, disponibilidad, más criterios de zona), orden por distancia si se usa geolocalización, y seguir unificando búsqueda por CategoriaServicios (y tipo Oficiante/Profesional) en backend y frontend.

8. **Certificados**  
   CRUD de certificados en el perfil del profesional y mostrarlos en el perfil público.

9. **Notificaciones**  
   Email o in-app cuando llega una solicitud, nueva reseña o vencimiento de suscripción.

---

## Recomendación: cómo seguir

Orden sugerido para que el producto sea usable de punta a punta sin depender de pagos ni lógica muy compleja al principio:

| Orden | Qué hacer | Por qué |
|-------|------------|--------|
| **1** | **Login y sesión** (JWT o cookies) | Sin login no hay “mi perfil”, “dejar reseña” ni “mi suscripción”. Es la base para todo lo que requiera “usuario identificado”. |
| **2** | **Página de perfil público** (vista detalle del profesional) | Cierra el flujo: buscar → ver resultados → **entrar al perfil** → ver info completa. Hoy la API devuelve el dato pero el frontend no tiene la vista. |
| **3** | **Contacto** (mostrar teléfono/email en el perfil, o botón “Contactar”) | El usuario que busca necesita poder contactar; es el cierre mínimo del ciclo “encontrar → contactar”. |
| **4** | **Panel del profesional** (editar perfil, ver suscripción, zonas, categorías) | El profesional tiene que poder mantener su perfil y ver el estado de su plan sin depender de admin. |
| **5** | **Reseñas** (API crear/listar + mostrar en perfil + formulario para dejar reseña) | Da contenido a la calificación y confianza a quien busca. Puede ser “cualquier usuario logueado deja reseña” al inicio; después se puede atar a “trabajo realizado” si agregás solicitudes. |
| **6** | **Pagos / flujo de suscripción** | Monetización: elegir plan, pagar, marcar suscripción como activa, renovar. Puede ir después de que el flujo buscar → perfil → contacto funcione bien. |

**Resumen:** Primero **login**, después **perfil público + contacto** para cerrar el flujo de quien busca; luego **panel del profesional** y **reseñas**; por último el **flujo de pago** de suscripciones. Certificados, notificaciones y mejoras de búsqueda pueden sumarse en cualquier momento según prioridad de negocio.

Este documento se puede mantener como referencia y actualizarse según se implementen estas partes.
