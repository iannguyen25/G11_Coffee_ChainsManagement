CREATE DATABASE CafeManagementDb
GO

USE CafeManagementDb
GO

-- Tạo bảng Users để quản lý tài khoản người dùng
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100),
    Role NVARCHAR(50) -- Ví dụ: Admin, Manager, Staff
);

-- Tạo bảng Cafes để quản lý các quán cafe
CREATE TABLE Cafes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    Phone NVARCHAR(15)
);

-- Tạo bảng Employees để quản lý nhân viên
CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY,
    CafeId INT FOREIGN KEY REFERENCES Cafes(Id),
    FullName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(50),
    Email NVARCHAR(100),
    Phone NVARCHAR(15)
);

-- Tạo bảng Categories để quản lý danh mục sản phẩm
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- Tạo bảng Products để quản lý sản phẩm
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY,
    CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(18, 2) NOT NULL
);

-- Tạo bảng Orders để quản lý đơn hàng
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    CafeId INT FOREIGN KEY REFERENCES Cafes(Id),
    EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL
);

-- Tạo bảng OrderDetails để quản lý chi tiết đơn hàng
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    ProductId INT FOREIGN KEY REFERENCES Products(Id),
    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL
);

INSERT INTO Users (Username, PasswordHash, Email, FullName, Role)
VALUES 
('admin', 'hashed_password1', 'admin@example.com', 'Admin User', 'Admin'),
('manager', 'hashed_password2', 'manager@example.com', 'Manager User', 'Manager'),
('staff', 'hashed_password3', 'staff@example.com', 'Staff User', 'Staff');

INSERT INTO Cafes (Name, Address, Phone)
VALUES 
('Cafe A', '123 Coffee St', '123-456-7890'),
('Cafe B', '456 Tea Ave', '987-654-3210');

INSERT INTO Employees (CafeId, FullName, Position, Email, Phone)
VALUES 
(1, 'John Doe', 'Barista', 'john.doe@example.com', '111-222-3333'),
(1, 'Jane Smith', 'Manager', 'jane.smith@example.com', '444-555-6666'),
(2, 'Alice Brown', 'Waitress', 'alice.brown@example.com', '777-888-9999');

INSERT INTO Categories (Name)
VALUES 
('Coffee'),
('Tea'),
('Pastry');

INSERT INTO Products (CategoryId, Name, Description, Price)
VALUES 
(1, 'Espresso', 'Strong coffee', 2.50),
(1, 'Latte', 'Milk coffee', 3.00),
(2, 'Green Tea', 'Refreshing green tea', 2.00),
(3, 'Croissant', 'Buttery croissant', 1.50);

INSERT INTO Orders (CafeId, EmployeeId, OrderDate, TotalAmount)
VALUES 
(1, 1, GETDATE(), 8.00),
(2, 3, GETDATE(), 3.50);

INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
VALUES 
(1, 1, 1, 2.50), -- 1 Espresso
(1, 3, 1, 2.00), -- 1 Green Tea
(2, 4, 1, 1.50); -- 1 Croissant

