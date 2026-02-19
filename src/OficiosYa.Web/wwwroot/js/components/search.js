// Componente de búsqueda de profesionales
import { oficioService } from '../services/oficioService.js';
export class SearchComponent {
    constructor() {
        this.searchForm = null;
        this.oficiosSelect = null;
        this.resultadosContainer = null;
        this.oficios = [];
        this.initializeElements();
        this.bindEvents();
        this.loadOficios();
    }
    initializeElements() {
        this.searchForm = document.getElementById('searchForm');
        this.oficiosSelect = document.getElementById('oficio');
        this.resultadosContainer = document.getElementById('resultados');
    }
    bindEvents() {
        if (this.searchForm) {
            this.searchForm.addEventListener('submit', this.handleSearch.bind(this));
        }
        // Búsqueda en tiempo real mientras se escribe
        const searchInput = document.getElementById('busqueda');
        if (searchInput) {
            let timeoutId;
            searchInput.addEventListener('input', () => {
                clearTimeout(timeoutId);
                timeoutId = window.setTimeout(() => {
                    this.handleSearch();
                }, 500); // Buscar después de 500ms de inactividad
            });
        }
    }
    async loadOficios() {
        try {
            this.oficios = await oficioService.obtenerOficios();
            this.populateOficiosSelect();
        }
        catch (error) {
            console.error('Error loading oficios:', error);
        }
    }
    populateOficiosSelect() {
        if (!this.oficiosSelect)
            return;
        // Limpiar opciones existentes (excepto la primera)
        this.oficiosSelect.innerHTML = '<option value="">Todos los oficios</option>';
        this.oficios.forEach(oficio => {
            const option = document.createElement('option');
            option.value = oficio.id.toString();
            option.textContent = oficio.nombre;
            this.oficiosSelect.appendChild(option);
        });
    }
    async handleSearch(event) {
        if (event)
            event.preventDefault();
        if (!this.searchForm || !this.resultadosContainer)
            return;
        const formData = new FormData(this.searchForm);
        const filters = {
            oficioId: formData.get('oficio') ? parseInt(formData.get('oficio')) : undefined,
            ciudad: formData.get('ciudad') || undefined,
            precioMin: formData.get('precioMin') ? parseFloat(formData.get('precioMin')) : undefined,
            precioMax: formData.get('precioMax') ? parseFloat(formData.get('precioMax')) : undefined,
            calificacionMin: formData.get('calificacionMin') ? parseFloat(formData.get('calificacionMin')) : undefined,
            disponible: formData.get('disponible') === 'on'
        };
        try {
            this.showLoading(true);
            const profesionales = await oficioService.buscarProfesionales(filters);
            this.displayResultados(profesionales);
        }
        catch (error) {
            this.showError('Error al buscar profesionales. Intenta nuevamente.');
            console.error('Search error:', error);
        }
        finally {
            this.showLoading(false);
        }
    }
    displayResultados(profesionales) {
        if (!this.resultadosContainer)
            return;
        if (profesionales.length === 0) {
            this.resultadosContainer.innerHTML = `
        <div class="alert alert-info">
          <h4>No se encontraron profesionales</h4>
          <p>Intenta ajustar los filtros de búsqueda.</p>
        </div>
      `;
            return;
        }
        const html = profesionales.map(profesional => this.createProfesionalCard(profesional)).join('');
        this.resultadosContainer.innerHTML = `
      <div class="row">
        ${html}
      </div>
    `;
    }
    createProfesionalCard(profesional) {
        const oficios = profesional.profesionalOficios?.map(po => po.oficio?.nombre).join(', ') || 'Sin oficios';
        const precioPromedio = profesional.profesionalOficios?.length
            ? (profesional.profesionalOficios.reduce((sum, po) => sum + po.precioHora, 0) / profesional.profesionalOficios.length).toFixed(2)
            : '0';
        return `
      <div class="col-md-6 col-lg-4 mb-4">
        <div class="card h-100">
          ${profesional.fotoPerfil ?
            `<img src="${profesional.fotoPerfil}" class="card-img-top" alt="Foto de ${profesional.usuario?.nombre}" style="height: 200px; object-fit: cover;">` :
            `<div class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
              <i class="fas fa-user fa-3x text-muted"></i>
            </div>`}
          <div class="card-body d-flex flex-column">
            <h5 class="card-title">${profesional.usuario?.nombre} ${profesional.usuario?.apellido}</h5>
            <p class="card-text text-muted">${oficios}</p>
            <p class="card-text">${profesional.descripcion || 'Sin descripción'}</p>
            <div class="mt-auto">
              <div class="d-flex justify-content-between align-items-center mb-2">
                <div class="rating">
                  ${this.generateStars(profesional.calificacionPromedio)}
                  <span class="ms-1">(${profesional.totalResenas})</span>
                </div>
                <span class="badge bg-success">$${precioPromedio}/h</span>
              </div>
              <div class="d-flex gap-2">
                <button class="btn btn-primary btn-sm flex-fill" onclick="verPerfil(${profesional.id})">
                  Ver Perfil
                </button>
                <button class="btn btn-outline-primary btn-sm flex-fill" onclick="contactar(${profesional.id})">
                  Contactar
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    `;
    }
    generateStars(calificacion) {
        const stars = Math.round(calificacion);
        const fullStars = '★'.repeat(stars);
        const emptyStars = '☆'.repeat(5 - stars);
        return `<span class="text-warning">${fullStars}${emptyStars}</span>`;
    }
    showLoading(show) {
        const searchButton = document.querySelector('#searchForm button[type="submit"]');
        if (searchButton) {
            searchButton.disabled = show;
            searchButton.textContent = show ? 'Buscando...' : 'Buscar';
        }
    }
    showError(message) {
        if (!this.resultadosContainer)
            return;
        this.resultadosContainer.innerHTML = `
      <div class="alert alert-danger">
        <h4>Error</h4>
        <p>${message}</p>
      </div>
    `;
    }
}
window.verPerfil = (profesionalId) => {
    window.location.href = `/Profesional/Perfil/${profesionalId}`;
};
window.contactar = (profesionalId) => {
    // Implementar lógica de contacto
    alert(`Contactando al profesional ${profesionalId}`);
};
// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    new SearchComponent();
});
//# sourceMappingURL=search.js.map