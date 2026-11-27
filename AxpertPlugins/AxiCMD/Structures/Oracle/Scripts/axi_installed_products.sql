<<
CREATE TABLE axi_installed_products (
    id NUMBER(16) NULL,
    product_id VARCHAR2(50) NOT NULL,
    product_name VARCHAR2(255) NOT NULL,
    product_version VARCHAR2(100) NOT NULL,
    product_type VARCHAR2(100) NULL,
    repository_name VARCHAR2(255) NULL,
    repository_url CLOB NULL,
    branch_name VARCHAR2(255) NULL,
    installed_by VARCHAR2(255) NULL,
    installed_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    installation_status VARCHAR2(50) NULL,
    installation_remarks VARCHAR2(4000) NULL
)
>>
