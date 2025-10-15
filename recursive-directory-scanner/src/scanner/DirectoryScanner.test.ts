import { DirectoryScanner, ScannerException } from './DirectoryScanner';
import * as path from 'path';

describe('DirectoryScanner', () => {
  describe('ScannerException', () => {
    it('should create exception with message', () => {
      const exception = new ScannerException('Test error');
      
      expect(exception.message).toBe('Test error');
      expect(exception.name).toBe('ScannerException');
      expect(exception.originalError).toBeUndefined();
    });

    it('should create exception with original error', () => {
      const originalError = new Error('Original');
      const exception = new ScannerException('Test error', originalError);
      
      expect(exception.message).toBe('Test error');
      expect(exception.originalError).toBe(originalError);
    });
  });

  describe('DirectoryScanner', () => {
    it('should create scanner with default options', () => {
      const scanner = new DirectoryScanner();
      expect(scanner).toBeInstanceOf(DirectoryScanner);
    });

    it('should create scanner with custom options', () => {
      const scanner = new DirectoryScanner({
        maxDepth: 2,
        includeHidden: true,
        ignoreErrors: false,
      });
      expect(scanner).toBeInstanceOf(DirectoryScanner);
    });

    it('should throw exception for non-existent path', async () => {
      const scanner = new DirectoryScanner();
      const nonExistentPath = path.join(__dirname, 'non-existent-directory-12345');
      
      await expect(scanner.scan(nonExistentPath)).rejects.toThrow(ScannerException);
    });

    it('should throw exception for file path instead of directory', async () => {
      const scanner = new DirectoryScanner();
      // Use this test file as the target
      const filePath = __filename;
      
      await expect(scanner.scan(filePath)).rejects.toThrow('Path is not a directory');
    });

    it('should successfully scan current directory', async () => {
      const scanner = new DirectoryScanner({ maxDepth: 1 });
      const currentDir = __dirname;
      
      const result = await scanner.scan(currentDir);
      
      expect(result).toBeDefined();
      expect(result.rootNode).toBeDefined();
      expect(result.rootNode.isDirectory).toBe(true);
      expect(result.scanDuration).toBeGreaterThanOrEqual(0);
    });

    it('should respect maxDepth option', async () => {
      const scanner = new DirectoryScanner({ maxDepth: 0 });
      const currentDir = __dirname;
      
      const result = await scanner.scan(currentDir);
      
      expect(result.rootNode.children.length).toBe(0);
    });
  });
});
