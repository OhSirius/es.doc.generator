USE [AuthDBTest]
GO


select * from dbo.BGAccount with(NOLOCK)

select * from dbo.BGLicense with(NOLOCK)

select * from dbo.BGLicenseType with(NOLOCK)

select * from dbo.BGApplication with(NOLOCK)

--delete dbo.BGLicenseType
--delete dbo.BGApplication

--drop table __MigrationHistory
--drop table BGLogonHistory
--drop table BGAccount
--drop table BGLicense
--drop table BGLicenseType
--drop table BGApplication


