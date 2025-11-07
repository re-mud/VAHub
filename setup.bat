@echo off

where python >nul 2>&1
if errorlevel 1 (
	echo Python not installed
	pause & exit
)

python -c "import sys; sys.exit() if sys.version_info >= (3,12) else sys.exit(1)"
if errorlevel 1 (
	echo Python 3.12+ required
	pause & exit
)

python -m pip install -r requirements.txt

pause
