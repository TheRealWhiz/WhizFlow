CREATE PROCEDURE dbo.WF_Configuration_Write
	@Hostname varchar(255),
	@Service varchar(255),
	@Domain varchar(255),
	@Configuration varchar(max),
	@Active bit
AS
BEGIN
	SET NOCOUNT ON;

	if (exists(select Id from WF_Configurations where [Service] = @Service and Hostname = @Hostname and Domain = @Domain))
		begin
			update [dbo].[WF_Configurations]
			set
				Configuration = isnull(@Configuration, Configuration),
				Active = @Active
			where [Service] = @Service and Hostname = @Hostname and Domain = @Domain
		end
	else
		begin
			insert into [dbo].[WF_Configurations]
				(Hostname, [Service], Domain, Configuration, Active)
			values
				(@Hostname, @Service, @Domain, @Configuration, @Active)
		end
END
