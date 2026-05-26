-- =============================================================================
-- FILE:   seed_mock_api_sources.sql
-- PURPOSE: Update api_integration.api_source to point to the correct
--          MockExternalApi endpoints for BCW, FSD, HFW.
--
-- RUN:    psql -U postgres -d wbrtps_final -f seed_mock_api_sources.sql
-- =============================================================================

-- ─────────────────────────────────────────────────────────────────────────────
-- BCW: fix endpoints (add /bcw/ prefix), keep existing key/secret
-- ─────────────────────────────────────────────────────────────────────────────
UPDATE api_integration.api_source
SET    endpoint_url = 'http://localhost:3001/bcw/offices',
       updated_at   = NOW()
WHERE  department_code = 'BCW' AND api_type = 'OFFICE';

UPDATE api_integration.api_source
SET    endpoint_url = 'http://localhost:3001/bcw/services',
       updated_at   = NOW()
WHERE  department_code = 'BCW' AND api_type = 'SERVICE';

UPDATE api_integration.api_source
SET    endpoint_url = 'http://localhost:3001/bcw/officers',
       updated_at   = NOW()
WHERE  department_code = 'BCW' AND api_type = 'USER';

UPDATE api_integration.api_source
SET    endpoint_url = 'http://localhost:3001/bcw/acknowledgements',
       updated_at   = NOW()
WHERE  department_code = 'BCW' AND api_type = 'ACK';

UPDATE api_integration.api_source
SET    endpoint_url = 'http://localhost:3001/bcw/sync-response',
       updated_at   = NOW()
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
-- Verify
-- ─────────────────────────────────────────────────────────────────────────────
SELECT id, department_code, api_type, endpoint_url, api_key, is_active
FROM   api_integration.api_source
WHERE  department_code IN ('BCW', 'FSD', 'HFW')
ORDER  BY department_code, api_type;
