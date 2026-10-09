Imports System.Data.SqlClient
Imports ClothStoreDesktop.Models

Namespace ClothStoreDesktop.Data
    Public Class UserRepository
        ''' <summary>
        ''' Authenticate user
        ''' </summary>
        Public Shared Function AuthenticateUser(username As String, password As String) As User
            Try
                Dim query As String = "SELECT UserId, Username, FullName, Email, Role, IsActive FROM Users WHERE Username = @Username AND Password = @Password AND IsActive = 1"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@Username", username),
                    New SqlParameter("@Password", password)
                }
                Dim dt = DatabaseConnection.ExecuteQuery(query, parameters)
                If dt.Rows.Count > 0 Then
                    Dim row = dt.Rows(0)
                    ' Update last login date
                    Dim updateQuery As String = "UPDATE Users SET LastLoginDate = GETDATE() WHERE UserId = @UserId"
                    Dim updateParam = New SqlParameter("@UserId", CInt(row("UserId")))
                    DatabaseConnection.ExecuteNonQuery(updateQuery, updateParam)

                    Return New User With {
                        .UserId = CInt(row("UserId")),
                        .Username = row("Username").ToString(),
                        .FullName = row("FullName").ToString(),
                        .Email = row("Email").ToString(),
                        .Role = row("Role").ToString(),
                        .IsActive = True
                    }
                End If
            Catch ex As Exception
                MessageBox.Show("Error authenticating user: " & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Get all users
        ''' </summary>
        Public Shared Function GetAllUsers() As List(Of User)
            Dim users As New List(Of User)
            Try
                Dim query As String = "SELECT UserId, Username, FullName, Email, Role, IsActive, CreatedDate, PhoneNumber FROM Users ORDER BY FullName"
                Dim dt = DatabaseConnection.ExecuteQuery(query)
                For Each row In dt.Rows
                    users.Add(New User With {
                        .UserId = CInt(row("UserId")),
                        .Username = row("Username").ToString(),
                        .FullName = row("FullName").ToString(),
                        .Email = row("Email").ToString(),
                        .Role = row("Role").ToString(),
                        .IsActive = CBool(row("IsActive")),
                        .PhoneNumber = row("PhoneNumber").ToString()
                    })
                Next
            Catch ex As Exception
                MessageBox.Show("Error loading users: " & ex.Message)
            End Try
            Return users
        End Function

        ''' <summary>
        ''' Add new user
        ''' </summary>
        Public Shared Function AddUser(user As User) As Boolean
            Try
                Dim query As String = "INSERT INTO Users (Username, Password, FullName, Email, Role, PhoneNumber, IsActive) VALUES (@Username, @Password, @FullName, @Email, @Role, @PhoneNumber, 1)"
                Dim parameters = New SqlParameter() {
                    New SqlParameter("@Username", user.Username),
                    New SqlParameter("@Password", user.Password),
                    New SqlParameter("@FullName", user.FullName),
                    New SqlParameter("@Email", If(String.IsNullOrEmpty(user.Email), DBNull.Value, user.Email)),
                    New SqlParameter("@Role", user.Role),
                    New SqlParameter("@PhoneNumber", If(String.IsNullOrEmpty(user.PhoneNumber), DBNull.Value, user.PhoneNumber))
                }
                DatabaseConnection.ExecuteNonQuery(query, parameters)
                Return True
            Catch ex As Exception
                MessageBox.Show("Error adding user: " & ex.Message)
                Return False
            End Try
        End Function
    End Class
End Namespace
