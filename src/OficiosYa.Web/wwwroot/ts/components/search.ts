// Componente de búsqueda de profesionales
import { oficioService } from '../services/oficioService';
import { Oficio, Profesional, SearchFilters } from '../types';

export class SearchComponent {
  private searchForm: HTMLFormElement | null = null;
  private oficiosSelect: HTMLSelectElement | null = null;
  private resultadosContainer: HTMLElement | null = null;
  private oficios: Oficio[] = [];

  constructor() {
    this.initializeElements();
    this.bindEvents();
    this.loadOficios();
  }

  private initializeElements(): void {
    this.searchForm = document.getElementById('searchForm') as HTMLFormElement;
    this.oficiosSelect = document.getElementById('oficio') as HTMLSelectElement;
    this.resultadosContainer = document.getElementById('resultados') as HTMLElement;
  }

  private bindEvents(): void {
    if (this.searchForm) {
      this.searchForm.addEventListener('submit', this.handleSearch.bind(this));
    }

    // Búsqueda en tiempo real mientras se escribe
    const searchInput = document.getElementById('busqueda') as HTMLInputElement;
    if (searchInput) {
      let timeoutId: number;
      searchInput.addEventListener('input', () => {
        clearTimeout(timeoutId);
        timeoutId = window.setTimeout(() => {
          this.handleSearch();
        }, 500); // Buscar después de 500ms de inactividad
      });
    }
  }

  private async loadOficios(): Promise<void> {
    try {
      this.oficios = await oficioService.obtenerOficios();
      this.populateOficiosSelect();
    } catch (error) {
      console.error('Error loading oficios:', error);
    }
  }

  private populateOficiosSelect(): void {
    if (!this.oficiosSelect) return;

    // Limpiar opciones existentes (excepto la primera)
    this.oficiosSelect.innerHTML = '<option value="">Todos los oficios</option>';

    this.oficios.forEach(oficio => {
      const option = document.createElement('option');
      option.value = oficio.id.toString();
      option.textContent = oficio.nombre;
      this.oficiosSelect!.appendChild(option);
    });
  }

  private async handleSearch(event?: Event): Promise<void> {
    if (event) event.preventDefault();

    if (!this.searchForm || !this.resultadosContainer) return;

    const formData = new FormData(this.searchForm);
    const filters: SearchFilters = {
      oficioId: formData.get('oficio') ? parseInt(formData.get('oficio') as string) : undefined,
      ciudad: formData.get('ciudad') as string || undefined,
      precioMin: formData.get('precioMin') ? parseFloat(formData.get('precioMin') as string) : undefined,
      precioMax: formData.get('precioMax') ? parseFloat(formData.get('precioMax') as string) : undefined,
      calificacionMin: formData.get('calificacionMin') ? parseFloat(formData.get('calificacionMin') as string) : undefined,
      disponible: formData.get('disponible') === 'on'
    };

    try {
      this.showLoading(true);
      const profesionales = await oficioService.buscarProfesionales(filters);
      this.displayResultados(profesionales);
    } catch (error) {
      this.showError('Error al buscar profesionales. Intenta nuevamente.');
      console.error('Search error:', error);
    } finally {
      this.showLoading(false);
    }
  }

  private displayResultados(profesionales: Profesional[]): void {
    if (!this.resultadosContainer) return;

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

  private createProfesionalCard(profesional: Profesional): string {
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
            </div>`
          }
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

  private generateStars(calificacion: number): string {
    const stars = Math.round(calificacion);
    const fullStars = '★'.repeat(stars);
    const emptyStars = '☆'.repeat(5 - stars);
    return `<span class="text-warning">${fullStars}${emptyStars}</span>`;
  }

  private showLoading(show: boolean): void {
    const searchButton = document.querySelector('#searchForm button[type="submit"]') as HTMLButtonElement;
    if (searchButton) {
      searchButton.disabled = show;
      searchButton.textContent = show ? 'Buscando...' : 'Buscar';
    }
  }

  private showError(message: string): void {
    if (!this.resultadosContainer) return;
    
    this.resultadosContainer.innerHTML = `
      <div class="alert alert-danger">
        <h4>Error</h4>
        <p>${message}</p>
      </div>
    `;
  }
}

// Funciones globales para los botones
declare global {
  function verPerfil(profesionalId: number): void;
  function contactar(profesionalId: number): void;
}

window.verPerfil = (profesionalId: number) => {
  window.location.href = `/Profesional/Perfil/${profesionalId}`;
};

window.contactar = (profesionalId: number) => {
  // Implementar lógica de contacto
  alert(`Contactando al profesional ${profesionalId}`);
};

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
  new SearchComponent();
});
