import { useCallback, useEffect, useState } from 'react'
import { getProductById, getProducts } from './Services/ProductService.js'
import {
  getCurrentAdmin,
  loginAdmin,
  logoutAdmin,
} from './Services/authService.js'
import { AdminLoginForm } from './Components/AdminLoginForm.jsx'
import { AdminProductManager } from './Components/AdminProductManager.jsx'
import { QuoteRequestForm } from './Components/QuoteRequestForm.jsx'
import { WhatsAppButton } from './Components/WhatsAppButton.jsx'
import './App.css'

function App() {
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [selectedProduct, setSelectedProduct] = useState(null)
  const [detailLoading, setDetailLoading] = useState(false)
  const [detailError, setDetailError] = useState('')
  const [quoteFormOpen, setQuoteFormOpen] = useState(false)

  const [searchTerm, setSearchTerm] = useState('')
  const [selectedCategory, setSelectedCategory] = useState('')
  const [sortOrder, setSortOrder] = useState('name-asc')

  const [currentAdmin, setCurrentAdmin] = useState(null)
  const [authLoading, setAuthLoading] = useState(true)
  const [loginOpen, setLoginOpen] = useState(false)
  const [authError, setAuthError] = useState('')

  const loadProducts = useCallback(async () => {
    setLoading(true)
    setError('')

    try {
      const data = await getProducts()
      setProducts(data)
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    loadProducts()
  }, [loadProducts])

  useEffect(() => {
    let isMounted = true

    async function checkAdminSession() {
      try {
        const admin = await getCurrentAdmin()
        if (isMounted) setCurrentAdmin(admin)
      } catch {
        if (isMounted) setCurrentAdmin(null)
      } finally {
        if (isMounted) setAuthLoading(false)
      }
    }

    checkAdminSession()

    return () => {
      isMounted = false
    }
  }, [])

  const categories = [
    ...new Set(products.map((product) => product.category).filter(Boolean)),
  ].sort((a, b) => a.localeCompare(b, 'es'))

  const normalizedSearch = searchTerm.trim().toLocaleLowerCase('es')

  const filteredProducts = products.filter((product) => {
    const matchesCategory =
      selectedCategory === '' || product.category === selectedCategory

    const searchableText = [
      product.name,
      product.description,
      product.category,
    ]
      .filter(Boolean)
      .join(' ')
      .toLocaleLowerCase('es')

    const matchesSearch =
      normalizedSearch === '' || searchableText.includes(normalizedSearch)

    return matchesCategory && matchesSearch
  })

  const sortedProducts = [...filteredProducts].sort((a, b) => {
    if (sortOrder === 'price-asc' || sortOrder === 'price-desc') {
      const priceA = a.price == null ? null : Number(a.price)
      const priceB = b.price == null ? null : Number(b.price)

      if (priceA === null && priceB === null) return 0
      if (priceA === null) return 1
      if (priceB === null) return -1

      return sortOrder === 'price-asc'
        ? priceA - priceB
        : priceB - priceA
    }

    const nameA = (a.name ?? '').toLocaleLowerCase('es')
    const nameB = (b.name ?? '').toLocaleLowerCase('es')

    return sortOrder === 'name-asc'
      ? nameA.localeCompare(nameB, 'es')
      : nameB.localeCompare(nameA, 'es')
  })

  async function openProduct(product) {
    setSelectedProduct(product)
    setQuoteFormOpen(false)
    setDetailError('')
    setDetailLoading(true)

    try {
      const productDetails = await getProductById(product.id)
      setSelectedProduct(productDetails)
    } catch (err) {
      setDetailError(err.message)
    } finally {
      setDetailLoading(false)
    }
  }

  function showCatalog() {
    setSelectedProduct(null)
    setDetailError('')
    setQuoteFormOpen(false)
  }

  function clearFilters() {
    setSearchTerm('')
    setSelectedCategory('')
  }

  async function handleAdminLogin(email, password) {
    const admin = await loginAdmin(email, password)
    setCurrentAdmin(admin)
    setLoginOpen(false)
  }

  async function handleAdminLogout() {
    setAuthError('')

    try {
      await logoutAdmin()
      setCurrentAdmin(null)
      setLoginOpen(false)
    } catch (err) {
      setAuthError(err.message)
    }
  }

  return (
    <main>
      <nav className="navbar">
        <a
          className="brand"
          href="#"
          onClick={(event) => {
            event.preventDefault()
            showCatalog()
          }}
        >
          <span className="brand-mark">A</span>
          <span>ALSISO</span>
        </a>

        <a className="nav-link" href="#productos" onClick={showCatalog}>
          Productos
        </a>

        {authLoading ? (
          <span>Verificando sesión...</span>
        ) : currentAdmin ? (
          <button
            type="button"
            className="detail-button"
            onClick={handleAdminLogout}
          >
            Cerrar sesión
          </button>
        ) : (
          <button
            type="button"
            className="detail-button"
            onClick={() => {
              setAuthError('')
              setLoginOpen(true)
            }}
          >
            Acceso administrador
          </button>
        )}
      </nav>

      <section className="hero">
        <div className="hero-content">
          <p className="eyebrow">SEÑALIZACIÓN Y SEGURIDAD INDUSTRIAL</p>
          <h1>Seguridad que se ve. Confianza que se siente.</h1>
          <p className="hero-description">
            Soluciones de señalización para proteger a las personas y orientar
            cada camino.
          </p>
          <a
            className="primary-button"
            href="#productos"
            onClick={showCatalog}
          >
            Explorar productos
          </a>
        </div>

        <div className="hero-symbol" aria-hidden="true">
          <div className="symbol-diamond">
            <span>!</span>
          </div>
        </div>
      </section>

      {loginOpen && !currentAdmin && (
        <AdminLoginForm
          onLogin={handleAdminLogin}
          onCancel={() => setLoginOpen(false)}
        />
      )}

      {authError && (
        <p className="quote-error" role="alert">
          {authError}
        </p>
      )}

      {currentAdmin && (
        <AdminProductManager
          products={products}
          onProductsChanged={loadProducts}
        />
      )}

      {selectedProduct ? (
        <section className="catalog" id="productos">
          <button
            type="button"
            className="detail-button back-button"
            onClick={showCatalog}
          >
            ← Volver al catálogo
          </button>

          {detailLoading && <p>Consultando detalle del producto...</p>}

          {detailError && (
            <p role="alert">
              No se pudo actualizar el detalle desde la API: {detailError}
            </p>
          )}

          <article className="product-detail-card">
            <div className="detail-art" aria-hidden="true">
              <div className="mini-sign">50</div>
            </div>

            <div className="detail-content">
              <p className="product-category">{selectedProduct.category}</p>
              <h2>{selectedProduct.name}</h2>
              <p className="product-description">
                {selectedProduct.description}
              </p>

              <strong className="detail-price">
                {selectedProduct.price == null
                  ? 'Solicitar cotización'
                  : `$${Number(selectedProduct.price).toFixed(2)}`}
              </strong>
            </div>
          </article>

          <div className="detail-actions">
            <button
              type="button"
              className="primary-button quote-open-button"
              onClick={() => setQuoteFormOpen(true)}
            >
              Solicitar cotización
            </button>

            <WhatsAppButton productName={selectedProduct.name} />
          </div>

          {quoteFormOpen && (
            <QuoteRequestForm
              product={selectedProduct}
              onClose={() => setQuoteFormOpen(false)}
            />
          )}
        </section>
      ) : (
        <section className="catalog" id="productos">
          <div className="section-heading">
            <div>
              <p className="eyebrow">NUESTRO CATÁLOGO</p>
              <h2>Productos destacados</h2>
            </div>

            <p className="catalog-intro">
              Encuentra soluciones para señalizar y proteger tus espacios.
            </p>
          </div>

          {!loading && !error && products.length > 0 && (
            <div className="catalog-filters">
              <label className="filter-field">
                <span>Buscar productos</span>
                <input
                  type="search"
                  value={searchTerm}
                  onChange={(event) => setSearchTerm(event.target.value)}
                  placeholder="Ej.: velocidad, seguridad..."
                />
              </label>

              <label className="filter-field">
                <span>Categoría</span>
                <select
                  value={selectedCategory}
                  onChange={(event) => setSelectedCategory(event.target.value)}
                >
                  <option value="">Todas las categorías</option>
                  {categories.map((category) => (
                    <option key={category} value={category}>
                      {category}
                    </option>
                  ))}
                </select>
              </label>

              <label className="filter-field">
                <span>Ordenar por</span>
                <select
                  value={sortOrder}
                  onChange={(event) => setSortOrder(event.target.value)}
                >
                  <option value="name-asc">Nombre: A a Z</option>
                  <option value="name-desc">Nombre: Z a A</option>
                  <option value="price-asc">Precio: menor a mayor</option>
                  <option value="price-desc">Precio: mayor a menor</option>
                </select>
              </label>

              <button
                type="button"
                className="clear-filters-button"
                onClick={clearFilters}
                disabled={!searchTerm && !selectedCategory}
              >
                Limpiar filtros
              </button>
            </div>
          )}

          {loading && <p>Cargando productos...</p>}

          {error && (
            <p role="alert">No se pudieron cargar los productos: {error}</p>
          )}

          {!loading && !error && products.length === 0 && (
            <p>Aún no hay productos en el catálogo.</p>
          )}

          {!loading &&
            !error &&
            products.length > 0 &&
            sortedProducts.length === 0 && (
              <p>No hay productos que coincidan con esos filtros.</p>
            )}

          {!loading && !error && sortedProducts.length > 0 && (
            <div className="product-grid">
              {sortedProducts.map((product) => (
                <article className="product-card" key={product.id}>
                  <div className="product-illustration" aria-hidden="true">
                    <div className="mini-sign">50</div>
                  </div>

                  <div className="product-info">
                    <p className="product-category">{product.category}</p>
                    <h3>{product.name}</h3>
                    <p className="product-description">
                      {product.description}
                    </p>

                    <div className="product-footer">
                      <strong>
                        {product.price == null
                          ? 'Solicitar cotización'
                          : `$${Number(product.price).toFixed(2)}`}
                      </strong>

                      <button
                        type="button"
                        className="detail-button"
                        onClick={() => openProduct(product)}
                      >
                        Ver producto
                      </button>
                    </div>
                  </div>
                </article>
              ))}
            </div>
          )}
        </section>
      )}

      <footer className="footer">
        <p>ALSISO · Señalización vial y seguridad industrial</p>
      </footer>
    </main>
  )
}

export default App