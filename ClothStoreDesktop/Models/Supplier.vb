Namespace ClothStoreDesktop.Models
    Public Class Supplier
        Public Property SupplierId As Integer
        Public Property SupplierName As String = String.Empty
        Public Property ContactPerson As String = String.Empty
        Public Property PhoneNumber As String = String.Empty
        Public Property Email As String = String.Empty
        Public Property Address As String = String.Empty
        Public Property City As String = String.Empty
        Public Property State As String = String.Empty
        Public Property PaymentTerms As String = String.Empty
        Public Property IsActive As Boolean = True
        Public Property CreatedDate As DateTime = DateTime.Now
    End Class
End Namespace
