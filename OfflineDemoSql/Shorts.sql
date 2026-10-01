CREATE TABLE [dbo].[Shorts]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(300),
    Hashtags NVARCHAR(75),
    Mp4FileUrl NVARCHAR(1024),
    ImageFileUrl NVARCHAR(1024), 
    [IsUploaded] BIT NOT NULL DEFAULT 0
)
