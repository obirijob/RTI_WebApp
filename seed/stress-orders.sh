#!/usr/bin/env bash
set -euo pipefail

API_URL="${API_URL:-http://localhost:5080}"
PRODUCT_ID="${PRODUCT_ID:-1}"
REQUESTS="${REQUESTS:-40}"
QUANTITY="${QUANTITY:-1}"
RESULTS="$(mktemp)"
trap 'rm -f "$RESULTS"' EXIT

payload="$(printf '{"lines":[{"productId":%s,"quantity":%s}]}' "$PRODUCT_ID" "$QUANTITY")"
for _ in $(seq "$REQUESTS"); do
  curl --silent --output /dev/null --write-out "%{http_code}\n" \
    --header "Content-Type: application/json" \
    --data "$payload" \
    "${API_URL}/api/orders" >> "$RESULTS" &
done
wait

echo "Concurrent requests: $REQUESTS"
sort "$RESULTS" | uniq -c
curl --silent "${API_URL}/api/products/${PRODUCT_ID}"
echo
