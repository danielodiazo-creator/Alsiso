import { useEffect, useState } from 'react'
import { getProductById, getProducts } from './Services/ProductService.js'
import './App.css'

function App() {
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [selectedProduct, setSelectedProduct] = useState(null)
  const [detailLoading, setDetailLoading] = useState(false)
  const [detailError, setDetailError] = useState('')

  useEffect(() => {
    async function loadProducts() {
      try {
        const data = await getProducts()
        setProducts(data)
      } catch (err) {
        setError(err.message)
      } finally {
        setLoading(false)
      }
    }

    loadProducts()
  }, [])

  async function openProduct(product) {
    setSelectedProduct(product)
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

        <a
          className="nav-link"
          href="#productos"
          onClick={showCatalog}
        >
          Productos
        </a>
      </nav>

      <section className="hero">
        <div className="hero-content">
          <p className="eyebrow">SEÑALIZACIÓN Y SEGURIDAD INDUSTRIAL</p>
          <h1>Seguridad que se ve. Confianza que se siente.</h1>
          <p className="hero-description">
            Soluciones de señalización para proteger a las personas y orientar
            cada camino.
          </p>
          <a className="primary-button" href="#productos" onClick={showCatalog}>
            Explorar productos
          </a>
        </div>

        <div className="hero-symbol" aria-hidden="true">
          <div className="symbol-diamond">
            <span>!</span>
          </div>
        </div>
      </section>

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
                {selectedProduct.price === null
                  ? 'Solicitar cotización'
                  : `$${Number(selectedProduct.price).toFixed(2)}`}
              </strong>
            </div>
          </article>
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

          {loading && <p>Cargando productos...</p>}

          {error && (
            <p role="alert">
              No se pudieron cargar los productos: {error}
            </p>
          )}

          {!loading && !error && products.length === 0 && (
            <p>Aún no hay productos en el catálogo.</p>
          )}

          {!loading && !error && products.length > 0 && (
            <div className="product-grid">
              {products.map((product) => (
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
                        {product.price === null
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