CREATE PROCEDURE [dbo].[spShorts_CreateNew]
	@title nvarchar(100),
	@description nvarchar(300),
	@hashtags nvarchar(75)
AS
begin
	insert into dbo.Shorts (Title, [Description], Hashtags)
	values (@title, @description, @hashtags);

	select cast(SCOPE_IDENTITY() as int);
end
