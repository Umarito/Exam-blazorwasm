@echo off
REM Avrang Project Launch Script
REM This script launches both WebAPI and Admin Panel

echo.
echo ====================================
echo   AVRANG PROJECT - QUICK START
echo ====================================
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET 9.0 SDK first
    pause
    exit /b 1
)

echo [INFO] .NET SDK found
echo.
echo This script will:
echo  1. Start WebAPI on http://localhost:5094
echo  2. Start Admin Panel on http://localhost:5001
echo.
pause

REM Start WebAPI in new window
echo [STARTING] WebAPI on port 5094...
start "Avrang WebAPI" cmd /k "cd webApi && dotnet run"

REM Wait a bit for API to start
echo [WAITING] 5 seconds for API to initialize...
timeout /t 5 /nobreak

REM Start Admin Panel in new window
echo [STARTING] Admin Panel on port 5001...
start "Avrang Admin Panel" cmd /k "cd Admin && dotnet run"

echo.
echo ====================================
echo   STARTUP COMPLETE!
echo ====================================
echo.
echo WebAPI:     http://localhost:5094
echo Swagger:    http://localhost:5094/swagger/index.html
echo Admin:      http://localhost:5001/login
echo.
echo Default Credentials:
echo   Email:    admin@gmail.com
echo   Password: Admin123
echo.
echo Press any key to open Admin Panel in browser...
pause

REM Open Admin Panel in default browser
start http://localhost:5001/login

echo.
echo Opening Swagger API Documentation...
start http://localhost:5094/swagger/index.html

echo.
echo [SUCCESS] All services started!
echo Close these windows to stop the services.
echo.
