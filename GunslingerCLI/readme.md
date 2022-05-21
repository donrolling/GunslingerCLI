These three lines are needed in the csproj:

<PackAsTool>true</PackAsTool>
<ToolCommandName>gs</ToolCommandName>
<PackageOutputPath>./nupkg</PackageOutputPath>

Then run in the terminal from the location of the csproj:
	dotnet pack

This will create a nupkg in the PackageOutputPath location

Now run  in the terminal from the location of the csproj:
	dotnet tool install -g --add-source ./nupkg GunslingerCLI

This would work differently if using a non-local nupkg (I think).
It installs the tool globally.

To update tool:
	dotnet tool update -g --add-source ./nupkg GunslingerCLI

To uninstall tool:
	dotnet tool uninstall GunslingerCLI -g

LINKS

https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install