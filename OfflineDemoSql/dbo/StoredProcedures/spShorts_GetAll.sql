CREATE PROCEDURE [dbo].[spShorts_GetAll]
	
AS
begin
	select [Id], [Title], [Description], [Hashtags], [Mp4FileUrl], [ImageFileUrl], [IsUploaded]
	from dbo.Shorts;
end
