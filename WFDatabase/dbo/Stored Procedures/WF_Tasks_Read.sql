CREATE PROCEDURE [dbo].[WF_Tasks_Read]
	@Queue as varchar(50),
	@RowCount as int
AS
BEGIN
	SET NOCOUNT ON;

	select count(fk_TaskContentId) as QueueItems from dbo.WF_Tasks with (nolock) where [TaskQueue] = @Queue

	if @@rowcount > 0
		begin
			set rowcount @RowCount

			update [dbo].[WF_Tasks] with (rowlock)
				set [Status] = 1
			where [TaskQueue] = @Queue and ([Status] = 0 or [Status] = 1)

			select
				[dbo].[WF_TaskContents].[Id],
				[dbo].[WF_TaskContents].[Content],
				[dbo].[WF_TaskContents].[Timestamp],
				[dbo].[WF_TaskContents].[Serialized],
				[dbo].[WF_TaskContents].[Disk],		
				[dbo].[WF_Tasks].[Signature],
				[dbo].[WF_Tasks].[TaskQueue]
			from
				[dbo].[WF_Tasks] with (nolock)
				inner join [dbo].[WF_TaskContents] with (nolock)
					on
						[dbo].[WF_Tasks].[TaskQueue] = @Queue
						and [dbo].[WF_Tasks].[Status] = 1
						and [dbo].[WF_Tasks].[fk_TaskContentId] = [dbo].[WF_TaskContents].[Id]

			set rowcount 0
		end

END

GO
