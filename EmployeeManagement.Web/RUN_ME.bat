@echo off
title Employee Management System
color 0A
cls

echo ========================================
echo    EMPLOYEE MANAGEMENT SYSTEM
echo ========================================
echo.
echo Starting application...
echo.
echo IMPORTANT: Keep this window open!
echo.
echo ========================================
echo.

cd /d "%~dp0"

echo Building application...
dotnet build
echo.

echo Starting server...
echo.
echo Once you see "Now listening on: http://localhost:5000"
echo Open your browser to: http://localhost:5000
echo.
echo Login with:
echo   Username: admin    Password: admin123
echo   Username: jemo     Password: jemo123
echo   Username: testuser Password: test123
echo.
echo ========================================
echo.

dotnet run --urls "http://localhost:5000"

echo.
echo Application stopped.
pause
