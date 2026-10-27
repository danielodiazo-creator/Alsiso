const API_URL = 'http://localhost:5063'

export async function requestJson(path, options = {}) {
  let response

  try {
    response = await fetch(`${API_URL}${path}`, {
      credentials: 'include',
      ...options,
    })
  } catch {
    throw new Error('No se pudo conectar con la API.')
  }

  const data =
    response.status === 204 ? null : await response.json().catch(() => null)

  if (!response.ok) {
    throw new Error(data?.message || `Error de la API: ${response.status}`)
  }

  return data
}