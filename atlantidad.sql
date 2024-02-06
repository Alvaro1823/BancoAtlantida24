
CREATE DATABASE atlantida;
GO

-- Usar la base de datos creada
USE atlantida;
GO

-- Tabla para las transacciones
CREATE TABLE Clientes (
     IDcliente INT PRIMARY KEY,
    Nombre  NVARCHAR(20),
    Apellido NVARCHAR(20), 
    Dui NVARCHAR(20), 
    fechanNacimiento DATE
   
);


-- Tabla para las tarjetas de crédito
CREATE TABLE TarjetaCredito (
    NumeroTarjeta NVARCHAR(20) PRIMARY KEY,
	IDcliente INT FOREIGN KEY REFERENCES Clientes (IDCliente),
    NombreTarjeta NVARCHAR(100) ,
	LimiteCredito DECIMAL(18, 2),
    SaldoActual DECIMAL(18, 2),
    SaldoDisponible DECIMAL(18, 2),
    PIC INT,
	PCSM  INT
    -- Otros campos relevantes
);

-- Tabla para las transacciones
CREATE TABLE Transacciones (
    NumeroAutorizacion INT PRIMARY KEY,
    NumeroTarjeta NVARCHAR(20) FOREIGN KEY REFERENCES TarjetaCredito(NumeroTarjeta),
    TipoTransaccion NVARCHAR(20), -- Compra, Pago, etc.
    Monto DECIMAL(18, 2),
    FechaTransaccion DATETIME,
	Descripcion NVARCHAR(255),
    -- Otros campos relevantes
);

INSERT INTO Clientes (IDcliente, Nombre, Apellido, Dui, fechanNacimiento) 
VALUES 
    (1, 'Juan', 'Pérez', '0000001-0', '1990-05-15'),
    (2, 'María', 'García', '0000002-0', '1985-12-20'),
    (3, 'Pedro', 'Martínez', '0000003-0', '1978-08-10'),
    (4, 'Ana', 'López', '0000004-0', '1995-03-25');

*************************************************************************************************************************************

CREATE PROCEDURE InsertarTarjetaCredito
    @NumeroTarjeta NVARCHAR(20),
    @NombreTarjeta NVARCHAR(100),
    @IDCliente INT,
    @LimiteCredito DECIMAL(18, 2),
    @SaldoActual DECIMAL(18, 2),
    @SaldoDisponible DECIMAL(18, 2),
    @PIC INT,
    @PCSM INT
AS
BEGIN
    INSERT INTO TarjetaCredito (NumeroTarjeta, NombreTarjeta, IDcliente, LimiteCredito, SaldoActual, SaldoDisponible, PIC, PCSM)
    VALUES (@NumeroTarjeta, @NombreTarjeta, @IDCliente, @LimiteCredito, @SaldoActual, @SaldoDisponible, @PIC, @PCSM);
END;	

***********************************************************************************************************************************

CREATE PROCEDURE InsertarTransaccion 
    @NumeroAutorizacion INT,
    @NumeroTarjeta NVARCHAR(20),
    @TipoTransaccion NVARCHAR(20),
    @Monto DECIMAL(18, 2),
    @FechaTransaccion DATETIME,
    @Descripcion NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si la tarjeta existe antes de insertar la transacción
    IF NOT EXISTS (SELECT 1 FROM TarjetaCredito WHERE NumeroTarjeta = @NumeroTarjeta)
    BEGIN
        PRINT 'La tarjeta especificada no existe.';
        RETURN;
    END;

    -- Insertar la transacción
    INSERT INTO Transacciones (NumeroAutorizacion, NumeroTarjeta, TipoTransaccion, Monto, FechaTransaccion, Descripcion)
    VALUES (@NumeroAutorizacion, @NumeroTarjeta, @TipoTransaccion, @Monto, @FechaTransaccion, @Descripcion);

    PRINT 'Transacción insertada correctamente.';
END;

**************************  PROCEDIMIENTO PARA LISTAR TRANSACCIONES POR TARJETA, POR MES Y POR TIPO **********************************************************	

CREATE PROCEDURE ListarTransaccionesPorTarjeta 
    @NumeroTarjeta NVARCHAR(20),
	@mes INT,
	@tipoTransccion NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    -- Consulta para obtener las transacciones por tarjeta
    SELECT NumeroAutorizacion, NumeroTarjeta, TipoTransaccion, Monto, FechaTransaccion, Descripcion
    FROM Transacciones
    WHERE NumeroTarjeta = @NumeroTarjeta AND MONTH(FechaTransaccion)= @mes AND TipoTransaccion = @tipoTransccion;
END;


******************************************** SE calcula el monto total de transacciones mediante el numero de tarjeta, por mes y por tipo ************************************************************

CREATE PROCEDURE MontoTotalTransacciones
 @NumeroTarjeta NVARCHAR(20),
 @mes INT,
 @tipoTransccion NVARCHAR(10) 
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MontoTotal DECIMAL(18, 2);

    -- Calcula el monto total de todas las transacciones
    SELECT @MontoTotal = SUM(Monto)
    FROM Transacciones
	WHERE NumeroTarjeta = @NumeroTarjeta AND MONTH(FechaTransaccion)= @mes AND TipoTransaccion = @tipoTransccion;

    -- Devuelve el monto total
    SELECT @MontoTotal AS MontoTotalTransacciones;
END;

**************************  PROCEDIMIENTO PARA LISTAR TARJETAS DEL CLIENTE **********************************************************	

CREATE PROCEDURE ListarTarjetasPorCliente 
    @idCliente NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    -- Consulta para obtener las transacciones por tarjeta
    SELECT *
    FROM tarjetaCredito
    WHERE idCliente = @idCliente;
END;

**************************  PROCEDIMIENTO PARA VER EL ESTADO DE LA TARJETA TARJETA **********************************************************	

CREATE PROCEDURE TarjetaCliente 
    @numTarjeta NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    -- Consulta para obtener las transacciones por tarjeta
    SELECT *
    FROM tarjetaCredito
    WHERE NumeroTarjeta = @numTarjeta;
END;

************************************ ACTUALIZA EL SALDO ACTUAL Y EL DISPONIBLE *******************************************************************
CREATE PROCEDURE ActualizarSaldo
    @NumeroTarjeta NVARCHAR(20),
    @MontoTransaccion DECIMAL(18, 2)
AS
BEGIN
    -- Declarar variables para almacenar saldos actuales
    DECLARE @SaldoActual DECIMAL(18, 2), @SaldoDisponible DECIMAL(18, 2);

    -- Obtener los saldos actuales de la tarjeta
    SELECT @SaldoActual = SaldoActual, @SaldoDisponible = SaldoDisponible
    FROM TarjetaCredito
    WHERE NumeroTarjeta = @NumeroTarjeta;

    -- Actualizar los saldos
    UPDATE TarjetaCredito
    SET SaldoActual = @SaldoActual - @MontoTransaccion,
        SaldoDisponible = @SaldoDisponible - @MontoTransaccion
    WHERE NumeroTarjeta = @NumeroTarjeta;
END;

CREATE PROCEDURE ObtenerClientes
AS
BEGIN
    -- Seleccionar todos los clientes
    SELECT * FROM Clientes order by Nombre ;
END;

***************************************************** 

-- Crear procedimiento almacenado para obtener un cliente por ID
CREATE PROCEDURE ObtenerClientePorID
    @IDCliente INT
AS
BEGIN
    -- Seleccionar cliente por ID
    SELECT *
    FROM Clientes
    WHERE IDCliente = @IDCliente;
END;
