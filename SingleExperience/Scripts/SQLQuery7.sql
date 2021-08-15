drop table Category;
CREATE TABLE Category(
	CategoryId INT NOT NULL,
	Description VARCHAR(50) NOT NULL
	PRIMARY KEY (CategoryId));

drop table Product;

CREATE TABLE Product( 
	ProductId INT NOT NULL IDENTITY(1,1),
	Name VARCHAR(100) NOT NULL,
	Price FLOAT NOT NULL,
	Detail VARCHAR(500) NOT NULL,
	Amount INT NOT NULL,
	CategoryId INT NOT NULL,
	Ranking INT NOT NULL,
	Available BIT,
	Rating FLOAT NOT NULL
	PRIMARY KEY (ProductId));

CREATE TABLE Enjoyer(
	Cpf VARCHAR(11) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	Phone VARCHAR(15) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	BirthDate DATE NOT NULL,
	Password VARCHAR(50) NOT NULL,
	Employee BIT
	PRIMARY KEY (Cpf));

CREATE TABLE AccessEmployee(
	Cpf VARCHAR(11) NOT NULL,
	AccessInventory BIT,
	AccessRegister BIT,
	PRIMARY KEY (Cpf));
	
DROP TABLE CreditCard;

CREATE TABLE CreditCard (
	CardId INT NOT NULL IDENTITY(1,1),
	Number VARCHAR(20) NOT NULL,
	Name VARCHAR(20) NOT NULL,
	ShelfLife DATE NOT NULL,
	Cvv VARCHAR(3) NOT NULL,
	Cpf VARCHAR(11) NOT NULL
	PRIMARY KEY (CardId));


CREATE TABLE Address(
	AddressId INT NOT NULL IDENTITY(1,1),
	Postcode VARCHAR(10) NOT NULL,
	Street VARCHAR(50) NOT NULL,
	Number VARCHAR(5) NOT NULL,
	City VARCHAR(10) NOT NULL,
	State VARCHAR(10) NOT NULL,
	Cpf VARCHAR(11) NOT NULL
	PRIMARY KEY (AddressId));

CREATE TABLE Cart(
	CartId INT NOT NULL IDENTITY(1,1),
	Cpf VARCHAR(11) NOT NULL,
	DateCreated DATE NOT NULL
	PRIMARY KEY (CartId));

	
CREATE TABLE StatusProductCart(
	StatusProductCartId INT NOT NULL,
	Description VARCHAR(50) NOT NULL
	PRIMARY KEY (StatusProductCartId));

drop table ProductCart;

CREATE TABLE ProductCart(
	ItemCartId INT NOT NULL IDENTITY(1,1),
	ProductId INT NOT NULL,
	CartId INT NOT NULL,
	Amount INT NOT NULL,
	StatusProductCartId INT NOT NULL
	PRIMARY KEY (ItemCartId));


CREATE TABLE Payment(
	PaymentId INT NOT NULL,
	Description VARCHAR(50) NOT NULL
	PRIMARY KEY (PaymentId));

CREATE TABLE StatusBought(
	StatusBoughtId INT NOT NULL,
	Description VARCHAR(50) NOT NULL
	PRIMARY KEY (StatusBoughtId));

DROP TABLE Bought;

CREATE TABLE Bought(
	BoughtId INT NOT NULL IDENTITY(1,1),
	TotalPrice FLOAT NOT NULL,
	AddressId INT NOT NULL,
	PaymentId INT NOT NULL,
	CardNumber VARCHAR(16),
	Cpf VARCHAR(11) NOT NULL,
	StatusBoughtId INT NOT NULL,
	DateBought DATE NOT NULL
	PRIMARY KEY (BoughtId));		

DROP TABLE ProductBought;

CREATE TABLE ProductBought(
	ProductBoughtId INT NOT NULL IDENTITY(1,1),
	ProductId INT NOT NULL,
	Amount INT NOT NULL,
	BoughtId INT NOT NULL
	PRIMARY KEY (ProductBoughtId));
	
--- End Create Table


