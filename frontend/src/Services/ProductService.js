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

export function uploadProductImage(id, imageFile) {
  const formData = new FormData()
  formData.append('image', imageFile)

  return requestJson(
    `/api/products/${encodeURIComponent(id)}/image`,
    {
      method: 'POST',
      body: formData,
    },
  )
}

export function deleteProductImage(id) {
  return requestJson(
    `/api/products/${encodeURIComponent(id)}/image`,
    {
      method: 'DELETE',
    },
  )
}
  