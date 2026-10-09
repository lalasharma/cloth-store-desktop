Imports System.Data.SqlClient
Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Models

Public Class ProductForm
    Inherits Form

    Private WithEvents dgvProducts As DataGridView
    Private txtSearch As TextBox
    Private txtProductName As TextBox
    Private txtCategory As ComboBox
    Private txtSize As ComboBox
    Private txtColor As TextBox
    Private txtPrice As TextBox
    Private txtStock As TextBox
    Private txtBarcode As TextBox
    Private btnSave As Button
    Private btnDelete As Button
    Private btnClear As Button
    Private btnRefresh As Button
    Private lblMessage As Label
    Private selectedProductId As Integer = 0

    Public Sub New()
        Me.Text = "Products - CRUD"
        Me.Size = New Size(1200, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 245, 245)

        lblMessage = New Label With {.Location = New Point(20, 20), .Size = New Size(250, 25), .Text = "Products Management"}

        txtSearch = New TextBox With {.Location = New Point(300, 20), .Size = New Size(220, 25), .PlaceholderText = "Search product..."}
        AddHandler txtSearch.TextChanged, AddressOf SearchProducts

        dgvProducts = New DataGridView With {
            .Location = New Point(20, 60),
            .Size = New Size(760, 520),
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        AddHandler dgvProducts.SelectionChanged, AddressOf ProductSelected

        txtProductName = New TextBox With {.Location = New Point(820, 60), .Size = New Size(220, 25)}
        txtCategory = New ComboBox With {.Location = New Point(820, 110), .Size = New Size(220, 25), .DropDownStyle = ComboBoxStyle.DropDownList}
        txtSize = New ComboBox With {.Location = New Point(820, 160), .Size = New Size(220, 25), .DropDownStyle = ComboBoxStyle.DropDownList}
        txtColor = New TextBox With {.Location = New Point(820, 210), .Size = New Size(220, 25)}
        txtPrice = New TextBox With {.Location = New Point(820, 260), .Size = New Size(220, 25)}
        txtStock = New TextBox With {.Location = New Point(820, 310), .Size = New Size(220, 25)}
        txtBarcode = New TextBox With {.Location = New Point(820, 360), .Size = New Size(220, 25)}

        btnSave = New Button With {.Text = "Save", .Location = New Point(820, 420), .Size = New Size(100, 35), .BackColor = Color.FromArgb(31, 120, 180), .ForeColor = Color.White}
        btnDelete = New Button With {.Text = "Delete", .Location = New Point(940, 420), .Size = New Size(100, 35), .BackColor = Color.FromArgb(192, 0, 0), .ForeColor = Color.White}
        btnClear = New Button With {.Text = "Clear", .Location = New Point(820, 470), .Size = New Size(100, 35), .BackColor = Color.FromArgb(128, 128, 128), .ForeColor = Color.White}
        btnRefresh = New Button With {.Text = "Refresh", .Location = New Point(940, 470), .Size = New Size(100, 35), .BackColor = Color.FromArgb(0, 128, 0), .ForeColor = Color.White}

        AddHandler btnSave.Click, AddressOf SaveProduct
        AddHandler btnDelete.Click, AddressOf DeleteProduct
        AddHandler btnClear.Click, AddressOf ClearForm
        AddHandler btnRefresh.Click, AddressOf RefreshGrid

        Me.Controls.Add(lblMessage)
        Me.Controls.Add(txtSearch)
        Me.Controls.Add(dgvProducts)
        Me.Controls.Add(New Label With {.Text = "Product Name", .Location = New Point(820, 40), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Category", .Location = New Point(820, 90), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Size", .Location = New Point(820, 140), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Color", .Location = New Point(820, 190), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Price", .Location = New Point(820, 240), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Stock", .Location = New Point(820, 290), .Size = New Size(120, 20)})
        Me.Controls.Add(New Label With {.Text = "Barcode", .Location = New Point(820, 340), .Size = New Size(120, 20)})
        Me.Controls.Add(txtProductName)
        Me.Controls.Add(txtCategory)
        Me.Controls.Add(txtSize)
        Me.Controls.Add(txtColor)
        Me.Controls.Add(txtPrice)
        Me.Controls.Add(txtStock)
        Me.Controls.Add(txtBarcode)
        Me.Controls.Add(btnSave)
        Me.Controls.Add(btnDelete)
        Me.Controls.Add(btnClear)
        Me.Controls.Add(btnRefresh)

        LoadCategoriesAndSizes()
        RefreshGrid()
    End Sub

    Private Sub LoadCategoriesAndSizes()
        txtCategory.Items.Clear()
        txtCategory.Items.Add("Men")
        txtCategory.Items.Add("Women")
        txtCategory.Items.Add("Kids")
        txtCategory.Items.Add("Accessories")

        txtSize.Items.Clear()
        txtSize.Items.Add("XS")
        txtSize.Items.Add("S")
        txtSize.Items.Add("M")
        txtSize.Items.Add("L")
        txtSize.Items.Add("XL")
        txtSize.Items.Add("Free")
    End Sub

    Private Sub SearchProducts(sender As Object, e As EventArgs)
        Dim keyword = txtSearch.Text.Trim()
        Dim products = ProductRepository.SearchProducts(keyword)
        dgvProducts.DataSource = products.Select(Function(p) New With {
            p.Id,
            p.Name,
            p.Category,
            p.Size,
            p.Color,
            p.Price,
            p.Stock,
            p.Barcode
        }).ToList()
    End Sub

    Private Sub RefreshGrid()
        Dim products = ProductRepository.GetAllProducts()
        dgvProducts.DataSource = products.Select(Function(p) New With {
            p.Id,
            p.Name,
            p.Category,
            p.Size,
            p.Color,
            p.Price,
            p.Stock,
            p.Barcode
        }).ToList()
        ClearForm()
    End Sub

    Private Sub ProductSelected(sender As Object, e As EventArgs)
        If dgvProducts.SelectedRows.Count = 0 Then Return
        Dim row = dgvProducts.SelectedRows(0)
        If row Is Nothing Then Return

        selectedProductId = If(row.Cells("Id").Value IsNot Nothing, CInt(row.Cells("Id").Value), 0)
        txtProductName.Text = row.Cells("Name").Value.ToString()
        txtCategory.Text = row.Cells("Category").Value.ToString()
        txtSize.Text = row.Cells("Size").Value.ToString()
        txtColor.Text = row.Cells("Color").Value.ToString()
        txtPrice.Text = row.Cells("Price").Value.ToString()
        txtStock.Text = row.Cells("Stock").Value.ToString()
        txtBarcode.Text = row.Cells("Barcode").Value.ToString()
    End Sub

    Private Sub SaveProduct(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtProductName.Text) Then
            MessageBox.Show("Product name is required.", "Validation")
            Return
        End If

        Dim product = New Product With {
            .Id = selectedProductId,
            .Name = txtProductName.Text.Trim(),
            .Category = If(String.IsNullOrWhiteSpace(txtCategory.Text), "Men", txtCategory.Text),
            .Size = If(String.IsNullOrWhiteSpace(txtSize.Text), "M", txtSize.Text),
            .Color = txtColor.Text.Trim(),
            .Price = CDec(If(String.IsNullOrEmpty(txtPrice.Text), 0, txtPrice.Text)),
            .Stock = CInt(If(String.IsNullOrEmpty(txtStock.Text), 0, txtStock.Text)),
            .Barcode = If(String.IsNullOrWhiteSpace(txtBarcode.Text), "AUTO-" & DateTime.Now.Ticks.ToString().Substring(0, 8), txtBarcode.Text.Trim())
        }

        Dim success As Boolean
        If selectedProductId > 0 Then
            success = ProductRepository.UpdateProduct(product)
        Else
            success = ProductRepository.AddProduct(product)
        End If

        If success Then
            MessageBox.Show("Product saved successfully.", "Success")
            selectedProductId = 0
            RefreshGrid()
        End If
    End Sub

    Private Sub DeleteProduct(sender As Object, e As EventArgs)
        If selectedProductId = 0 Then
            MessageBox.Show("Select a product first.", "Delete")
            Return
        End If

        Dim result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            If ProductRepository.DeleteProduct(selectedProductId) Then
                MessageBox.Show("Product deleted.", "Success")
                selectedProductId = 0
                RefreshGrid()
            End If
        End If
    End Sub

    Private Sub ClearForm()
        selectedProductId = 0
        txtProductName.Clear()
        txtCategory.SelectedIndex = -1
        txtSize.SelectedIndex = -1
        txtColor.Clear()
        txtPrice.Clear()
        txtStock.Clear()
        txtBarcode.Clear()
    End Sub
End Class
