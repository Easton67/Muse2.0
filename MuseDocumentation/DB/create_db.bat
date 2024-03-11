ECHO off

sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\MuseDocumentation\DB\musedb.sql
sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\MuseDocumentation\DB\musedbinserts.sql
sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\MuseDocumentation\DB\musedbsprocs.sql


rem server is localhost

ECHO . 
ECHO if no errors appear DB was created
PAUSE 