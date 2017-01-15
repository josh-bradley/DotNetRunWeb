#DotNetRunWeb

Extension for the dotnet CLI for specifying a port number and automatically opening a browser when running ASP.Net Core Websites.

##Installation

Include it in the tools section of your project.json

`"tools": {
     "LightAnchor.Extensions.RunWeb":"1.0.1"
}`

##Usage

Use the runw command with the dotnet CLI

* `-o` or `--open` - Open browser once website is running.
* `-r` or `--port` - Specify the port number.

`dotnet runw -o --port 9999`