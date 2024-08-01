CREATE DATABASE CafeManagementDb
GO

USE CafeManagementDb
GO

-- Tạo bảng Users để quản lý tài khoản người dùng
CREATE TABLE Users (
    Email NVARCHAR(100) PRIMARY KEY, -- Email lấy từ bảng Employees
    Password NVARCHAR(256) NOT NULL
);

-- Tạo bảng Cafes để quản lý các quán cafe
CREATE TABLE Cafes (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200),
    Phone NVARCHAR(15),
    Image VARCHAR(200)
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
    Price DECIMAL(18, 2) NOT NULL,
    Image VARCHAR(200)

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

INSERT INTO Users (Email, Password)
VALUES 
('john.doe@example.com', 'password123'),  -- Mật khẩu cho John Doe
('jane.smith@example.com', 'password123'), -- Mật khẩu cho Jane Smith
('alice.brown@example.com', 'password123');

INSERT INTO Cafes (Name, Address, Phone, Image)
VALUES 
('Cafe A', '123 Main Street', '123-456-7890', '/images/Cafe_1.jfif'),
('Cafe B', '456 Elm Street', '123-456-7891', '/images/Cafe_2.jfif'),
('Cafe C', '789 Oak Street', '123-456-7892', '/images/Cafe_3.jfif'),
('Cafe D', '101 Pine Street', '123-456-7893', '/images/Cafe_4.jfif'),
('Cafe E', '202 Maple Street', '123-456-7894', '/images/Cafe_5.jfif'),
('Cafe F', '303 Birch Street', '123-456-7895', '/images/Cafe_6.jfif'),
('Cafe G', '404 Cedar Street', '123-456-7896', '/images/Cafe_7.jfif'),
('Cafe H', '505 Walnut Street', '123-456-7897', '/images/Cafe_8.jfif');

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

INSERT INTO Products (Name, Description, Price, Image, CategoryId)
VALUES 
('Black Coffee', 'Rich and strong black coffee', 5.00, '/images/Black_Coffee.jfif', 1),
('Brown Coffee', 'Smooth and creamy brown coffee', 5.50, '/images/Brown_Coffee.jfif', 1),
('Matcha', 'Refreshing green matcha tea', 6.00, '/images/Matcha.jfif', 2),
('Milk Coffee', 'Delicious coffee with milk', 5.25, '/images/Milk_Coffee.jfif', 1),
('Pastry 1', 'Tasty pastry 1', 3.00, '/images/Pastry_1.jfif', 3),
('Pastry 2', 'Delicious pastry 2', 3.50, '/images/Pastry_2.jfif', 3),
('Pastry 3', 'Yummy pastry 3', 4.00, '/images/Pastry_3.jfif', 3),
('Tea', 'Classic black tea', 4.00, '/images/Tea.jfif', 2);


INSERT INTO Orders (CafeId, EmployeeId, OrderDate, TotalAmount)
VALUES 
(1, 1, GETDATE(), 8.00),
(2, 3, GETDATE(), 3.50);

INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
VALUES 
(1, 1, 1, 2.50), -- 1 Espresso
(1, 3, 1, 2.00), -- 1 Green Tea
(2, 4, 1, 1.50); -- 1 Croissant

