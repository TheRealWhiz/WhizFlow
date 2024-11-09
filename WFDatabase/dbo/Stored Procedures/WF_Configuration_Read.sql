CREATE PROCEDURE [dbo].[WF_Configuration_Read]
	@Hostname varchar(255),
	@Service varchar(255),
	@Domain varchar(255)
AS
BEGIN
	SET NOCOUNT ON;

	select * from [dbo].[WF_Configurations] where [Service] = @Service and Hostname = @Hostname and Domain = @Domain

END