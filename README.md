# Alsiso

Sitio web para un negocio de señalización y seguridad industrial.

## Estructura

- `backend/Alsiso/Alsiso.slnx`: solución de Visual Studio.
- `backend/src/Alsiso.Api/`: API HTTP con ASP.NET Core.
- `backend/src/Alsiso.Application/`: casos de uso y contratos.
- `backend/src/Alsiso.Domain/`: entidades y reglas del negocio.
- `backend/src/Alsiso.Infrastructure/`: implementaciones técnicas y acceso a datos.
- `frontend/`: interfaz web independiente que consumirá la API.

## Backend

El backend usa .NET 10 y Clean Architecture. La API expone actualmente `GET /api/products` con un producto de ejemplo guardado en memoria. Un precio `null` representa un producto cuyo precio se consulta mediante cotización.

Para iniciar el backend desde Visual Studio, abre `backend/Alsiso/Alsiso.slnx`, selecciona `Alsiso.Api` como proyecto de inicio y ejecuta el perfil `http`.
