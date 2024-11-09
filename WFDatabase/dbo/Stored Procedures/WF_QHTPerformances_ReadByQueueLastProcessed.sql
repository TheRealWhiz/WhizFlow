CREATE PROCEDURE [dbo].[WF_QHTPerformances_ReadByQueueLastProcessed]
	@HostName varchar(255) = null,
	@Service varchar(255) = null,
	@Domain varchar(255) = null,
	@Queue varchar(50),
	@NumberOfEntries int
AS
BEGIN
	SET NOCOUNT ON;

	set rowcount @NumberOfEntries

	select * from [dbo].[WF_QHTPerformances] with (nolock)
	where
		TaskQueue = @Queue
		and (@HostName is null or HostName = @HostName)
		and (@Service is null or [Service] = @Service)
		and (@Domain is null or Domain = @Domain)	order by [TimeStamp] desc
	set rowcount 0

END

GO
