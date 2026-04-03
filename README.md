# PowerEncoder

A PowerToys Run plugin for encoding/decoding text with automatic format detection.

## Features

- **Unicode Encode/Decode** - Convert between `\uXXXX` format and Chinese characters
- **URL Encode/Decode** - Handle `%XX` URL encoding
- **Unix Timestamp** - Convert timestamps to datetime and vice versa
- **Binary ↔ ASCII** - Convert binary strings to/from ASCII characters
- **Hex ↔ ASCII** - Convert hexadecimal to/from ASCII characters
- **Base64 Encode/Decode** - Standard Base64 encoding/decoding

## Installation

1. Download the latest release from [Releases](https://github.com/Pepste2/PowerEncoder/releases)
2. Extract `PowerEncoder.zip` to `%LOCALAPPDATA%\PowerToys\PowerToys Run\Plugins\`
3. Restart PowerToys

## Usage

Type `#` followed by your text in PowerToys Run. The plugin automatically detects the format and provides all possible conversion options.

### Examples

| Input | Output |
|-------|--------|
| `#\u4e2d\u6587` | 中文 (Unicode decode) |
| `#中文` | `\u4e2d\u6587` (Unicode encode) |
| `#Hello%20World` | Hello World (URL decode) |
| `#1700000000` | 2023-11-14 22:13:20 (Timestamp) |
| `#01001000` | H (Binary decode) |
| `#48656c6c6f` | Hello (Hex decode) |
| `#SGVsbG8=` | Hello (Base64 decode) |

## Build

```bash
dotnet build -p:Platform=x64
```

The output will be in `bin/output_x64/PowerEncoder/`.

## Requirements

- PowerToys Run (included in PowerToys)
- .NET 8.0 SDK (for building)

## License

MIT License