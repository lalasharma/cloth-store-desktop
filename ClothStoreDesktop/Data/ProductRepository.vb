Imports System.Data.SqlClient
Imports ClothStoreDesktop.Models

Namespace ClothStoreDesktop.Data
    Public Class ProductRepository
        ''' <summary>
        ''' Get all products
        ''' </summary>
        Public Shared Function GetAllProducts() As List(Of Product)
            Dim products As New List(Of Product)
            Try
                Dim query As String = "SELECT ProductId, ProductName, CategoryId, SupplierId, SKU, Barcode, Size, Color, Material, CostPrice, SellingPrice, DiscountPrice, Quantity, ReorderLevel, IsActive FROM Products WHERE IsActive = 1 ORDER BY ProductName"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    products.Add(New Product With {
                        .Id = CInt(row("ProductId")),
                        .Name = row("ProductName").ToString(),
                        .Category = GetCategoryName(CInt(row("CategoryId"))),
                        .Size = row("Size").ToString(),
                        .Color = row("Color").ToString(),
                        .Price = CDec(row("SellingPrice")),
                        .Stock = CInt(row("Quantity")),
                        .Supplier = GetSupplierName(If(IsDBNull(row("SupplierId")), 0, CInt(row("SupplierId")))),
                        .Barcode = row("Barcode").ToString()
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading products: " & ex.Message)
            End Try
            Return products
        End Function

        ''' <summary>
        ''' Get product by ID
        ''' </summary>
        Public Shared Function GetProductById(productId As Integer) As Product
            Try
                Dim query As String = "SELECT ProductId, ProductName, CategoryId, SupplierId, SKU, Barcode, Size, Color, Material, CostPrice, SellingPrice, DiscountPrice, Quantity, ReorderLevel FROM Products WHERE ProductId = @ProductId"
                Dim param = New SqlParameter("@ProductId", productId)
                Dim dt = DatabaseConnection.ExecuteQuery(query, param)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    Return New Product With {
                        .Id = CInt(row("ProductId")),
                        .Name = row("ProductName").ToString(),
                        .Category = GetCategoryName(CInt(row("CategoryId"))),
                        .Size = row("Size").ToString(),
                        .Color = row("Color").ToString(),
                        .Price = CDec(row("SellingPrice")),
                        .Stock = CInt(row("Quantity")),
                        .Supplier = GetSupplierName(If(IsDBNull(row("SupplierId")), 0, CInt(row("SupplierId")))),
                        .Barcode = row("Barcode").ToString()
                    }
                End If
            Catch ex As Exception
                MessageBox.Show("Error loading product: " & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Add new product
        ''' </summary>
        Public Shared Function AddProduct(product As Product) As Boolean
            Try
                Dim categoryId = GetCategoryIdByName(product.Category)
                Dim query As String = "INSERT INTO Products (ProductName, CategoryId, SKU, Barcode, Size, Color, CostPrice, SellingPrice, Quantity, ReorderLevel, IsActive) VALUES (@Name, @CategoryId, @SKU, @Barcode, @Size, @Color, @CostPrice, @SellingPrice, @Quantity, @ReorderLevel, 1)"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@Name", product.Name),
                    New SqlParameter("@CategoryId", categoryId),
                    New SqlParameter("@SKU", product.Name.Replace(" ", "-") & "-" & DateTime.Now.Ticks.ToString().Substring(0, 4)),
                    New SqlParameter("@Barcode", product.Barcode),
                    New SqlParameter("@Size", product.Size),
                    New SqlParameter("@Color", product.Color),
                    New SqlParameter("@CostPrice", product.Price * 0.4),
                    New SqlParameter("@SellingPrice", product.Price),
                    New SqlParameter("@Quantity", product.Stock),
                    New SqlParameter("@ReorderLevel", 10)
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error adding product: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Update existing product
        ''' </summary>
        Public Shared Function UpdateProduct(product As Product) As Boolean
            Try
                Dim categoryId = GetCategoryIdByName(product.Category)
                Dim query As String = "UPDATE Products SET ProductName = @Name, CategoryId = @CategoryId, Size = @Size, Color = @Color, SellingPrice = @Price, Quantity = @Stock, LastModifiedDate = GETDATE() WHERE ProductId = @ProductId"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@ProductId", product.Id),
                    New SqlParameter("@Name", product.Name),
                    New SqlParameter("@CategoryId", categoryId),
                    New SqlParameter("@Size", product.Size),
                    New SqlParameter("@Color", product.Color),
                    New SqlParameter("@Price", product.Price),
                    New SqlParameter("@Stock", product.Stock)
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error updating product: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Delete product (soft delete)
        ''' </summary>
        Public Shared Function DeleteProduct(productId As Integer) As Boolean
            Try
                Dim query As String = "UPDATE Products SET IsActive = 0 WHERE ProductId = @ProductId"
                Dim param = New SqlParameter("@ProductId", productId)
                DatabaseConnection.ExecuteNonQuery(query, param)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error deleting product: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Search products by name
        ''' </summary>
        Public Shared Function SearchProducts(searchTerm As String) As List(Of Product)
            Dim products As New List(Of Product)
            Try
                Dim query As String = "SELECT ProductId, ProductName, CategoryId, SupplierId, SKU, Barcode, Size, Color, CostPrice, SellingPrice, Quantity, ReorderLevel FROM Products WHERE ProductName LIKE @SearchTerm AND IsActive = 1"
                Dim param = New SqlParameter("@SearchTerm", "%" & searchTerm & "%")
                Dim dt = DatabaseConnection.ExecuteQuery(query, param)
                For Each row In dt.Rows
                    products.Add(New Product With {
                        .Id = CInt(row("ProductId")),
                        .Name = row("ProductName").ToString(),
                        .Category = GetCategoryName(CInt(row("CategoryId"))),
                        .Size = row("Size").ToString(),
                        .Color = row("Color").ToString(),
                        .Price = CDec(row("SellingPrice")),
                        .Stock = CInt(row("Quantity")),
                        .Barcode = row("Barcode").ToString()
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error searching products: " & ex.Message)
            End Try
            Return products
        End Function

        ''' <summary>
        ''' Get low stock products
        ''' </summary>
        Public Shared Function GetLowStockProducts() As List(Of Product)
            Dim products As New List(Of Product)
            Try
                Dim query As String = "SELECT ProductId, ProductName, CategoryId, Quantity, ReorderLevel FROM Products WHERE Quantity <= ReorderLevel AND IsActive = 1"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    products.Add(New Product With {
                        .Id = CInt(row("ProductId")),
                        .Name = row("ProductName").ToString(),
                        .Stock = CInt(row("Quantity"))
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading low stock products: " & ex.Message)
            End Try
            Return products
        End Function

        Private Shared Function GetCategoryName(categoryId As Integer) As String
            Try
                Dim query As String = "SELECT CategoryName FROM Categories WHERE CategoryId = @CategoryId"
                Dim param = New SqlParameter("@CategoryId", categoryId)
                Dim result = DatabaseConnection.ExecuteScalar(query, param)
                Return If(result Is Nothing, "Unknown", result.ToString())
            Catch
                Return "Unknown"
            End Try
        End Function

        Private Shared Function GetCategoryIdByName(categoryName As String) As Integer
            Try
                Dim query As String = "SELECT CategoryId FROM Categories WHERE CategoryName = @CategoryName"
                Dim param = New SqlParameter("@CategoryName", categoryName)
                Dim result = DatabaseConnection.ExecuteScalar(query, param)
                Return If(result Is Nothing, 1, CInt(result))
            Catch
                Return 1
            End Try
        End Function

        Private Shared Function GetSupplierName(supplierId As Integer) As String
            If supplierId = 0 Then Return "Unknown"
            Try
                Dim query As String = "SELECT SupplierName FROM Suppliers WHERE SupplierId = @SupplierId"
                Dim param = New SqlParameter("@SupplierId", supplierId)
                Dim result = DatabaseConnection.ExecuteScalar(query, param)
                Return If(result Is Nothing, "Unknown", result.ToString())
            Catch
                Return "Unknown"
            End Try
        End Function
    End Class
End Namespace
