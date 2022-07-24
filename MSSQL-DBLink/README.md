# Database Link between Tibero and Microsoft SQL Server  
## Tibero to Microsoft SQL Server  
1. Setup tbJavaGW  
Unarchive Tibero Java Gateway and modify encoding setting
```shell
cd $TB_HOME/client  
unzip bin/tbJavaGW.zip  
sed -i 's/ENCODING=MSWIN949/ENCODING=UTF-8/' tbJavaGW/jgw.cfg  
```
Get SQL Server JDBC driver and place in tbJavaGW/lib
```shell
cp ~/Downloads/sqljdbc4.jar $TB_HOME/client/tbJavaGW/lib  
```
Start Tibero Java Gateway
```shell
cd $TB_HOME/client/tbJavaGW && ./tbgw
```
2. Prepare tbdsn.tbr
```text
mssql=((GATEWAY=(LISTENER=(HOST=localhost)(PORT=9093))(TARGET=sqlserver:1433:master)(TX_MODE=LOCAL)))
```
3. Create Database Link
Create Database Link
```sql
create database link sqlserver connect to 'sa' identified by 'Welcome2' using 'mssql';
```
Test Query
```sql
select count(*) from sysobjects@sqlserver;

```
