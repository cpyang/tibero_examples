# Database Link between Tibero and Microsoft SQL Server  
## Tibero to Microsoft SQL Server  
1. Setup tbJavaGW  
    1. Unarchive Tibero Java Gateway and modify encoding setting
    ```shell
    cd $TB_HOME/client  
    unzip bin/tbJavaGW.zip  
    sed -i 's/ENCODING=MSWIN949/ENCODING=UTF-8/' tbJavaGW/jgw.cfg  
    ```
    2. Get SQL Server JDBC driver and place in tbJavaGW/lib as sqljdbc4.jar (or replace the filename in tbgw script)  
    ```shell
    cp ~/Downloads/sqljdbc4.jar $TB_HOME/client/tbJavaGW/lib  
    ```
    3. Start Tibero Java Gateway  
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
## Microsoft SQL Server Linked Server to Tibero via MSDASQL
1. Setup Tibero ODBC Driver and Tibero System DSN
    1. Copy Tibero ODBC Driver from $TB_HOME/client/win64 or $TB_HOME/client/win32
    2. Setup ODBC with scripts from [Tibero_ODBC_Windows](../../../../Tibero_ODBC_Windows)
2. Connect to Microsoft SQL Server with Administor account
    1. Create Linked Server  
    ```sql
    USE [master]  
    GO  

    /****** Object:  LinkedServer [TIBERO]    Script Date: 23/07/2022 23:24:51 ******/  
    EXEC sp_addlinkedserver @server = N'TIBERO', @srvproduct=N'tibero', @provider=N'MSDASQL', @datasrc=N'tibero'  
      
    /* For security reasons the linked server remote logins password is changed with ######## */  
    EXEC sp_addlinkedsrvlogin @rmtsrvname=N'TIBERO',@useself=N'False',@locallogin=NULL,@rmtuser=N'tibero',@rmtpassword='########'  
    EXEC sp_addlinkedsrvlogin @rmtsrvname=N'TIBERO',@useself=N'False',@locallogin=N'SQLServer\myaccount',@rmtuser=N'tibero',@rmtpassword='########'  
    EXEC sp_addlinkedsrvlogin @rmtsrvname=N'TIBERO',@useself=N'False',@locallogin=N'sa',@rmtuser=N'sys',@rmtpassword='tibero'  
    GO  
      
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'collation compatible', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'data access', @optvalue=N'true'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'dist', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'pub', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'rpc', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'rpc out', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'sub', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'connect timeout', @optvalue=N'0'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'collation name', @optvalue=null  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'lazy schema validation', @optvalue=N'false'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'query timeout', @optvalue=N'0'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'use remote collation', @optvalue=N'true'  
    EXEC sp_serveroption @server=N'TIBERO', @optname=N'remote proc transaction promotion', @optvalue=N'true'  
    GO  
    ```
    2. Test Query
    ```sql
    select count(*) from TIBERO..SYSCAT.USER_TABLES;
    select * from TIBERO..SYS._VT_DUAL;
    select * from openquery(TIBERO, 'select 1 from dual');
    ```
