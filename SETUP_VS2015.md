# Setup Guide for Visual Studio 2015

## Prerequisites

### 1. Visual Studio 2015
- Download from: https://visualstudio.microsoft.com/vs/older-downloads/
- Select: Visual Studio Community 2015 or Professional 2015
- Install with these workloads:
  - ✓ Desktop Development with C++
  - ✓ .NET Framework 4.5.2 SDK
  - ✓ Windows development tools

### 2. SQL Server LocalDB 2015 or Later
- Download: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- Choose: SQL Server LocalDB (Express or Developer Edition)
- Installation: Accept default options
- Verify installation: Open Command Prompt and run:
  ```bash
  sqllocaldb info
  ```
  You should see: `MSSQLLocalDB`

### 3. .NET Framework 4.7.2 or Higher
- Download: https://dotnet.microsoft.com/download/dotnet-framework
- Windows Update should have installed this automatically
- Verify: Control Panel > Programs > Programs and Features > Look for ".NET Framework 4.7.2"

---

## Step 1: Clone the Repository

### Option A: Using Git
1. Open Command Prompt
2. Navigate to your projects folder:
   ```bash
   cd C:\Projects
   ```
3. Clone the repository:
   ```bash
   git clone https://github.com/lalasharma/cloth-store-desktop.git
   cd cloth-store-desktop
   ```

### Option B: Download ZIP
1. Go to: https://github.com/lalasharma/cloth-store-desktop
2. Click **Code** > **Download ZIP**
3. Extract to `C:\Projects\cloth-store-desktop`

---

## Step 2: Open Project in Visual Studio 2015

1. Launch **Visual Studio 2015**
2. Go to **File** > **Open** > **Project/Solution**
3. Navigate to: `C:\Projects\cloth-store-desktop`
4. Select: **ClothStoreDesktop.sln**
5. Click **Open**

Visual Studio will load the solution. Wait for it to finish loading (check bottom-left for "Ready").

---

## Step 3: Adjust Project for VS 2015

### Step 3.1: Update Target Framework
1. In **Solution Explorer**, right-click **ClothStoreDesktop** project
2. Select **Properties**
3. Go to **Compile** tab
4. Change **Target framework** from `.NET 8.0` to **.NET Framework 4.7.2** or **4.5.2**
5. Click **Save**

### Step 3.2: Update Project File
1. Right-click **ClothStoreDesktop.vbproj** in Solution Explorer
2. Select **Edit Project File** (or open with Notepad)
3. Find this section:
   ```xml
   <TargetFramework>net8.0-windows</TargetFramework>
   <UseWindowsForms>true</UseWindowsForms>
   ```
4. Replace with:
   ```xml
   <TargetFramework>net472</TargetFramework>
   ```
5. Save and reload the project

### Step 3.3: Remove Modern SDK References (if needed)
1. Right-click project > **Unload Project**
2. Right-click again > **Edit Project File**
3. If you see `<Project Sdk="Microsoft.NET.Sdk">`, change to:
   ```xml
   <Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
   ```
4. Save, right-click project > **Reload Project**

---

## Step 4: Create Database

### Method 1: Using SQL Server Management Studio (SSMS)
1. Download and install SSMS 2015+
2. Open SSMS
3. **Connect to:** `(LocalDB)\MSSQLLocalDB`
4. Click **Connect**
5. Right-click **Databases** > **New Database**
6. **Database name:** `ClothStoreDB`
7. Click **OK**
8. Expand the database, right-click **Tables**
9. Open the SQL script: `Database/ClothStoreDB.sql` from the project folder
10. Copy all content and execute in SSMS Query window

### Method 2: Using Command Line
1. Open **Command Prompt** as Administrator
2. Run:
   ```bash
   sqlcmd -S (LocalDB)\MSSQLLocalDB
   ```
3. Then run:
   ```sql
   CREATE DATABASE ClothStoreDB;
   GO
   ```
4. Exit with `EXIT`
5. Create tables by running the SQL script from `Database/ClothStoreDB.sql`

### Method 3: Automatic (From App)
1. Skip manual setup
2. When you run the app for the first time, it will:
   - Check if database exists
   - Create tables automatically
   - Insert sample data
   - Create default admin user

---

## Step 5: Update Connection String (if needed)

1. In Visual Studio, open: `ClothStoreDesktop/Data/DatabaseConnection.vb`
2. Find this line:
   ```vb
   Public Shared ReadOnly ConnectionString As String = "Server=(LocalDB)\\MSSQLLocalDB;Database=ClothStoreDB;Integrated Security=true;Connection Timeout=30"
   ```
3. If your SQL Server is different, update it:
   - **Local Machine SQL Server 2015:** `Server=.\SQLEXPRESS;Database=ClothStoreDB;Integrated Security=true;`
   - **Named Instance:** `Server=(LocalDB)\InstanceName;Database=ClothStoreDB;Integrated Security=true;`
   - **Remote Server:** `Server=192.168.1.10;Database=ClothStoreDB;User Id=sa;Password=YourPassword;`

---

## Step 6: Restore NuGet Packages

