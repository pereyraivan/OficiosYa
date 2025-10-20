<template>
  <div class="home">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAInicio" style="cursor: pointer;">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        <nav class="navlinks">
          <a href="#inicio">Inicio</a>
          <a href="#categorias">Profesionales</a>
          <a href="#como-funciona">Cómo funciona</a>
          <a href="#">Ayuda</a>
          <button class="cta" @click="irARegistro">Publicar tu servicio</button>
        </nav>
      </div>
    </header>

    <!-- Hero Section -->
    <section class="hero" id="inicio">
      <div class="container hero-wrap">
        <div>
          <h1>Encontrá al profesional que necesitás, <br> en tu zona</h1>
          <p>Electricistas, plomeros, albañiles, gasistas, jardineros y mucho más. Compará perfiles, reseñas y contactá directo.</p>
          
          <form class="search" @submit.prevent="buscarProfesionales">
            <div class="chip">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M3 7h18M3 12h18M3 17h18" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
              </svg>
              <select v-model="busqueda.oficio" style="border:0;outline:none;width:100%;background:transparent">
                <option value="Electricista">Electricista</option>
                <option value="Plomero">Plomero</option>
                <option value="Albañil">Albañil</option>
                <option value="Gasista">Gasista</option>
                <option value="Jardinero">Jardinero</option>
                <option value="Pintor">Pintor</option>
                <option value="Cerrajero">Cerrajero</option>
              </select>
            </div>
            <div class="chip">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M12 21s8-6.1 8-12a8 8 0 1 0-16 0c0 5.9 8 12 8 12Z" stroke="currentColor" stroke-width="2"/>
                <circle cx="12" cy="9" r="3" stroke="currentColor" stroke-width="2"/>
              </svg>
              <input v-model="busqueda.zona" placeholder="Ej: San Miguel de Tucumán" 
                     style="border:0;outline:none;width:100%;background:transparent" />
            </div>
            <button type="submit">Buscar</button>
          </form>
          
          <div style="margin-top:14px" class="small">
            Tip: pronto podrás <b>dibujar tu zona</b> en el mapa para resultados más precisos.
          </div>
        </div>
        
        <div class="hero-card">
          <span class="badge">Nuevos perfiles verificados</span>
          <h3 style="margin-top:10px">Profesionales cerca tuyo</h3>
          <div style="display:grid;grid-template-columns:repeat(2,1fr);gap:12px">
            <div v-for="profesional in profesionalesDestacados" :key="profesional.id" class="pro">
              <div class="pro-head">
                <div class="avatar"></div>
                <div>
                  <strong>{{ profesional.usuario.nombre }}</strong>
                  <div class="small">{{ profesional.ciudad }} — {{ profesional.ciudad }}</div>
                </div>
              </div>
              <div class="small" style="margin-top:8px">
                <span class="stars">★★★★★</span> ({{ profesional.resenas.length }} reseñas)
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías -->
    <section class="section" id="categorias">
      <div class="container">
        <h2>Explorá categorías populares</h2>
        <p class="muted">Elegí una categoría para ver profesionales cerca de tu ubicación.</p>
        <div class="grid" style="margin-top:16px">
          <a href="#" class="card cat" v-for="categoria in categorias" :key="categoria.nombre">
            <div class="cat-icon">{{ categoria.icono }}</div>
            <div>
              <strong>{{ categoria.nombre }}</strong>
              <div class="small">{{ categoria.descripcion }}</div>
            </div>
          </a>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section class="section" id="como-funciona" style="background:#f8fafc">
      <div class="container">
        <h2>¿Cómo funciona?</h2>
        <div class="steps" style="margin-top:12px">
          <div class="step" v-for="paso in pasos" :key="paso.numero">
            <div class="badge" :style="paso.badgeStyle">{{ paso.titulo }}</div>
            <h3>{{ paso.titulo }}</h3>
            <p class="muted">{{ paso.descripcion }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Profesionales destacados -->
    <section class="section">
      <div class="container">
        <h2>Profesionales destacados</h2>
        <p class="muted">Perfiles completos, con certificaciones y excelentes reseñas.</p>
        <div class="pros" style="margin-top:16px">
          <article class="pro" v-for="profesional in profesionalesDestacados" :key="profesional.id">
            <div class="pro-head">
              <div class="avatar"></div>
              <div>
                <strong>{{ profesional.usuario.nombre }}</strong>
                <div class="small">{{ profesional.ciudad }} · {{ profesional.ciudad }}</div>
              </div>
            </div>
            <div class="small" style="margin-top:8px">
              <span class="stars">★★★★★</span> {{ profesional.puntuacionPromedio.toFixed(1) }} ({{ profesional.resenas.length }})
            </div>
            <div class="small" style="margin-top:10px">
              Certificaciones: {{ profesional.certificados.length }}
            </div>
          </article>
        </div>
      </div>
    </section>

    <!-- Testimonios -->
    <section class="section" style="background:#f8fafc">
      <div class="container">
        <h2>Lo que dicen nuestros usuarios</h2>
        <div class="testis" style="margin-top:16px">
          <blockquote class="testi" v-for="testimonio in testimonios" :key="testimonio.id">
            "{{ testimonio.comentario }}"
            <div class="small" style="margin-top:6px">— {{ testimonio.autor }}</div>
          </blockquote>
        </div>
      </div>
    </section>

    <!-- CTA profesional -->
    <section class="section">
      <div class="container">
        <div class="cta-block">
          <div>
            <h2 style="margin:0 0 6px">¿Sos profesional? Empezá a recibir trabajos</h2>
            <div class="small" style="color: white;">Registrate gratis, subí tus certificaciones y elegí tu zona de trabajo.</div>
          </div>
          <button @click="irARegistro">Crear cuenta de profesional</button>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer>
      <div class="container footer">
        <div>
          <div class="logo" style="color:white; cursor: pointer;" @click="irAInicio">
            <div class="logo-badge">🛠️</div>
            <span>OficiosYa</span>
          </div>
          <p class="small" style="margin-top:8px">Conectamos personas con profesionales de confianza en su zona.</p>
        </div>
        <div>
          <strong>Explorar</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Categorías</a></div>
          <div class="small"><a href="#" style="color:inherit">Profesionales</a></div>
        </div>
        <div>
          <strong>Ayuda</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Centro de ayuda</a></div>
          <div class="small"><a href="#" style="color:inherit">Contacto</a></div>
        </div>
        <div>
          <strong>Legal</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Términos</a></div>
          <div class="small"><a href="#" style="color:inherit">Privacidad</a></div>
        </div>
      </div>
      <div class="container" style="padding:12px 0;border-top:1px solid #1f2937;color:#94a3b8">
        <div class="small">© 2025 OficiosYa</div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import type { Profesional } from '../types'

const router = useRouter()

// Estado reactivo
const busqueda = ref({
  oficio: 'Electricista',
  zona: ''
})

const profesionalesDestacados = ref<Profesional[]>([])

// Datos estáticos
const categorias = ref([
  { nombre: 'Electricista', descripcion: 'Instalaciones y urgencias', icono: '⚡' },
  { nombre: 'Plomería', descripcion: 'Pérdidas, caños, bombas', icono: '🚰' },
  { nombre: 'Construcción', descripcion: 'Obras y refacciones', icono: '🧱' },
  { nombre: 'Gasista', descripcion: 'Habilitaciones y reparaciones', icono: '🔥' },
  { nombre: 'Pintura', descripcion: 'Interior y exterior', icono: '🎨' },
  { nombre: 'Jardinería', descripcion: 'Mantenimiento y riego', icono: '🌿' },
  { nombre: 'Cerrajería', descripcion: 'Aperturas y copias', icono: '🔒' },
  { nombre: 'Limpieza', descripcion: 'Hogar y oficinas', icono: '🧹' }
])

const pasos = ref([
  {
    numero: 1,
    titulo: 'Paso 1',
    descripcion: 'Usá el buscador o elegí una categoría. Podrás filtrar por distancia, precio y valoración.',
    badgeStyle: 'background:#e0f2fe;color:#075985'
  },
  {
    numero: 2,
    titulo: 'Paso 2',
    descripcion: 'Mirá fotos de trabajos, certificaciones y reseñas reales de clientes.',
    badgeStyle: 'background:#dcfce7;color:#14532d'
  },
  {
    numero: 3,
    titulo: 'Paso 3',
    descripcion: 'Enviá un mensaje o pedí presupuesto sin compromiso. ¡Listo!',
    badgeStyle: 'background:#fef9c3;color:#854d0e'
  }
])

const testimonios = ref([
  {
    id: 1,
    comentario: 'Conseguí electricista en 5 minutos y resolvió el mismo día.',
    autor: 'Sofía, SM de Tucumán'
  },
  {
    id: 2,
    comentario: 'Me encantó poder ver reseñas y fotos de trabajos reales.',
    autor: 'Martín, Yerba Buena'
  },
  {
    id: 3,
    comentario: 'Como profesional, recibo pedidos todas las semanas.',
    autor: 'Daniela, Plomería'
  }
])

// Métodos
const buscarProfesionales = async () => {
  try {
    const resultados = await profesionalService.buscar({
      oficio: busqueda.value.oficio,
      zona: busqueda.value.zona
    })
    console.log('Resultados de búsqueda:', resultados)
    // Aquí podrías navegar a una página de resultados o mostrar un modal
  } catch (error) {
    console.error('Error en búsqueda:', error)
  }
}

const irARegistro = () => {
  router.push('/registrar-profesional')
}

const irAInicio = () => {
  document.getElementById('inicio')?.scrollIntoView({ behavior: 'smooth' })
}

const cargarProfesionalesDestacados = async () => {
  try {
    profesionalesDestacados.value = await profesionalService.obtenerDestacados()
  } catch (error) {
    console.error('Error cargando profesionales destacados:', error)
    // Datos de ejemplo si falla la API
    profesionalesDestacados.value = [
      {
        id: 1,
        usuarioId: 1,
        descripcion: 'Electricista matriculada',
        ciudad: 'Centro',
        puntuacionPromedio: 4.9,
        suscripcionActiva: true,
        usuario: { id: 1, nombre: 'María López', email: 'maria@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        certificados: [],
        resenas: []
      },
      {
        id: 2,
        usuarioId: 2,
        descripcion: 'Plomería y gas',
        ciudad: 'Yerba Buena',
        puntuacionPromedio: 4.6,
        suscripcionActiva: true,
        usuario: { id: 2, nombre: 'Diego Pérez', email: 'diego@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        certificados: [],
        resenas: []
      }
    ]
  }
}

// Lifecycle
onMounted(() => {
  cargarProfesionalesDestacados()
})
</script>
