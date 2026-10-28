import { useCallback, useEffect, useState } from 'react'
import {
  getQuoteRequests,
  updateQuoteRequestStatus,
} from '../Services/quoteRequestService.js'

const statusLabels = {
  0: 'Nueva',
  1: 'Contactada',
  2: 'Cerrada',
}

export function AdminQuoteRequests() {
  const [requests, setRequests] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [actionError, setActionError] = useState('')
  const [actionMessage, setActionMessage] = useState('')
  const [updatingId, setUpdatingId] = useState('')

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

  async function handleStatusChange(requestId, event) {
    const status = Number(event.target.value)

    setActionError('')
    setActionMessage('')
    setUpdatingId(requestId)

    try {
      const updatedRequest = await updateQuoteRequestStatus(requestId, status)

      setRequests((currentRequests) =>
        currentRequests.map((request) =>
          request.id === updatedRequest.id ? updatedRequest : request,
        ),
      )

      setActionMessage('Estado actualizado.')
    } catch (err) {
      setActionError(err.message)
    } finally {
      setUpdatingId('')
    }
  }

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

      {actionError && (
        <p className="quote-error" role="alert">
          No se pudo actualizar el estado: {actionError}
        </p>
      )}

      {actionMessage && <p role="status">{actionMessage}</p>}

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

                <p>
                  <strong>Estado:</strong>{' '}
                  {statusLabels[request.status] ?? 'Desconocido'}
                </p>

                <label>
                  Cambiar estado:{' '}
                  <select
                    value={request.status}
                    onChange={(event) =>
                      handleStatusChange(request.id, event)
                    }
                    disabled={updatingId === request.id}
                    aria-label={`Cambiar estado de la solicitud de ${request.customerName}`}
                  >
                    <option value={0}>Nueva</option>
                    <option value={1}>Contactada</option>
                    <option value={2}>Cerrada</option>
                  </select>
                </label>

                {updatingId === request.id && <p>Guardando estado...</p>}

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