1. Go to **Tools** > **NuGet Package Manager** > **Package Manager Console**
2. Run:
   ```
   Update-Package
   ```
   or
   ```
   Restore-Package
   ```
3. Wait for completion (check bottom status bar)

If no packages are needed, you can skip this step.

---

## Step 7: Build the Project

1. Go to **Build** > **Clean Solution**
2. Go to **Build** > **Build Solution**
3. Wait for build to complete
4. Check **Output** window (View > Output) for any errors

If there are errors:
- Double-click the error to go to the problematic line
- Most common: Missing imports or wrong namespace
- Fix with: **Project** > **Add Reference** > Select missing DLL

---

## Step 8: Run the Application

1. Press **F5** or click **Start** (Debug mode)
   - OR click **Ctrl + F5** for Release mode (faster)
2. The application window will open
3. **Login screen** appears with:
   - Default Username: `admin`
   - Default Password: `admin123`
4. Click **Login**
5. Dashboard opens showing:
   - Total Products
   - Inventory Value
   - Low Stock Items
   - Sales Today
   - Total Customers

---

## Step 9: Use the Application

### Main Features:

**Dashboard (Main Form):**
- View summary statistics
- Quick access to all modules

**Inventory Management:**
- Click **Inventory** button
- Add, Edit, Delete products
- Search by name
- View stock levels
- Set reorder levels

**Sales Processing:**
- Click **Sales** button
- Select products from dropdown
- Add multiple items to cart
- Process payment
- Print receipt

**Customer Management:**
- Click **Customers** button
- Add new customers
- View customer history
- Manage VIP status
- Track loyalty points

**Reports:**
- Click **Reports** button
- View daily/weekly sales
- Top selling products
- Revenue trends
- Stock status reports

---

## Troubleshooting

### Problem 1: "Database not found" Error
**Solution:**
1. Verify SQL Server LocalDB is installed: `sqllocaldb info`
2. Create the database manually (see Step 4)
3. Check connection string in `DatabaseConnection.vb`
4. Restart Visual Studio and try again

### Problem 2: "Cannot connect to database" on Login
**Solution:**
1. Open Command Prompt and verify LocalDB is running:
   ```bash
   sqllocaldb start MSSQLLocalDB
   ```
2. Open SSMS and connect to `(LocalDB)\MSSQLLocalDB`
3. Verify `ClothStoreDB` database exists
4. Check if tables are created

### Problem 3: Build Fails with Errors
**Solution:**
1. Clean solution: **Build** > **Clean Solution**
2. Delete `bin` and `obj` folders manually
3. Rebuild: **Build** > **Build Solution**
4. If still failing, check Error List for specific issues

### Problem 4: "Index was out of range" Error During Login
**Solution:**
1. This usually means no users exist in database
2. Open SSMS and run:
   ```sql
   INSERT INTO Users (Username, Password, FullName, Email, Role, IsActive) 
   VALUES ('admin', 'admin123', 'Admin User', 'admin@clothstore.com', 'Admin', 1);
   ```
3. Restart the application

### Problem 5: Forms Won't Display Properly
**Solution:**
1. Check if Windows Forms is properly added to project
2. Right-click project > **Add Reference**
3. Look for `System.Windows.Forms` in Assemblies tab
4. If missing, add it

---

## Performance Tips for VS 2015

1. **Disable Visual Studio Hosting Process** (faster debugging):
   - Project Properties > Debug > Uncheck "Enable the Visual Studio hosting process"

2. **Use Release Mode for Testing**:
   - Press Ctrl + F5 instead of F5
   - Runs without debugger, much faster

3. **Disable Real-Time Error Checking**:
   - Tools > Options > Text Editor > VB.NET > Advanced > Set "Enable Error Reporting" to False

4. **Clean Build Cache**:
   - Delete `bin` and `obj` folders before building

---

## Upgrading to Newer Visual Studio

If you want to use **Visual Studio 2019/2022**:
1. Install the newer version
2. Open the solution (it will auto-upgrade)
3. Update target framework to `.NET Framework 4.7.2`
4. Build and run

The code is compatible with newer versions without major changes.

---

## Next Steps

1. **Test all forms:**
   - Add/Edit/Delete products
   - Process a sample sale
   - Generate reports

2. **Customize:**
   - Change company name in forms
   - Add your logo
   - Modify color scheme

3. **Extend functionality:**
   - Add receipt printing
   - Implement barcode scanning
   - Add user authentication levels
   - Create advanced reports

4. **Deploy:**
   - Build Release version: Build > Build Solution (Release mode)
   - Distribute .exe file from `bin/Release/`
   - Include database setup instructions for end users

---

## Support

If you encounter issues:
1. Check the **Output** window in Visual Studio for error details
2. Review **Error List** (View > Error List)
3. Search the error message online
4. Check database connectivity using SSMS
5. Verify all prerequisites are installed

---

**Version:** 1.0  
**Last Updated:** October 2026  
**Compatible With:** Visual Studio 2015, 2017, 2019, 2022  
**Database:** SQL Server LocalDB 2015+
