CREATE PROCEDURE [dbo].[spShorts_AddUploadedFiles]
	@id INT,
	@mp4FileUrl NVARCHAR(1024),
	@imageFileUrl NVARCHAR(1024)
AS
BEGIN
	UPDATE dbo.Shorts
	SET Mp4FileUrl = @mp4FileUrl, ImageFileUrl = @imageFileUrl
	WHERE Id = @id;
END
