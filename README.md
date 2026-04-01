# Finals_Q1 - Todo API

## Setup and Run
1. Open the project in Visual Studio or VS Code.
2. Restore dependencies if needed.
3. Run the project with:

```bash
dotnet run
```

4. Open Swagger in the browser to test the endpoints.

**Architecture**

This project follows a simple RESTful API structure using controllers and models. The Todo model represents the data, while TodosController handles the API endpoints and request processing. Data is stored in an in-memory list, which keeps the project lightweight and easy to test.

**Validation and Status Codes
**

The API rejects empty title values and returns appropriate HTTP status codes such as:

200 OK
201 Created
204 No Content
400 Bad Request

**Technical Debt Fixes**

Completed all required CRUD endpoints
Added validation for empty todo titles
Configured CORS to allow requests from http://localhost:5173
