CREATE TABLE pgbase114.axp_tstructprops (
	"name" varchar(5) NULL,
	caption varchar(500) NULL,
	keyfield varchar(200) NULL,
	userconfigured bpchar(1) NULL,
	createdon varchar(30) NULL,
	updatedon varchar(30) NULL,
	createdby varchar(100) NULL,
	updatedby varchar(100) NULL
);

-- Permissions

ALTER TABLE pgbase114.axp_tstructprops OWNER TO pgbase114;
GRANT ALL ON TABLE pgbase114.axp_tstructprops TO pgbase114;