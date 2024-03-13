ECHO off

sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\Documentation\DB\musedb.sql
sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\Documentation\DB\musedbinserts.sql
sqlcmd -S E67\SQLEXPRESS -E -i C:\Users\67Eas\source\repos\Muse2\Documentation\DB\musedbsprocs.sql


rem server is E67\SQLEXPRESS

ECHO . 
ECHO if no errors appear DB was created
PAUSE 