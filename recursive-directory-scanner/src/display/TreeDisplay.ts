import { FileNode } from '../models/FileNode';

/**
 * Handles the visual display of the directory tree structure
 * Uses various tree drawing characters to create an ASCII tree
 */
export class TreeDisplay {
  private output: string[];

  constructor() {
    this.output = [];
  }

  /**
   * Displays a FileNode tree in a visual tree format
   * @param node - The root node to display
   * @param prefix - The prefix string for tree drawing (used in recursion)
   * @param isLast - Whether this is the last child in its parent's list
   */
  public displayTree(
    node: FileNode,
    prefix: string = '',
    isLast: boolean = true
  ): void {
    // Determine the tree drawing characters
    const connector = isLast ? '└── ' : '├── ';
    const extension = isLast ? '    ' : '│   ';

    // Format the node display
    const icon = node.isDirectory ? '📁' : '📄';
    const sizeInfo = node.isDirectory
      ? `(${node.getChildCount()} items)`
      : `(${FileNode.formatSize(node.size)})`;

    // Add the current node to output
    const line = `${prefix}${connector}${icon} ${node.name} ${sizeInfo}`;
    this.output.push(line);
    console.log(line);

    // Recursively display children
    if (node.isDirectory && node.children.length > 0) {
      // Sort children: directories first, then files, alphabetically
      const sortedChildren = this.sortChildren(node.children);

      sortedChildren.forEach((child, index) => {
        const isLastChild = index === sortedChildren.length - 1;
        const newPrefix = prefix + extension;
        
        // Recursion: display each child
        this.displayTree(child, newPrefix, isLastChild);
      });
    }
  }

  /**
   * Sorts children nodes: directories first, then files, alphabetically
   * @param children - Array of FileNode children to sort
   * @returns Sorted array of FileNode
   */
  private sortChildren(children: FileNode[]): FileNode[] {
    return [...children].sort((a, b) => {
      // Directories come before files
      if (a.isDirectory && !b.isDirectory) return -1;
      if (!a.isDirectory && b.isDirectory) return 1;
      
      // Within same type, sort alphabetically (case-insensitive)
      return a.name.toLowerCase().localeCompare(b.name.toLowerCase());
    });
  }

  /**
   * Displays a flat list of all files in the tree
   * @param node - The root node
   * @param fileList - Accumulator for the list of files
   */
  public displayFlatList(node: FileNode, fileList: FileNode[] = []): FileNode[] {
    if (!node.isDirectory) {
      fileList.push(node);
    } else {
      // Recursion: process all children
      node.children.forEach(child => {
        this.displayFlatList(child, fileList);
      });
    }
    return fileList;
  }

  /**
   * Displays statistics about file types in the tree
   * @param node - The root node to analyze
   */
  public displayFileTypeStats(node: FileNode): void {
    const extensions = new Map<string, number>();
    
    // Recursively collect file extensions
    this.collectExtensions(node, extensions);

    console.log('\n📊 File Type Statistics:');
    console.log('═'.repeat(50));

    if (extensions.size === 0) {
      console.log('No files found.');
      return;
    }

    // Sort by count (descending)
    const sorted = Array.from(extensions.entries())
      .sort((a, b) => b[1] - a[1]);

    sorted.forEach(([ext, count]) => {
      const displayExt = ext || '(no extension)';
      const bar = '█'.repeat(Math.min(count, 50));
      console.log(`${displayExt.padEnd(20)} ${String(count).padStart(5)} ${bar}`);
    });
  }

  /**
   * Recursively collects file extensions and their counts
   * @param node - The current node being processed
   * @param extensions - Map to store extension counts
   */
  private collectExtensions(
    node: FileNode,
    extensions: Map<string, number>
  ): void {
    if (!node.isDirectory) {
      const ext = node.name.includes('.')
        ? node.name.split('.').pop() || ''
        : '';
      
      extensions.set(ext, (extensions.get(ext) || 0) + 1);
    } else {
      // Recursion: process all children
      node.children.forEach(child => {
        this.collectExtensions(child, extensions);
      });
    }
  }

  /**
   * Gets the accumulated output as a string array
   * @returns Array of output lines
   */
  public getOutput(): string[] {
    return [...this.output];
  }

  /**
   * Clears the accumulated output
   */
  public clear(): void {
    this.output = [];
  }
}
