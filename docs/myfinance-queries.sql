CREATE TABLE planoconta(
    id int IDENTITY(1,1) not null,
    descricao varchar(50) not null,
    tipo char(1) not null,
    PRIMARY KEY(id)
);

create table transacao(
    id int IDENTITY(1,1) not null,
    data datetime not null,
    valor decimal(9,2),
    historico text,
    planocontaid int not null,
    primary key(id),
    FOREIGN KEY(planocontaid) REFERENCES planoconta(id),
);