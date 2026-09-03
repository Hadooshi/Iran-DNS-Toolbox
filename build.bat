@echo off
title "Build DNS Changer Portable"
cd /d "%~dp0"

echo ========================================================
echo   Building DNS Changer Portable (.exe)
echo ========================================================
echo.

dotnet publish .\src\DNSChangerApp.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o .\publish_tmp

if %errorlevel% neq 0 (
    echo.
    echo [ERROR] Build failed! Please check the output above.
    pause
    exit /b %errorlevel%
)

copy /y .\publish_tmp\DNSChanger.exe .\DNSChanger.exe >nul
rd /s /q .\publish_tmp

echo.
echo ========================================================
echo   [SUCCESS] DNSChanger.exe generated successfully!
echo ========================================================
echo.
pause
