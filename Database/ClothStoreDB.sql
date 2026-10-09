-- =============================================
-- Cloth Store Database Schema
-- =============================================

-- 1. Users Table
CREATE TABLE [dbo].[Users] (
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(50) UNIQUE NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100),
    [Role] NVARCHAR(20) CHECK ([Role] IN ('Admin', 'Manager', 'Staff')) DEFAULT 'Staff',
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETDATE(),
    [LastLoginDate] DATETIME,
    [PhoneNumber] NVARCHAR(15)
);

-- 2. Categories Table
CREATE TABLE [dbo].[Categories] (
    [CategoryId] INT IDENTITY(1,1) PRIMARY KEY,
    [CategoryName] NVARCHAR(50) UNIQUE NOT NULL,
    [Description] NVARCHAR(255),
    [IsActive] BIT DEFAULT 1
);

-- 3. Suppliers Table
CREATE TABLE [dbo].[Suppliers] (
    [SupplierId] INT IDENTITY(1,1) PRIMARY KEY,
    [SupplierName] NVARCHAR(100) NOT NULL,
    [ContactPerson] NVARCHAR(100),
    [PhoneNumber] NVARCHAR(15),
    [Email] NVARCHAR(100),
    [Address] NVARCHAR(255),
    [City] NVARCHAR(50),
    [State] NVARCHAR(50),
    [PaymentTerms] NVARCHAR(50),
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETDATE()
);

-- 4. Products Table
CREATE TABLE [dbo].[Products] (
    [ProductId] INT IDENTITY(1,1) PRIMARY KEY,
    [ProductName] NVARCHAR(100) NOT NULL,
    [CategoryId] INT NOT NULL,
    [SupplierId] INT,
    [SKU] NVARCHAR(50) UNIQUE,
    [Barcode] NVARCHAR(50) UNIQUE,
    [Description] NVARCHAR(255),
    [Size] NVARCHAR(20),
    [Color] NVARCHAR(30),
    [Material] NVARCHAR(50),
    [CostPrice] DECIMAL(10,2) NOT NULL,
    [SellingPrice] DECIMAL(10,2) NOT NULL,
    [DiscountPrice] DECIMAL(10,2),
    [Quantity] INT DEFAULT 0,
    [ReorderLevel] INT DEFAULT 10,
    [IsActive] BIT DEFAULT 1,
    [CreatedDate] DATETIME DEFAULT GETDATE(),
    [LastModifiedDate] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([CategoryId]),
    FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers]([SupplierId])
);

-- 5. Customers Table
CREATE TABLE [dbo].[Customers] (
    [CustomerId] INT IDENTITY(1,1) PRIMARY KEY,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [PhoneNumber] NVARCHAR(15) UNIQUE,
    [Email] NVARCHAR(100),
    [Address] NVARCHAR(255),
    [City] NVARCHAR(50),
    [State] NVARCHAR(50),
    [PostalCode] NVARCHAR(10),
    [CustomerType] NVARCHAR(20) CHECK ([CustomerType] IN ('Regular', 'VIP', 'Bulk')) DEFAULT 'Regular',
    [LoyaltyPoints] INT DEFAULT 0,
    [DateOfBirth] DATE,
    [Gender] NVARCHAR(10),
    [CreatedDate] DATETIME DEFAULT GETDATE(),
    [LastPurchaseDate] DATETIME,
    [TotalSpent] DECIMAL(10,2) DEFAULT 0
);

-- 6. Payment Methods Table
CREATE TABLE [dbo].[PaymentMethods] (
    [PaymentMethodId] INT IDENTITY(1,1) PRIMARY KEY,
    [MethodName] NVARCHAR(50) UNIQUE NOT NULL,
    [IsActive] BIT DEFAULT 1
);

-- 7. Sales Table
CREATE TABLE [dbo].[Sales] (
    [SaleId] INT IDENTITY(1,1) PRIMARY KEY,
    [SaleNumber] NVARCHAR(50) UNIQUE NOT NULL,
    [CustomerId] INT,
    [UserId] INT NOT NULL,
    [SaleDate] DATETIME DEFAULT GETDATE(),
    [SubTotal] DECIMAL(10,2) NOT NULL,
    [DiscountAmount] DECIMAL(10,2) DEFAULT 0,
    [TaxAmount] DECIMAL(10,2) DEFAULT 0,
    [TotalAmount] DECIMAL(10,2) NOT NULL,
    [PaymentMethodId] INT,
    [PaymentStatus] NVARCHAR(20) CHECK ([PaymentStatus] IN ('Pending', 'Completed', 'Refunded')) DEFAULT 'Completed',
    [Notes] NVARCHAR(500),
    FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([CustomerId]),
    FOREIGN KEY ([UserId]) REFERENCES [Users]([UserId]),
    FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods]([PaymentMethodId])
);

