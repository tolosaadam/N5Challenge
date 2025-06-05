# 🔐 Permissions API & Frontend (React + .NET 8)

A fullstack solution including a .NET 8 Web API and a React application with TypeScript to manage permissions through a CRUD.
The API integrates Kafka for events, Elasticsearch for persistence, and optimizes user experience with caching and optimistic updates.

---

### ⚙️ Main functionality

- View list of permissions  
- Create new permissions  
- Edit existing permissions  

---

### 🚀 Key features

- Each operation (GET, POST, PUT) emits an event in Kafka with a unique GUID and the entity name as topic (e.g., `permission`, `permission-type`).  
- At the end of each operation, the full entity model is persisted in Elasticsearch.  
- The frontend app uses React Query for state and cache management, with optimistic updates to improve UX.  
- Architectural patterns used: CQRS, Unit of Work (with transactions), Repository, Pipeline Behaviors.  
- Uses AutoMapper, dependency injection, and custom middlewares in the API.  
- Unit tests implemented with MSTest on the Web API.  

---

### 🛠️ Technologies used

#### Backend
- .NET 8 (Minimal API)  
- Repository Pattern + Generic Repository  
- Unit of Work (with transactions)  
- AutoMapper  
- CQRS (with Pipeline Behaviors)  
- Dependency Injection  
- Custom Middlewares  
- Kafka (event production)  
- Elasticsearch (entity persistence)  
- Zookeeper (Kafka dependency)  
- SQL Server (relational database)  
- Unit tests with MSTest  

#### Frontend (React + TypeScript)
- React 18 + TypeScript  
- React Query (state and cache management)  
- React Hook Form (forms)  
- Material UI (UI components)  
- Axios (HTTP requests)  
- Optimistic Update (to improve UX on updates)  
- Cache key invalidation for creation  

---

### 📦 Docker Compose and ports

The project includes a `docker-compose.yml` file that brings up the following services:

| Service        | Internal Port | Description                        |
|----------------|---------------|----------------------------------|
| API (.NET)     | 8080          | Web API to manage permissions    |
| React App      | 3000          | Frontend application              |
| SQL Server     | 1433          | Database                         |
| Zookeeper      | 2181          | Kafka coordination               |
| Kafka          | 9092          | Kafka broker                    |
| Kafka UI       | 9090          | Web interface for Kafka admin   |
| Elasticsearch  | 9200          | Storage and search               |
| Kibana         | 5601          | Elasticsearch monitoring & visualization |

---

### 🔗 Service dependencies

- Kafka depends on Zookeeper for coordination.  
- Kafka UI depends on Kafka to show cluster info.  
- The API depends on Kafka, SQL Server, and Elasticsearch to fully operate.

---

### ▶️ How to run the project

From the root of the project, run the following command to start all services and the full application: `docker-compose up`.

---

### 🧪 Testing

The Web API includes unit tests written with **MSTest**.

---

### 👤 Author

**Adam Ezequiel Tolosa**  
📧 [LinkedIn](https://www.linkedin.com/in/adam-ezequiel-tolosa/)
