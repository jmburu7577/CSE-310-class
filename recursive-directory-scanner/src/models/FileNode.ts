/**
 * Represents a file or directory node in the file system tree
 * This class stores information about a single file or directory
 */
export class FileNode {
  public name: string;
  public path: string;
  public isDirectory: boolean;
  public size: number;
  public children: FileNode[];
  public depth: number;

  /**
   * Creates a new FileNode instance
   * @param name - The name of the file or directory
   * @param path - The full path to the file or directory
   * @param isDirectory - Whether this node represents a directory
   * @param size - The size of the file in bytes (0 for directories)
   * @param depth - The depth level in the directory tree
   */
  constructor(
    name: string,
    path: string,
    isDirectory: boolean,
    size: number = 0,
    depth: number = 0
  ) {
    this.name = name;
    this.path = path;
    this.isDirectory = isDirectory;
    this.size = size;
    this.children = [];
    this.depth = depth;
  }

  /**
   * Adds a child node to this directory node
   * @param child - The child FileNode to add
   * @throws Error if attempting to add a child to a file node
   */
  public addChild(child: FileNode): void {
    if (!this.isDirectory) {
      throw new Error(`Cannot add child to file node: ${this.name}`);
    }
    this.children.push(child);
  }

  /**
   * Gets the total number of children (files and directories)
   * @returns The count of direct children
   */
  public getChildCount(): number {
    return this.children.length;
  }

  /**
   * Recursively calculates the total size of this node and all its children
   * @returns The total size in bytes
   */
  public getTotalSize(): number {
    if (!this.isDirectory) {
      return this.size;
    }
    
    // Recursion: sum up sizes of all children
    return this.children.reduce((total, child) => {
      return total + child.getTotalSize();
    }, 0);
  }

  /**
   * Formats the file size in a human-readable format
   * @param bytes - The size in bytes
   * @returns Formatted string (e.g., "1.5 KB", "2.3 MB")
   */
  public static formatSize(bytes: number): string {
    const units = ['B', 'KB', 'MB', 'GB', 'TB'];
    let size = bytes;
    let unitIndex = 0;

    while (size >= 1024 && unitIndex < units.length - 1) {
      size /= 1024;
      unitIndex++;
    }

    return `${size.toFixed(2)} ${units[unitIndex]}`;
  }
}
