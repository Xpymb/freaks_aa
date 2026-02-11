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

echo.
echo === Grafana Alloy Configuration ===
echo.

echo --- Loki (logs) ---
set /p LOKI_URL="Loki URL: "
set /p LOKI_USERNAME="Loki username: "
set /p LOKI_PASSWORD="Loki password: "

echo.
echo --- Tempo (traces) ---
set /p TEMPO_URL="Tempo OTLP endpoint: "
set /p TEMPO_USERNAME="Tempo username: "
set /p TEMPO_PASSWORD="Tempo password: "

echo.
echo --- Prometheus (metrics) ---
set /p PROMETHEUS_URL="Prometheus remote write URL: "
set /p PROMETHEUS_USERNAME="Prometheus username: "
set /p PROMETHEUS_PASSWORD="Prometheus password: "

echo.
echo Generating GrafanaAlloy\config.alloy from template...

powershell -NoProfile -Command "$c = [IO.File]::ReadAllText('GrafanaAlloy\config.alloy.template'); $c = $c.Replace('your_loki_url',$env:LOKI_URL).Replace('your_loki_username',$env:LOKI_USERNAME).Replace('your_loki_password',$env:LOKI_PASSWORD).Replace('your_otlp_url',$env:TEMPO_URL).Replace('your_tempo_username',$env:TEMPO_USERNAME).Replace('your_tempo_password',$env:TEMPO_PASSWORD).Replace('your_prometheus_url',$env:PROMETHEUS_URL).Replace('your_prometheus_username',$env:PROMETHEUS_USERNAME).Replace('your_prometheus_password',$env:PROMETHEUS_PASSWORD); [IO.File]::WriteAllText('GrafanaAlloy\config.alloy',$c)"

echo Done! GrafanaAlloy\config.alloy is generated.
pause
