CREATE TABLE [dbo].[Notifications]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [IsRead] BIT NULL DEFAULT 0, 
    [Text] NTEXT NULL, 
    [Type] NTEXT NULL, 
	[DateTime] DATETIME NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT FK_Notifications_To_Users FOREIGN KEY (UserId)  REFERENCES Users (Id)
)
