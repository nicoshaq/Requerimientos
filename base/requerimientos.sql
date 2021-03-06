/****** Object:  Database [requerimientos]    Script Date: 11-Apr-19 3:58:41 PM ******/
CREATE DATABASE [requerimientos] 


USE [requerimientos]
GO
/****** Object:  StoredProcedure [dbo].[AgruparCarpetas]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AgruparCarpetas] 
	
	@Idmensaje INT,
	@Idcarpeta INT
	


AS
BEGIN
	
	SET NOCOUNT ON;

	update dbo.Mensajes
	set IdCarpeta = @Idcarpeta
	where Id = @Idmensaje



	

END

GO
/****** Object:  StoredProcedure [dbo].[historialDelegacion]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[historialDelegacion] 
	
	@Idmensaje INT,
	@Idproyecto INT,
	@Usuariodelega VARCHAR(150),
	@Usuariodelegado VARCHAR(150)


AS
BEGIN
	
	SET NOCOUNT ON;

	INSERT INTO dbo.HistorialDelega
	(
	    Idproyecto,
	    Idmensaje,
	    Usuariodelega,
	    Usuariodelegado
	)
	VALUES
	(   @Idproyecto,  -- Idproyecto - int
	    @Idmensaje,  -- Idmensaje - int
	    @Usuariodelega, -- Usuariodelega - varchar(150)
	    @Usuariodelegado  -- Usuariodelegado - varchar(150)
	    )

END

GO
/****** Object:  Table [dbo].[Archivos]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Archivos](
	[Id] [uniqueidentifier] NOT NULL,
	[Nombre] [varchar](max) NULL,
	[Idusuario] [int] NULL,
	[Fecha] [datetime] NULL,
	[Idmensaje] [int] NULL,
	[Extension] [varchar](50) NULL,
	[Idnovedad] [int] NULL,
	[Idproyecto] [int] NULL,
 CONSTRAINT [PK_Archivos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Area]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Area](
	[Idarea] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](500) NULL,
 CONSTRAINT [PK_Area] PRIMARY KEY CLUSTERED 
(
	[Idarea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Carpetas]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Carpetas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](250) NOT NULL,
	[User_Id] [int] NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_Carpetas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Estado_novedades]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Estado_novedades](
	[idestadonovedad] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[idusuario] [int] NULL,
	[color] [varchar](10) NULL,
 CONSTRAINT [PK_Estado_novedades] PRIMARY KEY CLUSTERED 
(
	[idestadonovedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Estado_requerimiento]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Estado_requerimiento](
	[Idestado] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](150) NULL,
	[idusuario] [int] NULL,
	[color] [varchar](10) NULL,
 CONSTRAINT [PK_Estado_requerimiento] PRIMARY KEY CLUSTERED 
(
	[Idestado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HistorialDelega]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HistorialDelega](
	[Idhistorial] [int] IDENTITY(1,1) NOT NULL,
	[Idproyecto] [int] NULL,
	[Idmensaje] [int] NULL,
	[Usuariodelega] [varchar](150) NULL,
	[Usuariodelegado] [varchar](150) NULL,
 CONSTRAINT [PK_HistorialDelega] PRIMARY KEY CLUSTERED 
(
	[Idhistorial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LNK_ROL_Permiso]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_ROL_Permiso](
	[Rol_Id] [int] NOT NULL,
	[Permiso_Id] [int] NOT NULL,
 CONSTRAINT [PK_LNK_ROL_Permiso] PRIMARY KEY CLUSTERED 
(
	[Rol_Id] ASC,
	[Permiso_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LNK_Usuario_Rol]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LNK_Usuario_Rol](
	[User_Id] [int] NOT NULL,
	[Rol_Id] [int] NOT NULL,
 CONSTRAINT [PK_LNK_Usuario_Rol] PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC,
	[Rol_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Mensajes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[Remitente] [varchar](150) NULL,
	[Asunto] [varchar](150) NULL,
	[Mensaje] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[Estado] [varchar](50) NULL,
	[Idusuariodestino] [int] NULL,
	[Maildestino] [nvarchar](150) NULL,
	[Destinatario] [varchar](150) NULL,
	[Email] [nvarchar](150) NULL,
	[Idpadre] [int] NULL,
	[Idproyecto] [int] NULL,
	[Idusuariodelega] [int] NULL,
	[Usuariodelega] [varchar](150) NULL,
	[Idestado] [int] NULL,
	[IdCarpeta] [int] NULL,
 CONSTRAINT [PK_Mensajes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Novedades]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Novedades](
	[Idnovedad] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [varchar](max) NULL,
	[Fecha] [datetime] NULL,
	[Idmensaje] [int] NULL,
	[Estado] [varchar](50) NULL,
	[Usuario] [varchar](255) NULL,
	[Titulo] [varchar](250) NULL,
	[Idproyecto] [int] NULL,
	[Idestadonovedad] [int] NULL,
	[User_Id] [int] NULL,
	[Privado] [bit] NULL,
 CONSTRAINT [PK_Novedades] PRIMARY KEY CLUSTERED 
(
	[Idnovedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permisos]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permisos](
	[Permiso_Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Permisos] PRIMARY KEY CLUSTERED 
(
	[Permiso_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Proyectos]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Proyectos](
	[Idproyecto] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](500) NULL,
	[Creadopor] [varchar](250) NULL,
	[Idmensaje] [int] NULL,
	[Descripcion] [varchar](max) NULL,
	[User_Id] [int] NULL,
	[Fecha] [datetime] NULL,
	[Fechamodif] [datetime] NULL,
	[Estado] [varchar](50) NULL,
 CONSTRAINT [PK_Proyectos] PRIMARY KEY CLUSTERED 
(
	[Idproyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ROLES]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ROLES](
	[Rol_Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](250) NULL,
	[EsAdmin] [bit] NULL,
	[Modificado] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_Roles] PRIMARY KEY CLUSTERED 
(
	[Rol_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 11-Apr-19 3:58:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[User_Id] [int] IDENTITY(1,1) NOT NULL,
	[Usuario] [nvarchar](30) NOT NULL,
	[Modificado] [datetime] NULL,
	[Inactivo] [bit] NULL,
	[Nombre] [nvarchar](50) NULL,
	[Apellido] [nvarchar](50) NULL,
	[Titulo] [nvarchar](30) NULL,
	[Inicial] [nvarchar](3) NULL,
	[EMail] [nvarchar](100) NULL,
	[Idproyectos] [int] NULL,
	[Idarea] [int] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ROLES] ADD  CONSTRAINT [DF_ROLES_EsAdmin]  DEFAULT ((0)) FOR [EsAdmin]
GO
ALTER TABLE [dbo].[ROLES] ADD  CONSTRAINT [DF_ROLES_Modificado]  DEFAULT (getdate()) FOR [Modificado]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_Inactivo]  DEFAULT ((0)) FOR [Inactivo]
GO
ALTER TABLE [dbo].[Archivos]  WITH CHECK ADD  CONSTRAINT [FK_Archivos_Mensajes] FOREIGN KEY([Idmensaje])
REFERENCES [dbo].[Mensajes] ([Id])
GO
ALTER TABLE [dbo].[Archivos] CHECK CONSTRAINT [FK_Archivos_Mensajes]
GO
ALTER TABLE [dbo].[Archivos]  WITH CHECK ADD  CONSTRAINT [FK_Archivos_Novedades] FOREIGN KEY([Idnovedad])
REFERENCES [dbo].[Novedades] ([Idnovedad])
GO
ALTER TABLE [dbo].[Archivos] CHECK CONSTRAINT [FK_Archivos_Novedades]
GO
ALTER TABLE [dbo].[Archivos]  WITH CHECK ADD  CONSTRAINT [FK_Archivos_Proyectos] FOREIGN KEY([Idproyecto])
REFERENCES [dbo].[Proyectos] ([Idproyecto])
GO
ALTER TABLE [dbo].[Archivos] CHECK CONSTRAINT [FK_Archivos_Proyectos]
GO
ALTER TABLE [dbo].[Carpetas]  WITH CHECK ADD  CONSTRAINT [FK_Carpetas_Usuarios] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Carpetas] CHECK CONSTRAINT [FK_Carpetas_Usuarios]
GO
ALTER TABLE [dbo].[Estado_novedades]  WITH CHECK ADD  CONSTRAINT [FK_Estado_novedades_Usuarios] FOREIGN KEY([idusuario])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Estado_novedades] CHECK CONSTRAINT [FK_Estado_novedades_Usuarios]
GO
ALTER TABLE [dbo].[Estado_requerimiento]  WITH CHECK ADD  CONSTRAINT [FK_Estado_requerimiento_Usuarios] FOREIGN KEY([idusuario])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Estado_requerimiento] CHECK CONSTRAINT [FK_Estado_requerimiento_Usuarios]
GO
ALTER TABLE [dbo].[HistorialDelega]  WITH CHECK ADD  CONSTRAINT [FK_HistorialDelega_Mensajes] FOREIGN KEY([Idmensaje])
REFERENCES [dbo].[Mensajes] ([Id])
GO
ALTER TABLE [dbo].[HistorialDelega] CHECK CONSTRAINT [FK_HistorialDelega_Mensajes]
GO
ALTER TABLE [dbo].[HistorialDelega]  WITH CHECK ADD  CONSTRAINT [FK_HistorialDelega_Proyectos] FOREIGN KEY([Idproyecto])
REFERENCES [dbo].[Proyectos] ([Idproyecto])
GO
ALTER TABLE [dbo].[HistorialDelega] CHECK CONSTRAINT [FK_HistorialDelega_Proyectos]
GO
ALTER TABLE [dbo].[LNK_ROL_Permiso]  WITH NOCHECK ADD  CONSTRAINT [FK_LNK_ROL_Permiso_Permisos] FOREIGN KEY([Permiso_Id])
REFERENCES [dbo].[Permisos] ([Permiso_Id])
GO
ALTER TABLE [dbo].[LNK_ROL_Permiso] CHECK CONSTRAINT [FK_LNK_ROL_Permiso_Permisos]
GO
ALTER TABLE [dbo].[LNK_ROL_Permiso]  WITH NOCHECK ADD  CONSTRAINT [FK_LNK_ROL_Permiso_ROLES] FOREIGN KEY([Rol_Id])
REFERENCES [dbo].[ROLES] ([Rol_Id])
GO
ALTER TABLE [dbo].[LNK_ROL_Permiso] CHECK CONSTRAINT [FK_LNK_ROL_Permiso_ROLES]
GO
ALTER TABLE [dbo].[LNK_Usuario_Rol]  WITH NOCHECK ADD  CONSTRAINT [FK_LNK_Usuario_Rol_ROLES] FOREIGN KEY([Rol_Id])
REFERENCES [dbo].[ROLES] ([Rol_Id])
GO
ALTER TABLE [dbo].[LNK_Usuario_Rol] CHECK CONSTRAINT [FK_LNK_Usuario_Rol_ROLES]
GO
ALTER TABLE [dbo].[LNK_Usuario_Rol]  WITH NOCHECK ADD  CONSTRAINT [FK_LNK_Usuario_Rol_Usuarios] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[LNK_Usuario_Rol] CHECK CONSTRAINT [FK_LNK_Usuario_Rol_Usuarios]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Carpetas_Mensajes] FOREIGN KEY([IdCarpeta])
REFERENCES [dbo].[Carpetas] ([Id])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Carpetas_Mensajes]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Estado_requerimiento] FOREIGN KEY([Idestado])
REFERENCES [dbo].[Estado_requerimiento] ([Idestado])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Estado_requerimiento]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Proyectos] FOREIGN KEY([Idproyecto])
REFERENCES [dbo].[Proyectos] ([Idproyecto])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Proyectos]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Usuarios] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Usuarios]
GO
ALTER TABLE [dbo].[Novedades]  WITH CHECK ADD  CONSTRAINT [FK_Novedades_Estado_novedades] FOREIGN KEY([Idestadonovedad])
REFERENCES [dbo].[Estado_novedades] ([idestadonovedad])
GO
ALTER TABLE [dbo].[Novedades] CHECK CONSTRAINT [FK_Novedades_Estado_novedades]
GO
ALTER TABLE [dbo].[Novedades]  WITH CHECK ADD  CONSTRAINT [FK_Novedades_Mensajes] FOREIGN KEY([Idmensaje])
REFERENCES [dbo].[Mensajes] ([Id])
GO
ALTER TABLE [dbo].[Novedades] CHECK CONSTRAINT [FK_Novedades_Mensajes]
GO
ALTER TABLE [dbo].[Novedades]  WITH CHECK ADD  CONSTRAINT [FK_Novedades_Proyectos] FOREIGN KEY([Idproyecto])
REFERENCES [dbo].[Proyectos] ([Idproyecto])
GO
ALTER TABLE [dbo].[Novedades] CHECK CONSTRAINT [FK_Novedades_Proyectos]
GO
ALTER TABLE [dbo].[Novedades]  WITH CHECK ADD  CONSTRAINT [FK_Novedades_Usuarios] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Novedades] CHECK CONSTRAINT [FK_Novedades_Usuarios]
GO
ALTER TABLE [dbo].[Proyectos]  WITH CHECK ADD  CONSTRAINT [FK_Proyectos_Usuarios] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Usuarios] ([User_Id])
GO
ALTER TABLE [dbo].[Proyectos] CHECK CONSTRAINT [FK_Proyectos_Usuarios]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Area] FOREIGN KEY([Idarea])
REFERENCES [dbo].[Area] ([Idarea])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Area]
GO

