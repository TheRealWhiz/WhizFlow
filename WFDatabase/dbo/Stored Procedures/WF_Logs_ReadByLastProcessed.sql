CREATE PROCEDURE [dbo].[WF_Logs_ReadByLastProcessed]
	@HostName varchar(255) = null,
	@Domain varchar(255) = null,
	@Service varchar(255) = null,
	@NumberOfEntries int = null,
	@ModuleId int = null,
	@LogTypeId int = null
AS
BEGIN
	SET NOCOUNT ON;

	set rowcount @NumberOfEntries
	
	select * from dbo.WF_Logs with (nolock)
	where
		(@HostName is null or HostName = @HostName)
		and (@Service is null or [Service] = @Service)
		and (@Domain is null or Domain = @Domain)
		and (@ModuleId is null or ModuleId = @ModuleId)
		and (@LogTypeId is null or LogTypeId = @LogTypeId)
	order by [Time] desc

	set rowcount 0
	
end

GO
