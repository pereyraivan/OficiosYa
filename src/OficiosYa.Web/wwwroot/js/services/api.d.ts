export declare class ApiService {
    private baseUrl;
    private token;
    constructor(baseUrl?: string);
    private loadToken;
    setToken(token: string): void;
    clearToken(): void;
    private request;
    get<T>(endpoint: string): Promise<T>;
    post<T>(endpoint: string, data?: any): Promise<T>;
    put<T>(endpoint: string, data?: any): Promise<T>;
    delete<T>(endpoint: string): Promise<T>;
}
export declare const apiService: ApiService;
//# sourceMappingURL=api.d.ts.map