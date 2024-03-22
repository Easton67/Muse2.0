ECHO off

sqlcmd -S E67\SQLEXPRESS -E -i musedb.sql
sqlcmd -S E67\SQLEXPRESS -E -i musedbinserts.sql
sqlcmd -S E67\SQLEXPRESS -E -i museSongInserts.sql
sqlcmd -S E67\SQLEXPRESS -E -i musedbsprocs.sql


rem server is localhost

ECHO . 
ECHO if no errors appear DB was created
PAUSE 