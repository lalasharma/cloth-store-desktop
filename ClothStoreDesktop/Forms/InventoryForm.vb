Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Models

Public Class InventoryForm
    Inherits Form

    Private dgvProducts As DataGridView
    Private txtName As TextBox
    Private txtCategory As TextBox
    Private txtSize As TextBox
    Private txtColor As TextBox
    Private txtPrice As TextBox
    Private txtStock As TextBox
    Private txtSupplier As TextBox
    Private txtBarcode As TextBox
    Private btnAdd As Button
    Private btnUpdate As Button
    Private btnDelete As Button
    Private btnRefresh As Button
    Private lblSearch As Label
    Private txtSearch As TextBox
    Private lblStockWarning As Label

    Public Sub New()
        Me.Text = "Inventory Management"
        Me.Size = New Size(1200, 720)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 240, 240)

        lblSearch = New Label With {.Text = "Search Product:", .Location = New Point(20, 20), .Size = New Size(100, 25), .Font = New Font("Segoe UI", 10)}
        txtSearch = New TextBox With {.Location = New Point(120, 20), .Size = New Size(250, 25)}

        dgvProducts = New DataGridView With {
            .Location = New Point(20, 60),
            .Size = New Size(750, 500),
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .AutoGenerateColumns = True,
            .BackgroundColor = Color.White
        }

        Dim leftX As Integer = 800
        Dim fieldWidth As Integer = 200
        Dim fieldHeight As Integer = 25
        Dim startY As Integer = 20

        Dim labels = New String() {"Name:", "Category:", "Size:", "Color:", "Price:", "Stock:", "Supplier:", "Barcode:"}
        Dim textBoxes = New List(Of TextBox)()

        For i = 0 To labels.Length - 1
            Dim label = New Label With {.Text = labels(i), .Location = New Point(leftX, startY + (i * 45)), .Size = New Size(70, 25), .Font = New Font("Segoe UI", 9)}
            Me.Controls.Add(label)

            Dim textBox = New TextBox With {.Location = New Point(leftX + 80, startY + (i * 45)), .Size = New Size(fieldWidth, fieldHeight)}
            textBoxes.Add(textBox)
            Me.Controls.Add(textBox)
        Next

        txtName = textBoxes(0)
        txtCategory = textBoxes(1)
        txtSize = textBoxes(2)
        txtColor = textBoxes(3)
        txtPrice = textBoxes(4)
        txtStock = textBoxes(5)
        txtSupplier = textBoxes(6)
        txtBarcode = textBoxes(7)

        btnAdd = New Button With {.Text = "Add", .Location = New Point(leftX, 440), .Size = New Size(90, 35), .BackColor = Color.FromArgb(0, 128, 0), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        btnUpdate = New Button With {.Text = "Update", .Location = New Point(leftX + 95, 440), .Size = New Size(90, 35), .BackColor = Color.FromArgb(31, 120, 180), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        btnDelete = New Button With {.Text = "Delete", .Location = New Point(leftX + 190, 440), .Size = New Size(90, 35), .BackColor = Color.FromArgb(192, 0, 0), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        btnRefresh = New Button With {.Text = "Refresh", .Location = New Point(leftX, 490), .Size = New Size(275, 35), .BackColor = Color.FromArgb(128, 128, 128), .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}

        lblStockWarning = New Label With {
            .Location = New Point(20, 600),
            .Size = New Size(750, 70),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.FromArgb(255, 240, 200),
            .Font = New Font("Segoe UI", 9),
            .Text = "Stock Warning: Items with stock below 10 units are at risk of stockout."
        }

        AddHandler dgvProducts.SelectionChanged, AddressOf DgvProducts_SelectionChanged
        AddHandler btnAdd.Click, AddressOf BtnAdd_Click
        AddHandler btnUpdate.Click, AddressOf BtnUpdate_Click
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        AddHandler btnRefresh.Click, AddressOf BtnRefresh_Click
        AddHandler txtSearch.TextChanged, AddressOf TxtSearch_TextChanged

        Me.Controls.Add(lblSearch)
        Me.Controls.Add(txtSearch)
        Me.Controls.Add(dgvProducts)
        Me.Controls.Add(btnAdd)
        Me.Controls.Add(btnUpdate)
        Me.Controls.Add(btnDelete)
        Me.Controls.Add(btnRefresh)
        Me.Controls.Add(lblStockWarning)

        RefreshGrid()
    End Sub

    Private Sub RefreshGrid(Optional searchTerm As String = "")
        Dim data = StoreDataManager.LoadStoreData()
        Dim products = data.Products.OrderBy(Function(p) p.Id).ToList()

        If Not String.IsNullOrWhiteSpace(searchTerm) Then
            products = products.Where(Function(p) p.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
        End If

        dgvProducts.DataSource = products
        ClearForm()
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
        RefreshGrid(txtSearch.Text.Trim())
    End Sub

    Private Sub DgvProducts_SelectionChanged(sender As Object, e As EventArgs)
        If dgvProducts.SelectedRows.Count = 0 Then Return

        Dim product = CType(dgvProducts.SelectedRows(0).DataBoundItem, Product)
        If product Is Nothing Then Return

        txtName.Text = product.Name
        txtCategory.Text = product.Category
        txtSize.Text = product.Size
        txtColor.Text = product.Color
        txtPrice.Text = product.Price.ToString()
        txtStock.Text = product.Stock.ToString()
        txtSupplier.Text = product.Supplier
        txtBarcode.Text = product.Barcode
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs)
        Dim product = BuildProductFromForm()
        If product Is Nothing Then Return

        Dim data = StoreDataManager.LoadStoreData()
        product.Id = If(data.Products.Any(), data.Products.Max(Function(p) p.Id) + 1, 1)
        data.Products.Add(product)
        StoreDataManager.SaveStoreData(data)
        RefreshGrid()
        MessageBox.Show($"Product '{product.Name}' added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs)
        If dgvProducts.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim product = BuildProductFromForm()
        If product Is Nothing Then Return

        Dim selectedId = CInt(dgvProducts.SelectedRows(0).Cells("Id").Value)
        Dim data = StoreDataManager.LoadStoreData()
        Dim targetProduct = data.Products.FirstOrDefault(Function(p) p.Id = selectedId)

        If targetProduct Is Nothing Then
            MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        targetProduct.Name = product.Name
        targetProduct.Category = product.Category
        targetProduct.Size = product.Size
        targetProduct.Color = product.Color
        targetProduct.Price = product.Price
        targetProduct.Stock = product.Stock
        targetProduct.Supplier = product.Supplier
        targetProduct.Barcode = product.Barcode

        StoreDataManager.SaveStoreData(data)
        RefreshGrid()
        MessageBox.Show("Product updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        If dgvProducts.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a product to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim selectedId = CInt(dgvProducts.SelectedRows(0).Cells("Id").Value)
        Dim data = StoreDataManager.LoadStoreData()
        Dim target = data.Products.FirstOrDefault(Function(p) p.Id = selectedId)

        If target Is Nothing Then
            MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result = MessageBox.Show($"Delete '{target.Name}' from inventory?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            data.Products.Remove(target)
            StoreDataManager.SaveStoreData(data)
            RefreshGrid()
            MessageBox.Show("Product deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function BuildProductFromForm() As Product
        Dim name = txtName.Text.Trim()
        Dim category = txtCategory.Text.Trim()
        Dim size = txtSize.Text.Trim()
        Dim color = txtColor.Text.Trim()
        Dim supplier = txtSupplier.Text.Trim()
        Dim barcode = txtBarcode.Text.Trim()

        If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(category) OrElse String.IsNullOrWhiteSpace(size) Then
            MessageBox.Show("Name, category, and size are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        Dim priceValue As Decimal
        Dim stockValue As Integer

        If Not Decimal.TryParse(txtPrice.Text, priceValue) OrElse priceValue <= 0 Then
            MessageBox.Show("Price must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        If Not Integer.TryParse(txtStock.Text, stockValue) OrElse stockValue < 0 Then
            MessageBox.Show("Stock must be a valid non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        Return New Product With {
            .Name = name,
            .Category = category,
            .Size = size,
            .Color = If(String.IsNullOrWhiteSpace(color), "N/A", color),
            .Price = priceValue,
            .Stock = stockValue,
            .Supplier = If(String.IsNullOrWhiteSpace(supplier), "Unknown", supplier),
            .Barcode = If(String.IsNullOrWhiteSpace(barcode), "N/A", barcode)
        }
    End Function

    Private Sub ClearForm()
        txtName.Clear()
        txtCategory.Clear()
        txtSize.Clear()
        txtColor.Clear()
        txtPrice.Clear()
        txtStock.Clear()
        txtSupplier.Clear()
        txtBarcode.Clear()
    End Sub
End Class
