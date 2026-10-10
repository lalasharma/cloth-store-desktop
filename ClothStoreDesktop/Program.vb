Imports ClothStoreDesktop.Data

Module Program
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        
        Try
            ' Initialize database on startup
            If Not DatabaseInitializer.EnsureDatabase() Then
                MessageBox.Show("Failed to initialize database. Please check your SQL Server connection.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            
            ' Start application with login form
            Application.Run(New LoginForm())
        Catch ex As Exception
            MessageBox.Show("Application startup error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Module
