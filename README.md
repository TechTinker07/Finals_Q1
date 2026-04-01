# Finals_Q1 - Todo API

## Overview
This project serves as the backend API for the Todo Management System. It is built using .NET Web API and handles the core CRUD operations for todo items.

## Features
- Retrieve all todo items
- Create a new todo
- Update an existing todo
- Delete a todo
- Store data in memory
- Allow frontend requests through CORS
- Validate empty title inputs

## API Endpoints
- `GET /api/todos` - returns the list of todos
- `POST /api/todos` - creates a new todo
- `PUT /api/todos/{id}` - updates an existing todo
- `DELETE /api/todos/{id}` - deletes a todo

## Setup and Run
1. Open the project in Visual Studio or VS Code.
2. Restore dependencies if needed.
3. Run the project with:

```bash
dotnet run

Open Swagger in the browser to test the endpoints.
Architecture
This project follows a simple RESTful API structure using controllers and models. The Todo model represents the data, while TodosController handles the API endpoints and request processing. Data is stored in an in-memory list, which keeps the project lightweight and easy to test.

Validation and Status Codes
The API rejects empty title values and returns appropriate HTTP status codes such as:

200 OK
201 Created
204 No Content
400 Bad Request
Technical Debt Fixes
Completed all required CRUD endpoints
Added validation for empty todo titles
Configured CORS to allow requests from http://localhost:5173
