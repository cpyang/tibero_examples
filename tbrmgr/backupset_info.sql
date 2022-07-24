col start_time for a20
col finish_time for a20
col SIZE_GB for 999,999,999
set linesize 150
alter session set nls_date_format = 'yyyymmdd hh24:mi:ss';
select set_id,start_time,finish_time,backup_type,backup_option,round("SIZE(MB)"/1024) SIZE_GB 
from v$backup_set order by set_id;
