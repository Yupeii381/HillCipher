# AGENTS.md - Development History

This document tracks significant changes and improvements made by AI agents and developers to the HillCipher project.

---

## 2025-11-16 - Initial Server Implementation

**PR #1: ServerOnly**
- **Author**: Yupeii381
- **Changes**: Complete server-side implementation of the Hill Cipher cryptography service

### Core Features Added

#### Authentication System
- JWT-based authentication with Bearer token support
- User registration and login endpoints
- Password change functionality with token versioning
- BCrypt password hashing for security
- Claims-based authorization using ASP.NET Core Identity

#### Hill Cipher Cryptography
- **HillCipherService**: Implementation of Hill cipher encryption/decryption algorithm
  - Matrix-based encryption using modular arithmetic
  - Support for custom alphabets
  - Automatic padding for plaintext blocks
  - Matrix-vector multiplication with modular reduction
  
- **HillKeyService**: Key management and validation
  - Key matrix generation from string keys
  - Matrix inversion with modular arithmetic
  - Input validation for plaintext, keys, and alphabets
  - Mathematical operations: determinant calculation, modular inverse, matrix cofactors

#### Text Management API
- CRUD operations for encrypted/decrypted text storage
- User-specific text storage and retrieval
- Text encryption and decryption endpoints
- Integration with Hill cipher service for cryptographic operations

#### Request History Tracking
- Automatic logging of all API requests
- Captures user actions, input/output text, and success/failure status
- Asynchronous history recording to avoid blocking requests
- Per-user history tracking

#### Database Layer
- PostgreSQL integration using Entity Framework Core
- Database models:
  - `UserEntity`: User accounts with password hashing
  - `TextEntity`: Stored text content per user
  - `RequestHistory`: Audit trail of API operations
- Repository pattern implementation
- Database migrations for schema management

#### API Infrastructure
- RESTful API design with proper HTTP status codes
- Swagger/OpenAPI documentation with JWT authentication support
- Global exception handling middleware
- Request/response logging
- Standardized API response format (`ApiResponse<T>`)

#### Development Environment
- Docker support with Dockerfile
- VS Code debug and task configurations
- Comprehensive .gitignore and .gitattributes
- Launch settings for HTTPS development

### Technical Stack
- **Framework**: ASP.NET Core 8.0
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Password Hashing**: BCrypt.Net
- **API Documentation**: Swashbuckle/Swagger
- **Language**: C# with .NET 8

### File Structure Overview
```
HillCipher/
├── Controllers/
│   ├── AuthController.cs       # Authentication endpoints
│   ├── HistoryController.cs    # Request history endpoints
│   └── TextController.cs       # Text CRUD and cipher operations
├── Services/
│   ├── HillCipherService.cs    # Encryption/decryption logic
│   └── HillKeyService.cs       # Key generation and validation
├── DataAccess/Postgres/
│   ├── CipherDbContext.cs      # EF Core database context
│   ├── Models/                 # Database entities
│   ├── Dtos/                   # Data transfer objects
│   ├── Configurations/         # EF entity configurations
│   └── Repositories/           # Data access repositories
├── Requests/                   # API request models
├── Responses/                  # API response models
├── Interfaces/                 # Service interfaces
└── Migrations/                 # EF Core migrations
```

### API Endpoints

**Authentication** (`/api/Auth`)
- `POST /register` - Register new user
- `POST /Login` - Authenticate user
- `PATCH /change-password` - Change user password (requires auth)

**Text Management** (`/api/Text`)
- `POST /` - Add new text
- `GET /` - Get all user texts
- `GET /{id}` - Get text by ID
- `PATCH /{id}` - Update text content
- `DELETE /{id}` - Delete text
- `POST /{id}/encrypt` - Encrypt text using Hill cipher
- `POST /{id}/decrypt` - Decrypt text using Hill cipher

**History** (`/api/History`)
- Request history tracking for audit purposes

### Configuration
- JWT settings for token generation (issuer, audience, secret key)
- PostgreSQL connection string
- CORS and authentication middleware
- Swagger UI for API testing

### Security Features
- Password hashing with BCrypt (cost factor 10)
- JWT token expiration (2 hours)
- Token versioning to invalidate old tokens on password change
- User-specific data isolation
- Authenticated endpoints using `[Authorize]` attribute

---

## Development Notes

### Hill Cipher Algorithm
The implementation uses classical Hill cipher with the following characteristics:
- Square key matrices (2x2, 3x3, etc.)
- Modular arithmetic based on alphabet size
- Matrix inversion using modular multiplicative inverse
- Automatic padding with last character if text length doesn't match block size

### Future Considerations
- Consider adding rate limiting for authentication endpoints
- Implement refresh token mechanism for extended sessions
- Add input sanitization for XSS prevention
- Consider adding API versioning
- Add comprehensive unit and integration tests
- Implement user roles and permissions
- Add email verification for registration

---

*This file is automatically maintained by AI agents. Last updated: 2025-11-16*
