CREATE TABLE [dbo].[GroupChatMessage]
(
	[Message_Id] INT NOT NULL PRIMARY KEY, 
    [TextMessage] NTEXT NULL, 
    [User_Id] INT NULL, 
    [Group_Id] INT NULL, 
    [Time] DATETIME NULL
)
