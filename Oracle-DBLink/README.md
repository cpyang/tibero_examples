# Database Link between Tibero and Oracle Database
## Tibero to Oracle
1. Downlaod Oracle Instant Client - Basic & SQL*Plus
    1. For example instanctclient_11_2 for Oracle Database 11g  
    Setup environment variables
    ```
    export ORACLE_HOME=/opt/oracle/instantclient_11_2
    export PATH=$PATH:$ORACLE_HOME
    export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:$ORACLE
    export TNS_ADMIN=$TB_HOME/client/gateway/oracle/config
    ```
    Create tnsnames.ora for Oracle
    ```text
    DB11G=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=db11g)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=DB11G)))
    DB11GS=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCPS)(HOST=db11g)(PORT=1522)))(CONNECT_DATA=(SERVICE_NAME=DB11G)))
    ```
    For TCPS connections, also create sqlnet.ora
    ```text
    WALLET_LOCATION = (SOURCE=(METHOD=FILE)(METHOD_DATA=(DIRECTORY=/opt/oracle/instantclient_11_2/wallet)))  
    SQLNET.AUTHENTICATION_SERVICES = (TCPS,NTS)  
    SSL_CLIENT_AUTHENTICATION = FALSE  
    SSL_CIPHER_SUITES = (SSL_RSA_WITH_AES_256_CBC_SHA, SSL_RSA_WITH_3DES_EDE_CBC_SHA)  
    ```
    ```shell
    # Create Wallet
    orapki wallet create -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -auto_login_local
    # Optionally, import server certificate and create client certificate 
    orapki wallet add -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -keysize 1024 -self_signed -validity 3650
    orapki wallet export -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -cert `hostname`.crt
    orapki wallet add -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -trusted_cert -cert server.crt
    orapki wallet display -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword 
    ```
    2. Configure Tibero Gateway for Oracle Database, set $TBGW_HOME or default directory $TB_HOME/client/gateway
    ```shell
    cd $TB_HOME/client  
    mkdir -p gateway/oracle/config
    mkdir -p gateway/oracle/log
    cat >> $TB_HOME/client/gateway/oracle/config << EOF
    LOG_DIR=${TB_HOME}/gateway/oracle/log
    LOG_LVL=2
    LISTENER_PORT=11521
    MAX_LOG_SIZE=100m
    MAX_LOG_BACKUP_SIZE=1G
    FETCH_SIZE=32k
    EOF
    ```
    3. Prepare tbdsn.tbr  
    Create Oracle Database connection entry in $TB_HOME/client/config/tbdsn.tbr, matching the name defined in tnsname.ora
    ```text
    db11g=((GATEWAY=(LISTENER=(HOST=localhost)(PORT=11521))(TARGET=db11g)(TX_MODE=GLOBAL)))
    ```
2. Start Tibero Gateway for Oracle Database matching the installed Oracle Instant Client version 
    ```shell
    gw4orcl_11g
    ```
3. Create Database Link  
    1. Create Database Link  
    ```sql
    create public database link db11g connect to 'myuser' identified by 'mypassword' using 'db11g';
    ```
    2. Test Query  
    ```sql
    select 1 from dual@db11g;
    ```
## Enabling TCPS connection in Oracle Listener
1. Create sqlnet.ora in $TNS_ADMIN  
    Use SSL_CLIENT_AUTHENTICATION=FALSE if you don't want to import client certificate into server
    ```text
    WALLET_LOCATION=(SOURCE=(METHOD=FILE)(METHOD_DATA=(DIRECTORY=/opt/oracle/db11g/wallet)))
    SQLNET.AUTHENTICATION_SERVICES = (TCPS,NTS,BEQ)
    SSL_CLIENT_AUTHENTICATION = FALSE
    SSL_CIPHER_SUITES = (SSL_RSA_WITH_AES_256_CBC_SHA, SSL_RSA_WITH_3DES_EDE_CBC_SHA)
    ```
2. Add TCPS listener in listener.ora  
    ```text
    LISTENER=(DESCRIPTION_LIST =
    (DESCRIPTION =
      (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))
      (ADDRESS = (PROTOCOL = IPC)(KEY = EXTPROC1521))
      (ADDRESS = (PROTOCOL = TCPS)(HOST = localhost)(PORT = 1522))
    ))

    ADR_BASE_LISTENER = /opt/oracle
    SSL_CLIENT_AUTHENTICATION = FALSE

    WALLET_LOCATION=(SOURCE=(METHOD=FILE)(METHOD_DATA=(DIRECTORY=/opt/oracle/db11g/wallet)))
    ```
3. Create wallet and server certificate
    ```shell
    orapki wallet create -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -auto_login_local
    orapki wallet add -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -keysize 1024 -self_signed -validity 3650
    # Optionally export certificate for client
    orapki wallet export -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -cert `hostname`.crt
