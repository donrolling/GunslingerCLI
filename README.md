# Gunslinger Templating Engine - CLI Version

The Gunslinger templating engine is built on top of the [Handlebars](https://github.com/Handlebars-Net/Handlebars.Net) templating system, which is an enhancement of the Logic-less [Mustache](https://mustache.github.io/) templating system. 

The primary idea is that a json config file and some templates can provide everything you need to generate massive portions of your project in 
a very flexible way.

This version is designed to be installed as a command line tool on the host machine which is probably going to be a developer machine, but could
potentially be a build server.

I'm continuing in the tradition of naming this tool after a style of facial hair, so please see the link below for examples of the gunslinger beard.

## Gunslinger Beard
![Gunslinger Beard](https://user-images.githubusercontent.com/1778167/122961824-64603780-d34a-11eb-887b-578300dd290c.png)

Use the wiki to see [documentation](https://github.com/donrolling/Gunslinger.Templates/wiki) and explanation of different elements.

# CLI Tool

## Project Setup

These three lines were needed in the csproj:
```
<PackAsTool>true</PackAsTool>
<ToolCommandName>gs</ToolCommandName>
<PackageOutputPath>./nupkg</PackageOutputPath>
```
## Building and deploying

Terminal command must be run in the terminal from the location of the csproj.
I think install works differently if using a non-local nupkg. More on that when I figure it out.

- build solution
- create a nupkg in the PackageOutputPath location 
	> dotnet pack
- install the tool globally
	> dotnet tool install -g --add-source ./nupkg GunslingerCLI

**Update the tool**
> dotnet tool update -g --add-source ./nupkg GunslingerCLI

**Uninstall the tool**
> dotnet tool uninstall GunslingerCLI -g

# Using Docker for testing

### Initial image without sample database
> docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04

### Personal image with sample database

> docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d drolling/gunslinger-sample-database:latest

### Notes: Making changes to the sample container

'sweet_tesla' and '426cde529148' were current names/ids of containers when I made these notes. 'drolling' is my dockerhub image repository, which is public.

> docker commit sweet_tesla gunslinger-sample-database

> docker tag 426cde529148 drolling/gunslinger-sample-database

> docker push drolling/gunslinger-sample-database

### Connection
> connection string: Server=localhost,1433;Database=Sample;User Id=sa;Password=yourStrong(!)Password;

> username: sa
> password: yourStrong(!)Password

### Test Notes
Some of these tests are old and I don't remember what they were for :(

Also, many of these tests are simply a shortcut to run a scenario and don't really perform a test at all. That sucks, but that is what I have for now.

### LINKS

[dotnet tool install documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install)

[Pluralizer Github link](https://github.com/sarathkcm/Pluralize.NET)