-- 8. Sale Items Table
CREATE TABLE [dbo].[SaleItems] (
    [SaleItemId] INT IDENTITY(1,1) PRIMARY KEY,
    [SaleId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [Quantity] INT NOT NULL,
    [UnitPrice] DECIMAL(10,2) NOT NULL,
    [Discount] DECIMAL(10,2) DEFAULT 0,
    [LineTotal] DECIMAL(10,2) NOT NULL,
    FOREIGN KEY ([SaleId]) REFERENCES [Sales]([SaleId]) ON DELETE CASCADE,
    FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
);

-- 9. Stock Alerts Table
CREATE TABLE [dbo].[StockAlerts] (
    [AlertId] INT IDENTITY(1,1) PRIMARY KEY,
    [ProductId] INT NOT NULL,
    [CurrentStock] INT NOT NULL,
    [ReorderLevel] INT NOT NULL,
    [AlertDate] DATETIME DEFAULT GETDATE(),
    [AlertType] NVARCHAR(20) CHECK ([AlertType] IN ('LowStock', 'OutOfStock')) DEFAULT 'LowStock',
    [IsResolved] BIT DEFAULT 0,
    [ResolvedDate] DATETIME,
    FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
);

-- =============================================
-- Indexes for Performance
-- =============================================

CREATE INDEX [IX_Products_CategoryId] ON [dbo].[Products]([CategoryId]);
CREATE INDEX [IX_Products_SupplierId] ON [dbo].[Products]([SupplierId]);
CREATE INDEX [IX_Products_Barcode] ON [dbo].[Products]([Barcode]);
CREATE INDEX [IX_Products_SKU] ON [dbo].[Products]([SKU]);

CREATE INDEX [IX_Sales_CustomerId] ON [dbo].[Sales]([CustomerId]);
CREATE INDEX [IX_Sales_UserId] ON [dbo].[Sales]([UserId]);
CREATE INDEX [IX_Sales_SaleDate] ON [dbo].[Sales]([SaleDate]);

CREATE INDEX [IX_SaleItems_SaleId] ON [dbo].[SaleItems]([SaleId]);
CREATE INDEX [IX_SaleItems_ProductId] ON [dbo].[SaleItems]([ProductId]);

CREATE INDEX [IX_Customers_PhoneNumber] ON [dbo].[Customers]([PhoneNumber]);
CREATE INDEX [IX_Customers_Email] ON [dbo].[Customers]([Email]);

CREATE INDEX [IX_StockAlerts_ProductId] ON [dbo].[StockAlerts]([ProductId]);
CREATE INDEX [IX_StockAlerts_AlertDate] ON [dbo].[StockAlerts]([AlertDate]);

-- =============================================
-- Sample Data Insertion
-- =============================================

-- Insert Default User
INSERT INTO [dbo].[Users] ([Username], [Password], [FullName], [Email], [Role], [PhoneNumber])
VALUES ('admin', 'admin123', 'Admin User', 'admin@clothstore.com', 'Admin', '9999999999');

-- Insert Payment Methods
INSERT INTO [dbo].[PaymentMethods] ([MethodName]) VALUES ('Cash');
INSERT INTO [dbo].[PaymentMethods] ([MethodName]) VALUES ('Card');
INSERT INTO [dbo].[PaymentMethods] ([MethodName]) VALUES ('UPI');
INSERT INTO [dbo].[PaymentMethods] ([MethodName]) VALUES ('Cheque');

-- Insert Categories
INSERT INTO [dbo].[Categories] ([CategoryName], [Description]) VALUES ('Men', 'Mens Clothing');
INSERT INTO [dbo].[Categories] ([CategoryName], [Description]) VALUES ('Women', 'Womens Clothing');
INSERT INTO [dbo].[Categories] ([CategoryName], [Description]) VALUES ('Kids', 'Kids Clothing');
INSERT INTO [dbo].[Categories] ([CategoryName], [Description]) VALUES ('Accessories', 'Clothing Accessories');

-- Insert Suppliers
INSERT INTO [dbo].[Suppliers] ([SupplierName], [ContactPerson], [PhoneNumber], [Email], [Address], [City], [State])
VALUES ('Threads Ltd', 'John Smith', '9876543210', 'supplier@threadsltd.com', '123 Textile Road', 'Mumbai', 'Maharashtra');
INSERT INTO [dbo].[Suppliers] ([SupplierName], [ContactPerson], [PhoneNumber], [Email], [Address], [City], [State])
VALUES ('Urban Weave', 'Sarah Johnson', '9876543211', 'info@urbanweave.com', '456 Fashion Ave', 'Delhi', 'Delhi');

-- Insert Products
INSERT INTO [dbo].[Products] ([ProductName], [CategoryId], [SupplierId], [SKU], [Barcode], [Size], [Color], [Material], [CostPrice], [SellingPrice], [Quantity], [ReorderLevel])
VALUES ('Cotton Shirt', 1, 1, 'CS-001', 'CS-001-BAR', 'M', 'Blue', 'Cotton', 300, 750, 25, 10);
INSERT INTO [dbo].[Products] ([ProductName], [CategoryId], [SupplierId], [SKU], [Barcode], [Size], [Color], [Material], [CostPrice], [SellingPrice], [Quantity], [ReorderLevel])
VALUES ('Denim Jeans', 1, 2, 'DJ-008', 'DJ-008-BAR', 'L', 'Black', 'Denim', 600, 1200, 18, 10);
INSERT INTO [dbo].[Products] ([ProductName], [CategoryId], [SupplierId], [SKU], [Barcode], [Size], [Color], [Material], [CostPrice], [SellingPrice], [Quantity], [ReorderLevel])
VALUES ('Silk Saree', 2, 1, 'SS-105', 'SS-105-BAR', 'Free', 'Maroon', 'Silk', 800, 1800, 12, 5);
INSERT INTO [dbo].[Products] ([ProductName], [CategoryId], [SupplierId], [SKU], [Barcode], [Size], [Color], [Material], [CostPrice], [SellingPrice], [Quantity], [ReorderLevel])
VALUES ('Kids T-Shirt', 3, 2, 'KT-214', 'KT-214-BAR', 'S', 'Green', 'Cotton', 200, 500, 35, 15);

-- Insert Sample Customer
INSERT INTO [dbo].[Customers] ([FirstName], [LastName], [PhoneNumber], [Email], [Address], [City], [State], [CustomerType])
VALUES ('Raj', 'Kumar', '9123456789', 'raj@email.com', '789 Main St', 'Mumbai', 'Maharashtra', 'Regular');
