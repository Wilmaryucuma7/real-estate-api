@echo off
echo ================================
echo   Real Estate API - Docker
echo ================================
echo.

:menu
echo Selecciona una opcion:
echo.
echo 1. Iniciar API + MongoDB
echo 2. Detener contenedores
echo 3. Ver logs del API
echo 4. Ver logs de MongoDB
echo 5. Reiniciar todo
echo 6. Limpiar todo (eliminar datos)
echo 7. Abrir Swagger en navegador
echo 8. Ver estado de contenedores
echo 9. Salir
echo.

set /p option="Opcion: "

if "%option%"=="1" goto start
if "%option%"=="2" goto stop
if "%option%"=="3" goto logs_api
if "%option%"=="4" goto logs_mongo
if "%option%"=="5" goto restart
if "%option%"=="6" goto clean
if "%option%"=="7" goto swagger
if "%option%"=="8" goto status
if "%option%"=="9" goto end

echo Opcion invalida
pause
goto menu

:start
echo.
echo Iniciando contenedores...
docker-compose up -d
echo.
echo ? Contenedores iniciados!
echo.
echo API: http://localhost:5000
echo Swagger: http://localhost:5000/swagger
echo MongoDB: mongodb://localhost:27017
echo.
pause
goto menu

:stop
echo.
echo Deteniendo contenedores...
docker-compose down
echo.
echo ? Contenedores detenidos!
echo.
pause
goto menu

:logs_api
echo.
echo Mostrando logs del API (Ctrl+C para salir)...
docker-compose logs -f api
goto menu

:logs_mongo
echo.
echo Mostrando logs de MongoDB (Ctrl+C para salir)...
docker-compose logs -f mongodb
goto menu

:restart
echo.
echo Reiniciando contenedores...
docker-compose restart
echo.
echo ? Contenedores reiniciados!
echo.
pause
goto menu

:clean
echo.
echo ??  ADVERTENCIA: Esto eliminara TODOS los datos!
echo.
set /p confirm="¿Estas seguro? (s/n): "
if /i "%confirm%"=="s" (
    docker-compose down -v
    echo.
    echo ? Datos eliminados! Ejecuta opcion 1 para crear nuevos.
    echo.
)
pause
goto menu

:swagger
echo.
echo Abriendo Swagger en el navegador...
start http://localhost:5000/swagger
pause
goto menu

:status
echo.
echo Estado de contenedores:
docker-compose ps
echo.
pause
goto menu

:end
echo.
echo ¡Hasta luego!
pause
exit
