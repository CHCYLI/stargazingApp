#!/usr/bin/env bash
set -e
cd "$(dirname "$0")/StargazingApi"
dotnet tool restore
dotnet restore
dotnet run