import { Oficio } from './types';
export declare class OficiosYaApp {
    private oficios;
    constructor();
    private initializeApp;
    private loadInitialData;
    private populateOficiosDropdown;
    private loadDestacados;
    private displayDestacados;
    private generateStars;
    private filterByOficio;
    private setupNavigation;
    private setupGlobalEvents;
    private updateAuthButtons;
    private logout;
    searchProfessionals(filters: any): Promise<any[]>;
    getOficios(): Promise<Oficio[]>;
    isUserLoggedIn(): boolean;
}
declare global {
    interface Window {
        oficiosYaApp: OficiosYaApp;
    }
}
//# sourceMappingURL=app.d.ts.map