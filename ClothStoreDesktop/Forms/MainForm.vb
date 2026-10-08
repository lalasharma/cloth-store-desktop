Imports ClothStoreDesktop.Data
Imports ClothStoreDesktop.Forms
Imports ClothStoreDesktop.Models

Public Class MainForm
    Inherits Form

    Private btnInventory As Button
    Private btnSales As Button
    Private btnCustomers As Button
    Private btnReports As Button
    Private btnLogout As Button
    Private lblTitle As Label
    Private lblTotalProducts As Label
    Private lblInventoryValue As Label
    Private lblLowStock As Label
    Private lblSalesToday As Label
    Private lblCustomersTotal As Label
    Private timerRefresh As Timer

    Public Sub New()
        Me.Text = "Cloth Store - Dashboard"
        Me.WindowState = FormWindowState.Maximized
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 245, 245)
        Me.FormClosing += AddressOf MainForm_FormClosing

        lblTitle = New Label With {
            .Text = "Cloth Store Dashboard",
            .Font = New Font("Segoe UI", 18, FontStyle.Bold),
            .Location = New Point(30, 20),
            .Size = New Size(500, 40),
            .ForeColor = Color.FromArgb(31, 120, 180)
        }

        btnInventory = CreateNavButton("📦 Inventory", 30, 80, Color.FromArgb(0, 128, 0))
        btnSales = CreateNavButton("💳 Sales", 230, 80, Color.FromArgb(31, 120, 180))
        btnCustomers = CreateNavButton("👥 Customers", 430, 80, Color.FromArgb(153, 76, 0))
        btnReports = CreateNavButton("📊 Reports", 630, 80, Color.FromArgb(128, 0, 128))
        btnLogout = CreateNavButton("🚪 Logout", 830, 80, Color.FromArgb(192, 0, 0))

        lblTotalProducts = CreateStatBox("📦 Total Products", 30, 180)
        lblInventoryValue = CreateStatBox("💰 Inventory Value", 300, 180)
        lblLowStock = CreateStatBox("⚠️ Low Stock Items", 570, 180)
        lblSalesToday = CreateStatBox("📈 Sales Today", 840, 180)
        lblCustomersTotal = CreateStatBox("👤 Total Customers", 30, 350)

        AddHandler btnInventory.Click, AddressOf BtnInventory_Click
        AddHandler btnSales.Click, AddressOf BtnSales_Click
        AddHandler btnCustomers.Click, AddressOf BtnCustomers_Click
        AddHandler btnReports.Click, AddressOf BtnReports_Click
        AddHandler btnLogout.Click, AddressOf BtnLogout_Click

        Me.Controls.Add(lblTitle)
        Me.Controls.Add(btnInventory)
        Me.Controls.Add(btnSales)
        Me.Controls.Add(btnCustomers)
        Me.Controls.Add(btnReports)
        Me.Controls.Add(btnLogout)
        Me.Controls.Add(lblTotalProducts)
        Me.Controls.Add(lblInventoryValue)
        Me.Controls.Add(lblLowStock)
        Me.Controls.Add(lblSalesToday)
        Me.Controls.Add(lblCustomersTotal)

        timerRefresh = New Timer With {.Interval = 5000}
        AddHandler timerRefresh.Tick, AddressOf TimerRefresh_Tick
        timerRefresh.Start()

        LoadSummary()
    End Sub

    Private Function CreateNavButton(text As String, x As Integer, y As Integer, backColor As Color) As Button
        Return New Button With {
            .Text = text,
            .Location = New Point(x, y),
            .Size = New Size(180, 50),
            .BackColor = backColor,
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Segoe UI", 11, FontStyle.Bold)
        }
    End Function

    Private Function CreateStatBox(title As String, x As Integer, y As Integer) As Label
        Return New Label With {
            .Location = New Point(x, y),
            .Size = New Size(250, 120),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Segoe UI", 14, FontStyle.Bold),
            .Text = title & vbCrLf & "Loading..."
        }
    End Function

    Private Sub LoadSummary()
        Dim data = StoreDataManager.LoadStoreData()
        Dim products = data.Products
        Dim lowStockCount = products.Count(Function(p) p.Stock < 10)
        Dim todaysSales As Decimal = data.Sales.Where(Function(s) s.Date.Date = Date.Today).Sum(Function(s) s.Total)

        lblTotalProducts.Text = "📦 Total Products" & vbCrLf & products.Count.ToString()
        lblInventoryValue.Text = "💰 Inventory Value" & vbCrLf & "₹" & products.Sum(Function(p) p.Price * p.Stock).ToString("N0")
        lblLowStock.Text = "⚠️ Low Stock Items" & vbCrLf & lowStockCount.ToString()
        lblSalesToday.Text = "📈 Sales Today" & vbCrLf & "₹" & todaysSales.ToString("N0")
        lblCustomersTotal.Text = "👤 Total Customers" & vbCrLf & data.Customers.Count.ToString()

        If lowStockCount > 0 Then
            lblLowStock.BackColor = Color.FromArgb(255, 200, 200)
        End If
    End Sub

    Private Sub TimerRefresh_Tick(sender As Object, e As EventArgs)
        LoadSummary()
    End Sub

    Private Sub BtnInventory_Click(sender As Object, e As EventArgs)
        Dim inventoryForm = New InventoryForm()
        inventoryForm.ShowDialog()
        LoadSummary()
    End Sub

    Private Sub BtnSales_Click(sender As Object, e As EventArgs)
        Dim salesForm = New SalesForm()
        salesForm.ShowDialog()
        LoadSummary()
    End Sub

    Private Sub BtnCustomers_Click(sender As Object, e As EventArgs)
        Dim customersForm = New CustomersForm()
        customersForm.ShowDialog()
        LoadSummary()
    End Sub

    Private Sub BtnReports_Click(sender As Object, e As EventArgs)
        Dim reportsForm = New ReportsForm()
        reportsForm.ShowDialog()
        LoadSummary()
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs)
        Dim result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            timerRefresh.Stop()
            Dim loginForm = New LoginForm()
            loginForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs)
        timerRefresh.Stop()
        Application.Exit()
    End Sub
End Class
