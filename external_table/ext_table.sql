CONNECT sys/tibero;
CREATE OR REPLACE DIRECTORY inbound AS '/home/cpyang/src/tibero_examples/external_table/in';
GRANT READ,WRITE ON DIRECTORY inbound to tibero;

CONNECT tibero/tmax;
-- Create External Table
DROP TABLE DEPT_EXT;
CREATE TABLE DEPT_EXT
(
    DEPTNO NUMBER(4),
    DEPTNAME VARCHAR(30 BYTE),
    MGRNO NUMBER(6),
    LOCNO NUMBER(4)
)
ORGANIZATION EXTERNAL                   -- Define the creation of an external table.
(
    DEFAULT DIRECTORY INBOUND           -- Set the name of the directory object.
    ACCESS PARAMETERS
    (
        LOAD DATA INTO TABLE DEPT_EXT
        FIELDS TERMINATED BY ','         -- Set the field delimitor.
        OPTIONALLY ENCLOSED BY '"'       -- Set the characters enclosing the field.
        ESCAPED BY '\\'                  -- Set the ESCAPE character using special letters.
        LINES TERMINATED BY '\n'         -- Set a record termination character.
        IGNORE 0 LINES                   -- Set the lines to be excluded. (if 5, the lines from 1~5 will be excluded)
        (DEPTNO, DEPTNAME, MGRNO, LOCNO)
    )
    LOCATION ('00/00-001.dat')
);
-- Create Table
DROP TABLE DEPT;
CREATE TABLE DEPT AS SELECT * FROM DEPT_EXT WHERE 1<>1;

-- Load Data
ALTER TABLE DEPT_EXT LOCATION('00/00-001.dat');
INSERT INTO DEPT SELECT * FROM DEPT_EXT;
COMMIT;
ALTER TABLE DEPT_EXT LOCATION('01/01-001.dat');
SELECT * FROM DEPT_EXT;
INSERT INTO DEPT SELECT * FROM DEPT_EXT;
COMMIT;
SELECT * FROM DEPT;
