<template>
  <div id="app">
    <!-- Header dinámico -->
    <header class="header">
      <div class="container nav">
        <div class="logo">
          <div class="logo-badge">🛠️</div>
          <span>TuOficioPro</span>
        </div>
        <nav class="navlinks">
          <router-link to="/">Inicio</router-link>
          <router-link to="/buscar">Profesionales</router-link>
          <a href="#como-funciona">Cómo funciona</a>
          <a href="#">Ayuda</a>
          <button class="cta" @click="goToRegister">Publicar tu servicio</button>
        </nav>
      </div>
    </header>

    <!-- Contenido principal -->
    <main>
      <router-view />
    </main>

    <!-- Footer -->
    <footer>
      <div class="container footer">
        <div>
          <div class="logo" style="color:white">
            <div class="logo-badge">🛠️</div>
            <span>TuOficioPro</span>
          </div>
          <p class="small" style="margin-top:8px">
            Conectamos personas con profesionales de confianza en su zona.
          </p>
        </div>
        <div>
          <strong>Explorar</strong>
          <div class="small" style="margin-top:8px">
            <router-link to="/buscar" style="color:inherit">Profesionales</router-link>
          </div>
        </div>
        <div>
          <strong>Ayuda</strong>
          <div class="small" style="margin-top:8px">
            <a href="#" style="color:inherit">Centro de ayuda</a>
          </div>
          <div class="small">
            <a href="#" style="color:inherit">Contacto</a>
          </div>
        </div>
        <div>
          <strong>Legal</strong>
          <div class="small" style="margin-top:8px">
            <a href="#" style="color:inherit">Términos</a>
          </div>
          <div class="small">
            <a href="#" style="color:inherit">Privacidad</a>
          </div>
        </div>
      </div>
      <div class="container" style="padding:12px 0;border-top:1px solid #1f2937;color:#94a3b8">
        <div class="small">© 2025 TuOficioPro</div>
      </div>
    </footer>
  </div>
</template>

<script>
export default {
  name: 'App',
  methods: {
    goToRegister() {
      // Redirigir al registro de profesionales
      window.location.href = '/Profesional/Registrar'
    }
  }
}
</script>

<style scoped>
/* Estilos específicos del componente App */
.header {
  position: sticky;
  top: 0;
  z-index: 10;
  background: #fff8;
  backdrop-filter: saturate(180%) blur(10px);
  border-bottom: 1px solid #ffffff40;
}

.nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 14px 0;
}

.logo {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 800;
}

.logo-badge {
  width: 34px;
  height: 34px;
  border-radius: 12px;
  background: linear-gradient(135deg, #22c55e, #06b6d4);
  display: grid;
  place-items: center;
  color: white;
  font-weight: 900;
}

.navlinks {
  display: flex;
  gap: 22px;
  align-items: center;
}

.navlinks a {
  color: #0f172aB3;
  text-decoration: none;
}

.navlinks a.router-link-active {
  color: #22c55e;
  font-weight: 600;
}

.cta {
  padding: 10px 16px;
  border-radius: 999px;
  background: #22c55e;
  color: #fff;
  border: 0;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
  cursor: pointer;
}

.cta:hover {
  background: #16a34a;
}

footer {
  background: #0f172a;
  color: #cbd5e1;
}

.footer {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 16px;
  padding: 32px 0;
}

.small {
  font-size: 13px;
  color: #94a3b8;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}
</style>
