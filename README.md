# DotNetRunWeb

Extension for the dotnet CLI for specifying a port number and automatically opening a browser when running ASP.Net Core Websites.

## Installation

Include it in the tools section of your project.json

`"tools": {
     "LightAnchor.Extensions.RunWeb":"1.0.*"
}`

Or if you are using .csproj add this to your .csproj 

```
<ItemGroup>
    <DotNetCliToolReference Include="LightAnchor.Extensions.RunWeb">
      <Version>1.0.*</Version>
    </DotNetCliToolReference>
</ItemGroup>
```

And then

`dotnet restore`

## Global Installation

To install for use across your machine copy the contents from the folder under /GlobalInstall
for your OS/Framework into a folder in PATH. 

For Linux/Mac you will need to set the execute bit on the dotnet-web file e.g. `chmod +x dotnet-web`.

## Usage

Use the web command with the dotnet CLI

* `-o` or `--open` - Open browser once website is running.
* `-r` or `--port` - Specify the port number.

`dotnet web -o --port 9999`

Include any option supported by dotnet run

`dotnet web -o --port 9999 -c Release`