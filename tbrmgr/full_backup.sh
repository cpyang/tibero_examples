#!/bin/bash
# Tibero Full backup 

# Execute Profile for environment variable
. $HOME/.bash_profile

# TIBERO ENV 
#export TB_HOME=/tmax/tibero6
#export TB_SID=icbms
#export PATH=.:$TB_HOME/bin:$TB_HOME/client/bin:$PATH
#export LD_LIBRARY_PATH=$TB_HOME/lib:$TB_HOME/client/lib:/usr/lib:$LD_LIBRARY_PATH
#export LIBPATH=$LD_LIBRARY_PATH:$LIBPATH
#export NLS_LANG="AMERICAN_AMERICA.AL32UTF8"

# Backup scripts ENV
export BACKUP_BASE=/opt/tmaxsoft/backup
export BINDIR=$BACKUP_BASE
export BACKUPDIR=$BACKUP_BASE/backup
export ARCHIVEDIR=$BACKUP_BASE/backup
export LOGDIR=$BACKUP_BASE/logs
export USERPWD="sys/tibero"
#export JAVA_HOME=/usr/java8_64
unset LANG

# Set the current month day and year.  
month=`date +%m`  
day=`date +%d`  
year=`date +%Y`  
DAY=`date +"%G%m%d"`
# Delete Backup Sets Older Than 1 Month
#RMDAY=`date +"%G%m%d000000" -d'-1 month'`
# Delete Backup Sets Older Than 10 Days
RMDAY=`date +"%G%m%d000000" -d'-10 days'`
export LOGFILE=$LOGDIR/full_$DAY.log

###### 190507 add ######
function s_time      # Returns time string "HH:MM:SS"
{
  date +%H:%M:%S
}

function s_interval      # Returns time difference "HH:MM:SS"
#===================================================================================
# Arg_1 = start_time (Format - see s_time)
# Arg_2 = stop_time  (Format - see s_time)
{
  _hour_1=`echo "$1" | cut -c1-2`
  _mins_1=`echo "$1" | cut -c4-5`
  _secs_1=`echo "$1" | cut -c7-8`
  _hour_2=`echo "$2" | cut -c1-2`
  _mins_2=`echo "$2" | cut -c4-5`
  _secs_2=`echo "$2" | cut -c7-8`
  _secs_3=`expr $_secs_2 - $_secs_1`
  if [ $_secs_3 -lt 0 ]
  then
    _secs_3=`expr $_secs_3 + 60`
    _mins_1=`expr $_mins_1 + 1`
  fi
  _mins_3=`expr $_mins_2 - $_mins_1`
  if [ $_mins_3 -lt 0 ]
  then
    _mins_3=`expr $_mins_3 + 60`
    _hour_1=`expr $_hour_1 + 1`
  fi
  _hour_3=`expr $_hour_2 - $_hour_1`
  if [ $_hour_3 -lt 0 ]
  then
    _hour_3=`expr $_hour_3 + 24`
  fi

  echo "$_hour_3:$_mins_3:$_secs_3"
}

#
# Backup scripts 
#
startdate=`date`
STIME=`s_time` 


# Backup archivelog
tbrmgr backup -s --userid $USERPWD -o $ARCHIVEDIR -a --delete-original > $LOGFILE
# Full Backup
# compress, skip-unused 
#tbrmgr backup --userid $USERPWD -o $BACKUPDIR -v -c -u --with-password-file > $LOGFILE
# no compress 
tbrmgr backup -s --userid $USERPWD -o $BACKUPDIR -p 8 --with-password-file > $LOGFILE
# Delete Old Backup Sets
tbrmgr delete --userid $USERPWD --beforetime $RMDAY -o $BACKUPDIR

ETIME=`s_time`  
TOTAL=`s_interval $STIME $ETIME` 
echo Time: $TOTAL 

echo backup completed at `date` >> $LOGFILE
echo backup begining at $startdate >> $LOGFILE

# Query backup set list
tbsql -s $USERPWD >> $LOGFILE <<EOF
@$BINDIR/backupset_info.sql
exit
EOF
