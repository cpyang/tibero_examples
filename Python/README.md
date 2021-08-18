# Requirements  
pyodbc

# Encoding Settings - to access Tibero with Unicode, either one of following setting is required.  
## Via Environment variable  
    export TB_NLS_LANG=UTF-8
    export TBCLI_WCHAR_TYPE=UCS2
## Via Python code  
    connstr='DSN=tibero;Host=localhost;Port=8629;UID=tibero;PWD=tmax;CharSet=utf8'
    con = pyodbc.connect(connstr)
    con.setdecoding(pyodbc.SQL_CHAR, encoding='utf-8')
    con.setdecoding(pyodbc.SQL_WCHAR, encoding='utf-8')
    con.setdecoding(pyodbc.SQL_WMETADATA, encoding='utf-32le')
    con.setencoding(encoding='utf-8')
