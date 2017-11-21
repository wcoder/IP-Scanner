# IP-Scanner
Simple IP subset scanner on .NET Core 2.0. Available for Windows, Linux and Mac.

## Use
Easy command to start app:
```
dotnet run --project src/IpScanner/IpScanner.csproj 192.168.0.1 192.168.0.254
```

Build `exe` for Window x64:
```
dotnet publish -c Release -r win10-x64
```
> Additional info about PID: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
