Imports System.Data.SqlClient
Imports ClothStoreDesktop.Models

Namespace ClothStoreDesktop.Data
    Public Class SalesRepository
        ''' <summary>
        ''' Get all sales
        ''' </summary>
        Public Shared Function GetAllSales() As DataTable
            Try
                Dim query As String = "SELECT s.SaleId, s.SaleNumber, CONCAT(c.FirstName, ' ', c.LastName) as CustomerName, u.FullName as EmployeeName, s.SaleDate, s.TotalAmount, s.PaymentStatus FROM Sales s LEFT JOIN Customers c ON s.CustomerId = c.CustomerId JOIN Users u ON s.UserId = u.UserId ORDER BY s.SaleDate DESC"
                Return DatabaseConnection.ExecuteQuery(query)
            Catch ex As Exception
                MessageBox.Show("Error loading sales: " & ex.Message)
                Return New DataTable()
            End Try
        End Function

        ''' <summary>
        ''' Get sales by date range
        ''' </summary>
        Public Shared Function GetSalesByDateRange(startDate As DateTime, endDate As DateTime) As DataTable
            Try
                Dim query As String = "SELECT s.SaleId, s.SaleNumber, CONCAT(c.FirstName, ' ', c.LastName) as CustomerName, s.SaleDate, s.TotalAmount FROM Sales s LEFT JOIN Customers c ON s.CustomerId = c.CustomerId WHERE s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate ORDER BY s.SaleDate DESC"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@StartDate", startDate),
                    New SqlParameter("@EndDate", endDate.AddDays(1))
                }
                Return DatabaseConnection.ExecuteQuery(query, parameters)
            Catch ex As Exception
                MessageBox.Show("Error loading sales: " & ex.Message)
                Return New DataTable()
            End Try
        End Function

        ''' <summary>
        ''' Get sales by customer
        ''' </summary>
        Public Shared Function GetSalesByCustomer(customerId As Integer) As DataTable
            Try
                Dim query As String = "SELECT s.SaleId, s.SaleNumber, s.SaleDate, s.TotalAmount, s.PaymentStatus FROM Sales s WHERE s.CustomerId = @CustomerId ORDER BY s.SaleDate DESC"
                Dim param = New SqlParameter("@CustomerId", customerId)
                Return DatabaseConnection.ExecuteQuery(query, param)
            Catch ex As Exception
                MessageBox.Show("Error loading customer sales: " & ex.Message)
                Return New DataTable()
            End Try
        End Function

        ''' <summary>
        ''' Add new sale
        ''' </summary>
        Public Shared Function AddSale(customerId As Integer, userId As Integer, totalAmount As Decimal, paymentMethodId As Integer, saleItems As List(Of SaleItem)) As Integer
            Dim saleId As Integer = -1
            Try
                Using connection As New SqlConnection(DatabaseConnection.ConnectionString)
                    connection.Open()
                    Using transaction = connection.BeginTransaction()
                        Try
                            ' Insert sale header
                            Dim saleNumber = "INV-" & DateTime.Now.ToString("yyyyMMddHHmmss")
                            Dim saleQuery As String = "INSERT INTO Sales (SaleNumber, CustomerId, UserId, SaleDate, SubTotal, TotalAmount, PaymentMethodId, PaymentStatus) OUTPUT INSERTED.SaleId VALUES (@SaleNumber, @CustomerId, @UserId, GETDATE(), @SubTotal, @TotalAmount, @PaymentMethodId, 'Completed')"
                            Using cmd As New SqlCommand(saleQuery, connection, transaction)
                                cmd.Parameters.AddWithValue("@SaleNumber", saleNumber)
                                cmd.Parameters.AddWithValue("@CustomerId", If(customerId = 0, DBNull.Value, customerId))
                                cmd.Parameters.AddWithValue("@UserId", userId)
                                cmd.Parameters.AddWithValue("@SubTotal", totalAmount)
                                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount)
                                cmd.Parameters.AddWithValue("@PaymentMethodId", paymentMethodId)
                                saleId = CInt(cmd.ExecuteScalar())
                            End Using

                            ' Insert sale items and update product stock
                            For Each item In saleItems
                                ' Insert sale item
                                Dim itemQuery As String = "INSERT INTO SaleItems (SaleId, ProductId, Quantity, UnitPrice, LineTotal) VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @LineTotal)"
                                Using cmd As New SqlCommand(itemQuery, connection, transaction)
                                    cmd.Parameters.AddWithValue("@SaleId", saleId)
                                    cmd.Parameters.AddWithValue("@ProductId", item.ProductId)
                                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity)
                                    cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice)
                                    cmd.Parameters.AddWithValue("@LineTotal", item.Total)
                                    cmd.ExecuteNonQuery()
                                End Using

                                ' Update product stock
                                Dim stockQuery As String = "UPDATE Products SET Quantity = Quantity - @Quantity WHERE ProductId = @ProductId"
                                Using cmd As New SqlCommand(stockQuery, connection, transaction)
                                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity)
                                    cmd.Parameters.AddWithValue("@ProductId", item.ProductId)
                                    cmd.ExecuteNonQuery()
                                End Using
                            Next

                            ' Update customer total spent
                            If customerId > 0 Then
                                Dim custQuery As String = "UPDATE Customers SET TotalSpent = TotalSpent + @Amount, LastPurchaseDate = GETDATE() WHERE CustomerId = @CustomerId"
                                Using cmd As New SqlCommand(custQuery, connection, transaction)
                                    cmd.Parameters.AddWithValue("@Amount", totalAmount)
                                    cmd.Parameters.AddWithValue("@CustomerId", customerId)
                                    cmd.ExecuteNonQuery()
                                End Using
                            End If

                            transaction.Commit()
                        Catch ex As Exception
                            transaction.Rollback()
                            Throw New Exception("Error processing sale: " & ex.Message)
                        End Try
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error adding sale: " & ex.Message)
            End Try
            Return saleId
        End Function

        ''' <summary>
        ''' Get daily revenue
        ''' </summary>
        Public Shared Function GetDailyRevenue(saleDate As DateTime) As Decimal
            Try
                Dim query As String = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Sales WHERE CAST(SaleDate AS DATE) = @SaleDate"
                Dim param = New SqlParameter("@SaleDate", saleDate.Date)
                Dim result = DatabaseConnection.ExecuteScalar(query, param)
                Return If(result Is Nothing, 0, CDec(result))
            Catch ex As Exception
                MessageBox.Show("Error calculating revenue: " & ex.Message)
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Get total sales count
        ''' </summary>
        Public Shared Function GetTotalSalesCount() As Integer
            Try
                Dim query As String = "SELECT COUNT(*) FROM Sales WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)"
                Dim result = DatabaseConnection.ExecuteScalar(query)
                Return If(result Is Nothing, 0, CInt(result))
            Catch ex As Exception
                MessageBox.Show("Error counting sales: " & ex.Message)
                Return 0
            End Try
        End Function

        ''' <summary>
        ''' Get top selling products
        ''' </summary>
        Public Shared Function GetTopSellingProducts(topCount As Integer) As DataTable
            Try
                Dim query As String = "SELECT TOP " & topCount & " p.ProductName, SUM(si.Quantity) as TotalSold, SUM(si.LineTotal) as TotalRevenue FROM SaleItems si JOIN Products p ON si.ProductId = p.ProductId GROUP BY p.ProductName ORDER BY TotalSold DESC"
                Return DatabaseConnection.ExecuteQuery(query)
            Catch ex As Exception
                MessageBox.Show("Error loading top products: " & ex.Message)
                Return New DataTable()
            End Try
        End Function
    End Class
End Namespace
