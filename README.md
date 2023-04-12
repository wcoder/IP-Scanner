<p align="center"><img src="logo/verticalversion.png" alt="IP-Scanner" height="200px"></p>

# IP-Scanner
Simple IP subset scanner on .NET 6. Available for Windows, Linux and Mac.

## Use
Easy command to start app:
```
dotnet run --project src/IpScanner/IpScanner.csproj 192.168.0.1 192.168.0.254
```
Or you can add third parameter to only display the available devices.
```
dotnet run --project src/IpScanner/IpScanner.csproj 192.168.0.1 192.168.0.254 e
```

## Build for publish:
```
cd src
```

Window x64:
```
dotnet publish -c Release -r win10-x64
```
MacOS x64:
```
dotnet publish -c Release -r osx-x64
```
Ubuntu x64:
```
dotnet publish -c Release -r ubuntu-x64
```
> Additional info about PID: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
