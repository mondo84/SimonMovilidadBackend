# 📊 Dashboard de Monitoreo IoT – Documento de Diseño

## 1. Descripción General

Este proyecto consiste en un dashboard para el monitoreo en tiempo real de dispositivos IoT dentro de una flota.
Permite visualizar información como ubicación, nivel de combustible, temperatura y alertas generadas por los dispositivos.

El sistema está diseñado para:

- Manejar datos en tiempo real
- Ofrecer una interfaz rápida y responsiva

---

## 2. Arquitectura

La aplicación sigue una arquitectura cliente-servidor:

- **Frontend**: Next.js (React)
- **Backend**: API en .NET version 10
- **Tiempo real**: SignalR
- **Almacenamiento local**: IndexedDB
- **Persistencia**: PostgreSql

### Flujo de datos

1. Los dispositivos envían datos al backend
2. El backend procesa y almacena la información
3. El frontend consulta los datos mediante la API
4. SignalR envía actualizaciones en tiempo real
5. IndexedDB guarda datos para uso offline

---

## 3. Stack Tecnológico

### Frontend

- **Next.js (App Router)**
  Elegido por su soporte moderno de React y capacidades de renderizado optimizado.

- **React Query (TanStack Query)**
  Utilizado para manejar el estado del servidor y las mutaciones.

- **Leaflet**
  Para la visualización de mapas y ubicación de dispositivos.

---

### Backend

- **.NET (Arquitectura Limpia)**
  Separación en capas: Domain, Application, Infrastructure.

- **SignalR**
  Permite comunicación en tiempo real entre servidor y cliente.

---

### Almacenamiento

- **IndexedDB (usando idb)**
  Se utiliza como caché local para soportar funcionamiento offline.
  PostgreSql como base de datos persistente.

---

## 5. Trade-offs (Compensaciones Técnicas)

### 5.1 Uso de `useMutation` en lugar de `useQuery`

**Ventajas:**

- Control total sobre las peticiones
- Ideal para eventos controlados (reconexión, acciones del usuario)

**Desventajas:**

- No hay cache automático
- Se requiere más lógica manual
- Mayor complejidad en manejo de estado

---

### 5.2 Uso de IndexedDB

**Ventajas:**

- Persistencia real de datos
- Soporte offline robusto
- Manejo de mayor volumen de información

**Desventajas:**

- Mayor complejidad de implementación
- Necesidad de manejar duplicados y sincronización
- Debug más complejo

---

### 5.3 Mezcla de tiempo real + modo offline

**Ventajas:**

- Mejor experiencia de usuario
- Resiliencia ante fallos de red

**Desventajas:**

- Mayor complejidad arquitectónica
- Múltiples fuentes de datos (API, SignalR, IndexedDB)
- Posibles inconsistencias si no se controla correctamente

---

## 6. Estrategia Offline

Cuando la aplicación detecta pérdida de conexión:

- Se cargan los datos desde IndexedDB
- La UI utiliza la información almacenada

Cuando vuelve la conexión:

1. Se obtienen los datos actualizados del backend
2. Se actualiza la UI
3. Se sincroniza la base local

---

## 7. Conclusión

El sistema fue diseñado priorizando:

- Visualización en tiempo real
- Escalabilidad en entornos IoT
