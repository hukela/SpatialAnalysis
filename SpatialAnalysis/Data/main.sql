/* for SQLite */

CREATE TABLE [dir_index]
-- 索引表
(
  [path] string primary key NOT NULL, -- 路径
  [incident_id] uint32 NOT NULL, -- 事件id
  [target_id] uint64 NOT NULL -- 对应节点id
);

CREATE TABLE [dir_tag]
-- 文件夹标签表
(
  [id] integer primary key autoincrement NOT NULL,
  [tag_id] uint32 NOT NULL, -- 对应的标签id
  [path] string NOT NULL -- 标签所标注的路径
);

CREATE TABLE [incident]
-- 事件表
(
  [id] integer primary key autoincrement NOT NULL, -- 事件id
  [title] string NOT NULL, -- 事件标题
  [description] string default NULL, -- 事件描述
  [state] sbyte NOT NULL, -- 状态，0代表正常
  [record_type] sbyte NOT NULL, -- 记录类型，0后续，1首次
  [create_time] datetime NOT NULL -- 创建时间
);

CREATE TABLE [tag]
-- 标签表
(
  [id] integer primary key autoincrement NOT NULL, -- 标签id
  [parent_id] uint32 NOT NULL, -- 父级标签id
  [name] string NOT NULL, -- 标签名称
  [color] string default NULL -- 标签颜色
);

insert  into [tag] values (null, 0, '系统文件','#FFFFFF');
insert  into [tag] values (null, 0, '用户文件','#FFFFFF');
insert  into [tag] values (null, 0, '软件','#FFFFFF');
