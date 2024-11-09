CREATE PROCEDURE [dbo].[WF_TaskContent_Write]
	@Content varchar(max),
	@Timestamp datetime,
	@Serialized bit,
	@Disk bit
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[WF_TaskContents]
           ([Timestamp]
           ,[Content]
           ,[Serialized]
           ,[Disk])
     VALUES
           (@Timestamp
           ,@Content
           ,@Serialized
           ,@Disk)

	select SCOPE_IDENTITY() as [Id]

END

GO
