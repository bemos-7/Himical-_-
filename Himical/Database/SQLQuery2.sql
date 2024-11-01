

-- Таблица Категории
CREATE TABLE Categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX)
);
GO

-- Таблица Продукты
CREATE TABLE Products (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    category_id INT,
    quantity_in_stock INT,
    price_per_unit DECIMAL(10, 2),
    description NVARCHAR(MAX),
    production_date DATE,
    expiry_date DATE,
    unit_of_measurement NVARCHAR(50),
    FOREIGN KEY (category_id) REFERENCES Categories (category_id)
);
GO

-- Таблица Производство
CREATE TABLE Production (
    production_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT,
    quantity_produced INT,
    production_date DATE,
    batch_number NVARCHAR(100),
    quality_check_passed BIT,
    FOREIGN KEY (product_id) REFERENCES Products (product_id)
);
GO

-- Таблица Клиенты
CREATE TABLE Customers (
    customer_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    contact_info NVARCHAR(255),
    address NVARCHAR(MAX)
);
GO

-- Таблица Реализация
CREATE TABLE Sales (
    sale_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT,
    quantity_sold INT,
    sale_date DATE,
    customer_id INT,
    total_price DECIMAL(10, 2),
    FOREIGN KEY (product_id) REFERENCES Products (product_id),
    FOREIGN KEY (customer_id) REFERENCES Customers (customer_id)
);
GO

-- Таблица Поставщики
CREATE TABLE Suppliers (
    supplier_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    contact_info NVARCHAR(255),
    address NVARCHAR(MAX)
);
GO

-- Таблица Поставки
CREATE TABLE Supplies (
    supply_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT,
    quantity_received INT,
    supply_date DATE,
    supplier_id INT,
    FOREIGN KEY (product_id) REFERENCES Products (product_id),
    FOREIGN KEY (supplier_id) REFERENCES Suppliers (supplier_id)
);
GO

-- Таблица Администраторы
CREATE TABLE Admins (
    admin_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(255) NOT NULL,
    password_hash NVARCHAR(255) NOT NULL
);
GO

-- Таблица Заказов
CREATE TABLE Orders (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    order_date DATE NOT NULL,
    total_amount AS (quantity * price) PERSISTED,
    FOREIGN KEY (product_id) REFERENCES Products (product_id),
);
GO