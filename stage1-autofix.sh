#!/usr/bin/env bash
# Apply the OPC Foundation migration analyzer's auto-fixes to one project.
# Fourteen of the 26 UA00xx rules ship a CodeFixProvider; this is the pass the
# OPC Foundation migration skill prescribes before any hand editing.
#
#   ./stage1-autofix.sh src/Technosoftware/UaConfiguration/Technosoftware.UaConfiguration.csproj
#
# Commit first: this rewrites source files, and `git diff` afterwards is the review.
set -euo pipefail
cd "$(dirname "$0")"
proj="${1:?usage: stage1-autofix.sh <project.csproj>}"

if [ -n "$(git status --porcelain)" ]; then
  echo "Working tree is dirty. Commit or stash first so the fixer's diff is reviewable." >&2
  exit 1
fi

dotnet format analyzers "$proj" \
  --diagnostics UA0002 UA0003 UA0004 UA0005 UA0006 UA0007 UA0008 \
                UA0009 UA0010 UA0012 UA0014 UA0019 UA0020 UA0022 \
  --severity warn -v diag 2>&1 | tail -40

echo
echo "================ what the fixer changed ================"
git --no-pager diff --stat
