Imports System.Text.Json
Imports ClothStoreDesktop.Data

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

    Public Class StoreDataManager
        Private Shared ReadOnly DataFilePath As String = Path.Combine(AppContext.BaseDirectory, "Data", "cloth_store_data.json")

        Public Shared Function LoadStoreData() As StoreData
            EnsureDataFileExists()
            Dim json As String = File.ReadAllText(DataFilePath)
            If String.IsNullOrWhiteSpace(json) Then Return New StoreData()

            Try
                Dim data = JsonSerializer.Deserialize(Of StoreData)(json)
                If data Is Nothing Then Return New StoreData()
                If data.Products Is Nothing Then data.Products = New List(Of Product)()
                If data.Sales Is Nothing Then data.Sales = New List(Of SaleRecord)()
                If data.Customers Is Nothing Then data.Customers = New List(Of Customer)()
                Return data
            Catch
                Return New StoreData()
            End Try
        End Function

        Public Shared Sub SaveStoreData(data As StoreData)
            Dim folder = Path.GetDirectoryName(DataFilePath)
            If Not String.IsNullOrEmpty(folder) Then Directory.CreateDirectory(folder)

            Dim json = JsonSerializer.Serialize(data, New JsonSerializerOptions With {.WriteIndented = True})
            File.WriteAllText(DataFilePath, json)
        End Sub

        Private Shared Sub EnsureDataFileExists()
            Dim folder = Path.GetDirectoryName(DataFilePath)
            If Not String.IsNullOrEmpty(folder) Then Directory.CreateDirectory(folder)

            If File.Exists(DataFilePath) Then Return

            Dim initialData = New StoreData With {
                .Products = New List(Of Product) From {
                    New Product With {.Id = 1, .Name = "Cotton Shirt", .Category = "Men", .Size = "M", .Color = "Blue", .Price = 750D, .Stock = 25, .Supplier = "Threads Ltd", .Barcode = "CS-001"},
                    New Product With {.Id = 2, .Name = "Denim Jeans", .Category = "Men", .Size = "L", .Color = "Black", .Price = 1200D, .Stock = 18, .Supplier = "Urban Weave", .Barcode = "DJ-008"},
                    New Product With {.Id = 3, .Name = "Silk Saree", .Category = "Women", .Size = "Free", .Color = "Maroon", .Price = 1800D, .Stock = 12, .Supplier = "Royal Fabric", .Barcode = "SS-105"},
                    New Product With {.Id = 4, .Name = "Kids T-Shirt", .Category = "Kids", .Size = "S", .Color = "Green", .Price = 500D, .Stock = 35, .Supplier = "Little Thread", .Barcode = "KT-214"},
                    New Product With {.Id = 5, .Name = "Formal Blazer", .Category = "Women", .Size = "M", .Color = "Navy", .Price = 2400D, .Stock = 8, .Supplier = "Grace Tailors", .Barcode = "FB-519"}
                },
                .Sales = New List(Of SaleRecord)(),
                .Customers = New List(Of Customer)()
            }

            SaveStoreData(initialData)
        End Sub
    End Class
End Namespace
