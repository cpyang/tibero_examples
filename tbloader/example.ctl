LOAD DATA
INFILE  'example.dat'
LOGFILE 'example.log'
BADFILE 'example.bad'
APPEND
INTO TABLE emp
FIELDS TERMINATED BY ','
       OPTIONALLY ENCLOSED BY '"'
       ESCAPED BY '\\'
LINES TERMINATED BY '\n'
(
    empno, 
    ename, 
    job, 
    mgr, 
    hiredate, 
    sal, 
    comm, 
    deptno
)
