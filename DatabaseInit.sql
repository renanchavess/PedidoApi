-- Verifica se o banco de dados já existe antes de criá-lo
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Pedidos')
BEGIN
    CREATE DATABASE Pedidos;
END;

USE Pedidos;

BEGIN TRANSACTION;

-- Criação da tabela Clientes
CREATE TABLE Clientes (
    id INT NOT NULL IDENTITY(1,1),
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL,
    telefone VARCHAR(20) NOT NULL,
    PRIMARY KEY (id)
);

-- Criação da tabela Enderecos
CREATE TABLE Enderecos (
    id INT NOT NULL IDENTITY(1,1),
    rua VARCHAR(100) NOT NULL,
    numero VARCHAR(20) NOT NULL,
    cidade VARCHAR(100) NOT NULL,
    estado VARCHAR(100) NOT NULL,
    cep VARCHAR(10) NOT NULL,
    cliente_id INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (cliente_id) REFERENCES Clientes(id)
);

-- Criação da tabela Produtos
CREATE TABLE Produtos (
    id INT NOT NULL IDENTITY(1,1),
    nome VARCHAR(100) NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    estoque INT NOT NULL,
    PRIMARY KEY (id)
);

-- Criação da tabela Pedidos
CREATE TABLE Pedidos (
    id INT NOT NULL IDENTITY(1,1),
    cliente_id INT NOT NULL,
    status INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (cliente_id) REFERENCES Clientes(id)
);

-- Criação da tabela ItensPedidos
CREATE TABLE ItensPedidos (
    pedido_id INT NOT NULL,
    produto_id INT NOT NULL,
    quantidade INT NOT NULL,
    preco DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (pedido_id, produto_id),
    FOREIGN KEY (pedido_id) REFERENCES Pedidos(id),
    FOREIGN KEY (produto_id) REFERENCES Produtos(id)
);

CREATE TABLE AuthTokens (
    id INT NOT NULL,
    token VARCHAR(200) NOT NULL,
    data_expiracao DATETIME NOT NULL,
    descricao VARCHAR(100) NOT NULL,
    PRIMARY KEY (id),    
);

-- Inserção de dados na tabela Produtos
INSERT INTO Produtos 
(nome, preco, estoque) 
VALUES
('Cimento CP-II 50kg', 35.90, 100),
('Tijolo Cerâmico 9x14x19cm', 1.20, 5000),
('Areia Média 20kg', 12.50, 200),
('Brita 1 20kg', 14.00, 150),
('Viga de Aço 6m', 120.00, 50),
('Telha de Fibrocimento 2,44x1,10m', 45.00, 75),
('Tinta Acrílica Branca 18L', 230.00, 30),
('Pincel 2 Polegadas', 15.50, 100),
('Massa Corrida 25kg', 65.00, 80),
('Chave de Fenda 1/4x6"', 22.90, 120);

COMMIT;