CREATE TABLE log(
    id int IDENTITY(1,1) not null,
    data varchar(50) not null,
    operacao char(1) not null,
    observacao varchar(500),
    tabela varchar(20) not null,
    id_registro int not null,
    PRIMARY KEY(id)
);