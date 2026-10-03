# Inventario de Equipos Celulares

Aplicación web para registrar, gestionar y monitorear el inventario de equipos celulares:
control de stock, movimientos y reportes de dispositivos.

## Enlaces de la entrega

| Entregable | Enlace |
| --- | --- |
| Aplicación publicada | https://inventario-celulares-beta.vercel.app |
| API publicada | https://afford-performs-characteristic-poet.trycloudflare.com |
| Repositorio | https://github.com/UPT-FAING-EPIS/si784-2026-ii-si784-2026-ii-examen-u1-korins707 |
| SonarQube | Configurar la organización en https://sonarcloud.io y luego los secrets `SONAR_TOKEN`, `SONAR_ORG`, `SONAR_PROJECT_KEY` |

## Arquitectura

- **Backend:** .NET 8 Web API con EF Core 8 y PostgreSQL 16.
- **Frontend:** React 18 + Vite + TypeScript estricto, routed con React Router.
- **Base de datos:** MySQL 8.4 (relacional), con migraciones de EF Core aplicadas al arrancar.
- **Despliegue:** frontend estático en Vercel; backend en contenedor Docker.

## Estructura del repositorio

```
backend/
  Inventario.Celulares.Api/            Controllers, middleware, seed y composition root
  Inventario.Celulares.Core/           Entidades, enumeraciones, DTOs, validacion y contratos
  Inventario.Celulares.Infrastructure/ EF Core, configuraciones, servicio de negocio, hashing
  Inventario.Celulares.Tests/          55 pruebas: unitarias y de integracion HTTP
frontend/
  src/components/                      Layout, badges, alertas, modal de confirmacion
  src/pages/                           Dashboard, DeviceForm, DeviceHistory, Movements,
                                       Reports, AdminPanel
  src/services/api.ts                  Cliente HTTP con manejo de errores tipado
  public/config.js                     URL de la API configurable sin recompilar
infra/                                 Terraform para Azure
docs/                                  Diccionario de datos y diagramas Mermaid
scripts/                               Generador de documentacion tecnica
.github/workflows/                     5 automatizaciones de CI/CD
```

## Endpoints REST

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| `POST` | `/devices` | Registra un nuevo equipo |
| `GET` | `/devices` | Lista equipos con filtros y paginacion |
| `GET` | `/devices/{id}` | Detalle de un equipo |
| `PUT` | `/devices/{id}` | Actualiza un equipo |
| `DELETE` | `/devices/{id}` | Elimina un equipo |
| `POST` | `/movements` | Registra un movimiento de inventario |
| `GET` | `/movements?deviceId={id}` | Historial de movimientos |
| `GET` | `/reports/stock` | Reporte de stock actual |
| `GET` | `/locations` | Lista de ubicaciones |
| `GET` | `/health` | Estado del servicio |

Documentacion interactiva de la API: `/swagger` en desarrollo.

## Validacion de datos

La validacion existe **en ambos extremos**, como pide el enunciado.

**Frontend** (`frontend/src/pages/DeviceForm.tsx`): la funcion `validar` comprueba marca,
modelo, formato de IMEI y ubicacion antes de llamar a la API, y muestra el error bajo el campo.

**Backend**:
- DataAnnotations en los DTO (`[Required]`, `[StringLength]`, `[RegularExpression]`, `[Range]`).
- Reglas de negocio en `InventoryService`: IMEI unico con digito verificador Luhn
  (`ImeiValidator`), existencia de la ubicacion, coherencia de movimientos y transiciones
  de estado.
- `ExceptionHandlingMiddleware` traduce cualquier excepcion a una respuesta JSON consistente
  con codigo y mensaje.

## Pruebas

```bash
cd backend
dotnet test
```

- **37 pruebas unitarias**: validador de IMEI, hashing PBKDF2 y reglas de inventario
  sobre SQLite en memoria.
- **18 pruebas de integracion**: `WebApplicationFactory` contra los endpoints HTTP reales
  (codigos 200, 201, 204, 400, 404, 409),Swagger y migraciones.

Resultado actual: **55/55 en verde**.

Dos defectos reales fueron encontrados por estas pruebas durante el desarrollo:

1. `ImeiValidator` no comprobaba la longitud antes de indexar, por lo que un IMEI de
   14 digitos lanzaba `IndexOutOfRangeException` en lugar de returning `false`.
2. La API no aceptaba enums como texto (`"Disponible"`), que es exactamente lo que envia
   el frontend. Se corrigio con `JsonStringEnumConverter`.

## Ejecucion local

```bash
docker compose up -d --build
```

- Frontend: `http://localhost:5173`
- API: `http://localhost:5080`
- PostgreSQL: `localhost:55432`

Las migraciones se aplican automaticamente al arrancar la API.

## Automatizaciones de CI/CD

| Workflow | Proposito |
| --- | --- |
| `sonar.yml` | Analisis de calidad del backend y del frontend |
| `snyk-semgrep.yml` | SAST con Semgrep, dependencias con Snyk e imagen del contenedor |
| `deploy.yml` | Construye, prueba y publica backend y frontend |
| `infra.yml` | `terraform validate`, `plan`, `tfsec` e Infracost |
| `generate-documentation.yml` | Regenera el diccionario de datos y los diagramas Mermaid |

### Secrets necesarios en GitHub Actions

| Secret | Para |
| --- | --- |
| `SONAR_TOKEN`, `SONAR_ORG`, `SONAR_PROJECT_KEY` | `sonar.yml` |
| `SNYK_TOKEN` | `snyk-semgrep.yml` |
| `INFRACOST_API_KEY` | `infra.yml` (estimacion de costo) |
| `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID` | `deploy.yml` y `infra.yml` |
| `ACR_NAME`, `ACR_USERNAME`, `ACR_PASSWORD` | Registro de contenedores |
| `CONNECTION_STRING`, `FRONTEND_URL` | Configuracion del contenedor en produccion |
| `VERCEL_TOKEN`, `VERCEL_ORG_ID`, `VERCEL_PROJECT_ID` | `deploy.yml` |

Los workflows fallan con un mensaje explicito si falta un secret, en lugar de errorrej obscure.

## Infraestructura (Terraform)

`infra/` aprovisiona en Azure: grupo de recursos, Azure Container Registry,
Azure Container Instances para la API, PostgreSQL Flexible Server con firewall restringido,
identidad administrada, Network Security Group y Log Analytics.

Verificado: `terraform validate` correcto y `tfsec` con **0 hallazgos criticos y 0 altos**.

## Documentacion tecnica

- `docs/diccionario-de-datos.md` — tablas, columnas, tipos, claves e indices.
- `docs/diagramas/diagrama-entidad-relacion.mmd`
- `docs/diagramas/diagrama-clases.mmd`
- `docs/diagramas/diagrama-componentes.mmd`
- `docs/diagramas/diagrama-despliegue.mmd`
- `docs/api/openapi.json`

Se regeneran con:

```bash
node scripts/generar-documentacion.mjs
```

## Seguridad

- Contrasenas con PBKDF2-HMAC-SHA256, 100 000 iteraciones y sal por usuario.
- Las contrasenas nunca se almacenan en texto plano.
- El contenedor ejecuta como usuario no root y expone un endpoint de salud.
- `.env` esta en `.gitignore`; solo se versiona `.env.example`.
- Sin credenciales en el codigo ni en los workflows.