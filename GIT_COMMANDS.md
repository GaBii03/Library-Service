# Git Setup Commands

Run these commands in your terminal, one at a time:

## Step 1: Navigate to your project folder
```bash
cd "/Users/gabrielalyavova/Development/C# Projects/myProject/LibraryService"
```

## Step 2: Initialize Git
```bash
git init
```

## Step 3: Add your remote repository
```bash
git remote add origin https://github.com/GaBii03/Library-Service.git
```

## Step 4: Stage all files
```bash
git add .
```

## Step 5: Make your first commit
```bash
git commit -m "Initial commit: LibraryService project structure"
```

## Step 6: Set main branch and push
```bash
git branch -M main
git push -u origin main
```

## All commands in one block (copy all at once):
```bash
cd "/Users/gabrielalyavova/Development/C# Projects/myProject/LibraryService"
git init
git remote add origin https://github.com/GaBii03/Library-Service.git
git add .
git commit -m "Initial commit: LibraryService project structure"
git branch -M main
git push -u origin main
```

## If you get authentication errors:
You may need to:
- Use a Personal Access Token instead of password
- Set up SSH keys
- Or use GitHub CLI: `gh auth login`
