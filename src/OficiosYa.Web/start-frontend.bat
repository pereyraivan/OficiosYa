@echo off
cd ..\frontend-oficiosya
if exist node_modules (
    npm run dev
) else (
    echo Instalando dependencias del frontend...
    npm install
    npm run dev
)

