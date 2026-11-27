<<
CREATE TABLE axi_installed_products (
    id NUMERIC(16) NULL,
    product_id VARCHAR(50) NOT NULL,
    product_name VARCHAR(255) NOT NULL,
    product_version VARCHAR(100) NOT NULL,
    product_type VARCHAR(100) NULL,
    repository_name VARCHAR(255) NULL,
    repository_url TEXT NULL,
    branch_name VARCHAR(255) NULL,
    installed_by VARCHAR(255) NULL,
    installed_on TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    installation_status VARCHAR(50) NULL,
    installation_remarks VARCHAR(4000) NULL
)
>>
