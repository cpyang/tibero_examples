# Tibero Recovery Manager Scripts
* Please change $BACKUP_BASE location in script and create $BACKUPDIR, $ARCHIVEDIR and $LOGDIR
```shell
export BACKUP_BASE=/opt/tmaxsoft/backup
export BINDIR=$BACKUP_BASE
export BACKUPDIR=$BACKUP_BASE/backup
export ARCHIVEDIR=$BACKUP_BASE/backup
export LOGDIR=$BACKUP_BASE/logs
```
* full_backup.sh - Full Backup & Delete Old Backup Set
    * Modify $RMDAYS for Delete Old Backup Sets
    ```shell
    # Delete Backup Sets Older Than 1 Month
    #RMDAY=`date +"%G%m%d000000" -d'-1 month'`
    # Delete Backup Sets Older Than 10 Days
    RMDAY=`date +"%G%m%d000000" -d'-10 days'`
    ```
* incr_backup.sh - Increamental Backup

* crontab setup
```crontab
# m h dom mon dow   command
# Full Backup on Sundays
  0 2 *   *   0     /opt/tmaxsoft/backup/full_backup.sh
# Increamental Backup on Weekdays
  0 2 *   *   1-6   /opt/tmaxsoft/backup/incr_backup.sh
```
