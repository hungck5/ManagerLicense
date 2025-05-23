USE [AuthDatabase]
GO
/****** Object:  Table [dbo].[OpenIddictApplications]    Script Date: 30/4/2025 11:35:30 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpenIddictApplications](
	[Id] [nvarchar](450) NOT NULL,
	[ApplicationType] [nvarchar](50) NULL,
	[ClientId] [nvarchar](100) NULL,
	[ClientSecret] [nvarchar](max) NULL,
	[ClientType] [nvarchar](50) NULL,
	[ConcurrencyToken] [nvarchar](50) NULL,
	[ConsentType] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[DisplayNames] [nvarchar](max) NULL,
	[JsonWebKeySet] [nvarchar](max) NULL,
	[Permissions] [nvarchar](max) NULL,
	[PostLogoutRedirectUris] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
	[RedirectUris] [nvarchar](max) NULL,
	[Requirements] [nvarchar](max) NULL,
	[Settings] [nvarchar](max) NULL,
 CONSTRAINT [PK_OpenIddictApplications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OpenIddictScopes]    Script Date: 30/4/2025 11:35:31 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OpenIddictScopes](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyToken] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Descriptions] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[DisplayNames] [nvarchar](max) NULL,
	[Name] [nvarchar](200) NULL,
	[Properties] [nvarchar](max) NULL,
	[Resources] [nvarchar](max) NULL,
 CONSTRAINT [PK_OpenIddictScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'7a039532-18a8-41ce-892f-d07620bc10e7', NULL, N'ecommerce_app', NULL, N'public', N'400f804a-2e85-4a9f-b78f-d529ce93ef3d', N'implicit', N'Ecommerce Web App', NULL, NULL, N'["ept:authorization","ept:token","ept:end_session","gt:authorization_code","rst:code","scp:openid","scp:profile","scp:email","scp:ecommerce_api","prn:resource_server"]', N'["https://localhost:7294/signout-callback-oidc"]', NULL, N'["https://localhost:7294/signin-oidc"]', N'["ft:pkce"]', NULL)
INSERT [dbo].[OpenIddictApplications] ([Id], [ApplicationType], [ClientId], [ClientSecret], [ClientType], [ConcurrencyToken], [ConsentType], [DisplayName], [DisplayNames], [JsonWebKeySet], [Permissions], [PostLogoutRedirectUris], [Properties], [RedirectUris], [Requirements], [Settings]) VALUES (N'fb036db4-e500-4cf1-9440-80612f51ffbc', NULL, N'react_client', NULL, N'public', N'eb26f23f-80b2-410c-8f91-0a60a77fb208', N'explicit', N'React.js App', NULL, NULL, N'["ept:authorization","ept:token","gt:authorization_code","rst:code","scp:email","scp:profile","scp:roles","scp:offline_access","gt:refresh_token","scp:ecommerce_api"]', N'["https://localhost:3001/dashboard"]', NULL, N'["https://localhost:3001/callback"]', N'["ft:pkce"]', NULL)
GO
INSERT [dbo].[OpenIddictScopes] ([Id], [ConcurrencyToken], [Description], [Descriptions], [DisplayName], [DisplayNames], [Name], [Properties], [Resources]) VALUES (N'3cba14c2-fdee-48fd-ad4d-4111fea05bf2', N'7734172c-8453-4982-bbef-c091139e0e9f', NULL, NULL, NULL, NULL, N'ecommerce_api', NULL, N'["resource_server"]')
GO
