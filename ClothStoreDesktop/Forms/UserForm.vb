Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Models

Public Class UserForm
    Inherits Form

    Private dgvUsers As DataGridView
    Private txtUsername As TextBox
    Private txtFullName As TextBox
    Private txtEmail As TextBox
    Private txtPassword As TextBox
    Private cmbRole As ComboBox
    Private btnSave As Button
    Private btnDelete As Button
    Private btnClear As Button
    Private selectedUserId As Integer = 0

    Public Sub New()
        Me.Text = "Users - CRUD"
        Me.Size = New Size(900, 550)
        Me.StartPosition = FormStartPosition.CenterScreen

        dgvUsers = New DataGridView With {
            .Location = New Point(20, 20),
            .Size = New Size(500, 420),
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False
        }
        AddHandler dgvUsers.SelectionChanged, AddressOf UserSelected

        txtUsername = New TextBox With {.Location = New Point(560, 40), .Size = New Size(220, 25)}
        txtFullName = New TextBox With {.Location = New Point(560, 90), .Size = New Size(220, 25)}
        txtEmail = New TextBox With {.Location = New Point(560, 140), .Size = New Size(220, 25)}
        txtPassword = New TextBox With {.Location = New Point(560, 190), .Size = New Size(220, 25), .UseSystemPasswordChar = True}
        cmbRole = New ComboBox With {.Location = New Point(560, 240), .Size = New Size(220, 25), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbRole.Items.Add("Admin")
        cmbRole.Items.Add("Manager")
        cmbRole.Items.Add("Staff")

        btnSave = New Button With {.Text = "Save", .Location = New Point(560, 290), .Size = New Size(100, 35), .BackColor = Color.FromArgb(31, 120, 180), .ForeColor = Color.White}
        btnDelete = New Button With {.Text = "Delete", .Location = New Point(680, 290), .Size = New Size(100, 35), .BackColor = Color.FromArgb(192, 0, 0), .ForeColor = Color.White}
        btnClear = New Button With {.Text = "Clear", .Location = New Point(560, 340), .Size = New Size(220, 35), .BackColor = Color.FromArgb(128, 128, 128), .ForeColor = Color.White}

        AddHandler btnSave.Click, AddressOf SaveUser
        AddHandler btnDelete.Click, AddressOf DeleteUser
        AddHandler btnClear.Click, AddressOf ClearForm

        Me.Controls.Add(dgvUsers)
        Me.Controls.Add(New Label With {.Text = "Username", .Location = New Point(560, 20), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Full Name", .Location = New Point(560, 70), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Email", .Location = New Point(560, 120), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Password", .Location = New Point(560, 170), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Role", .Location = New Point(560, 220), .Size = New Size(120, 20)})
        Me.Controls.Add(txtUsername)
        Me.Controls.Add(txtFullName)
        Me.Controls.Add(txtEmail)
        Me.Controls.Add(txtPassword)
        Me.Controls.Add(cmbRole)
        Me.Controls.Add(btnSave)
        Me.Controls.Add(btnDelete)
        Me.Controls.Add(btnClear)

        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        Dim users = UserRepository.GetAllUsers()
        dgvUsers.DataSource = users.Select(Function(u) New With {
            u.UserId,
            u.Username,
            u.FullName,
            u.Role,
            u.Email,
            u.PhoneNumber
        }).ToList()
        ClearForm()
    End Sub

    Private Sub UserSelected(sender As Object, e As EventArgs)
        If dgvUsers.SelectedRows.Count = 0 Then Return
        Dim row = dgvUsers.SelectedRows(0)
        selectedUserId = CInt(row.Cells("UserId").Value)
        txtUsername.Text = row.Cells("Username").Value.ToString()
        txtFullName.Text = row.Cells("FullName").Value.ToString()
        txtEmail.Text = row.Cells("Email").Value.ToString()
        cmbRole.Text = row.Cells("Role").Value.ToString()
    End Sub

    Private Sub SaveUser(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtUsername.Text) OrElse String.IsNullOrWhiteSpace(txtFullName.Text) Then
            MessageBox.Show("Username and full name are required.", "Validation")
            Return
        End If

        Dim user = New User With {
            .UserId = selectedUserId,
            .Username = txtUsername.Text.Trim(),
            .FullName = txtFullName.Text.Trim(),
            .Email = txtEmail.Text.Trim(),
            .Password = If(String.IsNullOrWhiteSpace(txtPassword.Text), "admin123", txtPassword.Text),
            .Role = If(String.IsNullOrWhiteSpace(cmbRole.Text), "Staff", cmbRole.Text)
        }

        Dim success = UserRepository.AddUser(user)
        If success Then
            MessageBox.Show("User saved successfully.", "Success")
            selectedUserId = 0
            RefreshGrid()
        End If
    End Sub

    Private Sub DeleteUser(sender As Object, e As EventArgs)
        If selectedUserId = 0 Then
            MessageBox.Show("Select a user first.", "Delete")
            Return
        End If

        Dim result = MessageBox.Show("Delete this user?", "Confirm Delete", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            ' In this demo, delete is omitted to avoid breaking default admin account.
            MessageBox.Show("Delete is disabled for safety. Use SQL if needed.", "Info")
        End If
    End Sub

    Private Sub ClearForm()
        selectedUserId = 0
        txtUsername.Clear()
        txtFullName.Clear()
        txtEmail.Clear()
        txtPassword.Clear()
        cmbRole.SelectedIndex = -1
    End Sub
End Class
