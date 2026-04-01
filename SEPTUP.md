# ⚙️ Backend Setup – IoT App (.NET 10)

## 1. Descripción General

Este proyecto corresponde al backend de una aplicación de monitoreo IoT para flotas.
Está desarrollado en .NET 10 siguiendo principios de **arquitectura limpia**, separando responsabilidades en múltiples capas.

El sistema incluye:

- Autenticación con JWT generados manualmente
- Comunicación en tiempo real con SignalR
- Persistencia en PostgreSQL usando Entity Framework
- Soporte para migraciones de base de datos
- Hash de contraseñas con BCrypt
- Middleware personalizado para autorización por roles

---

## 2. Estructura del Proyecto

La solución está dividida en 5 proyectos:

```
IotApp (API principal)
IotUnitTest (pruebas unitarias)
Infrastructure (acceso a datos)
Domain (entidades e interfaces)
Application (lógica de negocio)
```

### 📦 IotApp

- Proyecto principal (API)
- Configuración de middlewares
- Exposición de endpoints
- Configuración de Swagger (solo en desarrollo)

---

### 🧪 IotUnitTest

- Contiene pruebas unitarias
- Validación de lógica de negocio

---

### 🗄️ Infrastructure

- Implementación de repositorios
- DbContext
- Configuración de Entity Framework
- Migraciones de base de datos

---

### 🧱 Domain

- Entidades del sistema
- Interfaces de repositorios
- Contratos base

---

### ⚙️ Application

- Servicios (lógica de negocio)
- DTOs (Data Transfer Objects)
- Responses estandarizadas
- Clases comunes

---

## 3. Requisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- .NET SDK 10
- PostgreSQL
- Node.js (opcional, si integras frontend)
- Herramienta de migraciones de EF:

```bash
dotnet tool install --global dotnet-ef
```

---

## 4. Configuración del Proyecto

### 4.1 Clonar repositorio

```bash
git clone https://github.com/mondo84/SimonMovilidadBackend.git
cd IotApp
```

---

### 4.2 Configurar cadena de conexión

Editar `appsettings.json` en el proyecto principal:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=iotdb;Username=postgres;Password=tu_password"
}
```

---

## 5. Base de Datos (Migraciones)

### Crear base de datos

Asegúrate de que PostgreSQL esté corriendo y crea la base de datos:

```sql
CREATE DATABASE iotdb;
```

---

### Aplicar migraciones

Desde la raíz del proyecto:

```bash
dotnet ef database update --project Infrastructure --startup-project IotApp
Add-Migration NombreMigracion --project Infrastructure --startup-project IotApp
Update-Database --project Infrastructure --startup-project IotApp
```

---

## 6. Ejecución del Proyecto

```bash
dotnet run --project IotApp
ó desde visual Studio seleccionas el proyecto principal que es IotApp y le das click en el boton de Https que tiene una flecha verde
```

---

## 7. Swagger (Modo Desarrollo)

Swagger debe estas habilitado únicamente en entorno de desarrollo.
pero para fines de la prueba tecnica, se ha dejado habilitado en modo produccion.

Accede a:

```
https://localhost:<puerto>/swagger/index.html
```

---

## 8. Autenticación y Seguridad

### 8.1 Hash de contraseñas

Se utiliza **BCrypt** para almacenar contraseñas de forma segura:

- Hash irreversible
- Protección contra ataques de fuerza bruta

---

### 8.2 Generación de Token

El sistema implementa generación de JWT **manual**, sin librerías externas adicionales:

- Creación de Claims personalizada
- Control total del contenido del token
- Firma del token desde el backend
- Se recomienda implementar la dependencia JWT Bearer que ya está optimizada para evitar
  dolores de cabeza.

---

### 8.3 Middleware de Autorización

Se implementa un middleware personalizado que:

- Lee los Claims del token
- Valida roles manualmente
- Controla el acceso a endpoints protegidos
- Captura excepciones de manera global. Nativa y personalizada

---

## 9. Comunicación en Tiempo Real

Se utiliza **SignalR** para:

- Enviar eventos en tiempo real
- Notificar cambios (ej: alertas)
- Mantener sincronización con el frontend

---

## 10. Dependencias Principales

- Entity Framework Core (PostgreSQL)
- SignalR
- BCrypt
- Swagger
- Xunit
- Moq

---

## 11. Buenas Prácticas Implementadas

- Separación de responsabilidades (Clean Architecture)
- Uso de DTOs para desacoplar capas
- Manejo centralizado de respuestas
- Validación de seguridad sin dependencias externas
- Uso de migraciones para versionado de base de datos

---

## 12. Ejecución de Pruebas

```bash
dotnet test (corre todas las pruebas)

```

```bash
dotnet test .\IotUnitTest\IotUnitTest.csproj --filter FullyQualifiedName~IotUnitTest.AlertServiceTest.calcula_prediccion_correcta (Independiente)
```

---

## 13. Notas Importantes

- Asegúrate de ejecutar migraciones antes de iniciar la app
- Verifica que PostgreSQL esté activo
- Swagger solo está disponible en entorno DEV (Lo dejé activo solo para fines de la prueba)
- El manejo de autenticación es completamente personalizado

---

## 14. Conclusión

Este backend está diseñado para ser:

- Escalable
- Seguro
- Modular
- Fácil de mantener

La arquitectura permite extender funcionalidades sin afectar otras capas del sistema.
