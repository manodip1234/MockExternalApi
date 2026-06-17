-- =============================================================================
-- FILE:   seed_mock_api_sources.sql
-- PURPOSE: Update api_integration.api_source to point to the correct
--          MockExternalApi endpoints for BCW, FSD, HFW.
--
-- RUN:    psql -U postgres -d wbrtps_final -f seed_mock_api_sources.sql
-- =============================================================================

-- ─────────────────────────────────────────────────────────────────────────────
-- BCW: fix endpoints + credentials
-- ─────────────────────────────────────────────────────────────────────────────
UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/bcw/offices',
       api_key              = 'bcw-test-api-key-001',
       api_secret_encrypted = 'bcw-test-api-key-001',
       auth_credential      = 'bcw-test-api-key-001',
       updated_at           = NOW()
WHERE  department_code = 'BCW' AND api_type = 'OFFICE';

UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/bcw/services',
       api_key              = 'bcw-test-api-key-001',
       api_secret_encrypted = 'bcw-test-api-key-001',
       auth_credential      = 'bcw-test-api-key-001',
       updated_at           = NOW()
WHERE  department_code = 'BCW' AND api_type = 'SERVICE';

UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/bcw/officers',
       api_key              = 'bcw-test-api-key-001',
       api_secret_encrypted = 'bcw-test-api-key-001',
       auth_credential      = 'bcw-test-api-key-001',
       updated_at           = NOW()
WHERE  department_code = 'BCW' AND api_type = 'USER';

UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/bcw/acknowledgements',
       api_key              = 'bcw-test-api-key-001',
       api_secret_encrypted = 'bcw-test-api-key-001',
       auth_credential      = 'bcw-test-api-key-001',
       updated_at           = NOW()
WHERE  department_code = 'BCW' AND api_type = 'ACK';

UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/bcw/sync-response',
       api_key              = 'bcw-test-api-key-001',
       api_secret_encrypted = 'bcw-test-api-key-001',
       auth_credential      = 'bcw-test-api-key-001',
       updated_at           = NOW()
WHERE  department_code = 'BCW' AND api_type = 'CALLBACK';

-- ─────────────────────────────────────────────────────────────────────────────
-- FSD: fix endpoints + update credentials to food-api-key-001
-- ─────────────────────────────────────────────────────────────────────────────
UPDATE api_integration.api_source
SET    endpoint_url              = 'http://localhost:3001/food/offices',
       api_key                   = 'food-api-key-001',
       api_secret_encrypted      = 'food-hmac-secret-001',
       auth_credential           = 'food-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'FSD' AND api_type = 'OFFICE';

UPDATE api_integration.api_source
SET    endpoint_url              = 'http://localhost:3001/food/services',
       api_key                   = 'food-api-key-001',
       api_secret_encrypted      = 'food-hmac-secret-001',
       auth_credential           = 'food-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'FSD' AND api_type = 'SERVICE';

UPDATE api_integration.api_source
SET    endpoint_url              = 'http://localhost:3001/food/officers',
       api_key                   = 'food-api-key-001',
       api_secret_encrypted      = 'food-hmac-secret-001',
       auth_credential           = 'food-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'FSD' AND api_type = 'USER';

UPDATE api_integration.api_source
SET    endpoint_url              = 'http://localhost:3001/food/acknowledgements',
       api_key                   = 'food-api-key-001',
       api_secret_encrypted      = 'food-hmac-secret-001',
       auth_credential           = 'food-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'FSD' AND api_type = 'ACK';

UPDATE api_integration.api_source
SET    endpoint_url              = 'http://localhost:3001/food/sync-response',
       api_key                   = 'food-api-key-001',
       api_secret_encrypted      = 'food-hmac-secret-001',
       auth_credential           = 'food-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'FSD' AND api_type = 'CALLBACK';

-- ─────────────────────────────────────────────────────────────────────────────
-- TRANS → HFW: change department_code, update all endpoints + credentials
-- ─────────────────────────────────────────────────────────────────────────────
UPDATE api_integration.api_source
SET    department_code           = 'HFW',
       endpoint_url              = 'http://localhost:3001/hfw/offices',
       api_key                   = 'hfw-api-key-001',
       api_secret_encrypted      = 'hfw-hmac-secret-001',
       auth_credential           = 'hfw-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'TRANS' AND api_type = 'OFFICE';

UPDATE api_integration.api_source
SET    department_code           = 'HFW',
       endpoint_url              = 'http://localhost:3001/hfw/services',
       api_key                   = 'hfw-api-key-001',
       api_secret_encrypted      = 'hfw-hmac-secret-001',
       auth_credential           = 'hfw-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'TRANS' AND api_type = 'SERVICE';

UPDATE api_integration.api_source
SET    department_code           = 'HFW',
       endpoint_url              = 'http://localhost:3001/hfw/officers',
       api_key                   = 'hfw-api-key-001',
       api_secret_encrypted      = 'hfw-hmac-secret-001',
       auth_credential           = 'hfw-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'TRANS' AND api_type = 'USER';

UPDATE api_integration.api_source
SET    department_code           = 'HFW',
       endpoint_url              = 'http://localhost:3001/hfw/acknowledgements',
       api_key                   = 'hfw-api-key-001',
       api_secret_encrypted      = 'hfw-hmac-secret-001',
       auth_credential           = 'hfw-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'TRANS' AND api_type = 'ACK';

UPDATE api_integration.api_source
SET    department_code           = 'HFW',
       endpoint_url              = 'http://localhost:3001/hfw/sync-response',
       api_key                   = 'hfw-api-key-001',
       api_secret_encrypted      = 'hfw-hmac-secret-001',
       auth_credential           = 'hfw-api-key-001',
       updated_at                = NOW()
