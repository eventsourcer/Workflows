

echo "Creating asynchandler database..................."

echo "checking for azuresqlenv: $azuresqlenv"
echo "checking for mssqlenv: $mssqlenv"

echo "running script for mssql db: $mssql_db"

sqlcmd -s localhost -U sa -P $mssql_password -q "create database $mssql_db;"

echo "script completed successfully...."