import { FileNode } from './FileNode';

describe('FileNode', () => {
  describe('constructor', () => {
    it('should create a file node with correct properties', () => {
      const node = new FileNode('test.txt', '/path/to/test.txt', false, 1024, 1);
      
      expect(node.name).toBe('test.txt');
      expect(node.path).toBe('/path/to/test.txt');
      expect(node.isDirectory).toBe(false);
      expect(node.size).toBe(1024);
      expect(node.depth).toBe(1);
      expect(node.children).toEqual([]);
    });

    it('should create a directory node with correct properties', () => {
      const node = new FileNode('testDir', '/path/to/testDir', true, 0, 0);
      
      expect(node.name).toBe('testDir');
      expect(node.isDirectory).toBe(true);
      expect(node.children).toEqual([]);
    });
  });

  describe('addChild', () => {
    it('should add a child to a directory node', () => {
      const parent = new FileNode('parent', '/parent', true);
      const child = new FileNode('child.txt', '/parent/child.txt', false, 100);
      
      parent.addChild(child);
      
      expect(parent.children.length).toBe(1);
      expect(parent.children[0]).toBe(child);
    });

    it('should throw error when adding child to a file node', () => {
      const fileNode = new FileNode('file.txt', '/file.txt', false, 100);
      const child = new FileNode('child.txt', '/child.txt', false, 50);
      
      expect(() => fileNode.addChild(child)).toThrow('Cannot add child to file node: file.txt');
    });
  });

  describe('getChildCount', () => {
    it('should return 0 for node with no children', () => {
      const node = new FileNode('empty', '/empty', true);
      expect(node.getChildCount()).toBe(0);
    });

    it('should return correct count for node with children', () => {
      const parent = new FileNode('parent', '/parent', true);
      parent.addChild(new FileNode('child1.txt', '/parent/child1.txt', false, 100));
      parent.addChild(new FileNode('child2.txt', '/parent/child2.txt', false, 200));
      
      expect(parent.getChildCount()).toBe(2);
    });
  });

  describe('getTotalSize', () => {
    it('should return file size for file node', () => {
      const file = new FileNode('file.txt', '/file.txt', false, 1024);
      expect(file.getTotalSize()).toBe(1024);
    });

    it('should return 0 for empty directory', () => {
      const dir = new FileNode('dir', '/dir', true);
      expect(dir.getTotalSize()).toBe(0);
    });

    it('should recursively calculate total size for directory with files', () => {
      const dir = new FileNode('dir', '/dir', true);
      dir.addChild(new FileNode('file1.txt', '/dir/file1.txt', false, 100));
      dir.addChild(new FileNode('file2.txt', '/dir/file2.txt', false, 200));
      
      expect(dir.getTotalSize()).toBe(300);
    });

    it('should recursively calculate total size for nested directories', () => {
      const root = new FileNode('root', '/root', true);
      const subDir = new FileNode('subdir', '/root/subdir', true);
      
      subDir.addChild(new FileNode('file1.txt', '/root/subdir/file1.txt', false, 100));
      root.addChild(subDir);
      root.addChild(new FileNode('file2.txt', '/root/file2.txt', false, 200));
      
      expect(root.getTotalSize()).toBe(300);
    });
  });

  describe('formatSize', () => {
    it('should format bytes correctly', () => {
      expect(FileNode.formatSize(500)).toBe('500.00 B');
    });

    it('should format kilobytes correctly', () => {
      expect(FileNode.formatSize(1024)).toBe('1.00 KB');
      expect(FileNode.formatSize(1536)).toBe('1.50 KB');
    });

    it('should format megabytes correctly', () => {
      expect(FileNode.formatSize(1048576)).toBe('1.00 MB');
      expect(FileNode.formatSize(5242880)).toBe('5.00 MB');
    });

    it('should format gigabytes correctly', () => {
      expect(FileNode.formatSize(1073741824)).toBe('1.00 GB');
    });
  });
});
