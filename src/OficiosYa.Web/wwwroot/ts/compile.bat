@echo off
echo Compilando TypeScript...

cd /d "%~dp0"
tsc

if %ERRORLEVEL% EQU 0 (
    echo Compilacion exitosa!
    echo Archivos JavaScript generados en ../js/
) else (
    echo Error en la compilacion!
    pause
)
