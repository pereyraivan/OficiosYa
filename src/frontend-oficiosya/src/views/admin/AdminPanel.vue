<template>
  <div class="container mt-4 mb-5">
    <h2 class="mb-2">Administración</h2>
    <p class="text-muted mb-4">Gestioná contenido y pagos de suscripciones.</p>

    <div class="row g-3">
      <div class="col-md-6">
        <router-link to="/admin/profesionales" class="admin-card admin-card-highlight">
          <span class="admin-card-icon">👥</span>
          <h3>Profesionales y oficiantes</h3>
          <p>Listado de todos los perfiles registrados en la plataforma.</p>
        </router-link>
      </div>
      <div class="col-md-6">
        <router-link to="/admin/suscripciones" class="admin-card">
          <span class="admin-card-icon">💳</span>
          <h3>Pagos pendientes</h3>
          <p>Confirmá transferencias y activá planes de profesionales.</p>
        </router-link>
      </div>
      <div class="col-md-6">
        <router-link to="/admin/categorias" class="admin-card">
          <span class="admin-card-icon">📋</span>
          <h3>Categorías</h3>
          <p>Gestioná categorías de servicio (Plomero, Electricista, etc.).</p>
        </router-link>
      </div>
    </div>
  </div>
</template>

<style scoped>
.admin-card {
  display: block;
  height: 100%;
  padding: 1.5rem;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  background: #fff;
  text-decoration: none;
  color: inherit;
  transition: box-shadow 0.2s, border-color 0.2s;
}

.admin-card:hover {
  border-color: #22c55e;
  box-shadow: 0 4px 16px rgba(34, 197, 94, 0.12);
  color: inherit;
}

.admin-card-highlight {
  border-color: #bbf7d0;
  background: linear-gradient(135deg, #f0fdf4 0%, #fff 100%);
}

.admin-card-icon {
  font-size: 2rem;
  display: block;
  margin-bottom: 0.75rem;
}

.admin-card h3 {
  font-size: 1.15rem;
  margin: 0 0 0.5rem;
}

.admin-card p {
  margin: 0;
  color: #64748b;
  font-size: 0.95rem;
}
</style>
