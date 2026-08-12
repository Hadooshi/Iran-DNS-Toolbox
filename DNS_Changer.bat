@echo off
title "1-Click DNS Changer & System DNS Verifier (2026)"
cd /d "%~dp0"

:: Check for Administrator Privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Requesting Administrator privileges...
    powershell -NoProfile -ExecutionPolicy Bypass -Command "Start-Process cmd -ArgumentList '/c ""%~dp0DNS_Changer.bat""' -Verb RunAs" 2>nul
    if %errorLevel% equ 0 exit /b
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0DNS_Changer.ps1"
if errorlevel 1 (
    echo.
    echo Error executing PowerShell script.
    pause
)