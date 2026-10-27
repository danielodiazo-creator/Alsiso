import { useCallback, useEffect, useState } from 'react'
import { getQuoteRequests } from '../Services/quoteRequestService.js'

export function AdminQuoteRequests() {
  const [requests, setRequests] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  const loadRequests = useCallback(async () => {
    setLoading(true)
    setError('')

    try {
      const data = await getQuoteRequests()
      setRequests(data)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    loadRequests()
  }, [loadRequests])

  return (
    <section className="catalog admin-management">
      <div className="section-heading">
        <div>
          <p className="eyebrow">COTIZACIONES</p>
          <h2>Solicitudes de cotización</h2>
        </div>

        <button
          type="button"
          className="detail-button"
          onClick={loadRequests}
          disabled={loading}
        >
          {loading ? 'Actualizando...' : 'Actualizar'}
        </button>
      </div>

      {loading && <p>Cargando solicitudes...</p>}

      {error && (
        <p className="quote-error" role="alert">
          No se pudieron cargar las solicitudes: {error}
        </p>
      )}

      {!loading && !error && requests.length === 0 && (
        <p>Aún no hay solicitudes de cotización.</p>
      )}

      {!loading && !error && requests.length > 0 && (
        <div className="product-grid">
          {requests.map((request) => (
            <article className="product-card" key={request.id}>
              <div className="product-info">
                <p className="product-category">{request.productName}</p>
                <h3>{request.customerName}</h3>

                <p>
                  <strong>Correo:</strong>{' '}
                  <a href={`mailto:${request.email}`}>{request.email}</a>
                </p>

                {request.phone && (
                  <p>
                    <strong>Teléfono:</strong>{' '}
                    <a href={`tel:${request.phone}`}>{request.phone}</a>
                  </p>
                )}

                <p>
                  <strong>Cantidad:</strong> {request.quantity}
                </p>

                <p className="product-description">
                  <strong>Mensaje:</strong>{' '}
                  {request.message || 'Sin mensaje adicional.'}
                </p>

                <small>
                  Recibida:{' '}
                  {new Date(request.createdAt).toLocaleString('es')}
                </small>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}