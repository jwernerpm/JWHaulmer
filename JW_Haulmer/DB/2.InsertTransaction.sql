use CardTransaction
GO

CREATE PROCEDURE InsertTransaction @MerchantId NVARCHAR(50)
	,@PanMasked NVARCHAR(50)
	,@Expiry NVARCHAR(5)
	,@Amount DECIMAL(18, 2)
	,@Currency NVARCHAR(3)
	,@Status NVARCHAR(20)
	,@IsoCode NVARCHAR(5)
	,@AuthorizationCode NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO Transactions (
		MerchantId
		,PanMasked
		,Expiry
		,Amount
		,Currency
		,Status
		,IsoCode
		,AuthorizationCode
		)
	OUTPUT INSERTED.TransactionId
	VALUES (
		@MerchantId
		,@PanMasked
		,@Expiry
		,@Amount
		,@Currency
		,@Status
		,@IsoCode
		,@AuthorizationCode
		);
END
