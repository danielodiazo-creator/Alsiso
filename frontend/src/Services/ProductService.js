import { requestJson } from './apiClient.js'

export function getProducts() {
  return requestJson('/api/products')
}

export function getProductById(id) {
  return requestJson(`/api/products/${encodeURIComponent(id)}`)
}

export function createProduct(product) {
  return requestJson('/api/products', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(product),
  })
}

export function updateProduct(id, product) {
  return requestJson(`/api/products/${encodeURIComponent(id)}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(product),
  })
}

export function deleteProduct(id) {
  return requestJson(`/api/products/${encodeURIComponent(id)}`, {
    method: 'DELETE',
  })
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