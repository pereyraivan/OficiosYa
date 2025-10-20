// Componente de autenticación
import { usuarioService } from '../services/usuarioService';
import { LoginRequest, RegistrarUsuarioRequest } from '../types';

export class AuthComponent {
  private loginForm: HTMLFormElement | null = null;
  private registerForm: HTMLFormElement | null = null;

  constructor() {
    this.initializeElements();
    this.bindEvents();
  }

  private initializeElements(): void {
    this.loginForm = document.getElementById('loginForm') as HTMLFormElement;
    this.registerForm = document.getElementById('registerForm') as HTMLFormElement;
  }

  private bindEvents(): void {
    if (this.loginForm) {
      this.loginForm.addEventListener('submit', this.handleLogin.bind(this));
    }

    if (this.registerForm) {
      this.registerForm.addEventListener('submit', this.handleRegister.bind(this));
    }

    // Verificar email en tiempo real
    const emailInput = document.getElementById('email') as HTMLInputElement;
    if (emailInput) {
      emailInput.addEventListener('blur', this.verifyEmailAvailability.bind(this));
    }
  }

  private async handleLogin(event: Event): Promise<void> {
    event.preventDefault();
    
    if (!this.loginForm) return;

    const formData = new FormData(this.loginForm);
    const loginRequest: LoginRequest = {
      email: formData.get('email') as string,
      password: formData.get('password') as string
    };

    try {
      this.showLoading(true);
      const response = await usuarioService.login(loginRequest);
      
      if (response.success) {
        this.showMessage('Login exitoso!', 'success');
        this.redirectToHome();
      } else {
        this.showMessage(response.message || 'Error en el login', 'error');
      }
    } catch (error) {
      this.showMessage('Error de conexión. Intenta nuevamente.', 'error');
      console.error('Login error:', error);
    } finally {
      this.showLoading(false);
    }
  }

  private async handleRegister(event: Event): Promise<void> {
    event.preventDefault();
    
    if (!this.registerForm) return;

    const formData = new FormData(this.registerForm);
    const registerRequest: RegistrarUsuarioRequest = {
      nombre: formData.get('nombre') as string,
      apellido: formData.get('apellido') as string,
      email: formData.get('email') as string,
      password: formData.get('password') as string,
      telefono: formData.get('telefono') as string || undefined,
      direccion: formData.get('direccion') as string || undefined,
      ciudad: formData.get('ciudad') as string || undefined,
      provincia: formData.get('provincia') as string || undefined,
      codigoPostal: formData.get('codigoPostal') as string || undefined
    };

    try {
      this.showLoading(true);
      await usuarioService.registrarUsuario(registerRequest);
      
      this.showMessage('Usuario registrado exitosamente!', 'success');
      this.registerForm.reset();
      
      // Opcional: hacer login automático después del registro
      // await usuarioService.login({ email: registerRequest.email, password: registerRequest.password });
      
    } catch (error) {
      this.showMessage('Error al registrar usuario. Intenta nuevamente.', 'error');
      console.error('Register error:', error);
    } finally {
      this.showLoading(false);
    }
  }

  private async verifyEmailAvailability(): Promise<void> {
    const emailInput = document.getElementById('email') as HTMLInputElement;
    if (!emailInput || !emailInput.value) return;

    try {
      const response = await usuarioService.verificarEmailDisponible(emailInput.value);
      const emailFeedback = document.getElementById('emailFeedback');
      
      if (emailFeedback) {
        if (response.disponible) {
          emailFeedback.textContent = 'Email disponible';
          emailFeedback.className = 'text-success';
        } else {
          emailFeedback.textContent = 'Email ya registrado';
          emailFeedback.className = 'text-danger';
        }
      }
    } catch (error) {
      console.error('Email verification error:', error);
    }
  }

  private showMessage(message: string, type: 'success' | 'error'): void {
    const messageDiv = document.getElementById('message') || this.createMessageElement();
    messageDiv.textContent = message;
    messageDiv.className = `alert alert-${type === 'success' ? 'success' : 'danger'}`;
    messageDiv.style.display = 'block';

    // Auto-hide después de 5 segundos
    setTimeout(() => {
      messageDiv.style.display = 'none';
    }, 5000);
  }

  private createMessageElement(): HTMLElement {
    const messageDiv = document.createElement('div');
    messageDiv.id = 'message';
    messageDiv.style.display = 'none';
    messageDiv.style.marginTop = '10px';
    
    const container = document.querySelector('.container') || document.body;
    container.insertBefore(messageDiv, container.firstChild);
    
    return messageDiv;
  }

  private showLoading(show: boolean): void {
    const submitButtons = document.querySelectorAll('button[type="submit"]');
    submitButtons.forEach(button => {
      const htmlButton = button as HTMLButtonElement;
      htmlButton.disabled = show;
      htmlButton.textContent = show ? 'Cargando...' : htmlButton.getAttribute('data-original-text') || 'Enviar';
    });
  }

  private redirectToHome(): void {
    window.location.href = '/';
  }
}

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
  new AuthComponent();
});
