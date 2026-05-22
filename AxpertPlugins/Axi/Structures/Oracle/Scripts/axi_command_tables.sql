CREATE TABLE AXI_COMMANDS 
   (	"CMDTOKEN" NUMBER(10,0) NOT NULL ENABLE, 
	"COMMAND_GROUP" VARCHAR2(50) NOT NULL ENABLE, 
	"COMMAND" VARCHAR2(50) NOT NULL ENABLE, 
	"ACTIVE" VARCHAR2(1) DEFAULT 'T', 
	 CONSTRAINT "AXI_COMMANDS_PKEY" PRIMARY KEY ("CMDTOKEN"));
	 
CREATE TABLE AXI_COMMAND_PROMPTS 
   (	"ID" RAW(16) DEFAULT SYS_GUID() NOT NULL ENABLE, 
	"CMDTOKEN" NUMBER(10,0), 
	"WORDPOS" NUMBER(10,0), 
	"PROMPT" VARCHAR2(200), 
	"PROMPTSOURCE" VARCHAR2(500), 
	"PROMPTPARAMS" VARCHAR2(100), 
	"PROMPTVALUES" VARCHAR2(500), 
	"PROPS" VARCHAR2(100), 
	"EXTRAPARAMS" VARCHAR2(1000), 
	"REQUESTURL" VARCHAR2(2000), 
	 CONSTRAINT "AXI_COMMAND_PROMPTS_PKEY" PRIMARY KEY ("ID"));
	 
	
	
CREATE TABLE AXP_TSTRUCTPROPS
   (	"NAME" VARCHAR2(5), 
	"CAPTION" VARCHAR2(500), 
	"KEYFIELD" VARCHAR2(200), 
	"USERCONFIGURED" CHAR(1), 
	"CREATEDON" VARCHAR2(30), 
	"UPDATEDON" VARCHAR2(30), 
	"CREATEDBY" VARCHAR2(100), 
	"UPDATEDBY" VARCHAR2(100)
   );

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(1, 'Create', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(2, 'Edit', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(3, 'View', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(4, 'Configure', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(5, 'Upload', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(6, 'Download', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(7, 'DevTools', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(9, 'Run', ' ', 'T');

INSERT INTO axi_commands
(cmdtoken, command_group, command, active)
VALUES(10, 'Analyse', ' ', 'T');


