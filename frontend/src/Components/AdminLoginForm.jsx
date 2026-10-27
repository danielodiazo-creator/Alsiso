import { useState } from 'react'

export function AdminLoginForm({ onLogin, onCancel }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [sending, setSending] = useState(false)

  async function handleSubmit(event) {
    event.preventDefault()
    setError('')
    setSending(true)

    try {
      await onLogin(email.trim(), password)
      setPassword('')
    } catch (err) {
      setError(err.message)
    } finally {
      setSending(false)
    }
  }

  return (
    <section className="quote-panel">
      <div className="quote-heading">
        <div>
          <p className="eyebrow">ÁREA PRIVADA</p>
          <h3>Acceso de administrador</h3>
        </div>

        <button type="button" className="detail-button" onClick={onCancel}>
          Cerrar
        </button>
      </div>

      <form className="quote-form admin-login-form" onSubmit={handleSubmit}>
        <label>
          Correo electrónico
          <input
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            autoComplete="username"
            required
          />
        </label>

        <label>
          Contraseña
          <input
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            autoComplete="current-password"
            required
          />
        </label>

        {error && (
          <p className="quote-error" role="alert">
            No se pudo iniciar sesión: {error}
          </p>
        )}

        <button
          className="primary-button quote-submit"
          type="submit"
          disabled={sending}
        >
          {sending ? 'Iniciando sesión...' : 'Iniciar sesión'}
        </button>
      </form>
    </section>
  )
}