svcutil /t:code http://localhost:7576/idpe /out:Client.cs /namespace:*,Eyedia.IDPE.Clients /config:Client.config
svcutil /t:code http://localhost:7586/idpees /out:IdpeesClient.cs /namespace:*,Eyedia.IDPE.Clients /noconfig /reference:C:\Workspace\IDPE\Eyedia.IDPE.Common.dll /reference:C:\Workspace\IDPE\Eyedia.IDPE.DataManager.dll
