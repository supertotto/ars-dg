@echo off
setlocal
rem Limpa todos os modelos da pasta "entrada" e salva em "saida".
rem Se o Blender estiver em outro lugar, coloque o caminho do blender.exe abaixo:
set "BLENDER="

if "%BLENDER%"=="" (
  for /d %%D in ("C:\Program Files\Blender Foundation\Blender*") do set "BLENDER=%%D\blender.exe"
)
if not exist "%BLENDER%" (
  echo Blender nao encontrado. Edite este arquivo e coloque o caminho do blender.exe em BLENDER.
  pause
  exit /b 1
)

set "PASTA=%~dp0"
set "CONFIG="
if exist "%PASTA%config.json" set "CONFIG=--config "%PASTA%config.json""

"%BLENDER%" -b -P "%PASTA%limpar_modelo.py" -- --entrada "%PASTA%entrada" --saida "%PASTA%saida" %CONFIG%
pause
