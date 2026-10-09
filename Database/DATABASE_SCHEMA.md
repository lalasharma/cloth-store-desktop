# Database Schema Documentation

## Cloth Store Desktop Application - Database Structure

### Overview
This document describes the SQL Server database schema for the Cloth Store Management System.

---

## Tables

### 1. **Users Table**
Stores user account information and authentication credentials.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| UserId | INT | PK, Identity | Unique identifier |
| Username | NVARCHAR(50) | UNIQUE, NOT NULL | Login username |
| Password | NVARCHAR(255) | NOT NULL | Encrypted password |
| FullName | NVARCHAR(100) | NOT NULL | User full name |
| Email | NVARCHAR(100) | | Email address |
| Role | NVARCHAR(20) | CHECK (Admin/Manager/Staff) | User role/permission level |
| IsActive | BIT | DEFAULT 1 | Account status |
| CreatedDate | DATETIME | DEFAULT GETDATE() | Account creation date |
| LastLoginDate | DATETIME | | Last login timestamp |
| PhoneNumber | NVARCHAR(15) | | Contact number |

---

### 2. **Customers Table**
Stores customer information and purchase history.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| CustomerId | INT | PK, Identity | Unique identifier |
| FirstName | NVARCHAR(50) | NOT NULL | Customer first name |
| LastName | NVARCHAR(50) | NOT NULL | Customer last name |
| PhoneNumber | NVARCHAR(15) | UNIQUE | Contact number |
| Email | NVARCHAR(100) | | Email address |
| Address | NVARCHAR(255) | | Full address |
| City | NVARCHAR(50) | | City name |
| State | NVARCHAR(50) | | State/Province |
| PostalCode | NVARCHAR(10) | | Postal code |
| CustomerType | NVARCHAR(20) | CHECK (Regular/VIP/Bulk) | Customer classification |
| LoyaltyPoints | INT | DEFAULT 0 | Accumulated loyalty points |
| DateOfBirth | DATE | | Date of birth |
| Gender | NVARCHAR(10) | | Gender |
| CreatedDate | DATETIME | DEFAULT GETDATE() | Account creation date |
| LastPurchaseDate | DATETIME | | Last purchase date |
| TotalSpent | DECIMAL(10,2) | DEFAULT 0 | Total lifetime spending |

---

### 3. **Categories Table**
Stores product category information.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| CategoryId | INT | PK, Identity | Unique identifier |
| CategoryName | NVARCHAR(50) | UNIQUE, NOT NULL | Category name (Men, Women, Kids) |
| Description | NVARCHAR(255) | | Category description |
| IsActive | BIT | DEFAULT 1 | Category status |

---

### 4. **Suppliers Table**
Stores supplier/vendor information.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| SupplierId | INT | PK, Identity | Unique identifier |
| SupplierName | NVARCHAR(100) | NOT NULL | Company name |
| ContactPerson | NVARCHAR(100) | | Primary contact |
| PhoneNumber | NVARCHAR(15) | | Business phone |
| Email | NVARCHAR(100) | | Business email |
| Address | NVARCHAR(255) | | Business address |
| City | NVARCHAR(50) | | City |
| State | NVARCHAR(50) | | State/Province |
| PaymentTerms | NVARCHAR(50) | | Payment terms (COD, NET30, etc) |
| IsActive | BIT | DEFAULT 1 | Supplier status |
| CreatedDate | DATETIME | DEFAULT GETDATE() | Registration date |

---

### 5. **Products Table**
Stores product inventory details.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| ProductId | INT | PK, Identity | Unique identifier |
| ProductName | NVARCHAR(100) | NOT NULL | Product name |
| CategoryId | INT | FK (Categories) | Product category |
| SupplierId | INT | FK (Suppliers) | Primary supplier |
| SKU | NVARCHAR(50) | UNIQUE | Stock Keeping Unit |
| Barcode | NVARCHAR(50) | UNIQUE | Product barcode |
| Description | NVARCHAR(255) | | Product description |
| Size | NVARCHAR(20) | | Size (S, M, L, XL, Free) |
| Color | NVARCHAR(30) | | Color |
| Material | NVARCHAR(50) | | Material composition |
| CostPrice | DECIMAL(10,2) | NOT NULL | Cost to business |
| SellingPrice | DECIMAL(10,2) | NOT NULL | Retail selling price |
| DiscountPrice | DECIMAL(10,2) | | Special discount price |
| Quantity | INT | DEFAULT 0 | Current stock level |
| ReorderLevel | INT | DEFAULT 10 | Minimum stock threshold |
| IsActive | BIT | DEFAULT 1 | Product status |
| CreatedDate | DATETIME | DEFAULT GETDATE() | Product creation date |
| LastModifiedDate | DATETIME | DEFAULT GETDATE() | Last update timestamp |

---

### 6. **PaymentMethods Table**
Stores payment method options.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| PaymentMethodId | INT | PK, Identity | Unique identifier |
| MethodName | NVARCHAR(50) | UNIQUE, NOT NULL | Payment type (Cash, Card, UPI, Cheque) |
| IsActive | BIT | DEFAULT 1 | Method availability |

---

### 7. **Sales Table**
Stores sales/invoice header information.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| SaleId | INT | PK, Identity | Unique identifier |
| SaleNumber | NVARCHAR(50) | UNIQUE, NOT NULL | Invoice/Receipt number |
| CustomerId | INT | FK (Customers) | Customer who made purchase |
| UserId | INT | FK (Users) | Employee who processed sale |
| SaleDate | DATETIME | DEFAULT GETDATE() | Date and time of sale |
| SubTotal | DECIMAL(10,2) | NOT NULL | Total before discount/tax |
| DiscountAmount | DECIMAL(10,2) | DEFAULT 0 | Total discount applied |
| TaxAmount | DECIMAL(10,2) | DEFAULT 0 | GST/Tax amount |
| TotalAmount | DECIMAL(10,2) | NOT NULL | Final total amount |
| PaymentMethodId | INT | FK (PaymentMethods) | Payment method used |
| PaymentStatus | NVARCHAR(20) | CHECK (Pending/Completed/Refunded) | Payment status |
| Notes | NVARCHAR(500) | | Additional notes |

