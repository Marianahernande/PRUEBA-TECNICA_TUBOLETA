# 🎟️ PRUEBA TÉCNICA TUBOLETA – E-Commerce API con Roles, Pricing Dinámico y JWT

API REST desarrollada en **.NET 8** bajo principios de **Arquitectura Limpia** y **Microservicios**, con autenticación basada en JWT, manejo de roles **Usuario** y **Administrador**, y un motor de **Pricing Dinámico** optimizado con un algoritmo matemático adaptable a la demanda.

---

## 🚀 Tecnologías utilizadas

- **.NET 8 Web API**
- **Entity Framework Core** (ORM)
- **MySQL 8** (base de datos relacional)
- **Docker** (para contenedores de base de datos)
- **JWT Authentication** (autenticación y manejo de roles)
- **CQRS + MediatR** (separación de comandos y consultas)
- **AutoMapper** (mapeo de entidades a DTOs)
- **FluentValidation** (validación de entrada)
- **Microservicios** desacoplados para Pricing, Notificaciones y Autenticación

---

## 📂 Arquitectura

La solución está organizada siguiendo **Clean Architecture** con separación de capas:

📦 ECommerce.Application
┣ 📂 Abstractions
┣ 📂 Common
┣ 📂 Notifications
┗ 📂 Pricing
📦 ECommerce.Infrastructure
┣ 📂 Notifications
┗ 📂 Pricing
📦 ECommerce.WebApi
┗ Program.cs


Cada microservicio es inyectado en el contenedor de dependencias (`builder.Services`) desde el `Program.cs`.

## ⚙️ Instalación y Ejecución

