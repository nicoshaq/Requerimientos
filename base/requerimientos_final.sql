USE [master]
GO
/****** Object:  Database [requerimientos]    Script Date: 02-May-19 2:05:32 AM ******/
CREATE DATABASE [requerimientos] ON  PRIMARY 
( NAME = N'requerimientos', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\requerimientos.mdf' , SIZE = 3328KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'requerimientos_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\requerimientos_log.LDF' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [requerimientos] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [requerimientos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [requerimientos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [requerimientos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [requerimientos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [requerimientos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [requerimientos] SET ARITHABORT OFF 
GO
ALTER DATABASE [requerimientos] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [requerimientos] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [requerimientos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [requerimientos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [requerimientos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [requerimientos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [requerimientos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [requerimientos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [requerimientos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [requerimientos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [requerimientos] SET  ENABLE_BROKER 
GO
ALTER DATABASE [requerimientos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [requerimientos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [requerimientos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [requerimientos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [requerimientos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [requerimientos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [requerimientos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [requerimientos] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [requerimientos] SET  MULTI_USER 
GO
ALTER DATABASE [requerimientos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [requerimientos] SET DB_CHAINING OFF 
GO
USE [requerimientos]
GO
/****** Object:  StoredProcedure [dbo].[AgruparCarpetas]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  StoredProcedure [dbo].[historialDelegacion]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Archivos]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Area]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Carpetas]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Estado_novedades]    Script Date: 02-May-19 2:05:32 AM ******/
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
	[Idarea] [int] NULL,
 CONSTRAINT [PK_Estado_novedades] PRIMARY KEY CLUSTERED 
(
	[idestadonovedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Estado_requerimiento]    Script Date: 02-May-19 2:05:32 AM ******/
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
	[Idarea] [int] NULL,
 CONSTRAINT [PK_Estado_requerimiento] PRIMARY KEY CLUSTERED 
(
	[Idestado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HistorialDelega]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[LNK_ROL_Permiso]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[LNK_Usuario_Rol]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Mensajes]    Script Date: 02-May-19 2:05:32 AM ******/
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
	[Prioridad] [varchar](50) NULL,
	[Naturaleza] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Objetivo] [varchar](max) NULL,
	[Alcance] [varchar](max) NULL,
	[Func1] [varchar](250) NULL,
	[Func2] [varchar](250) NULL,
	[Func3] [varchar](250) NULL,
	[Func4] [varchar](250) NULL,
	[Func5] [varchar](250) NULL,
	[Func6] [varchar](250) NULL,
	[Impresa1] [varchar](250) NULL,
	[Impresa2] [varchar](250) NULL,
	[Impresa3] [varchar](250) NULL,
	[Pantalla1] [varchar](250) NULL,
	[Pantalla2] [varchar](250) NULL,
	[Pantalla3] [varchar](250) NULL,
	[Porarchivo1] [varchar](250) NULL,
	[Porarchivo2] [varchar](250) NULL,
	[Porarchivo3] [varchar](250) NULL,
	[Ventaja] [varchar](250) NULL,
	[Arearelacion] [varchar](100) NULL,
	[Afectado] [varchar](250) NULL,
	[Fechadeseada] [varchar](50) NULL,
	[Normas] [varchar](250) NULL,
	[Docadjunta] [varchar](250) NULL,
	[Area1] [varchar](100) NULL,
	[Area2] [varchar](100) NULL,
	[Area3] [varchar](100) NULL,
	[Gerente1] [varchar](100) NULL,
	[Gerente2] [varchar](100) NULL,
	[Gerente3] [varchar](100) NULL,
	[Firma1] [varchar](100) NULL,
	[Firma2] [varchar](100) NULL,
	[Firma3] [varchar](100) NULL,
	[Confeccionado] [varchar](100) NULL,
	[Gerentearea] [varchar](100) NULL,
	[Recepcionado] [varchar](100) NULL,
	[Nueva1] [bit] NOT NULL,
	[Nueva2] [bit] NOT NULL,
	[Nueva3] [bit] NOT NULL,
	[Nueva4] [bit] NOT NULL,
	[Nueva5] [bit] NOT NULL,
	[Nueva6] [bit] NOT NULL,
	[Modif1] [bit] NOT NULL,
	[Modif2] [bit] NOT NULL,
	[Modif3] [bit] NOT NULL,
	[Modif4] [bit] NOT NULL,
	[Modif5] [bit] NOT NULL,
	[Modif6] [bit] NOT NULL,
	[Eliminar1] [bit] NOT NULL,
	[Eliminar2] [bit] NOT NULL,
	[Eliminar3] [bit] NOT NULL,
	[Eliminar4] [bit] NOT NULL,
	[Eliminar5] [bit] NOT NULL,
	[Eliminar6] [bit] NOT NULL,
	[Modifimp1] [bit] NOT NULL,
	[Modifimp2] [bit] NOT NULL,
	[Modifimp3] [bit] NOT NULL,
	[Modifpant1] [bit] NOT NULL,
	[Modifpant2] [bit] NOT NULL,
	[Modifpant3] [bit] NOT NULL,
	[Modifarch1] [bit] NOT NULL,
	[Modifarch2] [bit] NOT NULL,
	[Modifarch3] [bit] NOT NULL,
	[Eliminaimp1] [bit] NOT NULL,
	[Eliminaimp2] [bit] NOT NULL,
	[Eliminaimp3] [bit] NOT NULL,
	[Eliminapant1] [bit] NOT NULL,
	[Eliminapant2] [bit] NOT NULL,
	[Eliminapant3] [bit] NOT NULL,
	[Eliminarch1] [bit] NOT NULL,
	[Eliminarch2] [bit] NOT NULL,
	[Eliminarch3] [bit] NOT NULL,
	[Nuevaimp1] [bit] NOT NULL,
	[Nuevaimp2] [bit] NOT NULL,
	[Nuevaimp3] [bit] NOT NULL,
	[Nuevapant1] [bit] NOT NULL,
	[Nuevapant2] [bit] NOT NULL,
	[Nuevapant3] [bit] NOT NULL,
	[Nuevarch1] [bit] NOT NULL,
	[Nuevarch2] [bit] NOT NULL,
	[Nuevarch3] [bit] NOT NULL,
	[Idarea] [int] NULL,
	[Estadodelegado] [varchar](50) NULL,
 CONSTRAINT [PK_Mensajes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Novedades]    Script Date: 02-May-19 2:05:32 AM ******/
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
	[Idarea] [int] NULL,
 CONSTRAINT [PK_Novedades] PRIMARY KEY CLUSTERED 
(
	[Idnovedad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permisos]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Proyectos]    Script Date: 02-May-19 2:05:32 AM ******/
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
	[Idarea] [int] NULL,
 CONSTRAINT [PK_Proyectos] PRIMARY KEY CLUSTERED 
(
	[Idproyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Requerimiento_mensaje]    Script Date: 02-May-19 2:05:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Requerimiento_mensaje](
	[Id] [int] NOT NULL,
	[User_id] [int] NULL,
	[Prioridad] [varchar](50) NULL,
	[Naturaleza] [varchar](50) NULL,
	[Descripcion] [varchar](max) NULL,
	[Objetivo] [varchar](max) NULL,
	[Alcance] [varchar](max) NULL,
	[Func1] [varchar](250) NULL,
	[Func2] [varchar](250) NULL,
	[Func3] [varchar](250) NULL,
	[Func4] [varchar](250) NULL,
	[Func5] [varchar](250) NULL,
	[Func6] [varchar](250) NULL,
	[Nueva1] [varchar](10) NULL,
	[Nueva2] [varchar](10) NULL,
	[Nueva3] [varchar](10) NULL,
	[Nueva4] [varchar](10) NULL,
	[Nueva5] [varchar](10) NULL,
	[Nueva6] [varchar](10) NULL,
	[Modif1] [varchar](10) NULL,
	[Modif2] [varchar](10) NULL,
	[Modif3] [varchar](10) NULL,
	[Modif4] [varchar](10) NULL,
	[Modif5] [varchar](10) NULL,
	[Modif6] [varchar](10) NULL,
	[Eliminar1] [varchar](10) NULL,
	[Eliminar2] [varchar](10) NULL,
	[Eliminar3] [varchar](10) NULL,
	[Eliminar4] [varchar](10) NULL,
	[Eliminar5] [varchar](10) NULL,
	[Eliminar6] [varchar](10) NULL,
	[Impresa1] [varchar](250) NULL,
	[Impresa2] [varchar](250) NULL,
	[Impresa3] [varchar](250) NULL,
	[Pantalla1] [varchar](250) NULL,
	[Pantalla2] [varchar](250) NULL,
	[Pantalla3] [varchar](250) NULL,
	[Porarchivo1] [varchar](250) NULL,
	[Porarchivo2] [varchar](250) NULL,
	[Porarchivo3] [varchar](250) NULL,
	[Modifimp1] [varchar](10) NULL,
	[Modifimp2] [varchar](10) NULL,
	[Modifimp3] [varchar](10) NULL,
	[Modifpant1] [varchar](10) NULL,
	[Modifpant2] [varchar](10) NULL,
	[Modifpant3] [varchar](10) NULL,
	[Modifarch1] [varchar](10) NULL,
	[Modifarch2] [varchar](10) NULL,
	[Modifarch3] [varchar](10) NULL,
	[Eliminaimp1] [varchar](10) NULL,
	[Eliminaimp2] [varchar](10) NULL,
	[Eliminaimp3] [varchar](10) NULL,
	[Eliminapant1] [varchar](10) NULL,
	[Eliminapant2] [varchar](10) NULL,
	[Eliminapant3] [varchar](10) NULL,
	[Eliminarch1] [varchar](10) NULL,
	[Eliminarch2] [varchar](10) NULL,
	[Eliminarch3] [varchar](10) NULL,
	[Ventaja] [varchar](250) NULL,
	[Arearelacion] [varchar](100) NULL,
	[Afectado] [varchar](250) NULL,
	[Fechadeseada] [varchar](50) NULL,
	[Normas] [varchar](250) NULL,
	[Docadjunta] [varchar](250) NULL,
	[Area1] [varchar](100) NULL,
	[Area2] [varchar](100) NULL,
	[Area3] [varchar](100) NULL,
	[Gerente1] [varchar](100) NULL,
	[Gerente2] [varchar](100) NULL,
	[Gerente3] [varchar](100) NULL,
	[Firma1] [varchar](100) NULL,
	[Firma2] [varchar](100) NULL,
	[Firma3] [varchar](100) NULL,
	[Confeccionado] [varchar](100) NULL,
	[Gerentearea] [varchar](100) NULL,
	[Recepcionado] [varchar](100) NULL,
	[Mensaje_id] [int] NULL,
 CONSTRAINT [PK_Requerimiento_mensaje] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ROLES]    Script Date: 02-May-19 2:05:32 AM ******/
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 02-May-19 2:05:32 AM ******/
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
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva4]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva5]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nueva6]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif4]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif5]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modif6]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar4]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar5]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminar6]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifimp1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifimp2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifimp3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifpant1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifpant2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifpant3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifarch1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifarch2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Modifarch3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminaimp1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminaimp2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminaimp3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminapant1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminapant2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminapant3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminarch1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminarch2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Eliminarch3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevaimp1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevaimp2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevaimp3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevapant1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevapant2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevapant3]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevarch1]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevarch2]
GO
ALTER TABLE [dbo].[Mensajes] ADD  DEFAULT ((0)) FOR [Nuevarch3]
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
ALTER TABLE [dbo].[Requerimiento_mensaje]  WITH CHECK ADD  CONSTRAINT [FK_Requerimiento_menasje_Mensajes] FOREIGN KEY([Mensaje_id])
REFERENCES [dbo].[Mensajes] ([Id])
GO
ALTER TABLE [dbo].[Requerimiento_mensaje] CHECK CONSTRAINT [FK_Requerimiento_menasje_Mensajes]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Area] FOREIGN KEY([Idarea])
REFERENCES [dbo].[Area] ([Idarea])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Area]
GO
USE [master]
GO
ALTER DATABASE [requerimientos] SET  READ_WRITE 
GO
