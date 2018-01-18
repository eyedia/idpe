sc stop "Integrated Data Processing Environment"
timeout /t 2 /nobreak
copy .\bin\Debug\Eyedia*.dll ..\..\..\..\IDPE
copy .\bin\Debug\idped.exe ..\..\..\..\IDPE
copy .\bin\Debug\idped.exe.config ..\..\..\..\IDPE

copy ..\Eyedia.IDPE.Host.Windows\bin\Debug\idpe.exe ..\..\..\..\IDPE
copy ..\Eyedia.IDPE.Host.Windows\bin\Debug\idpe.exe.config ..\..\..\..\IDPE

copy ..\Eyedia.IDPE.Command\bin\Debug\idpec.exe ..\..\..\..\IDPE
copy ..\Eyedia.IDPE.Command\bin\Debug\idpec.exe.config ..\..\..\..\IDPE

sc start "Integrated Data Processing Environment"
timeout /t 5 /nobreak