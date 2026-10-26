import { useState } from 'react'
import { createQuoteRequest } from '../Services/ProductService.js'

export function QuoteRequestForm({ product, onClose }) {
  const [status, setStatus] = useState('idle')
  const [error, setError] = useState('')

  async function handleSubmit(event) {
    event.preventDefault()

    const form = event.currentTarget
    const formData = new FormData(form)

    const quoteRequest = {
      productId: product.id,
      customerName: formData.get('customerName'),
      email: formData.get('email'),
      phone: formData.get('phone') || null,
      quantity: Number(formData.get('quantity')),
      message: formData.get('message') || null,
    }

    setStatus('sending')
    setError('')

    try {
      await createQuoteRequest(quoteRequest)
      setStatus('success')
      form.reset()
    } catch (err) {
      setError(err.message)
      setStatus('error')
    }
  }

  return (
    <section className="quote-panel">
      <div className="quote-heading">
        <div>
          <p className="eyebrow">SOLICITUD DE COTIZACIÓN</p>
          <h3>{product.name}</h3>
        </div>

        <button
          type="button"
          className="detail-button"
          onClick={onClose}
        >
          Cerrar
        </button>
      </div>

      {status === 'success' ? (
        <p className="quote-success" role="status">
          Recibimos tu solicitud. Nos pondremos en contacto contigo.
        </p>
      ) : (
        <form className="quote-form" onSubmit={handleSubmit}>
          <label>
            Nombre
            <input
              name="customerName"
              type="text"
              autoComplete="name"
              maxLength="100"
              required
            />
          </label>

          <label>
            Correo electrónico
            <input
              name="email"
              type="email"
              autoComplete="email"
              maxLength="254"
              required
            />
          </label>

          <label>
            Teléfono (opcional)
            <input
              name="phone"
              type="tel"
              autoComplete="tel"
              maxLength="30"
            />
          </label>

          <label>
            Cantidad
            <input
              name="quantity"
              type="number"
              min="1"
              max="100000"
              defaultValue="1"
              required
            />
          </label>

          <label className="quote-message">
            Mensaje (opcional)
            <textarea
              name="message"
              rows="4"
              maxLength="2000"
            />
          </label>

          {status === 'error' && (
            <p className="quote-error" role="alert">
              No se pudo enviar la solicitud: {error}
            </p>
          )}

          <button
            className="primary-button quote-submit"
            type="submit"
            disabled={status === 'sending'}
          >
            {status === 'sending' ? 'Enviando...' : 'Enviar solicitud'}
          </button>
        </form>
      )}
    </section>
  )
}