#!/usr/bin/env bash
set -euo pipefail
URL=${1:-http://localhost:8080/healthz}
for i in {1..90}; do
  if curl -fsS "$URL" >/dev/null; then echo "READY: $URL"; exit 0; fi
  echo "waiting $URL ($i/90)"; sleep 2
done
echo "timeout waiting $URL"; exit 1
