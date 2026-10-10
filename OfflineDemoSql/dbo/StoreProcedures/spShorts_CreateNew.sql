CREATE PROCEDURE [dbo].[spShorts_CreateNew]
	@title NVARCHAR(100),
	@description NVARCHAR(300),
	@hashtags NVARCHAR(75)
AS
BEGIN
	INSERT INTO dbo.Shorts (Title, [Description], Hashtags)
	VALUES (@title, @description, @hashtags);

	SELECT CAST(SCOPE_IDENTITY() AS INT);
END
