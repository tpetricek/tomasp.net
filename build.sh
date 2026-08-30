#!/bin/bash
cd "$(dirname "$0")"
# Local R2 credentials, if present (see .env.example)
if [ -f .env ]; then set -a; . ./.env; set +a; fi
dotnet run --project tools -c Release -- "$@"
