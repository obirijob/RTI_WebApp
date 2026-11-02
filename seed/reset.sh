#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
docker compose --project-directory "$ROOT" down -v
docker compose --project-directory "$ROOT" up --build
