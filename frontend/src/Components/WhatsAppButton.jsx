const WHATSAPP_NUMBER = '+46763941489'

export function WhatsAppButton({ productName }) {
  const message = `Hola, quisiera solicitar una cotización para: ${productName}.`
  const link = `https://wa.me/${WHATSAPP_NUMBER}?text=${encodeURIComponent(message)}`

  return (
    <a
      className="whatsapp-button"
      href={link}
      target="_blank"
      rel="noopener noreferrer"
      aria-label={`Solicitar cotización de ${productName} por WhatsApp`}
    >
      Contactar por WhatsApp
    </a>
  )
}