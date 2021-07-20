#!/bin/env perl
use strict;
use DBI;
my $dbh = DBI->connect("dbi:ODBC:tibero","tibero","tmax");
#my $sql = qq{select '0','CEO','Taipei' from dual};
my $sql = qq{select 'Hello World!' from dual};
my $sth = $dbh->prepare($sql);
$sth->execute();
my ($hello);
$sth->bind_columns(undef, \$hello);
while($sth->fetch()) {
	print "$hello\n";
}
$sth->finish();
$dbh->disconnect();
