-- pgbase114.axi_commands definition

-- Drop table

-- DROP TABLE pgbase114.axi_commands;

CREATE TABLE pgbase114.axi_commands (
	cmdtoken int4 NOT NULL,
	command_group varchar(50) NOT NULL,
	command varchar(50) NOT NULL,
	active varchar(1) NULL DEFAULT 'T'::character varying,
	CONSTRAINT axi_commands_pkey PRIMARY KEY (cmdtoken)
);

-- Permissions

ALTER TABLE pgbase114.axi_commands OWNER TO pgbase114;
GRANT ALL ON TABLE pgbase114.axi_commands TO pgbase114;


===========================================================================

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(1, 'create', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(2, 'edit', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(3, 'view', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(4, 'configure', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(5, 'upload', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(6, 'download', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(7, 'open', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(8, 'set', '', 'F');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'run', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(10, 'analyse', '', 'T');
INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(11, 'ai', '', 'T');
