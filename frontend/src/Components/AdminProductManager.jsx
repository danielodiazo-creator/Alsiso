import { useState } from 'react'
import {
  createProduct,
  deleteProduct,
  updateProduct,
} from '../Services/ProductService.js'

const emptyProduct = {
  name: '',
  description: '',
  category: '',
  price: '',
}

export function AdminProductManager({ products, onProductsChanged }) {
  const [formValues, setFormValues] = useState(emptyProduct)
  const [editingProductId, setEditingProductId] = useState(null)
  const [isSaving, setIsSaving] = useState(false)
  const [deletingProductId, setDeletingProductId] = useState(null)
  const [error, setError] = useState('')
  const [notice, setNotice] = useState('')

  function handleChange(event) {
    const { name, value } = event.target

    setFormValues((currentValues) => ({
      ...currentValues,
      [name]: value,
    }))

    setError('')
    setNotice('')
  }

  function startEditing(product) {
    setEditingProductId(product.id)
    setFormValues({
      name: product.name,
      description: product.description ?? '',
      category: product.category,
      price: product.price == null ? '' : String(product.price),
    })
    setError('')
    setNotice('')
  }

  function cancelEditing() {
    setEditingProductId(null)
    setFormValues(emptyProduct)
    setError('')
    setNotice('')
  }

  async function handleSubmit(event) {
    event.preventDefault()
    setError('')
    setNotice('')

    const productData = {
      name: formValues.name.trim(),
      description: formValues.description.trim(),
      category: formValues.category.trim(),
      price: formValues.price === '' ? null : Number(formValues.price),
    }

    setIsSaving(true)

    try {
      if (editingProductId) {
        await updateProduct(editingProductId, productData)
        setNotice('Producto actualizado correctamente.')
      } else {
        await createProduct(productData)
        setNotice('Producto creado correctamente.')
      }

      await onProductsChanged()
      setFormValues(emptyProduct)
      setEditingProductId(null)
    } catch (err) {
      setError(err.message)
    } finally {
      setIsSaving(false)
    }
  }

  async function handleDelete(product) {
    const confirmed = window.confirm(
      `¿Seguro que quieres eliminar "${product.name}"?`,
    )

    if (!confirmed) return

    setError('')
    setNotice('')
    setDeletingProductId(product.id)

    try {
      await deleteProduct(product.id)

      if (editingProductId === product.id) {
        cancelEditing()
      }

      await onProductsChanged()
      setNotice('Producto eliminado correctamente.')
    } catch (err) {
      setError(err.message)
    } finally {
      setDeletingProductId(null)
    }
  }

  return (
    <section className="catalog admin-management">
      <div className="section-heading">
        <div>
          <p className="eyebrow">ADMINISTRACIÓN</p>
          <h2>Gestionar productos</h2>
        </div>
        <p className="catalog-intro">
          Añade nuevos productos o actualiza los que ya están en el catálogo.
        </p>
      </div>

      <form className="quote-form admin-product-form" onSubmit={handleSubmit}>
        <label>
          Nombre
          <input
            name="name"
            type="text"
            value={formValues.name}
            onChange={handleChange}
            required
          />
        </label>

        <label>
          Categoría
          <input
            name="category"
            type="text"
            value={formValues.category}
            onChange={handleChange}
            required
          />
        </label>

        <label className="admin-product-description">
          Descripción
          <textarea
            name="description"
            rows={3}
            value={formValues.description}
            onChange={handleChange}
          />
        </label>

        <label>
          Precio
          <input
            name="price"
            type="number"
            min="0"
            step="0.01"
            value={formValues.price}
            onChange={handleChange}
          />
        </label>

        <p className="admin-form-hint">
          Puedes dejar el precio vacío si quieres que el cliente solicite una
          cotización.
        </p>

        {error && (
          <p className="quote-error" role="alert">
            {error}
          </p>
        )}

        {notice && (
          <p className="quote-success" role="status">
            {notice}
          </p>
        )}

        <div className="admin-product-actions">
          <button
            className="primary-button quote-submit"
            type="submit"
            disabled={isSaving}
          >
            {isSaving
              ? 'Guardando...'
              : editingProductId
                ? 'Guardar cambios'
                : 'Añadir producto'}
          </button>

          {editingProductId && (
            <button
              type="button"
              className="detail-button"
              onClick={cancelEditing}
              disabled={isSaving}
            >
              Cancelar edición
            </button>
          )}
        </div>
      </form>

      <h3 className="admin-product-list-heading">Productos existentes</h3>

      {products.length === 0 ? (
        <p>Aún no hay productos en el catálogo.</p>
      ) : (
        <div className="product-grid">
          {products.map((product) => (
            <article className="product-card" key={product.id}>
              <div className="product-info">
                <p className="product-category">{product.category}</p>
                <h3>{product.name}</h3>
                <p className="product-description">
                  {product.description || 'Sin descripción.'}
                </p>

                <div className="product-footer">
                  <strong>
                    {product.price == null
                      ? 'Solicitar cotización'
                      : `$${Number(product.price).toFixed(2)}`}
                  </strong>

                  <div className="admin-product-actions">
                    <button
                      type="button"
                      className="detail-button"
                      onClick={() => startEditing(product)}
                      disabled={deletingProductId !== null}
                    >
                      Editar
                    </button>

                    <button
                      type="button"
                      className="admin-delete-button"
                      onClick={() => handleDelete(product)}
                      disabled={deletingProductId === product.id}
                    >
                      {deletingProductId === product.id
                        ? 'Eliminando...'
                        : 'Eliminar'}
                    </button>
                  </div>
                </div>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}