svcutil /t:code http://localhost:7001/sre /out:SreClient.cs /namespace:*,Eyedia.IDPE.Clients /config:SreClient.config
svcutil /t:code http://localhost:7002/srees /out:SreesClient.cs /namespace:*,Eyedia.IDPE.Clients /noconfig
