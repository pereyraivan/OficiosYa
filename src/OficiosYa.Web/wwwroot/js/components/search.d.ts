export declare class SearchComponent {
    private searchForm;
    private oficiosSelect;
    private resultadosContainer;
    private oficios;
    constructor();
    private initializeElements;
    private bindEvents;
    private loadOficios;
    private populateOficiosSelect;
    private handleSearch;
    private displayResultados;
    private createProfesionalCard;
    private generateStars;
    private showLoading;
    private showError;
}
declare global {
    function verPerfil(profesionalId: number): void;
    function contactar(profesionalId: number): void;
}
//# sourceMappingURL=search.d.ts.map