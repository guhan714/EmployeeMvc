## Running the Project with Docker

This project provides Docker support for both the Employee UI and Employee Web API services, using .NET 8.0. The setup is managed via Docker Compose for easy orchestration.

### Requirements
- Docker and Docker Compose installed
- No external database service is defined in the current setup
- .NET version: **8.0** (as specified in the Dockerfiles)

### Services and Ports
- **Employee UI**
  - Service name: `csharp-employee_ui`
  - Exposed on host port **8080** (container port 8080)
- **Employee Web API**
  - Service name: `csharp-employee_webapi`
  - Exposed on host port **8081** (container port 80)

### Environment Variables
- No required environment variables are specified in the Dockerfiles or compose file by default.
- If you need to provide environment variables, you can uncomment the `env_file` lines in the `docker-compose.yml` and provide the appropriate `.env` files in each service directory.

### Build and Run Instructions
1. Ensure the Dockerfiles for each service are present at the paths referenced in `docker-compose.yml` (e.g., `../Employee_UIDockerfile` for the UI, `../Employee_WebapiDockerfile` for the API).
2. From the root of the project, run:
   ```sh
   docker compose up --build
   ```
   This will build and start both services.

### Special Configuration
- Both services are built and run as non-root users for improved security.
- The services are connected via a custom Docker network `employee_net`.
- The UI service depends on the Web API service and will wait for it to be available.
- No database or cache service is included by default. If your application requires one, you should extend the `docker-compose.yml` accordingly.

### Accessing the Services
- **Employee UI:** [http://localhost:8080](http://localhost:8080)
- **Employee Web API:** [http://localhost:8081](http://localhost:8081)

---

_If you add a database or other services in the future, update this section to reflect those changes._
