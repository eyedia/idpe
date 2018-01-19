goto main
This batch file is for developer's box to copy debug bin files to installed windows service folder.
stops the service
copy the required files from Debug folder
starts the service
:main
sc stop "Integrated Data Processing Environment"
timeout /t 2 /nobreak
set target=..\..\..\IDPE\
copy Eyedia.IDPE.Interface\bin\Debug\Eyedia*.dll %target%
copy Eyedia.IDPE.Interface\bin\Debug\idped.exe %target%
copy Eyedia.IDPE.Interface\bin\Debug\idped.exe.config %target%

copy Eyedia.IDPE.Host.Windows\bin\Debug\idpe.exe %target%
copy Eyedia.IDPE.Host.Windows\bin\Debug\idpe.exe.config %target%

copy Eyedia.IDPE.Command\bin\Debug\idpec.exe %target%
copy Eyedia.IDPE.Command\bin\Debug\idpec.exe.config %target%

sc start "Integrated Data Processing Environment"
timeout /t 5 /nobreak