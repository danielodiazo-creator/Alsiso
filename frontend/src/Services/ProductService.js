import { requestJson } from './apiClient.js'

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