#!/bin/env python3
# -*- coding: utf8 -*-
from datetime import datetime
import os, pyodbc

tb_connstr='DSN=tibero;Host=localhost;Port=8629;UID=tibero;PWD=tmax;CharSet=utf8'
con = pyodbc.connect(tb_connstr) #, unicode_results=True)
con.setdecoding(pyodbc.SQL_CHAR, encoding='utf-8')
con.setdecoding(pyodbc.SQL_WCHAR, encoding='utf-8')
con.setdecoding(pyodbc.SQL_WMETADATA, encoding='utf-16le')
con.setencoding(encoding='utf-8')
cur = con.cursor()

cur.execute("truncate table t")
cur.setinputsizes((pyodbc.SQL_WCHAR,0,0))
cur.execute("insert into t values (:1,:2,:3)", (1,'許茹芸淚海慶功宴吃蓋飯','許茹芸淚海慶功宴吃蓋飯'))
cur.setinputsizes((pyodbc.SQL_WCHAR,0,0))
cur.execute("insert into t values (:1,:2,:3)", (1,'游錫堃','游錫堃'))
cur.setinputsizes((pyodbc.SQL_WLONGVARCHAR,0,0))
cur.execute("insert into t values (:1,:2,:3)", (3,'許茹芸淚海慶功宴吃蓋飯','許茹芸淚海慶功宴吃蓋飯'))
cur.setinputsizes((pyodbc.SQL_WLONGVARCHAR,0,0))
cur.execute("insert into t values (:1,:2,:3)", (3,'游錫堃','游錫堃'))
con.commit()

#sql = "select 'Hello World!' AS HELLO from dual"
#sql = "select val from test"
sql = "select nvar from t"
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
