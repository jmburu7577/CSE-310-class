# Windows Setup Instructions

## PowerShell Execution Policy Issue

If you encounter an error like:
```
npm : File C:\Program Files\nodejs\npm.ps1 cannot be loaded because running scripts is disabled on this system.
```

This is due to PowerShell's execution policy. Here are the solutions:

### Solution 1: Use Command Prompt (Recommended for Quick Start)

Instead of PowerShell, use **Command Prompt (cmd.exe)**:

1. Press `Win + R`
2. Type `cmd` and press Enter
3. Navigate to the project directory:
   ```cmd
   cd C:\Users\BYU\Desktop\CSE-310-class-1\recursive-directory-scanner
   ```
4. Run npm commands normally:
   ```cmd
   npm install
   npm run build
   npm run dev
   ```

### Solution 2: Change PowerShell Execution Policy (Requires Admin)

1. Open PowerShell as Administrator (Right-click PowerShell → "Run as Administrator")
2. Run this command:
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```
3. Type `Y` to confirm
4. Close and reopen PowerShell (normal mode is fine now)
5. Navigate to project and run npm commands

### Solution 3: Bypass Policy for Single Session

In your current PowerShell window, run:
```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

This only affects the current PowerShell session.

### Solution 4: Use Git Bash or WSL

If you have Git for Windows or Windows Subsystem for Linux (WSL) installed:
- Use Git Bash terminal
- Or use WSL Ubuntu/Debian terminal

Both work like Linux terminals and don't have execution policy restrictions.

## Installation Steps (After Fixing Execution Policy)

1. **Install Dependencies**
   ```bash
   npm install
   ```

2. **Build the Project**
   ```bash
   npm run build
   ```

3. **Run the Application**
   ```bash
   # Development mode (recommended for testing)
   npm run dev

   # Or scan a specific directory
   npm run dev ./src

   # Or with options
   npm run dev . --max-depth=2 --include-hidden
   ```

4. **Run Tests**
   ```bash
   npm test
   ```

5. **Run Linter**
   ```bash
   npm run lint
   ```

## Verifying Installation

After running `npm install`, you should see:
- A `node_modules/` folder created
- A `package-lock.json` file created
- No error messages

After running `npm run build`, you should see:
- A `dist/` folder created with compiled JavaScript files

## Common Windows-Specific Issues

### Issue: Path with Spaces
If your path has spaces, wrap it in quotes:
```bash
npm run dev "C:\Users\My User\Documents\My Project"
```

### Issue: Backslashes in Paths
Windows uses backslashes (`\`), but you can also use forward slashes (`/`):
```bash
npm run dev C:/Users/BYU/Desktop/project
```

### Issue: Permission Denied
If scanning system directories (like `C:\Windows`), run your terminal as Administrator.

## Testing Without Installation

If you can't install dependencies right now, you can still review:
- The TypeScript source code in `src/`
- The README.md for documentation
- The configuration files (tsconfig.json, .eslintrc.json, jest.config.js)

## Next Steps

Once you have npm working:
1. Follow the installation steps above
2. Try running the scanner on the project itself: `npm run dev .`
3. Experiment with different directories and options
4. Run the tests to see Jest in action
5. Try the linter to see ESLint checking the code
