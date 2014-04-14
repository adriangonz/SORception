INSERT INTO ApplicationUser (created, updated, isAdmin, name, username, password) VALUES (GetDate(), GetDate(), 1, 'Administrador', 'admin', '4bb43533b8ed0f6a3f762f5b8acfbc25535fb8a4c5e74f9165800cc12989b43d');

DROP TABLE logging_event_property 
DROP TABLE logging_event_exception 
DROP TABLE logging_event 

CREATE TABLE logging_event 
  ( 
    timestmp         DECIMAL(20) NOT NULL,
   	formatted_message  VARCHAR(4000) NOT NULL,
    logger_name       VARCHAR(254) NOT NULL,
    level_string      VARCHAR(254) NOT NULL,
    thread_name       VARCHAR(254),
    reference_flag    SMALLINT,
    arg0              VARCHAR(254),
    arg1              VARCHAR(254),
    arg2              VARCHAR(254),
    arg3              VARCHAR(254),
    caller_filename   VARCHAR(254) NOT NULL,
    caller_class      VARCHAR(254) NOT NULL,
    caller_method     VARCHAR(254) NOT NULL,
    caller_line       CHAR(4) NOT NULL,
    event_id          DECIMAL(40) NOT NULL identity,
    PRIMARY KEY(event_id) 
  ) 

CREATE TABLE logging_event_property 
  ( 
    event_id          DECIMAL(40) NOT NULL, 
    mapped_key        VARCHAR(254) NOT NULL, 
    mapped_value      VARCHAR(1024), 
    PRIMARY KEY(event_id, mapped_key), 
    FOREIGN KEY (event_id) REFERENCES logging_event(event_id) 
  ) 

CREATE TABLE logging_event_exception 
  ( 
    event_id         DECIMAL(40) NOT NULL, 
    i                SMALLINT NOT NULL, 
    trace_line       VARCHAR(254) NOT NULL, 
    PRIMARY KEY(event_id, i), 
    FOREIGN KEY (event_id) REFERENCES logging_event(event_id) 
  ) 
