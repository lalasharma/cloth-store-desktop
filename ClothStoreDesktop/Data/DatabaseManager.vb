Imports System.Data.SqlClient

Namespace ClothStoreDesktop.Data
    Public Class DatabaseManager
        Private Shared ReadOnly ConnectionString As String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" & Path.Combine(AppContext.BaseDirectory, "ClothStoreDB.mdf") & ";Integrated Security=true;Connect Timeout=30"

        Public Shared Sub InitializeDatabase()
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    CreateTables(connection)
                End Using
            Catch ex As Exception
                MessageBox.Show("Database initialization failed: " & ex.Message, "Database Error")
            End Try
        End Sub

        Private Shared Sub CreateTables(connection As SqlConnection)
            Dim commands = New String() {
                CreateUsersTable(),
                CreateCustomersTable(),
                CreateProductsTable(),
                CreateCategoriesTable(),
                CreateSalesTable(),
                CreateSaleItemsTable(),
                CreateStockAlertsTable(),
                CreateSupplierTable(),
                CreatePaymentMethodsTable()
            }

            For Each commandText In commands
                Using command As New SqlCommand(commandText, connection)
                    Try
                        command.ExecuteNonQuery()
                    Catch ex As SqlException
                        If Not ex.Message.Contains("already exists") Then
                            Throw
                        End If
                    End Try
                End Using
            Next
        End Sub

        Private Shared Function CreateUsersTable() As String
            Return """CREATE TABLE [dbo].[Users] (
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
            )"""
        End Function

        Private Shared Function CreateCustomersTable() As String
            Return """CREATE TABLE [dbo].[Customers] (
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
            )"""
        End Function

        Private Shared Function CreateCategoriesTable() As String
            Return """CREATE TABLE [dbo].[Categories] (
                [CategoryId] INT IDENTITY(1,1) PRIMARY KEY,
                [CategoryName] NVARCHAR(50) UNIQUE NOT NULL,
                [Description] NVARCHAR(255),
                [IsActive] BIT DEFAULT 1
            )"""
        End Function

        Private Shared Function CreateSupplierTable() As String
            Return """CREATE TABLE [dbo].[Suppliers] (
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
            )"""
        End Function

        Private Shared Function CreateProductsTable() As String
            Return """CREATE TABLE [dbo].[Products] (
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
            )"""
        End Function

        Private Shared Function CreatePaymentMethodsTable() As String
            Return """CREATE TABLE [dbo].[PaymentMethods] (
                [PaymentMethodId] INT IDENTITY(1,1) PRIMARY KEY,
                [MethodName] NVARCHAR(50) UNIQUE NOT NULL,
                [IsActive] BIT DEFAULT 1
            )"""
        End Function

        Private Shared Function CreateSalesTable() As String
            Return """CREATE TABLE [dbo].[Sales] (
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
            )"""
        End Function

        Private Shared Function CreateSaleItemsTable() As String
            Return """CREATE TABLE [dbo].[SaleItems] (
                [SaleItemId] INT IDENTITY(1,1) PRIMARY KEY,
                [SaleId] INT NOT NULL,
                [ProductId] INT NOT NULL,
                [Quantity] INT NOT NULL,
                [UnitPrice] DECIMAL(10,2) NOT NULL,
                [Discount] DECIMAL(10,2) DEFAULT 0,
                [LineTotal] DECIMAL(10,2) NOT NULL,
                FOREIGN KEY ([SaleId]) REFERENCES [Sales]([SaleId]) ON DELETE CASCADE,
                FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
            )"""
        End Function

        Private Shared Function CreateStockAlertsTable() As String
            Return """CREATE TABLE [dbo].[StockAlerts] (
                [AlertId] INT IDENTITY(1,1) PRIMARY KEY,
                [ProductId] INT NOT NULL,
                [CurrentStock] INT NOT NULL,
                [ReorderLevel] INT NOT NULL,
                [AlertDate] DATETIME DEFAULT GETDATE(),
                [AlertType] NVARCHAR(20) CHECK ([AlertType] IN ('LowStock', 'OutOfStock')) DEFAULT 'LowStock',
                [IsResolved] BIT DEFAULT 0,
                [ResolvedDate] DATETIME,
                FOREIGN KEY ([ProductId]) REFERENCES [Products]([ProductId])
            )"""
        End Function

        Public Shared Function GetConnection() As SqlConnection
            Return New SqlConnection(ConnectionString)
        End Function
    End Class
End Namespace
