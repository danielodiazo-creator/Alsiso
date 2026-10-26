const API_URL = 'http://localhost:5063'

async function requestJson(path, options = {}) {
  const response = await fetch(`${API_URL}${path}`, options)

  if (!response.ok) {
    throw new Error(`Error de la API: ${response.status}`)
  }

  return response.json()
}

export function getProducts() {
  return requestJson('/api/products')
}

export function getProductById(id) {
  return requestJson(`/api/products/${encodeURIComponent(id)}`)
}

export function createQuoteRequest(quoteRequest) {
  return requestJson('/api/quote-requests', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(quoteRequest),
  })
}