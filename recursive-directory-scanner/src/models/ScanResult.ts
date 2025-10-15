import { FileNode } from './FileNode';

/**
 * Represents the result of a directory scan operation
 * Contains statistics and the root node of the scanned directory tree
 */
export class ScanResult {
  public rootNode: FileNode;
  public totalFiles: number;
  public totalDirectories: number;
  public totalSize: number;
  public scanDuration: number; // in milliseconds
  public errors: string[];

  /**
   * Creates a new ScanResult instance
   * @param rootNode - The root FileNode of the scanned directory
   * @param scanDuration - Time taken to complete the scan in milliseconds
   */
  constructor(rootNode: FileNode, scanDuration: number = 0) {
    this.rootNode = rootNode;
    this.totalFiles = 0;
    this.totalDirectories = 0;
    this.totalSize = 0;
    this.scanDuration = scanDuration;
    this.errors = [];
    this.calculateStatistics(rootNode);
  }

  /**
   * Recursively calculates statistics for the scanned directory tree
   * @param node - The current node being processed
   */
  private calculateStatistics(node: FileNode): void {
    if (node.isDirectory) {
      this.totalDirectories++;
      // Recursion: process all children
      for (const child of node.children) {
        this.calculateStatistics(child);
      }
    } else {
      this.totalFiles++;
      this.totalSize += node.size;
    }
  }

  /**
   * Adds an error message to the error list
   * @param error - The error message to add
   */
  public addError(error: string): void {
    this.errors.push(error);
  }

  /**
   * Gets a summary of the scan results
   * @returns A formatted string with scan statistics
   */
  public getSummary(): string {
    return `
╔════════════════════════════════════════════════════════════╗
║                    SCAN SUMMARY                            ║
╠════════════════════════════════════════════════════════════╣
║ Root Directory: ${this.rootNode.name.padEnd(40)} ║
║ Total Files: ${String(this.totalFiles).padEnd(43)} ║
║ Total Directories: ${String(this.totalDirectories).padEnd(37)} ║
║ Total Size: ${FileNode.formatSize(this.totalSize).padEnd(44)} ║
║ Scan Duration: ${this.scanDuration.toFixed(2).padEnd(41)} ms ║
║ Errors Encountered: ${String(this.errors.length).padEnd(36)} ║
╚════════════════════════════════════════════════════════════╝
    `.trim();
  }
}
