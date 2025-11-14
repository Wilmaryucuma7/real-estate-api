#!/bin/bash

echo "================================"
echo "  Real Estate API - Docker"
echo "================================"
echo ""

show_menu() {
    echo "Selecciona una opción:"
    echo ""
    echo "1. Iniciar API + MongoDB"
    echo "2. Detener contenedores"
    echo "3. Ver logs del API"
    echo "4. Ver logs de MongoDB"
    echo "5. Reiniciar todo"
    echo "6. Limpiar todo (eliminar datos)"
    echo "7. Abrir Swagger en navegador"
    echo "8. Ver estado de contenedores"
    echo "9. Salir"
    echo ""
    read -p "Opción: " option
    
    case $option in
        1) start_containers ;;
        2) stop_containers ;;
        3) logs_api ;;
        4) logs_mongo ;;
        5) restart_containers ;;
        6) clean_all ;;
        7) open_swagger ;;
        8) show_status ;;
        9) exit 0 ;;
        *) echo "Opción inválida"; sleep 2; show_menu ;;
    esac
}

start_containers() {
    echo ""
    echo "Iniciando contenedores..."
    docker-compose up -d
    echo ""
    echo "? Contenedores iniciados!"
    echo ""
    echo "API: http://localhost:5000"
    echo "Swagger: http://localhost:5000/swagger"
    echo "MongoDB: mongodb://localhost:27017"
    echo ""
    read -p "Presiona Enter para continuar..."
    show_menu
}

stop_containers() {
    echo ""
    echo "Deteniendo contenedores..."
    docker-compose down
    echo ""
    echo "? Contenedores detenidos!"
    echo ""
    read -p "Presiona Enter para continuar..."
    show_menu
}

logs_api() {
    echo ""
    echo "Mostrando logs del API (Ctrl+C para salir)..."
    docker-compose logs -f api
    show_menu
}

logs_mongo() {
    echo ""
    echo "Mostrando logs de MongoDB (Ctrl+C para salir)..."
    docker-compose logs -f mongodb
    show_menu
}

restart_containers() {
    echo ""
    echo "Reiniciando contenedores..."
    docker-compose restart
    echo ""
    echo "? Contenedores reiniciados!"
    echo ""
    read -p "Presiona Enter para continuar..."
    show_menu
}

clean_all() {
    echo ""
    echo "??  ADVERTENCIA: Esto eliminará TODOS los datos!"
    echo ""
    read -p "¿Estás seguro? (s/n): " confirm
    if [[ $confirm == "s" || $confirm == "S" ]]; then
        docker-compose down -v
        echo ""
        echo "? Datos eliminados! Ejecuta opción 1 para crear nuevos."
        echo ""
    fi
    read -p "Presiona Enter para continuar..."
    show_menu
}

open_swagger() {
    echo ""
    echo "Abriendo Swagger en el navegador..."
    
    if [[ "$OSTYPE" == "darwin"* ]]; then
        open http://localhost:5000/swagger
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        xdg-open http://localhost:5000/swagger 2>/dev/null || echo "Por favor abre manualmente: http://localhost:5000/swagger"
    fi
    
    read -p "Presiona Enter para continuar..."
    show_menu
}

show_status() {
    echo ""
    echo "Estado de contenedores:"
    docker-compose ps
    echo ""
    read -p "Presiona Enter para continuar..."
    show_menu
}

show_menu
