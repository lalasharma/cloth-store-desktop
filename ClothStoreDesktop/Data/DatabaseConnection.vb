Imports System.Data.SqlClient
Imports System.Configuration

Namespace ClothStoreDesktop.Data
    Public Class DatabaseConnection
        ' Connection string - Update with your SQL Server instance
        Public Shared ReadOnly ConnectionString As String = "Server=(LocalDB)\MSSQLLocalDB;Database=ClothStoreDB;Integrated Security=true;Connection Timeout=30"

        ''' <summary>
        ''' Test database connection
        ''' </summary>
        Public Shared Function TestConnection() As Boolean
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    Dim command As New SqlCommand("SELECT 1", connection)
                    command.ExecuteScalar()
                    connection.Close()
                    Return True
                End Using
            Catch ex As Exception
                MessageBox.Show("Connection Error: " & ex.Message, "Database Connection Failed")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Get a new database connection
        ''' </summary>
        Public Shared Function GetConnection() As SqlConnection
            Return New SqlConnection(ConnectionString)
        End Function

        ''' <summary>
        ''' Execute a non-query command (INSERT, UPDATE, DELETE)
        ''' </summary>
        Public Shared Function ExecuteNonQuery(query As String, ParamArray parameters As SqlParameter()) As Integer
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    Using command As New SqlCommand(query, connection)
                        If parameters IsNot Nothing Then
                            command.Parameters.AddRange(parameters)
                        End If
                        Return command.ExecuteNonQuery()
                    End Using
                End Using
            Catch ex As Exception
                Throw New Exception("Error executing query: " & ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Execute a scalar query (returns single value)
        ''' </summary>
        Public Shared Function ExecuteScalar(query As String, ParamArray parameters As SqlParameter()) As Object
            Try
                Using connection As New SqlConnection(ConnectionString)
                    connection.Open()
                    Using command As New SqlCommand(query, connection)
                        If parameters IsNot Nothing Then
                            command.Parameters.AddRange(parameters)
                        End If
                        Return command.ExecuteScalar()
                    End Using
                End Using
            Catch ex As Exception
                Throw New Exception("Error executing scalar query: " & ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Execute a query and return DataTable
        ''' </summary>
        Public Shared Function ExecuteQuery(query As String, ParamArray parameters As SqlParameter()) As DataTable
            Dim dt As New DataTable()
            Try
                Using connection As New SqlConnection(ConnectionString)
                    Using adapter As New SqlDataAdapter(query, connection)
                        If parameters IsNot Nothing Then
                            adapter.SelectCommand.Parameters.AddRange(parameters)
                        End If
                        adapter.Fill(dt)
                    End Using
                End Using
                Return dt
            Catch ex As Exception
                Throw New Exception("Error executing query: " & ex.Message)
            End Try
        End Function
    End Class
End Namespace
