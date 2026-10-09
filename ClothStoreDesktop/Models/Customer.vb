Imports ClothStoreDesktop.Data

Namespace ClothStoreDesktop.Models
    Public Class Customer
        Public Property CustomerId As Integer
        Public Property FirstName As String = String.Empty
        Public Property LastName As String = String.Empty
        Public Property PhoneNumber As String = String.Empty
        Public Property Email As String = String.Empty
        Public Property Address As String = String.Empty
        Public Property City As String = String.Empty
        Public Property State As String = String.Empty
        Public Property PostalCode As String = String.Empty
        Public Property CustomerType As String = "Regular"
        Public Property LoyaltyPoints As Integer = 0
        Public Property DateOfBirth As DateTime?
        Public Property Gender As String = String.Empty
        Public Property CreatedDate As DateTime = DateTime.Now
        Public Property LastPurchaseDate As DateTime?
        Public Property TotalSpent As Decimal = 0
    End Class
End Namespace
