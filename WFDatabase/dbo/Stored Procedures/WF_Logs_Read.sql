CREATE PROCEDURE [dbo].[WF_Logs_Read]
	@HostName varchar(255) = null,
	@Service varchar(255) = null,
	@Domain varchar(255) = null,
	@NumberOfEntries int = null,
	@ModuleId int = null,
	@LogTypeId int = null,
	@TaskContentId int = null,
	@From datetime = null,
	@To datetime = null,
	@Ascending bit = null,
	@Mode int
AS
BEGIN
	SET NOCOUNT ON;

	if @Mode = 1
		begin
		
			set rowcount @NumberOfEntries
			if @From is not null
				begin
					if @Ascending = 1
						begin
							select * from dbo.WF_Logs with (nolock)
							where
								[Time] > @From
								and (@HostName is null or HostName = @HostName)
								and (@Service is null or [Service] = @Service)
								and (@Domain is null or Domain = @Domain)
								and (@ModuleId is null or ModuleId = @ModuleId)
								and (@LogTypeId is null or LogTypeId = @LogTypeId)
							order by [Time] asc
						end
					else
						begin
							select * from dbo.WF_Logs with (nolock)
							where
								[Time] < @From
								and (@HostName is null or HostName = @HostName)
								and (@Service is null or [Service] = @Service)
								and (@Domain is null or Domain = @Domain)
								and (@ModuleId is null or ModuleId = @ModuleId)
								and (@LogTypeId is null or LogTypeId = @LogTypeId)
							order by [Time] desc
						end
				end
			else
				begin
					if @Ascending = 1
						begin
							select * from dbo.WF_Logs with (nolock)
							where
								(@HostName is null or HostName = @HostName)
								and (@Service is null or [Service] = @Service)
								and (@Domain is null or Domain = @Domain)
								and (@ModuleId is null or ModuleId = @ModuleId)
								and (@LogTypeId is null or LogTypeId = @LogTypeId)
							order by [Time] asc
						end
					else
						begin
							select * from dbo.WF_Logs with (nolock)
							where
								(@HostName is null or HostName = @HostName)
								and (@Service is null or [Service] = @Service)
								and (@Domain is null or Domain = @Domain)
								and (@ModuleId is null or ModuleId = @ModuleId)
								and (@LogTypeId is null or LogTypeId = @LogTypeId)
							order by [Time] desc
						end
				end
			set rowcount 0
		end
	if @Mode = 2
		begin
			select * from dbo.WF_Logs with (nolock)
			where
				[Time] >= @From and [Time] <= @To
				and (@HostName is null or HostName = @HostName)
				and (@Service is null or [Service] = @Service)
				and (@Domain is null or Domain = @Domain)
				and (@ModuleId is null or ModuleId = @ModuleId)
				and (@LogTypeId is null or LogTypeId = @LogTypeId)
			order by [Time] asc
		end
	if @Mode = 3
		begin
			select * from dbo.WF_Logs with (nolock)
			where
				fk_TaskContentId = @TaskContentId
				and (@HostName is null or HostName = @HostName)
				and (@Service is null or [Service] = @Service)
				and (@Domain is null or Domain = @Domain)
				and (@ModuleId is null or ModuleId = @ModuleId)
				and (@LogTypeId is null or LogTypeId = @LogTypeId)
			order by [Time] asc
		end

END

GO
