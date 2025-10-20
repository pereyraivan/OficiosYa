// Componente de autenticación
import { usuarioService } from '../services/usuarioService';
export class AuthComponent {
    constructor() {
        this.loginForm = null;
        this.registerForm = null;
        this.initializeElements();
        this.bindEvents();
    }
    initializeElements() {
        this.loginForm = document.getElementById('loginForm');
        this.registerForm = document.getElementById('registerForm');
    }
    bindEvents() {
        if (this.loginForm) {
            this.loginForm.addEventListener('submit', this.handleLogin.bind(this));
        }
        if (this.registerForm) {
            this.registerForm.addEventListener('submit', this.handleRegister.bind(this));
        }
        // Verificar email en tiempo real
        const emailInput = document.getElementById('email');
        if (emailInput) {
            emailInput.addEventListener('blur', this.verifyEmailAvailability.bind(this));
        }
    }
    async handleLogin(event) {
        event.preventDefault();
        if (!this.loginForm)
            return;
        const formData = new FormData(this.loginForm);
        const loginRequest = {
            email: formData.get('email'),
            password: formData.get('password')
        };
        try {
            this.showLoading(true);
            const response = await usuarioService.login(loginRequest);
            if (response.success) {
                this.showMessage('Login exitoso!', 'success');
                this.redirectToHome();
            }
            else {
                this.showMessage(response.message || 'Error en el login', 'error');
            }
        }
        catch (error) {
            this.showMessage('Error de conexión. Intenta nuevamente.', 'error');
            console.error('Login error:', error);
        }
        finally {
            this.showLoading(false);
        }
    }
    async handleRegister(event) {
        event.preventDefault();
        if (!this.registerForm)
            return;
        const formData = new FormData(this.registerForm);
        const registerRequest = {
            nombre: formData.get('nombre'),
            apellido: formData.get('apellido'),
            email: formData.get('email'),
            password: formData.get('password'),
            telefono: formData.get('telefono') || undefined,
            direccion: formData.get('direccion') || undefined,
            ciudad: formData.get('ciudad') || undefined,
            provincia: formData.get('provincia') || undefined,
            codigoPostal: formData.get('codigoPostal') || undefined
        };
        try {
            this.showLoading(true);
            await usuarioService.registrarUsuario(registerRequest);
            this.showMessage('Usuario registrado exitosamente!', 'success');
            this.registerForm.reset();
            // Opcional: hacer login automático después del registro
            // await usuarioService.login({ email: registerRequest.email, password: registerRequest.password });
        }
        catch (error) {
            this.showMessage('Error al registrar usuario. Intenta nuevamente.', 'error');
            console.error('Register error:', error);
        }
        finally {
            this.showLoading(false);
        }
    }
    async verifyEmailAvailability() {
        const emailInput = document.getElementById('email');
        if (!emailInput || !emailInput.value)
            return;
        try {
            const response = await usuarioService.verificarEmailDisponible(emailInput.value);
            const emailFeedback = document.getElementById('emailFeedback');
            if (emailFeedback) {
                if (response.disponible) {
                    emailFeedback.textContent = 'Email disponible';
                    emailFeedback.className = 'text-success';
                }
                else {
                    emailFeedback.textContent = 'Email ya registrado';
                    emailFeedback.className = 'text-danger';
                }
            }
        }
        catch (error) {
            console.error('Email verification error:', error);
        }
    }
    showMessage(message, type) {
        const messageDiv = document.getElementById('message') || this.createMessageElement();
        messageDiv.textContent = message;
        messageDiv.className = `alert alert-${type === 'success' ? 'success' : 'danger'}`;
        messageDiv.style.display = 'block';
        // Auto-hide después de 5 segundos
        setTimeout(() => {
            messageDiv.style.display = 'none';
        }, 5000);
    }
    createMessageElement() {
        const messageDiv = document.createElement('div');
        messageDiv.id = 'message';
        messageDiv.style.display = 'none';
        messageDiv.style.marginTop = '10px';
        const container = document.querySelector('.container') || document.body;
        container.insertBefore(messageDiv, container.firstChild);
        return messageDiv;
    }
    showLoading(show) {
        const submitButtons = document.querySelectorAll('button[type="submit"]');
        submitButtons.forEach(button => {
            const htmlButton = button;
            htmlButton.disabled = show;
            htmlButton.textContent = show ? 'Cargando...' : htmlButton.getAttribute('data-original-text') || 'Enviar';
        });
    }
    redirectToHome() {
        window.location.href = '/';
    }
}
// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', () => {
    new AuthComponent();
});
//# sourceMappingURL=auth.js.map