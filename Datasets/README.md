# Datasets

This folder is used to store exported JSON snapshots of the running `MockExternalApi` responses.

## Export (creates a new dataset + keeps the old one as emergency backup)

1. Start the API (example):
   - `dotnet run --urls http://localhost:3001`
2. In another terminal, run:
   - `powershell -ExecutionPolicy Bypass -File scripts\\export-datasets.ps1`

Output:
- `Datasets/current/` contains the latest exported snapshot.
- The previous `Datasets/current/` (if present) is moved to `Datasets/archive/<yyyyMMdd_HHmmss>/`.

