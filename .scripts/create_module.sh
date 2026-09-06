#!/usr/bin/env bash

set -euo pipefail

if [[ $# -lt 1 ]]; then
	echo "Usage: $0 <module-name>" >&2
	exit 1
fi

# find the correct module root independent of caller location.
WORKSPACE_ROOT="${containerWorkspaceFolder:-$PWD}"
MODULE_ROOT="$WORKSPACE_ROOT/Modules"

cd "$WORKSPACE_ROOT"

MODULE_NAME="$1"

MODULE_PATH="$MODULE_ROOT/$MODULE_NAME"
CONTRACTS_PATH="$MODULE_ROOT/$MODULE_NAME.Contracts"
TESTS_PATH="$MODULE_ROOT/$MODULE_NAME.Tests"

SHARED_KERNEL_PROJECT_PATH="$WORKSPACE_ROOT/Shared.Kernel/Shared.Kernel.csproj"

if [[ -e "$MODULE_PATH" || -e "$CONTRACTS_PATH" || -e "$TESTS_PATH" ]]; then
	echo "Error: module '$MODULE_NAME' already exists in $MODULE_ROOT" >&2
	exit 1
fi

echo "creating new module in $MODULE_ROOT"

mkdir -p "$MODULE_ROOT"

dotnet new classlib \
	--name "$MODULE_NAME" \
	--output "$MODULE_PATH" \
    --framework net10.0 \
    --no-restore

dotnet new classlib \
	--name "$MODULE_NAME.Contracts" \
	--output "$CONTRACTS_PATH" \
    --framework net10.0 \
    --no-restore

dotnet new xunit3 \
	--name "$MODULE_NAME.Tests" \
	--output "$TESTS_PATH" \
	--framework net10.0 \
	--test-runner mtp-v2 \
	--no-restore

# remove package version information. version is defined centrally.
sed -i 's/ Version="[^"]*"//' \
	"$TESTS_PATH/$MODULE_NAME.Tests.csproj"

# add project references within module
dotnet add "$MODULE_PATH/$MODULE_NAME.csproj" reference \
	"$CONTRACTS_PATH/$MODULE_NAME.Contracts.csproj" \
	"$SHARED_KERNEL_PROJECT_PATH"

dotnet add "$TESTS_PATH/$MODULE_NAME.Tests.csproj" reference \
	"$MODULE_PATH/$MODULE_NAME.csproj" \
	"$CONTRACTS_PATH/$MODULE_NAME.Contracts.csproj"

# add projects to solution
dotnet sln add \
	"$MODULE_PATH/$MODULE_NAME.csproj" \
	"$CONTRACTS_PATH/$MODULE_NAME.Contracts.csproj" \
	"$TESTS_PATH/$MODULE_NAME.Tests.csproj"

dotnet restore
