orapki wallet create -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -auto_login_local
orapki wallet add -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -keysize 1024 -self_signed -validity 3650
orapki wallet export -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -dn "CN=`hostname`" -cert `hostname`.crt
orapki wallet add -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword -trusted_cert -cert server.crt
orapki wallet display -wallet $ORACLE_HOME/wallet -pwd MyWalletPassword 
