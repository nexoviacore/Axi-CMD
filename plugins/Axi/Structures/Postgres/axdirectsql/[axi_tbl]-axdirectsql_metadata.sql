

-- pgbase114.axdirectsql_metadata definition

-- Drop table

-- DROP TABLE pgbase114.axdirectsql_metadata;

CREATE TABLE pgbase114.axdirectsql_metadata (
	axdirectsql_metadataid numeric(16) NOT NULL,
	axdirectsqlid numeric(16) NULL,
	axdirectsql_metadatarow int4 NULL,
	fldname varchar(100) NULL,
	fldcaption varchar(100) NULL,
	"normalized" varchar(3) NULL,
	sourcetable varchar(50) NULL,
	sourcefld varchar(50) NULL,
	CONSTRAINT aglaxdirectsql_metadataid PRIMARY KEY (axdirectsql_metadataid)
);

-- Permissions

ALTER TABLE pgbase114.axdirectsql_metadata OWNER TO pgbase114;
GRANT ALL ON TABLE pgbase114.axdirectsql_metadata TO pgbase114;

-- Alter table

ALTER TABLE pgbase114.axdirectsql_metadata ADD tbl_normalizedsource varchar(2000) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD tbl_hyperlink varchar(8000) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD hyp_struct varchar(500) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD hyp_structtype varchar(20) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD hyp_transid varchar(100) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD datatypeui varchar(20) NULL;
ALTER TABLE pgbase114.axdirectsql_metadata ADD fdatatype varchar(2) NULL;


--metadata manually added to axdirectsql_metadata when filling data for ADS | informed by abinash
=============================================================================================================================