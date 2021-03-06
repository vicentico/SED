USE [Template]
GO
/****** Object:  Table [dbo].[Email]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Email](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreRemitente] [varchar](100) NOT NULL,
	[DireccionRemitente] [varchar](100) NOT NULL,
	[DireccionesCC] [varchar](100) NULL,
	[DireccionesCCO] [varchar](100) NULL,
	[Asunto] [varchar](500) NOT NULL,
	[Cuerpo] [varchar](max) NOT NULL,
	[Descripcion] [varchar](100) NOT NULL,
	[Activo] [bit] NOT NULL CONSTRAINT [DF_Email_Activo]  DEFAULT ((1)),
 CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Icono]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Icono](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NULL,
 CONSTRAINT [PK_Iconos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Menu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Menu_Id] [int] NULL,
	[Url] [varchar](50) NULL,
	[Icono] [varchar](50) NOT NULL,
	[Texto] [varchar](50) NOT NULL,
	[Ayuda] [varchar](200) NOT NULL,
	[Orden] [int] NULL,
	[Activo] [bit] NOT NULL CONSTRAINT [DF_Menu_Activo]  DEFAULT ((1)),
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rol](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Activo] [bit] NOT NULL CONSTRAINT [DF_Rol_Activo]  DEFAULT ((1)),
	[Descripcion] [varchar](500) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RolMenu]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rol_Id] [int] NOT NULL,
	[Menu_Id] [int] NOT NULL,
 CONSTRAINT [PK_RolMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RolUsuario]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolUsuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rol_Id] [int] NOT NULL,
	[Usuario_Id] [int] NOT NULL,
 CONSTRAINT [PK_RolUsuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 26-06-2017 2:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[Foto] [image] NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido_Paterno] [varchar](50) NOT NULL,
	[Apellido_Materno] [varchar](50) NULL,
	[Ultimo_Acceso] [datetime] NULL,
	[Debe_Cambiar_Password] [bit] NOT NULL CONSTRAINT [DF_Usuario_Debe_Cambiar_Password]  DEFAULT ((0)),
	[Activo] [bit] NOT NULL CONSTRAINT [DF_Usuario_Activo]  DEFAULT ((1)),
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Menu]  WITH CHECK ADD  CONSTRAINT [FK_Menu_Menu] FOREIGN KEY([Menu_Id])
REFERENCES [dbo].[Menu] ([Id])
GO
ALTER TABLE [dbo].[Menu] CHECK CONSTRAINT [FK_Menu_Menu]
GO
ALTER TABLE [dbo].[RolMenu]  WITH CHECK ADD  CONSTRAINT [FK_RolMenu_Menu] FOREIGN KEY([Menu_Id])
REFERENCES [dbo].[Menu] ([Id])
GO
ALTER TABLE [dbo].[RolMenu] CHECK CONSTRAINT [FK_RolMenu_Menu]
GO
ALTER TABLE [dbo].[RolMenu]  WITH CHECK ADD  CONSTRAINT [FK_RolMenu_Rol] FOREIGN KEY([Rol_Id])
REFERENCES [dbo].[Rol] ([Id])
GO
ALTER TABLE [dbo].[RolMenu] CHECK CONSTRAINT [FK_RolMenu_Rol]
GO
ALTER TABLE [dbo].[RolUsuario]  WITH CHECK ADD  CONSTRAINT [FK_RolUsuario_Rol] FOREIGN KEY([Rol_Id])
REFERENCES [dbo].[Rol] ([Id])
GO
ALTER TABLE [dbo].[RolUsuario] CHECK CONSTRAINT [FK_RolUsuario_Rol]
GO
ALTER TABLE [dbo].[RolUsuario]  WITH CHECK ADD  CONSTRAINT [FK_RolUsuario_Usuario] FOREIGN KEY([Usuario_Id])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[RolUsuario] CHECK CONSTRAINT [FK_RolUsuario_Usuario]
GO