--- Create Alter Table
---CategoryId into Product
ALTER TABLE Product ADD CONSTRAINT FK_Product_Category
FOREIGN KEY (CategoryId) REFERENCES Category (CategoryId);

---Cpf into CreditCard
ALTER TABLE CreditCard ADD CONSTRAINT FK_CreditCard_Enjoyer
FOREIGN KEY (Cpf) REFERENCES Enjoyer (Cpf);

---Cpf into Address
ALTER TABLE Address ADD CONSTRAINT FK_Address_Enjoyer
FOREIGN KEY (Cpf) REFERENCES Enjoyer (Cpf);

---Cpf into Cart
ALTER TABLE Cart ADD CONSTRAINT FK_Cart_Enjoyer
FOREIGN KEY (Cpf) REFERENCES Enjoyer (Cpf);

---CartId into ItemCart
ALTER TABLE ProductCart ADD CONSTRAINT FK_ProductCart_Cart
FOREIGN KEY (CartId) REFERENCES Cart (CartId);

---StatusProductCartId into ItemCart
ALTER TABLE ProductCart ADD CONSTRAINT FK_ProductCart_StatusProductCart
FOREIGN KEY (StatusProductCartId) REFERENCES StatusProductCart (StatusProductCartId);

---ProductId into ItemCart
ALTER TABLE ProductCart ADD CONSTRAINT FK_ProductCart_Product
FOREIGN KEY (ProductId) REFERENCES Product (ProductId);

---Cpf into Bought
ALTER TABLE Bought ADD CONSTRAINT FK_Bought_Enjoyer
FOREIGN KEY (Cpf) REFERENCES Enjoyer (Cpf);

---PaymentId into Bought
ALTER TABLE Bought ADD CONSTRAINT FK_Bought_Payment
FOREIGN KEY (PaymentId) REFERENCES Payment (PaymentId);

---Address into Bought
ALTER TABLE Bought ADD CONSTRAINT FK_Bought_Address
FOREIGN KEY (AddressId) REFERENCES Address (AddressId);

---StatusBoughtId into Bought
ALTER TABLE Bought ADD CONSTRAINT FK_Bought_StatusBought
FOREIGN KEY (StatusBoughtId) REFERENCES StatusBought (StatusBoughtId);

ALTER TABLE Bought ADD CONSTRAINT FK_Bought_CreditCard
FOREIGN KEY (CreditCardId) REFERENCES CreditCard (CreditCardId);

---BoughtId into ProductBought
ALTER TABLE ProductBought ADD CONSTRAINT FK_ProductBought_Bought
FOREIGN KEY (BoughtId) REFERENCES Bought (BoughtId);

---ProductId into ProductBought
ALTER TABLE ProductBought ADD CONSTRAINT FK_ProductBought_Product
FOREIGN KEY (ProductId) REFERENCES Product (ProductId);

---ProductId into ProductBought
ALTER TABLE AccessEmployee ADD CONSTRAINT FK_Access_Enjoyer
FOREIGN KEY (Cpf) REFERENCES Enjoyer (Cpf);

--- Insert Datas Table

INSERT INTO Category (CategoryId, Description)
	VALUES (1, 'Accessories'),
			(2, 'Cellphone'),
			(3, 'Computer'),
			(4, 'Notebook'),
			(5, 'Tablet');

