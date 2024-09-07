
echo "Creating asynchandler database..................."

echo "checking for azuresqlenv: azuresqlenv"
echo "checking for mssqlenv: mssqlenv"


# sqlcmd -s localhost -U sa -P ${{ env.mssql_password }} -q "create database ${{ env.mssql_db }};"