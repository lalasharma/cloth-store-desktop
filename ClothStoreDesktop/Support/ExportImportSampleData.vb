Imports System.IO
Imports System.Text

Public Module ExportImportSampleData
    Public Sub ExportSampleDataToJson()
        Dim json As String = """
{
  "Products": [
    {"Name":"Cotton Shirt","Category":"Men","Size":"M","Color":"Blue","Price":700,"Stock":25,"Barcode":"BAR-CT-001"},
    {"Name":"Denim Jeans","Category":"Men","Size":"L","Color":"Black","Price":1200,"Stock":18,"Barcode":"BAR-DJ-001"},
    {"Name":"Silk Saree","Category":"Women","Size":"Free","Color":"Maroon","Price":1800,"Stock":12,"Barcode":"BAR-SS-001"},
    {"Name":"Kids T-Shirt","Category":"Kids","Size":"S","Color":"Green","Price":450,"Stock":35,"Barcode":"BAR-KT-001"}
  ],
  "Customers": [
    {"FirstName":"Raj","LastName":"Kumar","Phone":"9123456789","Email":"raj@gmail.com","City":"Mumbai","CustomerType":"Regular"},
    {"FirstName":"Neha","LastName":"Sharma","Phone":"9988776655","Email":"neha@gmail.com","City":"Delhi","CustomerType":"VIP"}
  ]
}
"""

        Dim folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData")
        Directory.CreateDirectory(folder)
        File.WriteAllText(Path.Combine(folder, "sample_data.json"), json, Encoding.UTF8)
    End Sub

    Public Sub ImportSampleDataFromJson()
        Dim filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData", "sample_data.json")
        If Not File.Exists(filePath) Then
            ExportSampleDataToJson()
            Return
        End If

        Dim json = File.ReadAllText(filePath)
        If Not String.IsNullOrWhiteSpace(json) Then
            ' In actual app, deserialize JSON and save to DB.
            MessageBox.Show("Sample data import is ready. Use the form screens to load data.", "Sample Data")
        End If
    End Sub
End Module
