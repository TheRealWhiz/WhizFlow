CREATE PROCEDURE [dbo].[WF_QHTPerformances_Read]
	@TaskContentId int
AS
BEGIN
	SET NOCOUNT ON;
	select * from [dbo].[WF_QHTPerformances] with (nolock)
	where 
		fk_TaskContentId = @TaskContentId
	order by [TimeStamp]
END

GO
