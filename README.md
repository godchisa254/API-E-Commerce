"# taller1" 
INSTRUCCIONES PARA EJECUTAR EL PROYECTO

REQUISITOS PREVIOS
Antes de comenzar, asegúrate de cumplir con los siguientes requisitos:
1. Tener instalado .NET SDK 
2. Contar con una cuenta en Cloudinary para la gestión de archivos multimedia.
3. Acceso al repositorio del proyecto.

PASOS PARA CONFIGURAR Y EJECUTAR EL PROYECTO

1. Clonar el repositorio
   En la terminal, navega a la carpeta donde deseas almacenar el proyecto y ejecuta el siguiente comando:
   git clone <URL-del-repositorio>

2. Configurar las credenciales
   Para que el proyecto funcione correctamente, realiza las siguientes configuraciones:

   Crear un archivo .env
   En la raíz del proyecto, crea un archivo llamado .env con el siguiente contenido "informacion sensible":
   
   Crear un archivo appSettings.json
   En la raíz del proyecto, crea un archivo llamado appSettings.json con la configuración de Cloudinary. Este archivo debe tener el siguiente formato:
   {
       "CloudinarySettings": {
           "CloudName": "tu_cloud_name",
           "ApiKey": "tu_api_key",
           "ApiSecret": "tu_api_secret"
       },
       "Logging": {
           "LogLevel": {
               "Default": "Information",
               "Microsoft": "Warning",
               "Microsoft.Hosting.Lifetime": "Information"
           }
       },
       "AllowedHosts": "*"
   }
  

3. Ejecutar el proyecto
   Para iniciar el proyecto, abre la terminal en la carpeta raíz y ejecuta el siguiente comando:
   dotnet watch run
   Esto iniciará el servidor y el proyecto estará listo para usarse.

NOTAS IMPORTANTES
1. Si tienes dudas sobre cómo obtener tus credenciales de Cloudinary, consulta su documentación oficial: https://cloudinary.com/documentation
