import * as fs from 'fs';
import * as path from 'path';
import { FileNode } from '../models/FileNode';
import { ScanResult } from '../models/ScanResult';

/**
 * Custom exception for directory scanning errors
 */
export class ScannerException extends Error {
  constructor(message: string, public originalError?: Error) {
    super(message);
    this.name = 'ScannerException';
    Object.setPrototypeOf(this, ScannerException.prototype);
  }
}

/**
 * Options for configuring the directory scanner
 */
export interface ScanOptions {
  maxDepth?: number;
  includeHidden?: boolean;
  followSymlinks?: boolean;
  ignoreErrors?: boolean;
}

/**
 * Main class for scanning directories recursively
 * Demonstrates async operations, recursion, and exception handling
 */
export class DirectoryScanner {
  private options: ScanOptions;
  private errors: string[];

  /**
   * Creates a new DirectoryScanner instance
   * @param options - Configuration options for the scanner
   */
  constructor(options: ScanOptions = {}) {
    this.options = {
      maxDepth: options.maxDepth ?? Infinity,
      includeHidden: options.includeHidden ?? false,
      followSymlinks: options.followSymlinks ?? false,
      ignoreErrors: options.ignoreErrors ?? true,
    };
    this.errors = [];
  }

  /**
   * Asynchronously scans a directory and returns the results
   * @param targetPath - The path to the directory to scan
   * @returns A Promise that resolves to a ScanResult
   * @throws ScannerException if the path doesn't exist or is not a directory
   */
  public async scan(targetPath: string): Promise<ScanResult> {
    const startTime = Date.now();
    this.errors = [];

    try {
      // Validate the target path
      const stats = await this.getStatsAsync(targetPath);
      
      if (!stats.isDirectory()) {
        throw new ScannerException(`Path is not a directory: ${targetPath}`);
      }

      // Start recursive scanning
      const rootNode = await this.scanDirectoryAsync(targetPath, 0);
      const duration = Date.now() - startTime;
      
      const result = new ScanResult(rootNode, duration);
      
      // Add any errors encountered during scanning
      this.errors.forEach(error => result.addError(error));
      
      return result;
    } catch (error) {
      if (error instanceof ScannerException) {
        throw error;
      }
      throw new ScannerException(
        `Failed to scan directory: ${targetPath}`,
        error as Error
      );
    }
  }

  /**
   * Recursively scans a directory and builds a FileNode tree
   * @param dirPath - The path to the directory to scan
   * @param depth - The current depth in the directory tree
   * @returns A Promise that resolves to a FileNode representing the directory
   */
  private async scanDirectoryAsync(
    dirPath: string,
    depth: number
  ): Promise<FileNode> {
    const dirName = path.basename(dirPath);
    const node = new FileNode(dirName, dirPath, true, 0, depth);

    // Check if we've reached the maximum depth
    if (depth >= this.options.maxDepth!) {
      return node;
    }

    try {
      // Read directory contents asynchronously
      const entries = await this.readDirAsync(dirPath);

      // Process all entries concurrently using Promise.all
      const childPromises = entries.map(async (entry) => {
        const entryPath = path.join(dirPath, entry);

        try {
          // Skip hidden files if configured
          if (!this.options.includeHidden && entry.startsWith('.')) {
            return null;
          }

          const stats = await this.getStatsAsync(entryPath);

          if (stats.isDirectory()) {
            // Recursion: scan subdirectory
            return await this.scanDirectoryAsync(entryPath, depth + 1);
          } else if (stats.isFile()) {
            return new FileNode(entry, entryPath, false, stats.size, depth + 1);
          }

          return null;
        } catch (error) {
          const errorMsg = `Error processing ${entryPath}: ${(error as Error).message}`;
          this.errors.push(errorMsg);
          
          if (!this.options.ignoreErrors) {
            throw new ScannerException(errorMsg, error as Error);
          }
          
          return null;
        }
      });

      // Wait for all child operations to complete
      const children = await Promise.all(childPromises);

      // Add non-null children to the node
      children.forEach((child) => {
        if (child !== null) {
          node.addChild(child);
        }
      });
    } catch (error) {
      const errorMsg = `Error reading directory ${dirPath}: ${(error as Error).message}`;
      this.errors.push(errorMsg);
      
      if (!this.options.ignoreErrors) {
        throw new ScannerException(errorMsg, error as Error);
      }
    }

    return node;
  }

  /**
   * Asynchronous wrapper for fs.promises.readdir
   * @param dirPath - The directory path to read
   * @returns A Promise that resolves to an array of file/directory names
   */
  private async readDirAsync(dirPath: string): Promise<string[]> {
    return await fs.promises.readdir(dirPath);
  }

  /**
   * Asynchronous wrapper for fs.promises.stat
   * @param filePath - The file path to get stats for
   * @returns A Promise that resolves to fs.Stats
   */
  private async getStatsAsync(filePath: string): Promise<fs.Stats> {
    if (this.options.followSymlinks) {
      return await fs.promises.stat(filePath);
    } else {
      return await fs.promises.lstat(filePath);
    }
  }

  /**
   * Gets the list of errors encountered during the last scan
   * @returns An array of error messages
   */
  public getErrors(): string[] {
    return [...this.errors];
  }
}
