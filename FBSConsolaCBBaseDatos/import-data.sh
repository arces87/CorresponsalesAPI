#wait for the SQL Server to come up
sleep 90s

#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P Root123*- -i Corresponsal.sql 