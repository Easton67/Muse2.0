ECHO off

sqlcmd -S localhost -E -i musedb.sql
sqlcmd -S localhost -E -i musedbinserts.sql
sqlcmd -S localhost -E -i musedbsprocs.sql


rem server is localhost

ECHO . 
ECHO if no errors appear DB was created
PAUSE 