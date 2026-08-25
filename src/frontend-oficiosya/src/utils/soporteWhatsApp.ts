/** Evento para abrir el menú flotante de soporte WhatsApp desde cualquier vista. */
export const SOPORTE_WHATSAPP_EVENT = 'oy-abrir-soporte-whatsapp'

export function abrirMenuSoporteWhatsApp () {
  if (typeof window === 'undefined') return
  window.dispatchEvent(new CustomEvent(SOPORTE_WHATSAPP_EVENT))
}
