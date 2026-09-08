#!/usr/bin/env bash
# Stage 0 of the OPC UA 2.0 migration: build the four libraries against
# 2.0.0-preview.4 and inventory the errors. Not tracked by git; delete when done.
#
#   ./stage0-count.sh            # libraries only  (the number that matters)
#   ./stage0-count.sh --all      # libraries, then tutorials and tests
#
# Notes on the flags:
#   -f net10.0                     one TFM, not three
#   -p:TreatWarningsAsErrors=false 2.0 marks the old API [Obsolete] on purpose;
#                                  with the repo default those become errors
#   -p:GeneratePackageOnBuild=false  don't pack a build that won't succeed
#   --tl:off                       plain output, so the log is greppable

set -u
cd "$(dirname "$0")"
OUT=./stage0-logs
rm -rf "$OUT"; mkdir -p "$OUT"

build() {  # build <label> <project>
  printf '\n=== %s\n' "$1"
  dotnet build "$2" -f net10.0 -c Release \
    -p:TreatWarningsAsErrors=false -p:GeneratePackageOnBuild=false \
    --nologo --tl:off > "$OUT/$1.log" 2>&1
  printf '    exit %s, %s errors -> %s\n' \
    "$?" "$(grep -c ': error ' "$OUT/$1.log")" "$OUT/$1.log"
}

# dependency order: a failure upstream explains the noise downstream
build UaUtilities      src/Technosoftware/UaUtilities/Technosoftware.UaUtilities.csproj
build UaConfiguration  src/Technosoftware/UaConfiguration/Technosoftware.UaConfiguration.csproj
build UaClient         src/Technosoftware/UaClient/Technosoftware.UaClient.csproj
build UaServer         src/Technosoftware/UaServer/Technosoftware.UaServer.csproj

if [ "${1:-}" = "--all" ]; then
  for p in tutorials/SampleCompany/*/*.csproj Tests/Technosoftware/*/*.csproj; do
    build "$(basename "$p" .csproj)" "$p"
  done
fi

echo
echo "================ errors by code ================"
grep -h -oE ': error [A-Z]+[0-9]+' "$OUT"/*.log | sed 's/: error //' | sort | uniq -c | sort -rn
echo
echo "================ errors by file (top 25) ================"
grep -h -oE '^[^(]+\([0-9]+,[0-9]+\): error' "$OUT"/*.log \
  | sed -E 's/\([0-9]+,[0-9]+\): error//' | sort | uniq -c | sort -rn | head -25
echo
echo "Total: $(grep -h -c ': error ' "$OUT"/*.log | paste -sd+ - | bc) error lines across $(ls "$OUT" | wc -l) projects."
