#!/usr/bin/env bash

set -euo pipefail

# find the correct module root independent of caller location.
WORKSPACE_ROOT="${containerWorkspaceFolder:-$PWD}"

cd "$WORKSPACE_ROOT"

if [[ $# -lt 1 ]]; then
	echo "Usage: $0 <module-name>" >&2
	exit 1
fi

MODULE_NAME="$1"
MODULE_PREFIX="Modules.$MODULE_NAME"

MODULES_DIR="$WORKSPACE_ROOT/Modules"
MODULE_PATH="$MODULES_DIR/$MODULE_NAME"

CORE="$MODULE_PREFIX"
CONTRACTS="$MODULE_PREFIX.Contracts"
TESTS="$MODULE_PREFIX.Tests"

CORE_PATH="$MODULE_PATH/$CORE"
CONTRACTS_PATH="$MODULE_PATH/$CONTRACTS"
TESTS_PATH="$MODULE_PATH/$TESTS"

SHARED_KERNEL_PROJECT_PATH="$WORKSPACE_ROOT/Shared.Kernel/Shared.Kernel.csproj"

if [[ -e "$MODULE_PATH" ]]; then
	echo "Error: module '$MODULE_NAME' already exists in $MODULES_DIR" >&2
	exit 1
fi

echo "creating new module in $MODULES_DIR"

dotnet new classlib \
	--name "$CORE" \
	--output "$CORE_PATH" \
    --framework net10.0 \
    --no-restore

sed -i '$d' "$CORE_PATH/$CORE.csproj"
cat >> "$CORE_PATH/$CORE.csproj" <<'EOF'

  <ItemGroup>
    <PackageReference Include="WolverineFx" />
  </ItemGroup>
</Project>
EOF

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
