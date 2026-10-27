import { useState } from 'react'
import { createQuoteRequest } from '../Services/ProductService.js'

const initialValues = {
  customerName: '',
  email: '',
  phone: '',
  quantity: '1',
  message: '',
}

function validateField(name, value) {
  const trimmedValue = value.trim()

  switch (name) {
    case 'customerName':
      if (!trimmedValue) return 'Escribe tu nombre.'
      if (trimmedValue.length > 100) {
        return 'El nombre no puede superar los 100 caracteres.'
      }
      return ''

    case 'email':
      if (!trimmedValue) return 'Escribe tu correo electrónico.'
      if (trimmedValue.length > 254) {
        return 'El correo no puede superar los 254 caracteres.'
      }
      if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(trimmedValue)) {
        return 'Escribe un correo electrónico válido.'
      }
      return ''

    case 'phone':
      if (trimmedValue.length > 30) {
        return 'El teléfono no puede superar los 30 caracteres.'
      }
      return ''

    case 'quantity': {
      if (!value) return 'Indica la cantidad.'
      const quantity = Number(value)

      if (!Number.isInteger(quantity)) {
        return 'La cantidad debe ser un número entero.'
      }
      if (quantity < 1 || quantity > 100000) {
        return 'La cantidad debe estar entre 1 y 100 000.'
      }
      return ''
    }

    case 'message':
      if (value.length > 2000) {
        return 'El mensaje no puede superar los 2 000 caracteres.'
      }
      return ''

    default:
      return ''
  }
}

export function QuoteRequestForm({ product, onClose }) {
  const [formValues, setFormValues] = useState(initialValues)
  const [fieldErrors, setFieldErrors] = useState({})
  const [status, setStatus] = useState('idle')
  const [error, setError] = useState('')

  function handleChange(event) {
    const { name, value } = event.target

    setFormValues((currentValues) => ({
      ...currentValues,
      [name]: value,
    }))

    setFieldErrors((currentErrors) => ({
      ...currentErrors,
      [name]: validateField(name, value),
    }))

    setError('')
    setStatus('idle')
  }

  function handleBlur(event) {
    const { name, value } = event.target

    setFieldErrors((currentErrors) => ({
      ...currentErrors,
      [name]: validateField(name, value),
    }))
  }

  async function handleSubmit(event) {
    event.preventDefault()

    const errors = Object.fromEntries(
      Object.entries(formValues).map(([name, value]) => [
        name,
        validateField(name, value),
      ]),
    )

    setFieldErrors(errors)

    if (Object.values(errors).some(Boolean)) {
      return
    }

    const quoteRequest = {
      productId: product.id,
      customerName: formValues.customerName.trim(),
      email: formValues.email.trim(),
      phone: formValues.phone.trim() || null,
      quantity: Number(formValues.quantity),
      message: formValues.message.trim() || null,
    }

    setStatus('sending')
    setError('')

    try {
      await createQuoteRequest(quoteRequest)
      setStatus('success')
      setFormValues(initialValues)
      setFieldErrors({})
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

        <button type="button" className="detail-button" onClick={onClose}>
          Cerrar
        </button>
      </div>

      {status === 'success' ? (
        <p className="quote-success" role="status">
          Recibimos tu solicitud. Nos pondremos en contacto contigo.
        </p>
      ) : (
        <form className="quote-form" onSubmit={handleSubmit} noValidate>
          <label>
            Nombre
            <input
              name="customerName"
              type="text"
              autoComplete="name"
              maxLength={100}
              value={formValues.customerName}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(fieldErrors.customerName)}
              aria-describedby={
                fieldErrors.customerName ? 'customerName-error' : undefined
              }
              required
            />
            {fieldErrors.customerName && (
              <small className="quote-field-error" id="customerName-error">
                {fieldErrors.customerName}
              </small>
            )}
          </label>

          <label>
            Correo electrónico
            <input
              name="email"
              type="email"
              autoComplete="email"
              maxLength={254}
              value={formValues.email}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(fieldErrors.email)}
              aria-describedby={fieldErrors.email ? 'email-error' : undefined}
              required
            />
            {fieldErrors.email && (
              <small className="quote-field-error" id="email-error">
                {fieldErrors.email}
              </small>
            )}
          </label>

          <label>
            Teléfono (opcional)
            <input
              name="phone"
              type="tel"
              autoComplete="tel"
              maxLength={30}
              value={formValues.phone}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(fieldErrors.phone)}
              aria-describedby={fieldErrors.phone ? 'phone-error' : undefined}
            />
            {fieldErrors.phone && (
              <small className="quote-field-error" id="phone-error">
                {fieldErrors.phone}
              </small>
            )}
          </label>

          <label>
            Cantidad
            <input
              name="quantity"
              type="number"
              min={1}
              max={100000}
              step={1}
              value={formValues.quantity}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(fieldErrors.quantity)}
              aria-describedby={
                fieldErrors.quantity ? 'quantity-error' : undefined
              }
              required
            />
            {fieldErrors.quantity && (
              <small className="quote-field-error" id="quantity-error">
                {fieldErrors.quantity}
              </small>
            )}
          </label>

          <label className="quote-message">
            Mensaje (opcional)
            <textarea
              name="message"
              rows={4}
              maxLength={2000}
              value={formValues.message}
              onChange={handleChange}
              onBlur={handleBlur}
              aria-invalid={Boolean(fieldErrors.message)}
              aria-describedby={
                fieldErrors.message ? 'message-error' : undefined
              }
            />
            {fieldErrors.message && (
              <small className="quote-field-error" id="message-error">
                {fieldErrors.message}
              </small>
            )}
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