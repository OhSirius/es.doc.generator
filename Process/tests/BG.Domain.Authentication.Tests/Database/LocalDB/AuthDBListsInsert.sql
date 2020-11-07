SET IDENTITY_INSERT dbo.BGApplication ON
INSERT INTO dbo.BGApplication ([Id], [Name]) VALUES 
  (1, N'DocumentsGenerator')
SET IDENTITY_INSERT dbo.BGApplication OFF

SET IDENTITY_INSERT dbo.BGLicenseType ON
INSERT INTO dbo.BGLicenseType ([Id], [Name], [Application]) VALUES 
  (1, N'Lite', 1),
  (2, N'Basis', 1),
  (3, N'Pro', 1)
SET IDENTITY_INSERT dbo.BGLicenseType OFF