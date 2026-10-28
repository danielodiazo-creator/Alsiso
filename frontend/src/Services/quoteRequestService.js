import { requestJson } from './apiClient.js'

export function getQuoteRequests() {
  return requestJson('/api/quote-requests')
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

export function updateQuoteRequestStatus(id, status) {
  return requestJson(
    `/api/quote-requests/${encodeURIComponent(id)}/status`,
    {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ status }),
    },
  )
}