# Avrang Project - PowerShell Launch Script
# Run: powershell -ExecutionPolicy Bypass -File START.ps1

Write-Host "===================================" -ForegroundColor Cyan
Write-Host "  AVRANG PROJECT - QUICK START" -ForegroundColor Cyan
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""

# Check if .NET is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "[✓] .NET SDK found: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "[✗] .NET SDK not found! Please install .NET 9.0" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "This script will start:
  1. WebAPI on http://localhost:5094
  2. Admin Panel on http://localhost:5001" -ForegroundColor Yellow
Write-Host ""
Read-Host "Press Enter to continue"

# Start WebAPI
Write-Host ""
Write-Host "[STARTING] WebAPI..." -ForegroundColor Cyan
$webApiJob = Start-Process -FilePath "powershell" -ArgumentList "-noExit", "-Command", "cd 'webApi' ; dotnet run" -PassThru -WindowStyle Normal

# Wait for API to start
Write-Host "[WAITING] 5 seconds for API to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Start Admin Panel
Write-Host "[STARTING] Admin Panel..." -ForegroundColor Cyan
$adminJob = Start-Process -FilePath "powershell" -ArgumentList "-noExit", "-Command", "cd 'Admin' ; dotnet run" -PassThru -WindowStyle Normal

# Wait a bit more
Start-Sleep -Seconds 3

Write-Host ""
Write-Host "===================================" -ForegroundColor Green
Write-Host "  STARTUP COMPLETE!" -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Green
Write-Host ""
Write-Host "Services:" -ForegroundColor Yellow
Write-Host "  WebAPI:     http://localhost:5094" -ForegroundColor Cyan
Write-Host "  Swagger:    http://localhost:5094/swagger/index.html" -ForegroundColor Cyan
Write-Host "  Admin:      http://localhost:5001/login" -ForegroundColor Cyan
Write-Host ""
Write-Host "Default Credentials:" -ForegroundColor Yellow
Write-Host "  Email:    admin@gmail.com" -ForegroundColor Cyan
Write-Host "  Password: Admin123" -ForegroundColor Cyan
Write-Host ""

# Open browsers
Write-Host "[OPENING] Admin Panel..." -ForegroundColor Cyan
Start-Process "http://localhost:5001/login"

Start-Sleep -Seconds 2

Write-Host "[OPENING] Swagger Documentation..." -ForegroundColor Cyan
Start-Process "http://localhost:5094/swagger/index.html"

Write-Host ""
Write-Host "[SUCCESS] All services started!" -ForegroundColor Green
Write-Host ""
Write-Host "Main PowerShell window will close, but services continue running"
Write-Host "in separate windows. Close those windows to stop services."
Write-Host ""

# Keep main window open briefly
Read-Host "Press Enter to close this window"
