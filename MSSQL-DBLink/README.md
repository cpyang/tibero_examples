# Database Link between Tibero and Microsoft SQL Server  
## Tibero to Microsoft SQL Server  
1. Setup tbJavaGW  
### Unarchive Tibero Java Gateway and modify encoding setting
```shell
cd $TB_HOME/client  
unzip bin/tbJavaGW.zip  
sed -i 's/ENCODING=MSWIN949/ENCODING=UTF-8/' tbJavaGW/jgw.cfg  
```
    1. Get SQL Server JDBC driver and place in tbJavaGW/lib as sqljdbc4.jar (or replace the filename in tbgw script)  
```shell
cp ~/Downloads/sqljdbc4.jar $TB_HOME/client/tbJavaGW/lib  
```
    2. Start Tibero Java Gateway  
```shell
cd $TB_HOME/client/tbJavaGW && ./tbgw  
```
2. Prepare tbdsn.tbr  
    1. Create SQL Server connection entry in $TB_HOME/client/config/tbdsn.tbr
```text
mssql=((GATEWAY=(LISTENER=(HOST=localhost)(PORT=9093))(TARGET=sqlserver:1433:master)(TX_MODE=LOCAL)))
```
3. Create Database Link  
    1. Create Database Link  
```sql
create public database link sqlserver connect to 'sa' identified by 'mypassword' using 'mssql';
```
    2. Test Query  
```sql
select count(*) from sysobjects@sqlserver;

```
