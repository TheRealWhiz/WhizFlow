CREATE PROCEDURE [dbo].[WF_Task_Write]
	@TaskContentId int,
	@Signature varchar(50),
	@TaskQueue varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[WF_Tasks]
           ([fk_TaskContentId]
           ,[TaskQueue]
           ,[Signature]
           ,[Status])
     VALUES
           (@TaskContentId
           ,@TaskQueue
           ,@Signature
           ,0)

END

GO