---

### 8. **SaleItems Table**
Stores line items for each sale.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| SaleItemId | INT | PK, Identity | Unique identifier |
| SaleId | INT | FK (Sales) | Parent sale |
| ProductId | INT | FK (Products) | Product sold |
| Quantity | INT | NOT NULL | Units sold |
| UnitPrice | DECIMAL(10,2) | NOT NULL | Price per unit at time of sale |
| Discount | DECIMAL(10,2) | DEFAULT 0 | Item-level discount |
| LineTotal | DECIMAL(10,2) | NOT NULL | Quantity × UnitPrice - Discount |

---

### 9. **StockAlerts Table**
Tracks low stock and out-of-stock alerts.

| Column | Type | Constraint | Description |
|--------|------|-----------|-------------|
| AlertId | INT | PK, Identity | Unique identifier |
| ProductId | INT | FK (Products) | Product with stock issue |
| CurrentStock | INT | NOT NULL | Stock level when alert was created |
| ReorderLevel | INT | NOT NULL | Minimum required stock |
| AlertDate | DATETIME | DEFAULT GETDATE() | When alert was generated |
| AlertType | NVARCHAR(20) | CHECK (LowStock/OutOfStock) | Type of alert |
| IsResolved | BIT | DEFAULT 0 | Whether issue is resolved |
| ResolvedDate | DATETIME | | When issue was resolved |

---

## Indexes

Performance indexes created for frequently queried columns:

```sql
-- Product Indexes
IX_Products_CategoryId - Filtering by category
IX_Products_SupplierId - Filtering by supplier
IX_Products_Barcode - Quick lookup by barcode
IX_Products_SKU - Quick lookup by SKU

-- Sales Indexes
IX_Sales_CustomerId - Customer purchase history
IX_Sales_UserId - Employee sales tracking
IX_Sales_SaleDate - Date range queries

-- SaleItems Indexes
IX_SaleItems_SaleId - Retrieve items per invoice
IX_SaleItems_ProductId - Product sales analysis

-- Customer Indexes
IX_Customers_PhoneNumber - Quick customer lookup
IX_Customers_Email - Email-based searches

-- StockAlerts Indexes
IX_StockAlerts_ProductId - Find alerts for product
IX_StockAlerts_AlertDate - Date-based alert queries
```

---

## Relationships

**Foreign Key Constraints:**
- Products.CategoryId → Categories.CategoryId
- Products.SupplierId → Suppliers.SupplierId
- Sales.CustomerId → Customers.CustomerId
- Sales.UserId → Users.UserId
- Sales.PaymentMethodId → PaymentMethods.PaymentMethodId
- SaleItems.SaleId → Sales.SaleId (CASCADE DELETE)
- SaleItems.ProductId → Products.ProductId
- StockAlerts.ProductId → Products.ProductId

---

## Data Integrity

- **Check Constraints:**
  - User Roles: Admin, Manager, Staff
  - Customer Types: Regular, VIP, Bulk
  - Payment Status: Pending, Completed, Refunded
  - Alert Types: LowStock, OutOfStock

- **Unique Constraints:**
  - Users.Username
  - Customers.PhoneNumber
  - Products.SKU, Products.Barcode
  - Sales.SaleNumber
  - PaymentMethods.MethodName
  - Categories.CategoryName

---

## Views (Optional - for reporting)

You can create these views for faster reporting:

```sql
-- Daily Sales Summary
CREATE VIEW vw_DailySales AS
SELECT CAST(SaleDate AS DATE) as SalesDate, COUNT(*) as TotalTransactions, SUM(TotalAmount) as DailyRevenue
FROM Sales
GROUP BY CAST(SaleDate AS DATE);

-- Low Stock Products
CREATE VIEW vw_LowStockProducts AS
SELECT ProductId, ProductName, Quantity, ReorderLevel
FROM Products
WHERE Quantity <= ReorderLevel AND IsActive = 1;

-- Top Selling Products
CREATE VIEW vw_TopSellingProducts AS
SELECT TOP 10 p.ProductName, SUM(si.Quantity) as TotalSold, SUM(si.LineTotal) as TotalRevenue
FROM SaleItems si
JOIN Products p ON si.ProductId = p.ProductId
GROUP BY p.ProductName
ORDER BY TotalSold DESC;
```

---

## Sample Queries

```sql
-- Find all sales for a customer
SELECT s.SaleNumber, s.SaleDate, s.TotalAmount, u.FullName
FROM Sales s
JOIN Users u ON s.UserId = u.UserId
WHERE s.CustomerId = 1;

-- Get inventory value
SELECT SUM(CostPrice * Quantity) as InventoryValue
FROM Products
WHERE IsActive = 1;

-- Daily sales report
SELECT CAST(SaleDate AS DATE) as Date, COUNT(*) as Orders, SUM(TotalAmount) as Revenue
FROM Sales
WHERE SaleDate >= DATEADD(DAY, -30, GETDATE())
GROUP BY CAST(SaleDate AS DATE)
ORDER BY Date DESC;
```

---

**Database Version:** 1.0  
**Last Updated:** October 2026  
**Created For:** Cloth Store Desktop Application (VB.NET)
