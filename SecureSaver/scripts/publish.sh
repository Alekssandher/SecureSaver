#!/bin/bash

# Builds directory
OUTPUT_DIR="./publish"

# Framework version
FRAMEWORK="net8.0"

publish() {
    RID=$1
    TARGET_DIR="$OUTPUT_DIR/$RID"

    echo "Building for $RID..."

    dotnet publish -c Release -r $RID --self-contained true /p:PublishSingleFile=true -o $TARGET_DIR

    if [ $? -eq 0 ]; then
        echo "Build for $RID completed: $TARGET_DIR"
    else
        echo "Error building for $RID"
    fi
    echo
}

# Windows 64-bit
publish win-x64

# Linux 64-bit
publish linux-x64

# macOS 64-bit (Intel)
publish osx-x64

# macOS ARM64 (Apple Silicon M1/M2)
publish osx-arm64

echo "Builds finished."
