-- pgbase114.axi_command_prompts definition

-- Drop table

-- DROP TABLE pgbase114.axi_command_prompts;

CREATE TABLE pgbase114.axi_command_prompts (
	id uuid DEFAULT gen_random_uuid() NOT NULL,
	cmdtoken int4 NULL,
	wordpos int4 NULL,
	prompt varchar(200) NULL,
	promptsource varchar(500) NULL,
	promptparams varchar(100) NULL,
	promptvalues varchar(500) NULL,
	props varchar(100) NULL,
	extraparams varchar(1000) NULL,
	requesturl varchar(2000) NULL,
	CONSTRAINT axi_command_prompts_pkey PRIMARY KEY (id)
);