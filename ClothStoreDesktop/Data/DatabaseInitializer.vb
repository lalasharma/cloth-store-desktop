Imports System.Text
Imports ClothStoreDesktop.Data

Public Class DatabaseInitializer
    Public Shared Function EnsureDatabase() As Boolean
        Try
            Dim connectionString = DatabaseConnection.ConnectionString
            Using connection As New SqlConnection(connectionString)
                connection.Open()
                Dim tables = GetRequiredTables(connection)
                For Each tableName In tables
                    If Not TableExists(connection, tableName) Then
                        CreateTable(connection, tableName)
                    End If
                Next

                EnsureDefaultAdmin(connection)
                EnsureSampleData(connection)
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Database initialization failed: " & ex.Message, "Setup Error")
            Return False
        End Try
    End Function

    Private Shared Function GetRequiredTables(connection As SqlConnection) As String()
        Return {"Users", "Customers", "Categories", "Suppliers", "Products", "PaymentMethods", "Sales", "SaleItems", "StockAlerts"}
    End Function

    Private Shared Function TableExists(connection As SqlConnection, tableName As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName"
        Using command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@TableName", tableName)
            Return CInt(command.ExecuteScalar()) > 0
        End Using
    End Function

    Private Shared Sub CreateTable(connection As SqlConnection, tableName As String)
        Dim script As String = GetTableCreateScript(tableName)
        Using command As New SqlCommand(script, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Function GetTableCreateScript(tableName As String) As String
        Select Case tableName
            Case "Users"
                Return "CREATE TABLE Users (UserId INT IDENTITY(1,1) PRIMARY KEY, Username NVARCHAR(50) NOT NULL UNIQUE, Password NVARCHAR(255) NOT NULL, FullName NVARCHAR(100) NOT NULL, Email NVARCHAR(100), Role NVARCHAR(20) DEFAULT 'Staff', IsActive BIT DEFAULT 1, CreatedDate DATETIME DEFAULT GETDATE(), LastLoginDate DATETIME, PhoneNumber NVARCHAR(15));"
            Case "Customers"
                Return "CREATE TABLE Customers (CustomerId INT IDENTITY(1,1) PRIMARY KEY, FirstName NVARCHAR(50) NOT NULL, LastName NVARCHAR(50) NOT NULL, PhoneNumber NVARCHAR(15), Email NVARCHAR(100), Address NVARCHAR(255), City NVARCHAR(50), State NVARCHAR(50), PostalCode NVARCHAR(10), CustomerType NVARCHAR(20) DEFAULT 'Regular', LoyaltyPoints INT DEFAULT 0, TotalSpent DECIMAL(10,2) DEFAULT 0, CreatedDate DATETIME DEFAULT GETDATE(), LastPurchaseDate DATETIME);"
            Case "Categories"
                Return "CREATE TABLE Categories (CategoryId INT IDENTITY(1,1) PRIMARY KEY, CategoryName NVARCHAR(50) NOT NULL UNIQUE, Description NVARCHAR(255), IsActive BIT DEFAULT 1);"
            Case "Suppliers"
                Return "CREATE TABLE Suppliers (SupplierId INT IDENTITY(1,1) PRIMARY KEY, SupplierName NVARCHAR(100) NOT NULL, ContactPerson NVARCHAR(100), PhoneNumber NVARCHAR(15), Email NVARCHAR(100), Address NVARCHAR(255), City NVARCHAR(50), State NVARCHAR(50), PaymentTerms NVARCHAR(50), IsActive BIT DEFAULT 1, CreatedDate DATETIME DEFAULT GETDATE());"
            Case "Products"
                Return "CREATE TABLE Products (ProductId INT IDENTITY(1,1) PRIMARY KEY, ProductName NVARCHAR(100) NOT NULL, CategoryId INT NOT NULL, SupplierId INT NULL, SKU NVARCHAR(50), Barcode NVARCHAR(50), Description NVARCHAR(255), Size NVARCHAR(20), Color NVARCHAR(30), Material NVARCHAR(50), CostPrice DECIMAL(10,2) NOT NULL, SellingPrice DECIMAL(10,2) NOT NULL, DiscountPrice DECIMAL(10,2) NULL, Quantity INT DEFAULT 0, ReorderLevel INT DEFAULT 10, IsActive BIT DEFAULT 1, CreatedDate DATETIME DEFAULT GETDATE(), LastModifiedDate DATETIME DEFAULT GETDATE());"
            Case "PaymentMethods"
                Return "CREATE TABLE PaymentMethods (PaymentMethodId INT IDENTITY(1,1) PRIMARY KEY, MethodName NVARCHAR(50) NOT NULL UNIQUE, IsActive BIT DEFAULT 1);"
            Case "Sales"
                Return "CREATE TABLE Sales (SaleId INT IDENTITY(1,1) PRIMARY KEY, SaleNumber NVARCHAR(50) NOT NULL UNIQUE, CustomerId INT NULL, UserId INT NOT NULL, SaleDate DATETIME DEFAULT GETDATE(), SubTotal DECIMAL(10,2) NOT NULL, DiscountAmount DECIMAL(10,2) DEFAULT 0, TaxAmount DECIMAL(10,2) DEFAULT 0, TotalAmount DECIMAL(10,2) NOT NULL, PaymentMethodId INT, PaymentStatus NVARCHAR(20) DEFAULT 'Completed', Notes NVARCHAR(500));"
            Case "SaleItems"
                Return "CREATE TABLE SaleItems (SaleItemId INT IDENTITY(1,1) PRIMARY KEY, SaleId INT NOT NULL, ProductId INT NOT NULL, Quantity INT NOT NULL, UnitPrice DECIMAL(10,2) NOT NULL, Discount DECIMAL(10,2) DEFAULT 0, LineTotal DECIMAL(10,2) NOT NULL);"
            Case "StockAlerts"
                Return "CREATE TABLE StockAlerts (AlertId INT IDENTITY(1,1) PRIMARY KEY, ProductId INT NOT NULL, CurrentStock INT NOT NULL, ReorderLevel INT NOT NULL, AlertDate DATETIME DEFAULT GETDATE(), AlertType NVARCHAR(20) DEFAULT 'LowStock', IsResolved BIT DEFAULT 0, ResolvedDate DATETIME);"
            Case Else
                Return ""
        End Select
    End Function

    Private Shared Sub EnsureDefaultAdmin(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin') INSERT INTO Users (Username, Password, FullName, Email, Role, IsActive) VALUES ('admin', 'admin123', 'System Administrator', 'admin@clothstore.com', 'Admin', 1)"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Sub EnsureSampleData(connection As SqlConnection)
        EnsureCategories(connection)
        EnsurePaymentMethods(connection)
        EnsureSuppliers(connection)
        EnsureProducts(connection)
        EnsureCustomers(connection)
    End Sub

    Private Shared Sub EnsureCategories(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Men') INSERT INTO Categories (CategoryName, Description) VALUES ('Men', 'Mens wear'); IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Women') INSERT INTO Categories (CategoryName, Description) VALUES ('Women', 'Womens wear'); IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Kids') INSERT INTO Categories (CategoryName, Description) VALUES ('Kids', 'Kids wear'); IF NOT EXISTS (SELECT 1 FROM Categories WHERE CategoryName = 'Accessories') INSERT INTO Categories (CategoryName, Description) VALUES ('Accessories', 'Accessories and misc.');"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Sub EnsurePaymentMethods(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE MethodName = 'Cash') INSERT INTO PaymentMethods (MethodName) VALUES ('Cash'); IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE MethodName = 'Card') INSERT INTO PaymentMethods (MethodName) VALUES ('Card'); IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE MethodName = 'UPI') INSERT INTO PaymentMethods (MethodName) VALUES ('UPI'); IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE MethodName = 'Cheque') INSERT INTO PaymentMethods (MethodName) VALUES ('Cheque');"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Sub EnsureSuppliers(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM Suppliers WHERE SupplierName = 'Threads Ltd') INSERT INTO Suppliers (SupplierName, ContactPerson, PhoneNumber, Email, Address, City, State) VALUES ('Threads Ltd', 'John Smith', '9876543210', 'contact@threadsltd.com', 'Textile Lane', 'Mumbai', 'Maharashtra'); IF NOT EXISTS (SELECT 1 FROM Suppliers WHERE SupplierName = 'Urban Weave') INSERT INTO Suppliers (SupplierName, ContactPerson, PhoneNumber, Email, Address, City, State) VALUES ('Urban Weave', 'Sara Roy', '9812345678', 'sales@urbanweave.com', 'Fashion Road', 'Delhi', 'Delhi');"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Sub EnsureProducts(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductName = 'Cotton Shirt') INSERT INTO Products (ProductName, CategoryId, SupplierId, SKU, Barcode, Description, Size, Color, Material, CostPrice, SellingPrice, Quantity, ReorderLevel, IsActive) VALUES ('Cotton Shirt', 1, 1, 'CT-001', 'BAR-CT-001', 'Basic cotton shirt', 'M', 'Blue', 'Cotton', 250, 700, 25, 10, 1); IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductName = 'Denim Jeans') INSERT INTO Products (ProductName, CategoryId, SupplierId, SKU, Barcode, Description, Size, Color, Material, CostPrice, SellingPrice, Quantity, ReorderLevel, IsActive) VALUES ('Denim Jeans', 1, 2, 'DJ-001', 'BAR-DJ-001', 'Classic denim jeans', 'L', 'Black', 'Denim', 500, 1200, 18, 10, 1); IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductName = 'Silk Saree') INSERT INTO Products (ProductName, CategoryId, SupplierId, SKU, Barcode, Description, Size, Color, Material, CostPrice, SellingPrice, Quantity, ReorderLevel, IsActive) VALUES ('Silk Saree', 2, 1, 'SS-001', 'BAR-SS-001', 'Traditional silk saree', 'Free', 'Maroon', 'Silk', 800, 1800, 12, 5, 1); IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductName = 'Kids T-Shirt') INSERT INTO Products (ProductName, CategoryId, SupplierId, SKU, Barcode, Description, Size, Color, Material, CostPrice, SellingPrice, Quantity, ReorderLevel, IsActive) VALUES ('Kids T-Shirt', 3, 2, 'KT-001', 'BAR-KT-001', 'Kids cotton tee', 'S', 'Green', 'Cotton', 180, 450, 35, 15, 1);"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Sub EnsureCustomers(connection As SqlConnection)
        Dim query As String = "IF NOT EXISTS (SELECT 1 FROM Customers WHERE PhoneNumber = '9123456789') INSERT INTO Customers (FirstName, LastName, PhoneNumber, Email, City, State, CustomerType, TotalSpent) VALUES ('Raj', 'Kumar', '9123456789', 'raj@gmail.com', 'Mumbai', 'Maharashtra', 'Regular', 5400); IF NOT EXISTS (SELECT 1 FROM Customers WHERE PhoneNumber = '9988776655') INSERT INTO Customers (FirstName, LastName, PhoneNumber, Email, City, State, CustomerType, TotalSpent) VALUES ('Neha', 'Sharma', '9988776655', 'neha@gmail.com', 'Delhi', 'Delhi', 'VIP', 12000);"
        Using command As New SqlCommand(query, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub
End Class
