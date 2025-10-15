# Quick Reference Guide

## 🚀 Getting Started (3 Steps)

1. **Install dependencies** (first time only):
   ```bash
   npm install
   ```

2. **Build the project**:
   ```bash
   npm run build
   ```

3. **Run the scanner**:
   ```bash
   npm run dev
   ```

## 📋 Common Commands

| Command | What it does |
|---------|--------------|
| `npm install` | Install all dependencies (first time setup) |
| `npm run build` | Compile TypeScript → JavaScript |
| `npm run dev` | Run without building (development mode) |
| `npm start` | Run the compiled version |
| `npm test` | Run all unit tests |
| `npm run lint` | Check code quality |
| `npm run clean` | Delete compiled files |

## 🎯 Usage Examples

### Scan current directory
```bash
npm run dev
```

### Scan a specific folder
```bash
npm run dev ./src
npm run dev C:\Users\YourName\Documents
```

### Limit scan depth
```bash
npm run dev . --max-depth=2
```

### Include hidden files
```bash
npm run dev . --include-hidden
```

### Show only statistics (no tree)
```bash
npm run dev . --stats-only
```

### Combine multiple options
```bash
npm run dev ./project --max-depth=3 --include-hidden
```

### Get help
```bash
npm run dev --help
```

## 📁 Project Structure

```
recursive-directory-scanner/
├── src/                    # TypeScript source code
│   ├── models/            # Data models (FileNode, ScanResult)
│   ├── scanner/           # Scanner logic
│   ├── display/           # Display formatting
│   └── index.ts           # Main entry point
├── dist/                  # Compiled JavaScript (after build)
├── package.json           # Dependencies and scripts
├── tsconfig.json          # TypeScript configuration
├── .eslintrc.json         # Linting rules
├── jest.config.js         # Testing configuration
└── README.md              # Full documentation
```

## 🔧 Key Files to Explore

| File | Purpose |
|------|---------|
| `src/index.ts` | Main application entry point |
| `src/scanner/DirectoryScanner.ts` | Core scanning logic with recursion |
| `src/models/FileNode.ts` | File/directory node class |
| `src/display/TreeDisplay.ts` | Tree visualization |
| `README.md` | Complete project documentation |

## ✨ Features Demonstrated

- ✅ **Recursion** - Directory traversal, size calculation, tree display
- ✅ **Classes** - FileNode, ScanResult, DirectoryScanner, TreeDisplay
- ✅ **Async/Await** - File system operations
- ✅ **Exception Handling** - Custom exceptions, try-catch
- ✅ **Lists/Arrays** - Children nodes, file lists, statistics
- ✅ **ESLint** - Code quality checking
- ✅ **Jest** - Unit testing framework

## 🐛 Troubleshooting

### PowerShell script execution error
**Solution**: Use Command Prompt (cmd) instead, or see `WINDOWS_SETUP.md`

### "Cannot find module" error
**Solution**: Run `npm install`

### TypeScript errors
**Solution**: Run `npm run build` to compile

### Permission denied
**Solution**: Run terminal as Administrator (for system directories)

## 📚 Documentation Files

- **README.md** - Complete project documentation
- **SETUP.md** - Quick setup guide
- **WINDOWS_SETUP.md** - Windows-specific instructions
- **REQUIREMENTS_CHECKLIST.md** - Verification of all requirements
- **QUICK_REFERENCE.md** - This file

## 🎓 Learning Resources

- [TypeScript Docs](https://www.typescriptlang.org/docs/)
- [Node.js File System](https://nodejs.org/api/fs.html)
- [Jest Testing](https://jestjs.io/docs/getting-started)
- [ESLint TypeScript](https://typescript-eslint.io/)

## 💡 Tips

1. **Start small**: Try scanning a small directory first
2. **Use --max-depth**: Limit depth for large directories
3. **Check the tests**: Run `npm test` to see examples
4. **Read the code**: All files are well-commented
5. **Experiment**: Modify the code and see what happens!

## ⚡ Quick Test

After installation, test that everything works:

```bash
# Test on the project itself
npm run dev . --max-depth=2

# Run the tests
npm test

# Check code quality
npm run lint
```

If all three commands work, you're all set! 🎉
