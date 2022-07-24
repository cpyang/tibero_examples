#!/bin/env php
<?php
/***********************************************************************
* FILENAME :    Hello.php
*
* DESCRIPTION :
*               Simple PHP PDO ODBC example to find and count the number
*               of records in a table where the first letter of the name
*               matches a given letter.
*
* ODBC USAGE :
*               Creates an associative array of letters against which names
*               are to be matched.
*               Connects to Data Source using a Data Source Name
*               Prepares a SQL statement which performs 2 SELECTs. One takes
*               an array value to match names against and the second returns
*               the SQL Server global variable '@@ROWCOUNT'. This causes two
*               rowsets to be returned (in associative arrays), one with the
*               records found (if any) and one with the count of records
*               returned by the first.
*               For each name to match against
*                   Calls $stmt->execute()
*                   Loops using $stmt->fetchAll() and $stmt->nextRowset()
*                       output array key values in results returned
*               Closes statement and data base connection
*
*/
// Datasource name
//$dsn = "DSN=tibero;Host=localhost;Port=8629;UID=tibero;PWD=tmax;CharSet=utf8";
$dsn = "odbc:docker";

try {
    // Connect to the data source
    $dbh = new PDO($dsn);

    // Prepare the statement with 2 Select statements.
    // Note the first SELECT uses the array key 'fn' as the select criteria
    // in the PDO form ':fn'.
    //$stmt = $dbh->prepare('SELECT id FROM test;SELECT @@ROWCOUNT as Rows;');
    $stmt = $dbh->prepare('SELECT \'Hello World! 哈囉 世界！\' AS HELLO FROM dual;');

    // Execute the prepared statement for each name in the array
    $stmt->execute();

    // Print results for each rowset returned. We will get 1 rowset for
    // each SELECT. The first will be any records found (which may be empty)
    // and the second with be a count of the records returned by the first
    // (which may be zero). Note this relies on SQL Server returning this
    // value after a SELECT.
    do {
        $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
        foreach($result as $rst)
        {
            // These keys will exists if this rowset is a record
            if (array_key_exists('HELLO', $rst)) printf ("%s\n",     $rst['HELLO']);

            // This key will exist if this rowset is the record count set
            if (array_key_exists('Rows', $rst)) printf ("Records Found : %d\n",$rst['Rows']);
        }
    } while ($stmt->nextRowset());

    // Close statement and data base connection
    $stmt = NULL;
    $dbh = NULL;
}

catch(PDOException $e) {
    echo $e->getMessage();
}
?>
