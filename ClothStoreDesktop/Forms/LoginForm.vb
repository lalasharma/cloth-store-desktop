Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Models

Public Class LoginForm
    Inherits Form

    Private lblTitle As Label
    Private lblUserName As Label
    Private txtUserName As TextBox
    Private lblPassword As Label
    Private txtPassword As TextBox
    Private btnLogin As Button
    Private lblInfo As Label

    Public Sub New()
        Me.Text = "Cloth Store - Login"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Width = 420
        Me.Height = 320
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.BackColor = Color.FromArgb(240, 240, 240)

        lblTitle = New Label With {
            .Text = "Cloth Store Management",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .Location = New Point(80, 20),
            .Size = New Size(260, 35),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        lblUserName = New Label With {.Text = "Username", .Location = New Point(60, 70), .Size = New Size(80, 25), .Font = New Font("Segoe UI", 10)}
        txtUserName = New TextBox With {.Location = New Point(150, 70), .Size = New Size(180, 25), .Text = "admin"}

        lblPassword = New Label With {.Text = "Password", .Location = New Point(60, 110), .Size = New Size(80, 25), .Font = New Font("Segoe UI", 10)}
        txtPassword = New TextBox With {.Location = New Point(150, 110), .Size = New Size(180, 25), .UseSystemPasswordChar = True, .Text = "admin"}

        btnLogin = New Button With {
            .Text = "Login",
            .Location = New Point(150, 160),
            .Size = New Size(180, 40),
            .BackColor = Color.FromArgb(31, 120, 180),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 11, FontStyle.Bold)
        }

        lblInfo = New Label With {
            .Text = "Demo: admin / admin",
            .Location = New Point(80, 220),
            .Size = New Size(260, 35),
            .TextAlign = ContentAlignment.TopCenter,
            .ForeColor = Color.Gray,
            .Font = New Font("Segoe UI", 9)
        }

        AddHandler btnLogin.Click, AddressOf BtnLogin_Click
        AddHandler txtPassword.KeyDown, AddressOf TxtPassword_KeyDown

        Me.Controls.Add(lblTitle)
        Me.Controls.Add(lblUserName)
        Me.Controls.Add(txtUserName)
        Me.Controls.Add(lblPassword)
        Me.Controls.Add(txtPassword)
        Me.Controls.Add(btnLogin)
        Me.Controls.Add(lblInfo)
    End Sub

    Private Sub BtnLogin_Click(sender As Object, e As EventArgs)
        ValidateLogin()
    End Sub

    Private Sub TxtPassword_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Return Then
            ValidateLogin()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub ValidateLogin()
        Dim userName As String = txtUserName.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        If userName = "admin" AndAlso password = "admin" Then
            Dim mainForm = New MainForm()
            mainForm.Show()
            Me.Hide()
        Else
            MessageBox.Show("Invalid username or password." & vbCrLf & "Use admin/admin", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPassword.Clear()
            txtUserName.Focus()
        End If
    End Sub
End Class
