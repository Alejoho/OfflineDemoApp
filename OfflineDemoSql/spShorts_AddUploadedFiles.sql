CREATE PROCEDURE [dbo].[spShorts_AddUploadedFiles]
	@id int,
	@mp4FileUrl nvarchar(1024),
	@imageFileUrl nvarchar(1024)
AS
begin
	update dbo.Shorts
	set Mp4FileUrl = @mp4FileUrl, ImageFileUrl = @imageFileUrl
	where Id = @id;
end
