import { requestJson } from './apiClient.js'

export function loginAdmin(email, password) {
  return requestJson('/api/auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password }),
  })
}

export function getCurrentAdmin() {
  return requestJson('/api/auth/me')
}

export function logoutAdmin() {
  return requestJson('/api/auth/logout', {
    method: 'POST',
  })
}