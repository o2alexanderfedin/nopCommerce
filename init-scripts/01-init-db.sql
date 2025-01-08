-- Enable required extensions
CREATE EXTENSION IF NOT EXISTS vector;
CREATE EXTENSION IF NOT EXISTS citext;
CREATE EXTENSION IF NOT EXISTS btree_gin;
CREATE EXTENSION IF NOT EXISTS pg_trgm;

-- Set up optimized configuration for minimal footprint
ALTER SYSTEM SET max_connections = '100';
ALTER SYSTEM SET shared_buffers = '256MB';
ALTER SYSTEM SET work_mem = '4MB';
ALTER SYSTEM SET maintenance_work_mem = '64MB';
ALTER SYSTEM SET effective_cache_size = '512MB';
ALTER SYSTEM SET random_page_cost = '1.1';
ALTER SYSTEM SET effective_io_concurrency = '200';

-- Logging configuration (minimal)
ALTER SYSTEM SET log_min_duration_statement = '2000';
ALTER SYSTEM SET log_checkpoints = 'on';
ALTER SYSTEM SET log_connections = 'off';
ALTER SYSTEM SET log_disconnections = 'off';

-- Optimize autovacuum for small installations
ALTER SYSTEM SET autovacuum_vacuum_scale_factor = '0.1';
ALTER SYSTEM SET autovacuum_analyze_scale_factor = '0.05';

-- Set timezone
ALTER SYSTEM SET timezone = 'UTC';

-- Create nopcommerce database objects
\c nopcommerce

-- Create schema if it doesn't exist
CREATE SCHEMA IF NOT EXISTS public;

-- Grant privileges
GRANT ALL ON SCHEMA public TO nop;
GRANT ALL ON ALL TABLES IN SCHEMA public TO nop;
GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO nop;

-- Create extension in the nopcommerce database
CREATE EXTENSION IF NOT EXISTS vector;
CREATE EXTENSION IF NOT EXISTS citext;
CREATE EXTENSION IF NOT EXISTS btree_gin;
CREATE EXTENSION IF NOT EXISTS pg_trgm;
