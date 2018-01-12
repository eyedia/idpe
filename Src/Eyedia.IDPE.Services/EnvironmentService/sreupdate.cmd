cd /d {1}
>sreupdate0.log (
@echo %DATE% %TIME% updating IDPE...Please wait...
)
timeout /t 10 /nobreak
@echo off
call srestop >> sreupdate1.log 2>>&1
@echo|set /p="Updating files..."
copy {0} {1} /y >> sreupdate2.log 2>>&1
@echo Done.
timeout /t 10 /nobreak
@echo|set /p="Starting services..."
call srestart >> sreupdate3.log 2>>&1
@echo Done?
@echo "Cleaning up..."
copy sreupdate0.log + sreupdate1.log + sreupdate2.log + sreupdate3.log sreupdate.log  >nul 2>nul
del sreupdate0.log
del sreupdate1.log
del sreupdate2.log
del sreupdate3.log
rmdir {0} /s /q
@echo temp repository {0} deleted.
@echo %DATE% %TIME% Update Completed.