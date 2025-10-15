# Overview

This project is a **Recursive Directory Scanner** built with TypeScript that demonstrates core programming concepts including recursion, asynchronous operations, object-oriented programming, and exception handling. The application scans a directory structure recursively and displays a visual tree representation along with detailed statistics.

The purpose of this software is to provide a practical tool for analyzing directory structures while demonstrating proficiency in TypeScript development. It showcases the use of modern TypeScript features, proper error handling, and clean code architecture.

[Software Demo Video](http://youtube.link.goes.here)

# Development Environment

## Tools Used
- **Visual Studio Code** - Primary code editor
- **Node.js** (v18+) - JavaScript runtime environment
- **npm** - Package manager for Node.js
- **Git** - Version control system

## Programming Language and Libraries
- **TypeScript** (v5.3.2) - Strongly typed programming language that builds on JavaScript
- **Node.js Built-in Modules**:
  - `fs/promises` - File system operations (async)
  - `path` - Path manipulation utilities
- **Development Dependencies**:
  - **ESLint** - Code linting and style enforcement
  - **@typescript-eslint** - TypeScript-specific linting rules
  - **Jest** - Testing framework
  - **ts-jest** - TypeScript preprocessor for Jest
  - **ts-node** - TypeScript execution engine for Node.js

# TypeScript Language Features Demonstrated

## Required Features

### 1. Display Output to Terminal ✓
The application extensively uses `console.log()` to display:
- Directory tree structure with visual ASCII art
- Scan statistics and summaries
- File type distribution charts
- Error messages and warnings
- Top largest files listing

### 2. Recursion ✓
Recursion is implemented in multiple locations:
- **`DirectoryScanner.scanDirectoryAsync()`** - Recursively scans subdirectories
- **`FileNode.getTotalSize()`** - Recursively calculates total size of directory trees
- **`ScanResult.calculateStatistics()`** - Recursively counts files and directories
- **`TreeDisplay.displayTree()`** - Recursively displays the tree structure
- **`collectFiles()`** - Recursively collects all file nodes from the tree

### 3. Classes ✓
The project uses object-oriented programming with multiple classes:
- **`FileNode`** - Represents a file or directory node in the tree
- **`ScanResult`** - Encapsulates scan results and statistics
- **`DirectoryScanner`** - Main scanner class with configurable options
- **`TreeDisplay`** - Handles visual display of directory trees
- **`ScannerException`** - Custom exception class for error handling

### 4. Lists (Arrays) ✓
Arrays are used throughout the project:
- `FileNode.children: FileNode[]` - List of child nodes
- `ScanResult.errors: string[]` - List of error messages
- File lists for sorting and displaying largest files
- Extension maps for file type statistics

### 5. Asynchronous Functions ✓
Async/await is used extensively for file system operations:
- **`DirectoryScanner.scan()`** - Async scan operation
- **`scanDirectoryAsync()`** - Async recursive directory scanning
- **`readDirAsync()`** - Async directory reading
- **`getStatsAsync()`** - Async file statistics retrieval
- **`Promise.all()`** - Concurrent processing of directory entries

## Additional Requirements

### Exception Handling ✓
The project demonstrates comprehensive exception handling:
- **Custom Exception Class**: `ScannerException` extends `Error`
- **Try-Catch Blocks**: Used in `main()`, `scan()`, and `scanDirectoryAsync()`
- **Error Propagation**: Errors are collected and reported to users
- **Graceful Degradation**: Option to continue scanning despite errors (`ignoreErrors` flag)

### TypeScript Lint (ESLint) ✓
The project incorporates ESLint with TypeScript-specific rules:
- Configuration in `.eslintrc.json`
- TypeScript parser and plugin: `@typescript-eslint`
- Enforces type safety and code quality standards
- Run with: `npm run lint`

### Jest Testing Framework ✓
Unit tests are included using Jest:
- Test files: `FileNode.test.ts`, `DirectoryScanner.test.ts`
- Configuration in `jest.config.js`
- Tests cover core functionality including recursion, exception handling, and calculations
- Run with: `npm test`

# Project Structure

```
recursive-directory-scanner/
├── src/
│   ├── models/
│   │   ├── FileNode.ts          # File/directory node class
│   │   ├── FileNode.test.ts     # Unit tests for FileNode
│   │   └── ScanResult.ts        # Scan result container class
│   ├── scanner/
│   │   ├── DirectoryScanner.ts  # Main scanner implementation
│   │   └── DirectoryScanner.test.ts  # Unit tests for scanner
│   ├── display/
│   │   └── TreeDisplay.ts       # Tree visualization class
│   └── index.ts                 # Main entry point
├── dist/                        # Compiled JavaScript output
├── package.json                 # Project dependencies and scripts
├── tsconfig.json               # TypeScript compiler configuration
├── .eslintrc.json              # ESLint configuration
├── jest.config.js              # Jest testing configuration
└── README.md                   # This file
```

# Installation and Setup

## Prerequisites
- Node.js (version 18 or higher)
- npm (comes with Node.js)

## Installation Steps

1. **Clone or navigate to the project directory**:
   ```bash
   cd recursive-directory-scanner
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Build the project** (compile TypeScript to JavaScript):
   ```bash
   npm run build
   ```

# Usage

## Running the Application

### Development Mode (with ts-node)
```bash
npm run dev [directory] [options]
```

### Production Mode (compiled JavaScript)
```bash
npm start [directory] [options]
```

## Command Line Options

- **`[directory]`** - Path to scan (defaults to current directory)
- **`--max-depth=N`** - Limit scan depth to N levels
- **`--include-hidden`** - Include hidden files and directories
- **`--no-ignore-errors`** - Stop scanning on first error
- **`--stats-only`** - Show only statistics without tree display
- **`--help`** - Display help message

## Examples

### Scan current directory
```bash
npm run dev
```

### Scan specific directory with depth limit
```bash
npm run dev ./src --max-depth=2
```

### Scan including hidden files
```bash
npm run dev C:\Users\MyUser\Documents --include-hidden
```

### Show only statistics
```bash
npm run dev ./project --stats-only
```

## Running Tests
```bash
npm test
```

## Running Linter
```bash
npm run lint
```

## Cleaning Build Output
```bash
npm run clean
```

# Sample Output

```
🔍 Starting Directory Scan...

Target: C:\Users\MyUser\project

╔════════════════════════════════════════════════════════════╗
║                    SCAN SUMMARY                            ║
╠════════════════════════════════════════════════════════════╣
║ Root Directory: project                                    ║
║ Total Files: 42                                            ║
║ Total Directories: 8                                       ║
║ Total Size: 1.25 MB                                        ║
║ Scan Duration: 15.32 ms                                    ║
║ Errors Encountered: 0                                      ║
╚════════════════════════════════════════════════════════════╝

🌳 Directory Tree:

└── 📁 project (8 items)
    ├── 📁 src (15 items)
    │   ├── 📁 models (3 items)
    │   │   ├── 📄 FileNode.ts (2.45 KB)
    │   │   └── 📄 ScanResult.ts (1.89 KB)
    │   └── 📄 index.ts (3.21 KB)
    ├── 📄 package.json (1.12 KB)
    └── 📄 README.md (8.45 KB)

📊 File Type Statistics:
══════════════════════════════════════════════════
ts                      25 █████████████████████████
json                     3 ███
md                       1 █
```

# Key Learning Outcomes

Through this project, I learned:

1. **TypeScript Type System**: How to leverage TypeScript's static typing to catch errors at compile-time and improve code quality
2. **Asynchronous Programming**: Proper use of async/await and Promise.all() for concurrent file system operations
3. **Recursion Patterns**: Implementing recursive algorithms for tree traversal and data aggregation
4. **Error Handling**: Creating custom exception classes and implementing robust error handling strategies
5. **Testing**: Writing unit tests with Jest and ts-jest for TypeScript code
6. **Code Quality**: Using ESLint to enforce coding standards and best practices
7. **Node.js File System API**: Working with the fs/promises module for asynchronous file operations
8. **Project Structure**: Organizing a TypeScript project with proper separation of concerns

# Useful Websites

- [TypeScript Official Documentation](https://www.typescriptlang.org/docs/) - Comprehensive guide to TypeScript
- [Node.js File System Documentation](https://nodejs.org/api/fs.html) - Official Node.js fs module documentation
- [TypeScript Deep Dive](https://basarat.gitbook.io/typescript/) - In-depth TypeScript guide
- [Jest Documentation](https://jestjs.io/docs/getting-started) - Testing framework documentation
- [ESLint TypeScript](https://typescript-eslint.io/) - TypeScript ESLint plugin documentation

# Future Work

Potential enhancements for this project:

- [ ] Add support for file filtering by extension or pattern (glob patterns)
- [ ] Implement parallel directory scanning for improved performance
- [ ] Add export functionality (JSON, CSV, HTML reports)
- [ ] Create a web-based UI using React or Vue.js
- [ ] Add file content searching capabilities
- [ ] Implement directory comparison features
- [ ] Add support for remote directory scanning (FTP, SFTP)
- [ ] Create visualization charts using Chart.js or D3.js
- [ ] Add progress bars for large directory scans
- [ ] Implement caching mechanism for faster re-scans
