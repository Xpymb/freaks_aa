@echo off
setlocal enabledelayedexpansion

echo Copying .env.local to .env...
copy /Y .env.local .env >nul

echo Please enter the value for KEYCLOAK_ADMIN_CLIENT_SECRET:
set /p secret=KEYCLOAK_ADMIN_CLIENT_SECRET=

echo Updating .env file...

> temp_env (
    for /f "usebackq delims=" %%A in (".env") do (
        set line=%%A
        echo !line! | findstr /B "KEYCLOAK_ADMIN_CLIENT_SECRET=" >nul
        if !errorlevel! == 0 (
            echo KEYCLOAK_ADMIN_CLIENT_SECRET=!secret!
        ) else (
            echo !line!
        )
    )
)

move /Y temp_env .env >nul

echo Done! .env file is updated.
pause
