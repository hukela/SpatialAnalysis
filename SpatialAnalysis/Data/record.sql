/* for SQLite */

CREATE TABLE [record_{incidentId}]
-- 记录表
(
  [id] integer primary key autoincrement NOT NULL, -- 记录id,
  [parent_id] uint64 NOT NULL, -- 父级id
  {isNotFirst}
  [target_incident_id] uint32 default 0 NOT NULL, -- 指向的事件id,
  {/isNotFirst}
  [from_incident_id] uint32 default 0 NOT NULL, -- 执行该事件的事件id,
  [path] string unique NOT NULL, -- 路径,
  [plies] uint32 NOT NULL, -- 层数：上面有多少个文件夹
  [size] String default '0' NOT NULL, -- 大小(字节)
  [space_usage] String default '0' NOT NULL, -- 占用空间(字节)
  [create_time] datetime default NULL, -- 创建时间
  [modify_time] datetime default NULL, -- 修改时间
  [visit_time] datetime default NULL, -- 访问时间
  [owner] String default NULL, -- 所有者
  [exception_code] int32 default 0, -- 异常码,
  [file_count] uint32 default 0 NOT NULL, -- 文件总数
  [dir_count] uint32 default 0 NOT NULL -- 文件夹总数
);

/*Data for the table [record] */
insert into [record_{incidentId}] values
(0,0,{isNotFirst}0,{/isNotFirst}0,'\\\\.\\',0,'0','0','1601-01-01 00:00:00','1601-01-01 00:00:00','1601-01-01 00:00:00',NULL,0,0,0);
