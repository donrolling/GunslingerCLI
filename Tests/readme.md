===================================================================================================
Using Docker for testing
===================================================================================================

initial image without sample database

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04

personal image with sample database

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -p 1433:1433 -d drolling/gunslinger-sample-database:latest

===================================================================================================
Making changes to the sample container
===================================================================================================

docker commit sweet_tesla gunslinger-sample-database

docker tag 426cde529148 drolling/gunslinger-sample-database

docker push drolling/gunslinger-sample-database

===================================================================================================
Connection
===================================================================================================

connection string: Server=localhost,1433;Database=Sample;User Id=sa;Password=yourStrong(!)Password;
username: sa
password: yourStrong(!)Password

===================================================================================================
Notes
===================================================================================================

Some of these tests are old and I don't remember what they were for :(

Also, many of these tests are simply a shortcut to run a scenario and don't really perform a test at all. That sucks, but that is what I have for now.
