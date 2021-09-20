/* for SQLite */

create index [pid_index_{incidentId}] on [record_{incidentId}] ([parent_id]);