### 1️⃣ Clonar el repositorio
```bash
git clone https://github.com/Marianahernande/PRUEBA-TECNICA_TUBOLETA.git
cd PRUEBA-TECNICA_TUBOLETA

2️⃣ Levantar la base de datos en Docker
docker run -d --name ecommerce-mysql -p 3306:3306 \
-e MYSQL_ROOT_PASSWORD=ecommerce123 \
-e MYSQL_DATABASE=ecommerce \
-e MYSQL_USER=ecommerce \
-e MYSQL_PASSWORD=ecommerce123 \
mysql:8.0 --default-authentication-plugin=mysql_native_password


3️⃣ Restaurar dependencias
   dotnet restore

4️⃣ Ejecutar migraciones de base de datos
    dotnet ef database update

5️⃣ Ejecutar la API
dotnet run --project ECommerce.WebApi


🔑 Autenticación y Roles
La API utiliza JWT con manejo de dos roles principales:

Administrador (Admin): acceso a operaciones críticas como gestión de precios, usuarios y configuración global.

---------------------------------------------------
Primero, registrarse como USUARIO y luego actualizar ese usuario a ADMIN con la siguiente consulta:
docker exec -it ecommerce-mysql mysql -uecommerce -pecommerce123 -D ecommerce -e "UPDATE Users SET Role='Admin' WHERE Email='EMAIL USER';"

INICIAR SESIÓN COMO ADMIN. 
Usuario: acceso a compra de boletos, consulta de eventos y precios:

Flujo de registro e inicio de sesión
Registro

Endpoint: POST /api/auth/register

Datos requeridos:
{
  "username": "usuario1",
  "email": "usuario1@email.com",
  "password": "Contraseña123!",
  "role": "User"
}

Inicio de sesión

Endpoint: POST /api/auth/login

Respuesta: {
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp..."
}

Uso del token

Añadir en cada petición en la parte superior derecha para autorizar.
Authorization: Bearer {token}

Protección por rol:
[Authorize(Roles = "Admin")]
public IActionResult UpdatePricing(...) { ... }

----------------------------------------------------

📈 Algoritmo de Pricing Dinámico
El microservicio Pricing calcula precios de boletos en tiempo real según demanda, tiempo y disponibilidad.

Fórmula Matemática
Si:

P0 = Precio base

D = Demanda actual (0 a 1, donde 1 es demanda máxima)

T = Factor de tiempo (0 a 1, donde 1 es fecha del evento)

S = Factor de escasez de boletos (0 a 1, donde 1 es sold out)

La fórmula del precio dinámico es:
 P = P0 × (1 + αD + βT + γS)

Donde:

α (alpha): peso de la demanda

β (beta): peso del tiempo

γ (gamma): peso de escasez

P0 = 100
D = 0.7 (70% de demanda)
T = 0.5 (evento a mitad del tiempo de venta)
S = 0.2 (20% de entradas vendidas)
α = 0.3, β = 0.2, γ = 0.5

P = 100 × (1 + 0.3×0.7 + 0.2×0.5 + 0.5×0.2)
P = 100 × (1 + 0.21 + 0.10 + 0.10)
P = 100 × 1.41
P = 141

-------------------------------------------------------------------------------------
🧩 Microservicios Implementados:

.PricingService

       Calcula precios dinámicos en tiempo real.

       Totalmente desacoplado, inyectado vía IPricingService.

.NotificationService

       Implementación en consola (ConsoleNotificationService) para registrar eventos de notificación.

        Puede sustituirse fácilmente por correo, SMS o WebPush.

.AuthenticationService

       Registro e inicio de sesión con JWT.

        Asignación y validación de roles.


Una vez levantada la API, abrir:
https://localhost:5001/swagger

/api/auth/register → Registro de usuario.

/api/auth/login → Inicio de sesión y obtención de token.

/api/pricing/calculate → Cálculo del precio de un boleto según demanda.

/api/events → CRUD de eventos (protegido por rol).

---------------------------------------------------------------------------------------------------------------------

🛠 Comandos útiles para desarrollo

  Compilar y ejecutar:
            dotnet build
            dotnet run --project ECommerce.WebApi

  Crear nueva migración:
         dotnet ef migrations add NombreMigracion

Actualizar base de datos:
         dotnet ef database update


----------------------------------------------------------------------------------------------------------------

## 📬 Ejemplos de Requests y Responses

### 1️⃣ Registro de usuario
**Request**
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "usuario_demo",
  "email": "usuario@demo.com",
  "password": "Contraseña123!",
  "role": "User"
}


Response
{
  "id": "1f5a5b40-15a9-4f7f-9ac0-88b845dc1e65",
  "username": "usuario_demo",
  "email": "usuario@demo.com",
  "role": "User",
  "message": "Usuario registrado exitosamente"
}




2️⃣ Inicio de sesión:

POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@demo.com",
  "password": "Contraseña123!"
}

Response

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp..."
}

3️⃣ Obtener perfil autenticado:

Request

GET /api/users/me
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ikp...


Response:

{
  "id": "1f5a5b40-15a9-4f7f-9ac0-88b845dc1e65",
  "username": "usuario_demo",
  "email": "usuario@demo.com",
  "role": "User"
}


4️⃣ Crear evento (solo Admin):


Request

POST /api/events
Authorization: Bearer {token_de_admin}
Content-Type: application/json

{
  "title": "Concierto de Rock",
  "date": "2025-09-01T20:00:00",
  "basePrice": 150,
  "capacity": 500
}

Response

{
  "id": "c2f0e11e-1a09-4a47-b8ff-654fb015f345",
  "title": "Concierto de Rock",
  "date": "2025-09-01T20:00:00",
  "basePrice": 150,
  "capacity": 500,
  "availableTickets": 500
}

5️⃣ Calcular precio dinámico
Request
POST /api/pricing/calculate
Content-Type: application/json
Authorization: Bearer {token}

{
  "basePrice": 150,
  "demand": 0.65,
  "timeFactor": 0.40,
  "scarcityFactor": 0.30
}

Response

{
  "basePrice": 150,
  "finalPrice": 192.5,
  "details": {
    "alpha": 0.3,
    "beta": 0.2,
    "gamma": 0.5,
    "formula": "P = P0 × (1 + αD + βT + γS)"
  }
}

6️⃣ Notificación (Microservicio)

Request

POST /api/notifications/send
Authorization: Bearer {token}
Content-Type: application/json

{
  "message": "Compra confirmada para el evento Concierto de Rock"
}

Response

{
  "status": "ok",
  "sentAt": "2025-08-11T21:00:00Z",
  "message": "Compra confirmada para el evento Concierto de Rock"
}


------------------------------------------------------------------------------------------------------------------
📄 Licencia
Proyecto creado para fines de prueba técnica a disposición de tuboleta.
Desarrollado por Mariana Hernandez.
