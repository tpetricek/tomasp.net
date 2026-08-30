@echo off
rem Local R2 credentials, if present (see .env.example)
if exist "%~dp0.env" (
  for /f "usebackq eol=# tokens=1,* delims==" %%a in ("%~dp0.env") do set "%%a=%%b"
)
dotnet run --project "%~dp0tools" -c Release -- %*
