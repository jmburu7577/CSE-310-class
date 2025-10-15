import * as path from 'path';
import { DirectoryScanner, ScannerException } from './scanner/DirectoryScanner';
import { TreeDisplay } from './display/TreeDisplay';
import { FileNode } from './models/FileNode';

/**
 * Main entry point for the Recursive Directory Scanner application
 * Demonstrates TypeScript features including:
 * - Classes (FileNode, ScanResult, DirectoryScanner, TreeDisplay)
 * - Recursion (in scanning and display methods)
 * - Async/await operations (file system operations)
 * - Exception handling (try-catch blocks)
 * - Lists/Arrays (children nodes, file lists)
 */

/**
 * Displays usage information
 */
function displayUsage(): void {
  console.log(`
╔════════════════════════════════════════════════════════════╗
║        Recursive Directory Scanner - TypeScript            ║
╠════════════════════════════════════════════════════════════╣
║ Usage:                                                     ║
║   npm run dev [directory] [options]                        ║
║   npm start [directory] [options]                          ║
║                                                            ║
║ Arguments:                                                 ║
║   directory    Path to scan (default: current directory)  ║
║                                                            ║
║ Options:                                                   ║
║   --max-depth=N       Maximum depth to scan (default: ∞)  ║
║   --include-hidden    Include hidden files/directories    ║
║   --no-ignore-errors  Stop on first error                 ║
║   --stats-only        Show only statistics, not tree      ║
║   --help              Display this help message           ║
║                                                            ║
║ Examples:                                                  ║
║   npm run dev                                              ║
║   npm run dev ./src --max-depth=2                          ║
║   npm run dev C:\\Users --include-hidden                   ║
╚════════════════════════════════════════════════════════════╝
  `);
}

/**
 * Parses command line arguments
 * @param args - Command line arguments
 * @returns Parsed configuration object
 */
function parseArguments(args: string[]): {
  targetPath: string;
  maxDepth?: number;
  includeHidden: boolean;
  ignoreErrors: boolean;
  statsOnly: boolean;
  showHelp: boolean;
} {
  const config = {
    targetPath: process.cwd(),
    maxDepth: undefined as number | undefined,
    includeHidden: false,
    ignoreErrors: true,
    statsOnly: false,
    showHelp: false,
  };

  for (const arg of args) {
    if (arg === '--help' || arg === '-h') {
      config.showHelp = true;
    } else if (arg === '--include-hidden') {
      config.includeHidden = true;
    } else if (arg === '--no-ignore-errors') {
      config.ignoreErrors = false;
    } else if (arg === '--stats-only') {
      config.statsOnly = true;
    } else if (arg.startsWith('--max-depth=')) {
      const depth = parseInt(arg.split('=')[1], 10);
      if (!isNaN(depth) && depth > 0) {
        config.maxDepth = depth;
      }
    } else if (!arg.startsWith('--')) {
      config.targetPath = path.resolve(arg);
    }
  }

  return config;
}

/**
 * Main application function
 * Demonstrates async/await and exception handling
 */
async function main(): Promise<void> {
  try {
    // Parse command line arguments
    const args = process.argv.slice(2);
    const config = parseArguments(args);

    if (config.showHelp) {
      displayUsage();
      return;
    }

    console.log('\n🔍 Starting Directory Scan...\n');
    console.log(`Target: ${config.targetPath}\n`);

    // Create scanner with options
    const scanner = new DirectoryScanner({
      maxDepth: config.maxDepth,
      includeHidden: config.includeHidden,
      ignoreErrors: config.ignoreErrors,
    });

    // Perform async scan operation
    const result = await scanner.scan(config.targetPath);

    // Display results
    console.log(result.getSummary());

    // Display errors if any
    if (result.errors.length > 0) {
      console.log('\n⚠️  Errors encountered during scan:');
      result.errors.forEach((error, index) => {
        console.log(`  ${index + 1}. ${error}`);
      });
    }

    // Display tree structure unless stats-only mode
    if (!config.statsOnly) {
      console.log('\n🌳 Directory Tree:\n');
      const display = new TreeDisplay();
      display.displayTree(result.rootNode);

      // Display file type statistics
      console.log('');
      display.displayFileTypeStats(result.rootNode);
    }

    // Display largest files
    console.log('\n📦 Top 10 Largest Files:');
    console.log('═'.repeat(70));
    displayLargestFiles(result.rootNode, 10);

  } catch (error) {
    // Exception handling
    if (error instanceof ScannerException) {
      console.error(`\n❌ Scanner Error: ${error.message}`);
      if (error.originalError) {
        console.error(`   Original Error: ${error.originalError.message}`);
      }
    } else {
      console.error(`\n❌ Unexpected Error: ${(error as Error).message}`);
    }
    process.exit(1);
  }
}

/**
 * Displays the largest files in the scanned directory
 * Demonstrates recursion and list operations
 * @param node - The root node to search
 * @param count - Number of files to display
 */
function displayLargestFiles(node: FileNode, count: number): void {
  const files: FileNode[] = [];
  
  // Recursively collect all files
  collectFiles(node, files);

  // Sort by size (descending) and take top N
  const largest = files
    .sort((a, b) => b.size - a.size)
    .slice(0, count);

  if (largest.length === 0) {
    console.log('No files found.');
    return;
  }

  largest.forEach((file, index) => {
    const size = FileNode.formatSize(file.size);
    const relativePath = file.path;
    console.log(`${String(index + 1).padStart(2)}. ${size.padEnd(12)} ${relativePath}`);
  });
}

/**
 * Recursively collects all file nodes from a tree
 * @param node - The current node being processed
 * @param files - Accumulator array for files
 */
function collectFiles(node: FileNode, files: FileNode[]): void {
  if (!node.isDirectory) {
    files.push(node);
  } else {
    // Recursion: process all children
    for (const child of node.children) {
      collectFiles(child, files);
    }
  }
}

// Run the main function
main().catch((error) => {
  console.error('Fatal error:', error);
  process.exit(1);
});
