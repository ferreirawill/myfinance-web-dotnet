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


INSERT INTO planoconta(descricao, tipo) VALUES('Combustível', 'D')
INSERT INTO planoconta(descricao, tipo) VALUES('Alimentação', 'D')
INSERT INTO planoconta(descricao, tipo) VALUES('Plano de Saúde', 'D')
INSERT INTO planoconta(descricao, tipo) VALUES('IPTU', 'D')
INSERT INTO planoconta(descricao, tipo) VALUES('Salário', 'R')
INSERT INTO planoconta(descricao, tipo) VALUES('Dividendos de ações', 'R')


INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230214 21:00', 800, 'Gasolina', 1)
INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230214 21:20', 100, 'Jantar', 2)
INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230213 10:47', 150, 'Plano de saude', 3)
INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230212 11:00', 30, 'IPTU', 4)
INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230205 08:00', 100, 'Salario', 5)
INSERT INTO transacao(data, valor, historico, planocontaid) VALUES('20230205 09:00', 500000, 'Dividendos', 6)