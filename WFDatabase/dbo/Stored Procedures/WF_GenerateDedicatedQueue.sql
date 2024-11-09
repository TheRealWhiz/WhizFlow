CREATE PROCEDURE [dbo].[WF_GenerateDedicatedQueue]
	@QueueName varchar(50)
AS
BEGIN
	if (object_id('dbo.WF_Tasks_' + @QueueName) is null)
		begin
			declare @TableScript as varchar(max)
			set @TableScript = '
CREATE TABLE [dbo].[WF_Tasks_' + @QueueName+ '](
	[fk_TaskContentId] [int] NOT NULL,
	[Signature] [varchar](50) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_WF_Tasks_' + @QueueName + '] PRIMARY KEY CLUSTERED 
(
	[fk_TaskContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]'
			execute (@TableScript)
		end
	if (object_id('dbo.WF_Task_' + @QueueName + '_Write') is null)
		begin
			declare @SPInsert as varchar(max)
			
			set @SPInsert = '
CREATE PROCEDURE [dbo].[WF_Task_' + @QueueName + '_Write]
	@TaskContentId int,
	@Signature varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[WF_Tasks_' + @QueueName + ']
           ([fk_TaskContentId]
           ,[Signature]
           ,[Status])
     VALUES
           (@TaskContentId
           ,@Signature
           ,0)

END'
			execute (@SPInsert)
		end
	if (object_id('WF_Tasks_' + @QueueName + '_Read') is null)
		begin
			declare @SPExtract as varchar(max)
			
			set @SPExtract = '
CREATE PROCEDURE [dbo].[WF_Tasks_' + @QueueName + '_Read]
	@RowCount as int
AS
BEGIN
	SET NOCOUNT ON;

	select count(fk_TaskContentId) as QueueItems from dbo.WF_Tasks_' + @QueueName + ' with (nolock)

	if @@rowcount > 0
		begin
			set rowcount @RowCount

			update [dbo].[WF_Tasks_' + @QueueName + '] with (rowlock)
				set [Status] = 1
			where ([Status] = 0 or [Status] = 1)

			select
				[dbo].[WF_TaskContents].[Id],
				[dbo].[WF_TaskContents].[Content],
				[dbo].[WF_TaskContents].[Timestamp],
				[dbo].[WF_TaskContents].[Serialized],
				[dbo].[WF_TaskContents].[Disk],		
				[dbo].[WF_Tasks_' + @QueueName + '].[Signature]
			from
				[dbo].[WF_Tasks_' + @QueueName + '] with (nolock)
				inner join [dbo].[WF_TaskContents] with (nolock)
					on
						[dbo].[WF_Tasks_' + @QueueName + '].[Status] = 1
						and [dbo].[WF_Tasks_' + @QueueName + '].[fk_TaskContentId] = [dbo].[WF_TaskContents].[Id]

			set rowcount 0
		end

END'
			execute (@SPExtract)
		end
	if (object_id('WF_Tasks_' + @QueueName + '_Remove') is null)
		begin
			declare @SPRemove as varchar(max)
			
			set @SPRemove = '
CREATE PROCEDURE [dbo].[WF_Tasks_' + @QueueName + '_Remove]
	@RowCount as int
AS
BEGIN
	SET NOCOUNT ON;

	set rowcount @RowCount

	delete from [dbo].[WF_Tasks_' + @QueueName + '] with (rowlock) where [Status] = 1

	set rowcount 0

END'
			execute (@SPRemove)
		end

END
