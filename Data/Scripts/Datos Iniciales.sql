USE [Template]

/*
UPDATE [Usuario] SET [Foto] = (SELECT IM.BulkColumn AS Bytes FROM OPENROWSET(BULK 'D:\GGaldamesD\IConstruye\Proyectos\Labs\dotNet\Template\Template.MVCApp\public\img\duck.jpg', SINGLE_BLOB) AS IM) WHERE [Id] = 1;
*/

/*
DELETE [Menu]
DELETE [Rol]
DELETE [RolMenu]
DELETE [RolUsuario]
DELETE [Usuario]

DBCC CHECKIDENT([Menu], RESEED, 0)
DBCC CHECKIDENT([Rol], RESEED, 0)
DBCC CHECKIDENT([RolMenu], RESEED, 0)
DBCC CHECKIDENT([RolUsuario], RESEED, 0)
DBCC CHECKIDENT([Usuario], RESEED, 0)

INSERT INTO [Menu] VALUES  ( NULL,'/Administration','icon-th-large','Administraci�n','Men� de Administraci�n del Sistema.',1,1)
INSERT INTO [Menu] VALUES  ( 1,'/Administration/Role','icon-tasks','Rol','Men� de Administraci�n de Roles.',2,1)
INSERT INTO [Menu] VALUES  ( 1,'/Administration/User','icon-group','Usuario','Men� de Administraci�n de Usuarios.',3,1)
INSERT INTO [Menu] VALUES  ( 1,'/Administration/Menu','icon-th-list','Men�','Men� de Administraci�n de Opciones de Men�.',4,1)
INSERT INTO [Menu] VALUES  ( NULL,'/Home/Help','icon-question-sign','Ayuda','Men� de Ayuda.',5,1)
INSERT INTO [Rol] VALUES  ( 'Administrador', 1, 'Rol de Administraci�n del Sistema.' )
INSERT INTO [Rol] VALUES  ( 'Usuario', 1, 'Rol de Usuario Com�n del Sistema.' )
INSERT INTO [Usuario] VALUES  ( 'gerardogaldames@gmail.com','f1lLXFpXHwA=',NULL,'Gerardo','Galdames','D�az','2016-11-10 17:44:08',0,1 )
INSERT INTO [Usuario] VALUES  ( 'gerardo.galdames@iconstruye.com','f1lLXFpXHwA=',NULL,'Gerardo','Galdames','D�az','2016-11-10 17:44:08',0,1 )
INSERT INTO [RolUsuario] VALUES  ( 1, 1 )
INSERT INTO [RolUsuario] VALUES  ( 2, 2 )
INSERT [RolMenu] VALUES  ( 1, 1 )
INSERT [RolMenu] VALUES  ( 1, 2 )
INSERT [RolMenu] VALUES  ( 1, 3 )
INSERT [RolMenu] VALUES  ( 1, 4 )
INSERT [RolMenu] VALUES  ( 1, 5 )

INSERT INTO [Email]
        ( [NombreRemitente]
        ,[DireccionRemitente]
        ,[DireccionesCC]
        ,[DireccionesCCO]
        ,[Asunto]
        ,[Cuerpo]
        ,[Descripcion]
        ,[Activo]
        )
VALUES  ( 'Template'
        ,'sender@template.com'
        ,NULL
        ,NULL
        ,'Password Recovery'
        ,'<!DOCTYPE html>
<html lang="es">
<head>
	<meta charset="UTF-8">
	<style type="text/css">
		body {
			font-family: Arial, Helvetica, sans-serif;
			font-size: small;
			padding: 12px;
		}

		p {
			text-align: justify;
		@
		p.parrafo {
			text-indent: 24px;
		}
	</style>
</head>
<body>
	<p><strong>Estimad@ {NombreDestinatario}</strong></p>
	<br><br>
	<p class="parrafo">Hemos recibido una solicitud para cambiar su contrase�a en <strong>{NombreAplicacion}</strong></p>
	<br><br>
	<p class="parrafo">Su nueva contrase�a es <strong>{NuevaPassword}</strong>, le recomendamos encarecidamente actualizarla lo antes posible.</p>
	<br><br>
	<p>Atentamente,</p>
	<p>El Equipo de {NombreAplicacion}.</p>
</body>
</html>
'
        ,'Recover Password Email Template'
        ,1 
        )
*/

SELECT * FROM [Menu]
SELECT * FROM [Rol]
SELECT * FROM [RolMenu]
SELECT * FROM [RolUsuario]
SELECT * FROM [Usuario]
SELECT * FROM [Email]





