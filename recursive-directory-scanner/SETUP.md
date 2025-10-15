# Quick Setup Guide

## First Time Setup

1. **Install Node.js** (if not already installed)
   - Download from: https://nodejs.org/
   - Verify installation: `node --version` and `npm --version`

2. **Install Project Dependencies**
   ```bash
   npm install
   ```

3. **Build the Project**
   ```bash
   npm run build
   ```

## Quick Start

### Run in Development Mode
```bash
npm run dev
```

### Run with a Specific Directory
```bash
npm run dev ./src
```

### Run with Options
```bash
npm run dev ./src --max-depth=3 --include-hidden
```

## Available Commands

| Command | Description |
|---------|-------------|
| `npm install` | Install all dependencies |
| `npm run build` | Compile TypeScript to JavaScript |
| `npm start` | Run the compiled application |
| `npm run dev` | Run in development mode (no build needed) |
| `npm test` | Run unit tests |
| `npm run lint` | Check code quality with ESLint |
| `npm run clean` | Remove compiled files |

## Troubleshooting

### Error: "Cannot find module"
- Run `npm install` to ensure all dependencies are installed

### Error: "tsc: command not found"
- TypeScript is installed locally, use `npm run build` instead of `tsc` directly

### Permission Errors on Windows
- Run your terminal as Administrator if scanning system directories

### Tests Failing
- Ensure you've run `npm install` to get all dev dependencies
- Check that Node.js version is 18 or higher

## Project Structure Overview

```
src/
├── models/          # Data models (FileNode, ScanResult)
├── scanner/         # Core scanning logic
├── display/         # Display and formatting
└── index.ts         # Main entry point
```

## Next Steps

1. Read the main README.md for detailed documentation
2. Explore the source code in the `src/` directory
3. Run the tests: `npm test`
4. Try scanning different directories with various options
5. Modify the code and experiment!
