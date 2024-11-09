CREATE PROCEDURE [dbo].[WF_Log_Write]
	@HostName varchar(255),
	@Service varchar(255),
	@Domain varchar(255),
	@ModuleId int,
	@LogTypeId int,
	@Object varchar(max),
	@TaskContentId int = null,
	@Message varchar(max),
	@AdditionalInformation varchar(max),
	@Time datetime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[WF_Logs]
           ([HostName]
		   ,[Service]
		   ,[Domain]
           ,[ModuleId]
           ,[LogTypeId]
           ,[Object]
           ,[fk_TaskContentId]
           ,[Message]
           ,[AdditionalInformation]
           ,[Time])
     VALUES
           (@HostName
		   ,@Service
		   ,@Domain
           ,@ModuleId
           ,@LogTypeId
           ,@Object
           ,@TaskContentId
           ,@Message
           ,@AdditionalInformation
           ,@Time)

END

GO
