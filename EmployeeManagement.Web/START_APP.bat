@echo off
echo ========================================
echo Starting Employee Management System
echo ========================================
echo.
echo Application will be available at:
echo http://localhost:5000
echo.
echo Press Ctrl+C to stop the application
echo ========================================
echo.

cd /d "%~dp0"
dotnet run --urls "http://localhost:5000"

pause