INSERT INTO Product (Name, Price, Detail, Amount, CategoryId, Ranking, Available, Rating)
	VALUES ('Computador Acer',3000,'Marca: Acer;Processador: Intel Core I5;Velocidade do Processador: 2.9 GHz;Tamanho da Memoria: 8 GB;Tecnologia HD: SSD 250 GB;Placa de Video: Radeon RX 550',100,3,8,'True',0),
	('Notebook Gamer Samsung',5000,'Marca: Samsung;Processador: Intel Core I5;Velocidade do Processador: 2.9 GHz;Tamanho da Memoria: 8 GB;Tecnologia HD: SSD 250 GB;Placa de Video: Radeon RX 550',50,4,9,'True',0),
	('Celular Samsung A50',2500,'Marca: Samsung;Modelo: A50;Tamanho da Memoria: 4 GB;Armazenamento: 128 GB;Tela: 6.4 Polegadas;Câmera Traseira: 25MP;Câmera Frontal: 16MP',195,2,10,'True',0),
	('Celular Redmi Note 10',2500,'Marca: Redmi;Modelo: Note 10;Tamanho da Memoria: 4 GB;Armazenamento: 128 GB;Tela: 6.4 Polegadas;Câmera Traseira: 25MP;Câmera Frontal: 16MP',27,2,20,'True',0),
	('Computador Gamer',7000,'Processador: AMD Ryzen 5 5600X;Velocidade do Processador: 3.7GHz (4.6GHz Turbo);Tamanho da Memoria: 8 GB;Tecnologia HD: SSD 256 GB;Placa de Video: RTX 2060 6GB',150,3,6,'True',0),
	('Computador IBM',3893,'Processador:  Intel Pentium G4560;Velocidade do Processador: 3.5GHz;Tamanho da Memoria: 8 GB;Tecnologia HD: 1TB;Placa de Video: GTX 1050 4GB',50,3,10,'True',0),
	('Celular Iphone 12',4999,'Marca: Apple;Modelo: 12;Armazenamento: 64 GB;Tela: 6.1 Polegadas;Câmera Traseira: 12MP;Câmera Frontal: 12MP',98,2,5,'True',0),
	('Notebook Dell',2800,'Marca: Dell;Processador: Intel® Pentium;Velocidade do Processador: 2GHz;Tamanho da Memoria: 4 GB;Tecnologia HD: SSD 128 GB;Placa de Video: Placa de vídeo integrada',199,4,9,'True',0),
	('Notebook Asus',3499,'Marca: ASUS;Processador: AMD RYZEN 5;Velocidade do Processador: 2.1 GHz;Tamanho da Memoria: 8 GB;Tecnologia HD: SSD 256 GB;Placa de Video: Placa de vídeo integrada',138,4,20,'True',0),
	('Teclado Gamer',129,'Marca: Trust;Tipo de teclado: Membrana;Comprimento do cabo: 180 cm;Fonte de Alimentação: USB;Iluminação: LED Rainbow',98,1,10,'True',0),
	('Headset Gamer',1149,'Marca: Audio Technica;Resposta em frequência: 20 - 20.000 HZ;Sensibilidade: 96 dB/mW',89,1,5,'True',0),
	('Mouse Gamer',139,'Marca: Corsair;Botões: 8;DPI: 12.400;Tipo de Sensor: Ótico;Conectividade: com fio',77,1,15,'True',0),
	('iPad',2549,'Marca: Apple;Memória RAM: 6GB;Armazenamento: 32 GB',46,5,10,'True',0),
	('Tablet Samsung',1399,'Marca: Samsung;Câmera Traseira: 8MP;Câmera Frontal: 5MP;Memória RAM: 3GB;Armazenamento: 64GB',88,5,13,'False',0),
	('Tablet Multilaser',399,'Marca: Multilaser;Sistema operacional: Android 8.1; Armazenamento: 16 GB;Memória: 1GB',284,5,20,'True',0),
	('Fone de ouvido',2000,'Marca: Xiaomi;Qualidade do audio: boa',100,1,1,'True',0);

INSERT INTO Enjoyer (Cpf, Name, Phone, Email, BirthDate, Password, Employee)
	VALUES ('10328698954', 'Maria da Silva', '995427605', 'maria@email.com', '19980705', '123456', 'true');

INSERT INTO StatusProductCart (StatusProductCartId, Description)
	VALUES(1, 'Inactive'),
			(2, 'Active'),
			(3, 'Bought');

INSERT INTO Payment (PaymentId, Description)
	VALUES(1, 'Credit Card'),
			(2, 'Bank Slip'),
			(3, 'Pix');

INSERT INTO StatusBought (StatusBoughtId, Description)
	VALUES(1, 'Pending Confirmation'),
			(2, 'Pending Payment'),
			(3, 'Confirmed'),
			(4, 'Canceled');

INSERT INTO AccessEmployee (Cpf, AccessInventory, AccessRegister)
	VALUES('10328698954', 'true', 'true');

--- End Insert Datas Table