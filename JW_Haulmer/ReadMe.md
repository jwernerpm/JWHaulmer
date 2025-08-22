# JW_Haulmer - API de Transacciones

Este proyecto implementa una **API REST en .NET 3.1  y SQL Server para simular el flujo de autorización y registro de transacciones con tarjetas.  
Se desarrolló un servicio mock de adquirente (`AcquirerMockService`) y un TransactionsController que gestiona la validación y persistencia en base de datos.


##    Directorio
      en carpeta BD se encuentan los Script SQL para creacion de Database, Table, StoreProcedure
      en Raiz del proyecto se encuentra archivo PostManCollection para realizar pruebas del serivcio,
---

## 🚀 Decisiones Técnicas

### 1. Validaciones de Entrada
Se realizan validaciones estrictas en el `TransactionsController`:
- **PAN (Primary Account Number):** debe tener entre 12 y 19 dígitos.
- **Expiry (MM/YY):**
  - Validado con regex `^(0[1-9]|1[0-2])\/\d{2}$`.
- **Amount:** debe ser mayor a 0.
- **Currency:** exactamente 3 caracteres (ej: USD, CLP, EUR).

### 2. Servicio Mock `AcquirerMockService`
- Genera un **código ISO de autorización** (`00`, `05`, `51`, `91`, `87`).
- Si el PAN termina en **1234**, la operación **siempre se aprueba** con `iso = "00"` para tener una prueba controlada
- Genera un **código de autorización aleatorio** solo si el ISO es `00`.

### 3. Persistencia en SQL Server
- Se decidió usar un **Stored Procedure (`InsertTransaction`)** en lugar de un `INSERT` directo.
  - Razones:
    - Mantener la lógica de negocio centralizada en la base de datos.
    - Facilitar auditoría y cambios futuros sin alterar el código.
    - Mejor manejo de errores y transacciones.
  - El Stored Procedure retorna el `TransactionId` generado, que luego es devuelto al cliente.
  - En este proyecto **no se utilizó Entity Framework** para evitar dependencias externas y mantener la solución lo más liviana posible.  
    Se decidió usar **ADO.NET nativo** para la conexión a SQL Server, generando la instrucción SQL directamente desde el código o llamando a un Stored Procedure.  

Ventajas de esta decisión:
- Evita instalar paquetes NuGet adicionales.
- Menor sobrecarga en la aplicación.
- Permite un control explícito sobre las conexiones y comandos SQL.
- Facilita la comprensión del flujo de datos desde la aplicación hasta la base de datos.

### 4. Diseño del Controller
- Endpoint `POST /api/transactions`
- Flujo:
  1. Validación de parámetros.
  2. Llamada a `Authorize` en `AcquirerMockService`.
  3. Inserción de la transacción mediante `InsertTransaction`.
  4. Retorno de `TransactionId`, `Status`, `IsoCode`, `AuthorizationCode`.

### 5. Manejo de Seguridad PCI DSS
- **PAN almacenado enmascarado con formato:  12334********1234
- **EL CVV no es almacenado en base de datos



