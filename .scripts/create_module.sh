#!/usr/bin/env bash

set -euo pipefail

if [[ $# -lt 1 ]]; then
	echo "Usage: $0 <module-name>" >&2
	exit 1
fi

# find the correct module root independent of caller location.
WORKSPACE_ROOT="${containerWorkspaceFolder:-$PWD}"

cd "$WORKSPACE_ROOT"

MODULE_NAME="$1"
MODULE_PREFIX="Modules.$MODULE_NAME"

MODULE_PATH="$WORKSPACE_ROOT/$MODULE_PREFIX"

CORE="$MODULE_PREFIX.Core"
CONTRACTS="$MODULE_PREFIX.Contracts"
TESTS="$MODULE_PREFIX.Tests"

CORE_PATH="$MODULE_PATH/$CORE"
CONTRACTS_PATH="$MODULE_PATH/$CONTRACTS"
TESTS_PATH="$MODULE_PATH/$TESTS"

SHARED_KERNEL_PROJECT_PATH="$WORKSPACE_ROOT/Shared.Kernel/Shared.Kernel.csproj"

if [[ -e "$CORE_PATH" || -e "$CONTRACTS_PATH" || -e "$TESTS_PATH" ]]; then
	echo "Error: module '$MODULE_PREFIX' already exists in $WORKSPACE_ROOT" >&2
	exit 1
fi

echo "creating new module in $WORKSPACE_ROOT"

dotnet new classlib \
	--name "$CORE" \
	--output "$CORE_PATH" \
    --framework net10.0 \
    --no-restore

dotnet new classlib \
	--name "$CONTRACTS" \
	--output "$CONTRACTS_PATH" \
    --framework net10.0 \
    --no-restore

dotnet new xunit3 \
	--name "$TESTS" \
	--output "$TESTS_PATH" \
	--framework net10.0 \
	--test-runner mtp-v2 \
	--no-restore

# remove package version information. version is defined centrally.
sed -i 's/ Version="[^"]*"//' \
	"$TESTS_PATH/$TESTS.csproj"

# add project references within module
dotnet add "$CORE_PATH/$CORE.csproj" reference \
	"$CONTRACTS_PATH/$CONTRACTS.csproj" \
	"$SHARED_KERNEL_PROJECT_PATH"

dotnet add "$TESTS_PATH/$TESTS.csproj" reference \
	"$CORE_PATH/$CORE.csproj" \
	"$CONTRACTS_PATH/$CONTRACTS.csproj"

# add projects to solution
dotnet sln add \
	"$CORE_PATH/$CORE.csproj" \
	"$CONTRACTS_PATH/$CONTRACTS.csproj" \
	"$TESTS_PATH/$TESTS.csproj"

dotnet restore
