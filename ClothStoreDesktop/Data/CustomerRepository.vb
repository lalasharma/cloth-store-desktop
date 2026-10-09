Imports System.Data.SqlClient
Imports ClothStoreDesktop.Models

Namespace ClothStoreDesktop.Data
    Public Class CustomerRepository
        ''' <summary>
        ''' Get all customers
        ''' </summary>
        Public Shared Function GetAllCustomers() As List(Of Customer)
            Dim customers As New List(Of Customer)
            Try
                Dim query As String = "SELECT CustomerId, FirstName, LastName, PhoneNumber, Email, Address, City, State, PostalCode, CustomerType, LoyaltyPoints, TotalSpent FROM Customers ORDER BY FirstName, LastName"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    customers.Add(New Customer With {
                        .CustomerId = CInt(row("CustomerId")),
                        .FirstName = row("FirstName").ToString(),
                        .LastName = row("LastName").ToString(),
                        .PhoneNumber = row("PhoneNumber").ToString(),
                        .Email = row("Email").ToString(),
                        .Address = row("Address").ToString(),
                        .City = row("City").ToString(),
                        .State = row("State").ToString(),
                        .PostalCode = row("PostalCode").ToString(),
                        .CustomerType = row("CustomerType").ToString(),
                        .LoyaltyPoints = CInt(row("LoyaltyPoints")),
                        .TotalSpent = CDec(row("TotalSpent"))
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading customers: " & ex.Message)
            End Try
            Return customers
        End Function

        ''' <summary>
        ''' Get customer by ID
        ''' </summary>
        Public Shared Function GetCustomerById(customerId As Integer) As Customer
            Try
                Dim query As String = "SELECT CustomerId, FirstName, LastName, PhoneNumber, Email, Address, City, State, PostalCode, CustomerType, LoyaltyPoints, TotalSpent FROM Customers WHERE CustomerId = @CustomerId"
                Dim param = New SqlParameter("@CustomerId", customerId)
                Dim dt = DatabaseConnection.ExecuteQuery(query, param)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    Return New Customer With {
                        .CustomerId = CInt(row("CustomerId")),
                        .FirstName = row("FirstName").ToString(),
                        .LastName = row("LastName").ToString(),
                        .PhoneNumber = row("PhoneNumber").ToString(),
                        .Email = row("Email").ToString(),
                        .Address = row("Address").ToString(),
                        .City = row("City").ToString(),
                        .State = row("State").ToString(),
                        .PostalCode = row("PostalCode").ToString(),
                        .CustomerType = row("CustomerType").ToString(),
                        .LoyaltyPoints = CInt(row("LoyaltyPoints")),
                        .TotalSpent = CDec(row("TotalSpent"))
                    }
                End If
            Catch ex As Exception
                MessageBox.Show("Error loading customer: " & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Add new customer
        ''' </summary>
        Public Shared Function AddCustomer(customer As Customer) As Boolean
            Try
                Dim query As String = "INSERT INTO Customers (FirstName, LastName, PhoneNumber, Email, Address, City, State, PostalCode, CustomerType, LoyaltyPoints) VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Address, @City, @State, @PostalCode, @CustomerType, 0)"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@FirstName", customer.FirstName),
                    New SqlParameter("@LastName", customer.LastName),
                    New SqlParameter("@PhoneNumber", customer.PhoneNumber),
                    New SqlParameter("@Email", If(String.IsNullOrEmpty(customer.Email), DBNull.Value, customer.Email)),
                    New SqlParameter("@Address", If(String.IsNullOrEmpty(customer.Address), DBNull.Value, customer.Address)),
                    New SqlParameter("@City", If(String.IsNullOrEmpty(customer.City), DBNull.Value, customer.City)),
                    New SqlParameter("@State", If(String.IsNullOrEmpty(customer.State), DBNull.Value, customer.State)),
                    New SqlParameter("@PostalCode", If(String.IsNullOrEmpty(customer.PostalCode), DBNull.Value, customer.PostalCode)),
                    New SqlParameter("@CustomerType", customer.CustomerType)
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error adding customer: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Update customer
        ''' </summary>
        Public Shared Function UpdateCustomer(customer As Customer) As Boolean
            Try
                Dim query As String = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Address = @Address, City = @City, State = @State, PostalCode = @PostalCode, CustomerType = @CustomerType WHERE CustomerId = @CustomerId"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@CustomerId", customer.CustomerId),
                    New SqlParameter("@FirstName", customer.FirstName),
                    New SqlParameter("@LastName", customer.LastName),
                    New SqlParameter("@Email", If(String.IsNullOrEmpty(customer.Email), DBNull.Value, customer.Email)),
                    New SqlParameter("@Address", If(String.IsNullOrEmpty(customer.Address), DBNull.Value, customer.Address)),
                    New SqlParameter("@City", If(String.IsNullOrEmpty(customer.City), DBNull.Value, customer.City)),
                    New SqlParameter("@State", If(String.IsNullOrEmpty(customer.State), DBNull.Value, customer.State)),
                    New SqlParameter("@PostalCode", If(String.IsNullOrEmpty(customer.PostalCode), DBNull.Value, customer.PostalCode)),
                    New SqlParameter("@CustomerType", customer.CustomerType)
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error updating customer: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Delete customer
        ''' </summary>
        Public Shared Function DeleteCustomer(customerId As Integer) As Boolean
            Try
                Dim query As String = "DELETE FROM Customers WHERE CustomerId = @CustomerId"
                Dim param = New SqlParameter("@CustomerId", customerId)
                DatabaseConnection.ExecuteNonQuery(query, param)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error deleting customer: " & ex.Message)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Search customers by name or phone
        ''' </summary>
        Public Shared Function SearchCustomers(searchTerm As String) As List(Of Customer)
            Dim customers As New List(Of Customer)
            Try
                Dim query As String = "SELECT CustomerId, FirstName, LastName, PhoneNumber, Email, Address, City, State, PostalCode, CustomerType, LoyaltyPoints, TotalSpent FROM Customers WHERE FirstName LIKE @SearchTerm OR LastName LIKE @SearchTerm OR PhoneNumber LIKE @SearchTerm ORDER BY FirstName, LastName"
                Dim param = New SqlParameter("@SearchTerm", "%" & searchTerm & "%")
                Dim dt = DatabaseConnection.ExecuteQuery(query, param)
                For Each row In dt.Rows
                    customers.Add(New Customer With {
                        .CustomerId = CInt(row("CustomerId")),
                        .FirstName = row("FirstName").ToString(),
                        .LastName = row("LastName").ToString(),
                        .PhoneNumber = row("PhoneNumber").ToString(),
                        .Email = row("Email").ToString(),
                        .City = row("City").ToString(),
                        .CustomerType = row("CustomerType").ToString(),
                        .TotalSpent = CDec(row("TotalSpent"))
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error searching customers: " & ex.Message)
            End Try
            Return customers
        End Function

        ''' <summary>
        ''' Get VIP customers
        ''' </summary>
        Public Shared Function GetVIPCustomers() As List(Of Customer)
            Dim customers As New List(Of Customer)
            Try
                Dim query As String = "SELECT CustomerId, FirstName, LastName, PhoneNumber, Email, TotalSpent FROM Customers WHERE CustomerType = 'VIP' ORDER BY TotalSpent DESC"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    customers.Add(New Customer With {
                        .CustomerId = CInt(row("CustomerId")),
                        .FirstName = row("FirstName").ToString(),
                        .LastName = row("LastName").ToString(),
                        .PhoneNumber = row("PhoneNumber").ToString(),
                        .Email = row("Email").ToString(),
                        .TotalSpent = CDec(row("TotalSpent"))
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading VIP customers: " & ex.Message)
            End Try
            Return customers
        End Function
    End Class
End Namespace
