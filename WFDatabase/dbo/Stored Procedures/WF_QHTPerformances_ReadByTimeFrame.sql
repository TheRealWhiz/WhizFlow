CREATE PROCEDURE [dbo].[WF_QHTPerformances_ReadByTimeFrame]
	@HostName varchar(255) = null,
	@Service varchar(255) = null,
	@Domain varchar(255) = null,
	@From datetime,
	@To datetime
AS
BEGIN
	SET NOCOUNT ON;

	select * from [dbo].[WF_QHTPerformances] with (nolock)
	where
		[TimeStamp] >= @From
		and [TimeStamp] <= @To
		and (@HostName is null or HostName = @HostName)
		and (@Service is null or [Service] = @Service)
		and (@Domain is null or Domain = @Domain)
	order by [TimeStamp] desc

END

GO

