#!/bin/env python3
# -*- coding: utf8 -*-
from datetime import datetime
import os, pyodbc

tb_connstr='DSN=tibero;Host=localhost;Port=8629;UID=tibero;PWD=tmax;CharSet=utf8'
con = pyodbc.connect(tb_connstr) #, unicode_results=True)
#con.setdecoding(pyodbc.SQL_CHAR, encoding='utf-8')
#con.setdecoding(pyodbc.SQL_WCHAR, encoding='utf-8')
#con.setdecoding(pyodbc.SQL_WMETADATA, encoding='utf-32le')
#con.setencoding(encoding='utf-8')
cur = con.cursor()
sql = "select 'Hello World!' AS HELLO from dual"
print("Query: %s" % sql)
cur.execute(sql)
cols = [col[0] for col in cur.description]
print("Column:")
for col in cols:
    print(col)
print()
rs = cur.fetchall()
print("Data:")
for (value,) in rs:
    print(value)


