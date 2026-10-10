CREATE PROCEDURE [dbo].[spShorts_GetAll]
	
AS
BEGIN
	SELECT [Id], [Title], [Description], [Hashtags], 
		   [Mp4FileUrl], [ImageFileUrl], [IsUploaded]
	FROM dbo.Shorts;
END
