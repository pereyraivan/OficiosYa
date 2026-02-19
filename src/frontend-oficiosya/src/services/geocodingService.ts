// Servicio para geocodificación usando API Georef de Argentina
// API oficial y gratuita del gobierno argentino
// https://apis.datos.gob.ar/georef/api/

export interface CiudadSugerida {
  nombre: string
  nombreCompleto: string
  provincia: string
  departamento?: string
  latitud: number
  longitud: number
  id: string
}

export const geocodingService = {
  /**
   * Busca localidades (ciudades) de Argentina usando la API Georef
   * @param query - Texto de búsqueda (nombre de ciudad)
   * @param limit - Número máximo de resultados (default: 10)
   * @param provincia - Nombre de provincia para filtrar (opcional)
   */
  async buscarCiudades(
    query: string, 
    limit: number = 10,
    provincia?: string
  ): Promise<CiudadSugerida[]> {
    if (!query || query.trim().length < 2) {
      return []
    }

    try {
      // API Georef - Oficial del gobierno argentino, completamente gratuita
      const url = new URL('https://apis.datos.gob.ar/georef/api/localidades')
      url.searchParams.set('nombre', query.trim())
      url.searchParams.set('max', limit.toString())
      url.searchParams.set('orden', 'nombre')
      
      // Si se especifica provincia, filtrar por ella
      if (provincia) {
        url.searchParams.set('provincia', provincia)
      }

      const response = await fetch(url.toString())

      if (!response.ok) {
        throw new Error(`Error en la búsqueda: ${response.status}`)
      }

      const data = await response.json()

      if (!data.localidades || data.localidades.length === 0) {
        return []
      }

      return data.localidades
        .map((item: any) => {
          const nombre = item.nombre || ''
          const provinciaNombre = item.provincia?.nombre || ''
          const departamento = item.departamento?.nombre
          
          const nombreCompleto = departamento 
            ? `${nombre}, ${departamento}, ${provinciaNombre}`
            : `${nombre}, ${provinciaNombre}`

          return {
            nombre: nombre.trim(),
            nombreCompleto: nombreCompleto.trim(),
            provincia: provinciaNombre,
            departamento: departamento,
            latitud: item.centroide?.lat || 0,
            longitud: item.centroide?.lon || 0,
            id: item.id?.toString() || ''
          }
        })
        .filter((ciudad: CiudadSugerida) => ciudad.nombre.length > 0)
        .slice(0, limit)
    } catch (error) {
      return []
    }
  },

  /**
   * Obtiene todas las provincias de Argentina
   */
  async obtenerProvincias(): Promise<Array<{ id: string; nombre: string }>> {
    try {
      const response = await fetch('https://apis.datos.gob.ar/georef/api/provincias')
      
      if (!response.ok) {
        throw new Error(`Error obteniendo provincias: ${response.status}`)
      }

      const data = await response.json()

      if (!data.provincias) {
        return []
      }

      return data.provincias.map((p: any) => ({
        id: p.id?.toString() || '',
        nombre: p.nombre || ''
      })).sort((a: any, b: any) => a.nombre.localeCompare(b.nombre))
    } catch (error) {
      return []
    }
  },

  /**
   * Obtiene localidades de una provincia específica
   */
  async obtenerLocalidadesPorProvincia(
    provinciaId: string,
    limit: number = 100
  ): Promise<CiudadSugerida[]> {
    try {
      const url = new URL('https://apis.datos.gob.ar/georef/api/localidades')
      url.searchParams.set('provincia', provinciaId)
      url.searchParams.set('max', limit.toString())
      url.searchParams.set('orden', 'nombre')

      const response = await fetch(url.toString())

      if (!response.ok) {
        throw new Error(`Error obteniendo localidades: ${response.status}`)
      }

      const data = await response.json()

      if (!data.localidades) {
        return []
      }

      return data.localidades.map((item: any) => {
        const nombre = item.nombre || ''
        const provinciaNombre = item.provincia?.nombre || ''
        const departamento = item.departamento?.nombre

        return {
          nombre: nombre.trim(),
          nombreCompleto: departamento 
            ? `${nombre}, ${departamento}, ${provinciaNombre}`
            : `${nombre}, ${provinciaNombre}`,
          provincia: provinciaNombre,
          departamento: departamento,
          latitud: item.centroide?.lat || 0,
          longitud: item.centroide?.lon || 0,
          id: item.id?.toString() || ''
        }
      })
    } catch (error) {
      return []
    }
  }
}
