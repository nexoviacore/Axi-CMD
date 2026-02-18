-- pgbase114.axi_commands definition

-- Drop table

-- DROP TABLE pgbase114.axi_commands;

CREATE TABLE pgbase114.axi_commands (
	cmdtoken int4 NOT NULL,
	command_group varchar(50) NOT NULL,
	command varchar(50) NOT NULL,
	CONSTRAINT axi_commands_pkey PRIMARY KEY (cmdtoken)
);