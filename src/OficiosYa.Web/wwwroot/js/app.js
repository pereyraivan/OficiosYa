// Aplicación principal de OficiosYa
import { usuarioService } from './services/usuarioService.js';
import { oficioService } from './services/oficioService.js';
export class OficiosYaApp {
    constructor() {
        this.oficios = [];
        this.initializeApp();
    }
    async initializeApp() {
        console.log('Inicializando OficiosYa App...');
        // Cargar datos iniciales
        await this.loadInitialData();
        // Configurar navegación
        this.setupNavigation();
        // Configurar eventos globales
        this.setupGlobalEvents();
        console.log('OficiosYa App inicializada correctamente');
    }
    async loadInitialData() {
        try {
            // Cargar oficios
            this.oficios = await oficioService.obtenerOficios();
            this.populateOficiosDropdown();
            // Cargar profesionales destacados si estamos en la página principal
            if (window.location.pathname === '/' || window.location.pathname === '/Home') {
                await this.loadDestacados();
            }
        }
        catch (error) {
            console.error('Error loading initial data:', error);
        }
    }
    populateOficiosDropdown() {
        const oficiosDropdown = document.getElementById('oficiosDropdown');
        if (!oficiosDropdown)
            return;
        oficiosDropdown.innerHTML = this.oficios.map(oficio => `<a class="dropdown-item" href="#" data-oficio-id="${oficio.id}">
        <i class="${oficio.icono || '🔧'}"></i> ${oficio.nombre}
      </a>`).join('');
        // Agregar eventos a los elementos del dropdown
        oficiosDropdown.querySelectorAll('.dropdown-item').forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                const oficioId = e.currentTarget.dataset.oficioId;
                if (oficioId) {
                    this.filterByOficio(parseInt(oficioId));
                }
            });
        });
    }
    async loadDestacados() {
        try {
            const destacados = await oficioService.obtenerProfesionalesDestacados();
            this.displayDestacados(destacados);
        }
        catch (error) {
            console.error('Error loading destacados:', error);
        }
    }
    displayDestacados(profesionales) {
        const container = document.getElementById('destacadosContainer');
        if (!container)
            return;
        if (profesionales.length === 0) {
            container.innerHTML = '<p class="text-muted">No hay profesionales destacados disponibles.</p>';
            return;
        }
        const html = profesionales.slice(0, 6).map(prof => `
      <div class="col-md-4 mb-4">
        <div class="card">
          ${prof.fotoPerfil ?
            `<img src="${prof.fotoPerfil}" class="card-img-top" alt="${prof.usuario?.nombre}" style="height: 200px; object-fit: cover;">` :
            `<div class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
              <i class="fas fa-user fa-3x text-muted"></i>
            </div>`}
          <div class="card-body">
            <h5 class="card-title">${prof.usuario?.nombre} ${prof.usuario?.apellido}</h5>
            <p class="card-text">${prof.descripcion || 'Profesional destacado'}</p>
            <div class="d-flex justify-content-between align-items-center">
              <div class="rating">
                ${this.generateStars(prof.calificacionPromedio)}
                <span class="ms-1">(${prof.totalResenas})</span>
              </div>
              <button class="btn btn-primary btn-sm" onclick="verPerfil(${prof.id})">
                Ver Perfil
              </button>
            </div>
          </div>
        </div>
      </div>
    `).join('');
        container.innerHTML = `<div class="row">${html}</div>`;
    }
    generateStars(calificacion) {
        const stars = Math.round(calificacion);
        const fullStars = '★'.repeat(stars);
        const emptyStars = '☆'.repeat(5 - stars);
        return `<span class="text-warning">${fullStars}${emptyStars}</span>`;
    }
    filterByOficio(oficioId) {
        // Redirigir a la página de búsqueda con el filtro aplicado
        window.location.href = `/Buscar?oficio=${oficioId}`;
    }
    setupNavigation() {
        // Configurar navegación responsive
        const navbarToggler = document.querySelector('.navbar-toggler');
        const navbarCollapse = document.querySelector('.navbar-collapse');
        if (navbarToggler && navbarCollapse) {
            navbarToggler.addEventListener('click', () => {
                navbarCollapse.classList.toggle('show');
            });
        }
        // Cerrar menú móvil al hacer clic en un enlace
        document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
            link.addEventListener('click', () => {
                if (navbarCollapse.classList.contains('show')) {
                    navbarCollapse.classList.remove('show');
                }
            });
        });
    }
    setupGlobalEvents() {
        // Configurar eventos globales de la aplicación
        // Mostrar/ocultar botones de autenticación
        this.updateAuthButtons();
        // Configurar logout
        const logoutBtn = document.getElementById('logoutBtn');
        if (logoutBtn) {
            logoutBtn.addEventListener('click', () => {
                this.logout();
            });
        }
    }
    updateAuthButtons() {
        const isLoggedIn = usuarioService.isLoggedIn();
        const loginBtn = document.getElementById('loginBtn');
        const registerBtn = document.getElementById('registerBtn');
        const userMenu = document.getElementById('userMenu');
        if (isLoggedIn) {
            if (loginBtn)
                loginBtn.style.display = 'none';
            if (registerBtn)
                registerBtn.style.display = 'none';
            if (userMenu)
                userMenu.style.display = 'block';
        }
        else {
            if (loginBtn)
                loginBtn.style.display = 'block';
            if (registerBtn)
                registerBtn.style.display = 'block';
            if (userMenu)
                userMenu.style.display = 'none';
        }
    }
    logout() {
        usuarioService.logout();
        this.updateAuthButtons();
        window.location.href = '/';
    }
    // Métodos públicos para uso desde las vistas
    async searchProfessionals(filters) {
        return oficioService.buscarProfesionales(filters);
    }
    async getOficios() {
        return this.oficios;
    }
    isUserLoggedIn() {
        return usuarioService.isLoggedIn();
    }
}
// Inicializar la aplicación cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    window.oficiosYaApp = new OficiosYaApp();
});
//# sourceMappingURL=app.js.map