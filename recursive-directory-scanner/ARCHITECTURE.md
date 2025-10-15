# Project Architecture

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                         User Input                          │
│                    (Command Line Args)                      │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                      index.ts (Main)                        │
│  • Parses arguments                                         │
│  • Creates DirectoryScanner instance                        │
│  • Handles exceptions                                       │
│  • Displays results                                         │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                   DirectoryScanner                          │
│  • scan() - Main async entry point                          │
│  • scanDirectoryAsync() - Recursive scanning                │
│  • Error handling and collection                            │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                      File System                            │
│  • fs.promises.readdir() - Read directory                   │
│  • fs.promises.stat() - Get file stats                      │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Models                              │
│  ┌──────────────┐  ┌──────────────┐                        │
│  │  FileNode    │  │  ScanResult  │                        │
│  │  • name      │  │  • rootNode  │                        │
│  │  • path      │  │  • stats     │                        │
│  │  • children  │  │  • errors    │                        │
│  └──────────────┘  └──────────────┘                        │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    TreeDisplay                              │
│  • displayTree() - Visual tree rendering                    │
│  • displayFileTypeStats() - Statistics                      │
│  • Formatting and sorting                                   │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    Console Output                           │
│  • Tree structure                                           │
│  • Statistics                                               │
│  • Error messages                                           │
└─────────────────────────────────────────────────────────────┘
```

## 📦 Module Dependencies

```
index.ts
  ├─→ DirectoryScanner
  │     ├─→ FileNode
  │     ├─→ ScanResult
  │     └─→ fs/promises (Node.js)
  │
  ├─→ TreeDisplay
  │     └─→ FileNode
  │
  └─→ FileNode
```

## 🔄 Recursion Flow

### Directory Scanning Recursion

```
scanDirectoryAsync(path, depth=0)
  │
  ├─→ Read directory contents
  │
  ├─→ For each entry:
  │     │
  │     ├─→ If directory:
  │     │     └─→ scanDirectoryAsync(entry, depth+1) ← RECURSION
  │     │
  │     └─→ If file:
  │           └─→ Create FileNode
  │
  └─→ Return FileNode with all children
```

### Size Calculation Recursion

```
getTotalSize(node)
  │
  ├─→ If file:
  │     └─→ Return node.size
  │
  └─→ If directory:
        └─→ Sum of getTotalSize(child) for all children ← RECURSION
```

## 🎯 Class Relationships

```
┌──────────────────┐
│    FileNode      │
├──────────────────┤
│ - name: string   │
│ - path: string   │
│ - isDirectory    │
│ - size: number   │
│ - children: []   │
│ - depth: number  │
├──────────────────┤
│ + addChild()     │
│ + getTotalSize() │ ← Uses recursion
│ + getChildCount()│
└────────┬─────────┘
         │
         │ contains
         │
         ▼
┌──────────────────┐
│   ScanResult     │
├──────────────────┤
│ - rootNode       │ ← FileNode
│ - totalFiles     │
│ - totalDirs      │
│ - totalSize      │
│ - errors: []     │
├──────────────────┤
│ + getSummary()   │
│ + addError()     │
└──────────────────┘

┌──────────────────────┐
│  DirectoryScanner    │
├──────────────────────┤
│ - options            │
│ - errors: []         │
├──────────────────────┤
│ + scan()             │ ← Async
│ - scanDirectoryAsync()│ ← Async + Recursion
│ - readDirAsync()     │ ← Async
│ - getStatsAsync()    │ ← Async
└──────────────────────┘
         │
         │ creates
         │
         ▼
┌──────────────────┐
│   ScanResult     │
└──────────────────┘

┌──────────────────┐
│   TreeDisplay    │
├──────────────────┤
│ - output: []     │
├──────────────────┤
│ + displayTree()  │ ← Uses recursion
│ + displayStats() │
└──────────────────┘
```

## 🔀 Data Flow

### 1. Initialization Phase
```
User → Command Line Args → parseArguments() → Config Object
```

### 2. Scanning Phase
```
Config → DirectoryScanner.scan()
  ↓
  Validate path
  ↓
  scanDirectoryAsync() [RECURSIVE]
  ↓
  Create FileNode tree
  ↓
  Return ScanResult
```

### 3. Display Phase
```
ScanResult → Display Summary
  ↓
  TreeDisplay.displayTree() [RECURSIVE]
  ↓
  Display Statistics
  ↓
  Display Largest Files
```

## 🧪 Testing Structure

```
FileNode.test.ts
  ├─→ Tests FileNode class
  ├─→ Tests recursion (getTotalSize)
  └─→ Tests error handling (addChild)

DirectoryScanner.test.ts
  ├─→ Tests ScannerException
  ├─→ Tests async scanning
  └─→ Tests error handling
```

## 🎨 Design Patterns Used

### 1. **Builder Pattern**
- `DirectoryScanner` accepts options object for configuration

### 2. **Composite Pattern**
- `FileNode` can contain other `FileNode` objects (tree structure)

### 3. **Strategy Pattern**
- Different display strategies (tree, flat list, statistics)

### 4. **Error Handling Pattern**
- Custom exception class (`ScannerException`)
- Centralized error collection

## 🔑 Key TypeScript Features

### Type Safety
```typescript
interface ScanOptions {
  maxDepth?: number;
  includeHidden?: boolean;
  followSymlinks?: boolean;
  ignoreErrors?: boolean;
}
```

### Async/Await
```typescript
async scan(targetPath: string): Promise<ScanResult>
```

### Generics (in arrays)
```typescript
children: FileNode[]
errors: string[]
```

### Access Modifiers
```typescript
public scan()
private scanDirectoryAsync()
```

## 📊 Performance Considerations

### Concurrent Processing
- Uses `Promise.all()` to scan directory entries concurrently
- Improves performance for directories with many items

### Depth Limiting
- `maxDepth` option prevents infinite recursion
- Reduces memory usage for deep directory trees

### Error Recovery
- `ignoreErrors` option allows continuing despite permission errors
- Collects errors without stopping the scan

## 🔒 Error Handling Strategy

```
Try-Catch Hierarchy:
  ├─→ main() - Top level error handler
  ├─→ scan() - Validates path, catches scanner errors
  └─→ scanDirectoryAsync() - Handles individual file/dir errors
```

## 🚀 Execution Flow Example

```
1. User runs: npm run dev ./src --max-depth=2

2. index.ts main() executes
   ├─→ Parse args: { targetPath: './src', maxDepth: 2 }
   └─→ Create DirectoryScanner({ maxDepth: 2 })

3. scanner.scan('./src')
   ├─→ Validate './src' exists and is directory
   └─→ scanDirectoryAsync('./src', 0)
       ├─→ Read directory entries
       ├─→ For each entry (concurrent):
       │   ├─→ If dir: scanDirectoryAsync(entry, 1)
       │   │   └─→ For each entry:
       │   │       └─→ If dir: scanDirectoryAsync(entry, 2)
       │   │           └─→ depth=2, stop (maxDepth reached)
       │   └─→ If file: Create FileNode
       └─→ Return root FileNode

4. Create ScanResult
   └─→ Calculate statistics (recursive)

5. Display results
   ├─→ Print summary
   ├─→ TreeDisplay.displayTree() (recursive)
   └─→ Print statistics
```

This architecture demonstrates professional TypeScript development with proper separation of concerns, error handling, and efficient async operations.
