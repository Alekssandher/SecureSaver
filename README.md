# SecureSaver

A robust command-line file encryption and decryption tool built with C# and .NET 8.0, utilizing AES-256 encryption with PBKDF2 key derivation for maximum security.

## Features

- **Strong Encryption**: AES-256 encryption with PBKDF2-SHA256 key derivation
- **Secure Key Derivation**: Configurable iterations (default: 500,000) for enhanced security
- **Secure File Deletion**: Optional overwriting of original files with random data
- **Cross-Platform**: Supports Windows, Linux, and macOS (Intel & Apple Silicon)
- **Interactive Password Input**: Hidden password entry with confirmation for encryption
- **Comprehensive Error Handling**: Detailed error messages with appropriate exit codes

## Installation

### Prerequisites

- .NET 8.0 SDK or Runtime

### Building from Source

1. Clone the repository:
```bash
git clone <repository-url>
cd SecureSaver
```

2. Build the project:
```bash
dotnet build -c Release
```

3. (Optional) Create cross-platform binaries:
```bash
chmod +x scripts/publish.sh
./scripts/publish.sh
```

This will generate self-contained executables for:
- Windows x64
- Linux x64
- macOS x64 (Intel)
- macOS ARM64 (Apple Silicon)

## Usage

### Basic Syntax

```bash
SecureSaver --input <input_file> --output <output_file> --operation <enc|dec> [options]
```

### Required Arguments

- `--input, -i`: Path to the file to encrypt/decrypt
- `--output, -o`: Path where the processed file will be saved
- `--operation, -op`: Operation type (`enc` for encryption, `dec` for decryption)

### Optional Arguments

- `--iterations, -itr`: Number of PBKDF2 iterations (default: 500,000)
- `--verbose, -v`: Enable verbose output
- `--overwriteOriginalFile, -oof`: Securely overwrite the original file after processing

### Examples

#### Encrypt a file:
```bash
SecureSaver -i document.txt -o document.txt.encrypted -op enc
```

#### Decrypt a file:
```bash
SecureSaver -i document.txt.encrypted -o document.txt -op dec
```

#### Encrypt with custom iterations and overwrite original:
```bash
SecureSaver -i sensitive.pdf -o sensitive.pdf.enc -op enc -itr 1000000 -oof
```

#### Decrypt with verbose output:
```bash
SecureSaver -i data.enc -o data.original -op dec -v
```

## Security Features

### Encryption Details

- **Algorithm**: AES-256 in CBC mode
- **Key Derivation**: PBKDF2 with SHA-256
- **Salt**: 16-byte random salt (unique per encryption)
- **IV**: 16-byte random initialization vector (unique per encryption)
- **Default Iterations**: 500,000 (configurable)

### File Format

Encrypted files contain:
1. 16-byte salt
2. 16-byte IV
3. Encrypted data

### Secure Deletion

When using the `--overwriteOriginalFile` option, the original file is securely deleted by:
1. Overwriting the entire file with cryptographically secure random data
2. Deleting the file from the filesystem

## Error Codes

- `0`: Success
- `1`: General cryptographic or application error
- `3`: User cancelled operation (low iteration warning)
- `66`: File not found
- `73`: Directory not found
- `77`: Unauthorized access

## Security Considerations

### Password Security
- Use strong, unique passwords
- Passwords are never stored or logged
- Password input is hidden during entry

### Iteration Count
- Higher iteration counts provide better security against brute-force attacks
- Minimum recommended: 100,000 iterations
- Default: 500,000 iterations
- The tool warns when using less than 100,000 iterations

### Best Practices
- Always verify decrypted files before deleting encrypted versions
- Use the secure deletion option for sensitive files
- Keep encrypted files in secure locations
- Consider using the highest iteration count your system can handle

## Platform Support

| Platform | Architecture | Status |
|----------|-------------|--------|
| Windows | x64 | ✅ Supported |
| Linux | x64 | ✅ Supported |
| macOS | x64 (Intel) | ✅ Supported |
| macOS | ARM64 (Apple Silicon) | ✅ Supported |

## Dependencies

- **.NET 8.0**: Core runtime
- **System.CommandLine**: Command-line argument parsing
- **System.Security.Cryptography**: Cryptographic operations

## Development

### Project Structure

```
SecureSaver/
├── Program.cs              # Main entry point
├── src/
│   ├── Models/
│   │   └── Base.cs         # Configuration model
│   ├── Services/
│   │   ├── EncryptionService.cs  # Main encryption/decryption logic
│   │   └── FileService.cs        # Secure file operations
│   └── Utils/
│       ├── ArgValidator.cs       # Command-line argument validation
│       ├── DecryptFile.cs        # Decryption utilities
│       ├── EncryptFile.cs        # Encryption utilities
│       └── PasswordReader.cs     # Secure password input
└── scripts/
    └── publish.sh          # Cross-platform build script
```

### Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the GNU General Public License v3.0 (GPL-3.0).

This means you can:
- Use the software for any purpose
- Study and modify the source code
- Distribute copies of the software
- Distribute modified versions

**Requirements:**
- Any distributed copies or modifications must also be licensed under GPL-3.0
- Source code must be made available when distributing
- Changes must be documented

See the [LICENSE](LICENSE) file for the full license text.

## Disclaimer

This software is provided "as is" without warranty of any kind. While it uses industry-standard encryption algorithms, no encryption tool is 100% secure. Users are responsible for:
- Choosing strong passwords
- Keeping passwords secure
- Testing the software with non-critical data first
- Maintaining backups of important files

The developers are not responsible for any data loss or security breaches resulting from the use of this software.

## Support

For bug reports, feature requests, or questions:
- Open an issue on the project repository
- Ensure you're using the latest version
- Include system information and steps to reproduce any issues

## Version History

- **v1.0.0**: Initial release with AES-256 encryption, PBKDF2 key derivation, and cross-platform support
