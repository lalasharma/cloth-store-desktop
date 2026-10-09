Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Models

Public Class CustomerForm
    Inherits Form

    Private dgvCustomers As DataGridView
    Private txtSearch As TextBox
    Private txtFirstName As TextBox
    Private txtLastName As TextBox
    Private txtPhone As TextBox
    Private txtEmail As TextBox
    Private txtCity As TextBox
    Private txtAddress As TextBox
    Private cmbType As ComboBox
    Private btnSave As Button
    Private btnDelete As Button
    Private btnClear As Button
    Private selectedCustomerId As Integer = 0

    Public Sub New()
        Me.Text = "Customers - CRUD"
        Me.Size = New Size(1200, 650)
        Me.StartPosition = FormStartPosition.CenterScreen

        txtSearch = New TextBox With {.Location = New Point(20, 20), .Size = New Size(220, 25), .PlaceholderText = "Search customer..."}
        AddHandler txtSearch.TextChanged, AddressOf SearchCustomers

        dgvCustomers = New DataGridView With {
            .Location = New Point(20, 60),
            .Size = New Size(760, 500),
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False
        }
        AddHandler dgvCustomers.SelectionChanged, AddressOf CustomerSelected

        txtFirstName = New TextBox With {.Location = New Point(820, 60), .Size = New Size(220, 25)}
        txtLastName = New TextBox With {.Location = New Point(820, 110), .Size = New Size(220, 25)}
        txtPhone = New TextBox With {.Location = New Point(820, 160), .Size = New Size(220, 25)}
        txtEmail = New TextBox With {.Location = New Point(820, 210), .Size = New Size(220, 25)}
        txtCity = New TextBox With {.Location = New Point(820, 260), .Size = New Size(220, 25)}
        txtAddress = New TextBox With {.Location = New Point(820, 310), .Size = New Size(220, 90)}
        cmbType = New ComboBox With {.Location = New Point(820, 420), .Size = New Size(220, 25), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbType.Items.Add("Regular")
        cmbType.Items.Add("VIP")
        cmbType.Items.Add("Bulk")

        btnSave = New Button With {.Text = "Save", .Location = New Point(820, 470), .Size = New Size(100, 35), .BackColor = Color.FromArgb(31, 120, 180), .ForeColor = Color.White}
        btnDelete = New Button With {.Text = "Delete", .Location = New Point(940, 470), .Size = New Size(100, 35), .BackColor = Color.FromArgb(192, 0, 0), .ForeColor = Color.White}
        btnClear = New Button With {.Text = "Clear", .Location = New Point(820, 520), .Size = New Size(100, 35), .BackColor = Color.FromArgb(128, 128, 128), .ForeColor = Color.White}

        AddHandler btnSave.Click, AddressOf SaveCustomer
        AddHandler btnDelete.Click, AddressOf DeleteCustomer
        AddHandler btnClear.Click, AddressOf ClearForm

        Me.Controls.Add(txtSearch)
        Me.Controls.Add(dgvCustomers)
        Me.Controls.Add(New Label With {.Text = "First Name", .Location = New Point(820, 40), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Last Name", .Location = New Point(820, 90), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Phone", .Location = New Point(820, 140), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Email", .Location = New Point(820, 190), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "City", .Location = New Point(820, 240), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Address", .Location = New Point(820, 290), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Customer Type", .Location = New Point(820, 400), .Size = New Size(120, 20)})

        Me.Controls.Add(txtFirstName)
        Me.Controls.Add(txtLastName)
        Me.Controls.Add(txtPhone)
        Me.Controls.Add(txtEmail)
        Me.Controls.Add(txtCity)
        Me.Controls.Add(txtAddress)
        Me.Controls.Add(cmbType)
        Me.Controls.Add(btnSave)
        Me.Controls.Add(btnDelete)
        Me.Controls.Add(btnClear)

        RefreshGrid()
    End Sub

    Private Sub RefreshGrid()
        Dim customers = CustomerRepository.GetAllCustomers()
        dgvCustomers.DataSource = customers.Select(Function(c) New With {
            c.CustomerId,
            c.FirstName,
            c.LastName,
            c.PhoneNumber,
            c.Email,
            c.City,
            c.CustomerType,
            c.TotalSpent
        }).ToList()
        ClearForm()
    End Sub

    Private Sub SearchCustomers(sender As Object, e As EventArgs)
        Dim keyword = txtSearch.Text.Trim()
        Dim customers = CustomerRepository.SearchCustomers(keyword)
        dgvCustomers.DataSource = customers.Select(Function(c) New With {
            c.CustomerId,
            c.FirstName,
            c.LastName,
            c.PhoneNumber,
            c.Email,
            c.City,
            c.CustomerType,
            c.TotalSpent
        }).ToList()
    End Sub

    Private Sub CustomerSelected(sender As Object, e As EventArgs)
        If dgvCustomers.SelectedRows.Count = 0 Then Return
        Dim row = dgvCustomers.SelectedRows(0)
        selectedCustomerId = CInt(row.Cells("CustomerId").Value)
        txtFirstName.Text = row.Cells("FirstName").Value.ToString()
        txtLastName.Text = row.Cells("LastName").Value.ToString()
        txtPhone.Text = row.Cells("PhoneNumber").Value.ToString()
        txtEmail.Text = row.Cells("Email").Value.ToString()
        txtCity.Text = row.Cells("City").Value.ToString()
        cmbType.Text = row.Cells("CustomerType").Value.ToString()
    End Sub

    Private Sub SaveCustomer(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtFirstName.Text) OrElse String.IsNullOrWhiteSpace(txtLastName.Text) Then
            MessageBox.Show("First name and last name are required.", "Validation")
            Return
        End If

        Dim customer = New Customer With {
            .CustomerId = selectedCustomerId,
            .FirstName = txtFirstName.Text.Trim(),
            .LastName = txtLastName.Text.Trim(),
            .PhoneNumber = txtPhone.Text.Trim(),
            .Email = txtEmail.Text.Trim(),
            .City = txtCity.Text.Trim(),
            .Address = txtAddress.Text.Trim(),
            .CustomerType = If(String.IsNullOrEmpty(cmbType.Text), "Regular", cmbType.Text)
        }

        Dim success As Boolean
        If selectedCustomerId > 0 Then
            success = CustomerRepository.UpdateCustomer(customer)
        Else
            success = CustomerRepository.AddCustomer(customer)
        End If

        If success Then
            MessageBox.Show("Customer saved successfully.", "Success")
            selectedCustomerId = 0
            RefreshGrid()
        End If
    End Sub

    Private Sub DeleteCustomer(sender As Object, e As EventArgs)
        If selectedCustomerId = 0 Then
            MessageBox.Show("Select a customer first.", "Delete")
            Return
        End If

        Dim result = MessageBox.Show("Delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            If CustomerRepository.DeleteCustomer(selectedCustomerId) Then
                MessageBox.Show("Customer deleted.", "Success")
                selectedCustomerId = 0
                RefreshGrid()
            End If
        End If
    End Sub

    Private Sub ClearForm()
        selectedCustomerId = 0
        txtFirstName.Clear()
        txtLastName.Clear()
        txtPhone.Clear()
        txtEmail.Clear()
        txtCity.Clear()
        txtAddress.Clear()
        cmbType.SelectedIndex = -1
    End Sub
End Class
