# 🔐 Permissions API & Frontend (React + .NET 8)

Una solución fullstack que incluye una Web API en .NET 8 y una aplicación frontend en React con TypeScript para gestionar permisos a través de un CRUD.
La API integra Kafka para eventos, Elasticsearch para persistencia, y optimiza la experiencia del usuario con caché y actualizaciones optimistas.


---


### ⚙️ Funcionalidad principal

- Visualizar lista de permisos  
- Crear nuevos permisos  
- Editar permisos existentes  


---


### 🚀 Características destacadas

- Cada operación (GET, POST, PUT) emite un evento en Kafka con un GUID único y el nombre de la entidad como tópico (ejemplo: `permission`, `permission-type`).  
- Al finalizar una operación, el modelo completo de la entidad se persiste en Elasticsearch.  
- La aplicación frontend utiliza React Query para manejo de caché y actualizaciones optimistas para mejorar la UX.  
- Patrones arquitectónicos usados: CQRS, Unit of Work (con transacciones), Repository, Pipeline Behaviors.  
- Uso de AutoMapper, inyección de dependencias y middlewares personalizados en la API.  
- Tests unitarios implementados con MSTest en la Web API.


---


### 🛠️ Tecnologías utilizadas

#### Backend
- .NET 8 (Minimal API)
- Repository Pattern + Generic Repository
- Unit of Work (con transacciones)
- AutoMapper
- CQRS (con Pipeline Behaviors)
- Inyección de dependencias
- Middlewares personalizados
- Kafka (producción de eventos)
- Elasticsearch (persistencia de entidades)
- Zookeeper (dependencia de Kafka)
- SQL Server (base de datos relacional)
- Unit tests con MSTest

#### Frontend (React + TypeScript)
- React 18 + TypeScript
- React Query (manejo de estado y caché)
- React Hook Form (formularios)
- Material UI (componentes de interfaz)
- Axios (requests HTTP)
- Optimistic Update (para mejorar UX en actualizaciones)
- Invalidación de cacheKeys para creación
  

---


### 📦 Docker Compose y puertos

El proyecto incluye un archivo `docker-compose.yml` que levanta los siguientes servicios:

| Servicio       | Puerto externo | Descripción                         |
|----------------|----------------|-----------------------------------|
| API (.NET)     | 8080           | Web API para gestionar permisos   |
| React App      | 3000           | Aplicación frontend                |
| SQL Server     | 1433           | Base de datos                     |
| Zookeeper      | 2181           | Coordinación Kafka                |
| Kafka          | 9092           | Broker Kafka                     |
| Kafka UI       | 9090           | Interfaz web para administración Kafka |
| Elasticsearch  | 9200           | Almacenamiento y búsqueda         |
| Kibana         | 5601           | Visualización y monitoreo de Elasticsearch |


---


### 🔗 Dependencias entre servicios

- Kafka depende de Zookeeper para coordinación.  
- Kafka UI depende de Kafka para mostrar la información del cluster.  
- La API depende de Kafka, SQL Server y Elasticsearch para su funcionamiento completo.
- 

---


### ▶️ Cómo correr el proyecto

Desde la raíz del proyecto, ejecutar el siguiente comando para levantar todos los servicios y la aplicación completa: 'docker-compose up'.


---


### 🧪 Testing

La Web API cuenta con tests unitarios escritos en **MSTest**.


---


### 👤 Autor

**Adam Ezequiel Tolosa**  
📧 [LinkedIn](https://www.linkedin.com/in/adam-ezequiel-tolosa/).
