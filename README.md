# DrawManager
A generic draw manager.

# Para construir la imagen del API:
1. Abrir Command Promp del SO.
2. Posicionarse en la carpeta donde se encuentra el archivo Dockerfile
3. Ejecutar el comando "docker build -t draw_manager_api:runtime --no-cache .", donde -t especifica el nombre de la etiqueta que tendrá la imagen, el punto está indicando que el archivo Dockerfile se encuentra en la carpeta donde estamos situados.

# Para crear un contenedor, levantarlo y copiar bd:
1. Ejecutar el comando "docker run -e "ASPNETCORE_ENVIRONMENT=Release" --name "clientDrawTest" -d -p 9999:80 draw_manager_api:runtime", donde -e especifica las variables de ambiente que deseo pasar, --name especifica el nombre del contenedor para identificarlo, -d que correrá en modo detach, -p que el puerto 9999 de host publicara la salida del puerto 80 del contenedor y finalmente el nombre de la imagen a partir de la que se creará el contenedor.
2. Ejecutar el comando "docker cp C:\In\draw_manager_docker_db.sqlite clientDrawTest:/app/db/" para copiar la bd desde el host hacia el contenedor.
3. Ejecutar el comando "docker cp clientDrawTest:/app/db/draw_manager_docker_db.sqlite C:\Out\" para copiar la bd desde el contenedor hacia el host.