WHERE  department_code = 'TRANS' AND api_type = 'CALLBACK';

-- ─────────────────────────────────────────────────────────────────────────────
-- ABC: insert source row pointing to mock API submissions endpoint
-- api_type = 'ABC' — isolated from all other dept syncs.
-- Fetches monthly MIS aggregate data (Form A shape).
-- AbcSyncService groups by period and calls AddBatchAsync.
-- Credentials match MockApiController constants:
--   ValidApiKey = 'rtps-demo-key-001'
--   HmacSecret  = 'rtps-demo-secret-001'
-- ─────────────────────────────────────────────────────────────────────────────

-- Fix api_type constraint first to accept 'ABC'
ALTER TABLE api_integration.api_source
    DROP CONSTRAINT IF EXISTS api_source_api_type_check;

ALTER TABLE api_integration.api_source
    ADD CONSTRAINT api_source_api_type_check
    CHECK (api_type IN ('OFFICE','SERVICE','USER','ACK','CALLBACK','ABC'));

-- Insert ABC source row (skip if already exists)
INSERT INTO api_integration.api_source
    (department_code, department_name, api_type, endpoint_url,
     auth_type, auth_header_name, is_active, timeout_seconds, max_retry_count,
     api_key, api_secret_encrypted, auth_credential,
     uses_name_mapping, supports_incremental_sync, incremental_since_param,
     date_range_initialized)
SELECT
    'ABC',
    'ABC Department',
    'ABC',
    'http://localhost:3001/mock-api/abc/submissions',
    'HMAC',
    'X-API-KEY',
    true,
    30,
    3,
    'rtps-demo-key-001',
    'rtps-demo-secret-001',
    'rtps-demo-key-001',
    false,
    false,
    'updated_since',
    false
WHERE NOT EXISTS (
    SELECT 1 FROM api_integration.api_source
    WHERE department_code = 'ABC' AND api_type = 'ABC'
);

-- Update if already exists (e.g. from 20260608 migration with wrong api_type)
UPDATE api_integration.api_source
SET    endpoint_url         = 'http://localhost:3001/mock-api/abc/submissions',
       api_type             = 'ABC',
       api_key              = 'rtps-demo-key-001',
       api_secret_encrypted = 'rtps-demo-secret-001',
       auth_credential      = 'rtps-demo-key-001',
       auth_type            = 'HMAC',
       auth_header_name     = 'X-API-KEY',
       is_active            = true,
       updated_at           = NOW()
WHERE  department_code = 'ABC' AND api_type = 'ABC_WEBHOOK';

-- ABC callback — mock API receives and logs the callback
INSERT INTO api_integration.api_source
    (department_code, department_name, api_type, endpoint_url,
     auth_type, auth_header_name, is_active, timeout_seconds, max_retry_count,
     api_key, api_secret_encrypted, auth_credential,
     uses_name_mapping, supports_incremental_sync, incremental_since_param,
     date_range_initialized)
SELECT
    'ABC',
    'ABC Department',
    'CALLBACK',
    'http://localhost:3001/mock-api/abc/sync-response',
    'HMAC',
    'X-API-KEY',
    true,
    30,
    3,
    'rtps-demo-key-001',
    'rtps-demo-secret-001',
    'rtps-demo-key-001',
    false,
    false,
    'updated_since',
    false
WHERE NOT EXISTS (
    SELECT 1 FROM api_integration.api_source
    WHERE department_code = 'ABC' AND api_type = 'CALLBACK'
);

-- Dedicated page-wise Form A/B/C source used by AbcSyncService.
-- Requires Database/20260612_abc_sync_production_upgrade.sql.
INSERT INTO abc_integration.abc_source
    (source_code, dept_code, dept_name, endpoint_url, callback_url,
     api_key, api_secret, auth_header_name, timeout_seconds,
     max_retry_count, page_size, contract_version, sync_cron,
     callback_enabled, is_active, created_at, updated_at)
VALUES
    ('BCW-ABC', 'BCW', 'Backward Classes Welfare Department',
     'http://localhost:3001/mock-api/abc/submissions',
     'http://localhost:3001/mock-api/abc/sync-response',
     'rtps-demo-key-001', 'rtps-demo-secret-001', 'X-API-KEY',
     30, 3, 100, 'v1', '0 */6 * * *', true, true, NOW(), NOW())
ON CONFLICT ((upper(source_code))) DO UPDATE
SET dept_code = EXCLUDED.dept_code,
    dept_name = EXCLUDED.dept_name,
    endpoint_url = EXCLUDED.endpoint_url,
    callback_url = EXCLUDED.callback_url,
    api_key = EXCLUDED.api_key,
    api_secret = EXCLUDED.api_secret,
    auth_header_name = EXCLUDED.auth_header_name,
    timeout_seconds = EXCLUDED.timeout_seconds,
    max_retry_count = EXCLUDED.max_retry_count,
    page_size = EXCLUDED.page_size,
    contract_version = EXCLUDED.contract_version,
    sync_cron = EXCLUDED.sync_cron,
    callback_enabled = EXCLUDED.callback_enabled,
    is_active = true,
    updated_at = NOW();

-- ─────────────────────────────────────────────────────────────────────────────
-- Verify
-- ─────────────────────────────────────────────────────────────────────────────
SELECT id, department_code, api_type, endpoint_url, api_key, is_active
FROM   api_integration.api_source
WHERE  department_code IN ('BCW', 'FSD', 'HFW', 'ABC')
ORDER  BY department_code, api_type;
