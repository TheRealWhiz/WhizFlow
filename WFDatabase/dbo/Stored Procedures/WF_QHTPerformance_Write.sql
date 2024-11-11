CREATE PROCEDURE [dbo].[WF_QHTPerformance_Write]
	@HostName varchar(255),
	@Service varchar(255),
	@Domain varchar(255),
	@TaskContentId int,
	@Milliseconds int,
	@Signature varchar(50),
	@TaskQueue varchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	insert into [dbo].[WF_QHTPerformances]
		(fk_TaskContentId, Milliseconds, [HostName], [Service], [Domain], [Signature], [TaskQueue], [TimeStamp])
	values
		(@TaskContentId, @Milliseconds, @HostName, @Service, @Domain, @Signature, @TaskQueue, GETDATE())
END

GO
