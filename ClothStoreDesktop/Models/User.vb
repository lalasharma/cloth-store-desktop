Namespace ClothStoreDesktop.Models
    Public Class User
        Public Property UserId As Integer
        Public Property Username As String = String.Empty
        Public Property Password As String = String.Empty
        Public Property FullName As String = String.Empty
        Public Property Email As String = String.Empty
        Public Property Role As String = "Staff"
        Public Property IsActive As Boolean = True
        Public Property CreatedDate As DateTime = DateTime.Now
        Public Property LastLoginDate As DateTime?
        Public Property PhoneNumber As String = String.Empty
    End Class
End Namespace
