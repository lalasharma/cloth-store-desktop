Imports System.Data.SqlClient
Imports ClothStoreDesktop.Models

Namespace ClothStoreDesktop.Data
    Public Class CategoryRepository
        ''' <summary>
        ''' Get all categories
        ''' </summary>
        Public Shared Function GetAllCategories() As List(Of Category)
            Dim categories As New List(Of Category)
            Try
                Dim query As String = "SELECT CategoryId, CategoryName, Description FROM Categories WHERE IsActive = 1 ORDER BY CategoryName"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    categories.Add(New Category With {
                        .CategoryId = CInt(row("CategoryId")),
                        .CategoryName = row("CategoryName").ToString(),
                        .Description = row("Description").ToString()
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading categories: " & ex.Message)
            End Try
            Return categories
        End Function

        ''' <summary>
        ''' Add new category
        ''' </summary>
        Public Shared Function AddCategory(categoryName As String, description As String) As Boolean
            Try
                Dim query As String = "INSERT INTO Categories (CategoryName, Description, IsActive) VALUES (@CategoryName, @Description, 1)"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@CategoryName", categoryName),
                    New SqlParameter("@Description", If(String.IsNullOrEmpty(description), DBNull.Value, description))
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error adding category: " & ex.Message)
                Return False
            End Try
        End Function
    End Class
End Namespace
