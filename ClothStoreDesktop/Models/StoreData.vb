Imports System.Text.Json

Namespace ClothStoreDesktop.Models
    Public Class StoreData
        Public Property Products As List(Of Product) = New List(Of Product)()
        Public Property Sales As List(Of SaleRecord) = New List(Of SaleRecord)()
        Public Property Customers As List(Of Customer) = New List(Of Customer)()
        Public Property Categories As List(Of Category) = New List(Of Category)()
        Public Property Suppliers As List(Of Supplier) = New List(Of Supplier)()
        Public Property Users As List(Of User) = New List(Of User)()
        Public Property PaymentMethods As List(Of PaymentMethod) = New List(Of PaymentMethod)()
    End Class
End Namespace
