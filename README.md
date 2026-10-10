# Cloth Store Desktop Application
## VB.NET Windows Forms Desktop Application for Inventory & Sales Management

### Quick Start (Visual Studio 2015)

**See: [SETUP_VS2015.md](SETUP_VS2015.md) for complete setup instructions**

### Key Features

✅ **Inventory Management**
- Add, edit, delete products
- Track stock levels
- Low stock alerts
- Search by name/barcode/category
- Supplier management

✅ **Sales Processing**
- Quick invoice generation
- Multiple payment methods
- Customer billing
- Receipt printing
- Sales history tracking

✅ **Customer Management**
- Customer database
- VIP customer tracking
- Purchase history
- Loyalty points
- Customer segmentation

✅ **Reports & Analytics**
- Daily/Weekly/Monthly sales reports
- Top selling products
- Revenue analysis
- Stock status reports
- Customer insights

✅ **User Management**
- Admin, Manager, Staff roles
- User authentication
- Activity logging
- Password management

✅ **Database**
- SQL Server LocalDB support
- Automatic database initialization
- Sample data import/export
- Transaction support for sales

### System Requirements

- **OS:** Windows 7 or later
- **VS:** Visual Studio 2015 or later
- **Database:** SQL Server LocalDB 2015+
- **.NET Framework:** 4.7.2 or higher
- **RAM:** 2GB minimum
- **Disk:** 500MB free space

### Installation Steps

1. **Install Prerequisites**
   - Visual Studio 2015
   - SQL Server LocalDB
   - .NET Framework 4.7.2

2. **Clone/Download Repository**
   ```bash
   git clone https://github.com/lalasharma/cloth-store-desktop.git
   cd cloth-store-desktop
   ```

3. **Open in Visual Studio**
   - File > Open > Project/Solution
   - Select: ClothStoreDesktop.sln

4. **Configure Database**
   - Update connection string in: Data/DatabaseConnection.vb
   - Or use default LocalDB

5. **Build & Run**
   - Build > Build Solution
   - Press F5 to run
   - Login: admin / admin123

### Project Structure

```
ClothStoreDesktop/
├── Forms/
│   ├── LoginForm.vb              # Authentication
│   ├── MainForm.vb               # Dashboard
│   ├── ProductForm.vb            # Product CRUD
│   ├── CustomerForm.vb           # Customer CRUD
│   ├── UserForm.vb               # User management
│   ├── SalesForm.vb              # Sales processing
│   ├── InventoryForm.vb          # Inventory tracking
│   └── ReportsForm.vb            # Reports & analytics
├── Data/
│   ├── DatabaseConnection.vb     # DB connection handler
│   ├── DatabaseInitializer.vb    # Auto-setup on startup
│   ├── ProductRepository.vb      # Product CRUD operations
│   ├── CustomerRepository.vb     # Customer CRUD operations
│   ├── SalesRepository.vb        # Sales CRUD operations
│   ├── UserRepository.vb         # User authentication
│   └── CategoryRepository.vb     # Category management
├── Models/
│   ├── Product.vb                # Product model
│   ├── Customer.vb               # Customer model
│   ├── SaleItem.vb               # Sale line item
│   ├── SaleRecord.vb             # Sale header
│   ├── User.vb                   # User model
│   ├── Category.vb               # Category model
│   ├── Supplier.vb               # Supplier model
│   └── StoreDataManager.vb       # JSON-based backup
├── Database/
│   ├── ClothStoreDB.sql          # Complete schema
│   └── DATABASE_SCHEMA.md        # Documentation
├── Program.vb                     # Application entry point
├── App.config                     # Configuration file
├── ClothStoreDesktop.vbproj       # Project file
├── ClothStoreDesktop.sln          # Solution file
├── SETUP_VS2015.md               # VS 2015 setup guide
└── README.md                      # This file
```

### Default Credentials

- **Username:** admin
- **Password:** admin123
- **Role:** Admin

### Database Connection String

**LocalDB (Default):**
```
Server=(LocalDB)\MSSQLLocalDB;Database=ClothStoreDB;Integrated Security=true;Connection Timeout=30
```

**SQL Server Express:**
```
Server=.\SQLEXPRESS;Database=ClothStoreDB;Integrated Security=true;
```

**Remote Server:**
```
Server=192.168.1.10;Database=ClothStoreDB;User Id=sa;Password=YourPassword;
```

### Database Tables

1. **Users** - User accounts and authentication
2. **Customers** - Customer database with purchase history
3. **Categories** - Product categories (Men, Women, Kids, Accessories)
4. **Suppliers** - Vendor/supplier information
5. **Products** - Inventory with pricing and stock levels
6. **PaymentMethods** - Payment options (Cash, Card, UPI, Cheque)
7. **Sales** - Invoice headers and transaction details
8. **SaleItems** - Line items for each sale
9. **StockAlerts** - Low stock and out-of-stock tracking

### Main Forms

**LoginForm**
- User authentication
- Role-based access

**MainForm (Dashboard)**
- Summary statistics
- Quick access to all modules
- Real-time updates

**ProductForm**
- Full CRUD for products
- Search functionality
- Barcode support
- Stock level management

**CustomerForm**
- Customer database management
- Purchase history tracking
- VIP customer classification
- Loyalty points management

**SalesForm**
- Point-of-sale interface
- Shopping cart functionality
- Multiple payment methods
- Receipt generation

**ReportsForm**
- Daily/weekly/monthly reports
- Revenue analysis
- Top selling products
- Customer insights

### Usage Guide

**Adding a Product:**
1. Click Inventory button on dashboard
2. Fill in product details
3. Click Save
4. Product appears in list

**Processing a Sale:**
1. Click Sales button
2. Select customer (optional)
3. Add products to cart
4. Select payment method
5. Click Finalize Sale

**Viewing Reports:**
1. Click Reports button
2. Select date range
3. View revenue, products sold, etc.

**Managing Users:**
1. Click Users (Admin only)
2. Add/Edit user roles
3. Manage permissions

### Troubleshooting

**Database Connection Error**
- Verify SQL Server LocalDB is running
- Check connection string
- Confirm database exists

**Login Fails**
- Check username/password (default: admin/admin123)
- Verify Users table has records

**Forms Not Displaying**
- Update target framework to .NET 4.7.2
- Rebuild solution
- Clear bin/obj folders

**Build Errors**
- Update NuGet packages
- Clean solution
- Rebuild

See **[SETUP_VS2015.md](SETUP_VS2015.md)** for detailed troubleshooting.

### Support & Documentation

- **Setup Guide:** [SETUP_VS2015.md](SETUP_VS2015.md)
- **Database Schema:** [Database/DATABASE_SCHEMA.md](Database/DATABASE_SCHEMA.md)
- **SQL Script:** [Database/ClothStoreDB.sql](Database/ClothStoreDB.sql)

### Future Enhancements

- [ ] Barcode scanner integration
- [ ] Receipt printing
- [ ] Advanced reporting with charts
- [ ] Mobile app sync
- [ ] Cloud backup
- [ ] Multi-location support
- [ ] Advanced inventory analytics
- [ ] Employee time tracking
- [ ] Commission tracking
- [ ] Email notifications

### License

MIT License - Feel free to modify and distribute

### Author

**lalasharma** - GitHub: https://github.com/lalasharma

---

**Version:** 1.0  
**Last Updated:** October 2026  
**Status:** Ready for Testing & Development
