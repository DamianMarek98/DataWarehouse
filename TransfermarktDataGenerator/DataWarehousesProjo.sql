Create table Agent(
	Id int identity(1,1) primary key,
	Imię varchar(50),
	Nazwisko varchar(50)
);

Create table Klub(
	Id int identity(1,1) primary key,
	Nazwa varchar(MAX),
	Kraj varchar(MAX),
	Budżet int
);

Create table TransferZawodnika(
	Id int identity(1,1) primary key,
	DataTransferu datetime not null,
	TypPlatnosci varchar(20),
	KlubSprzedajacyId int foreign key references Klub(Id),
	KlubKupujacyId int foreign key references Klub(Id),
	KwotaTransferu integer,
	ZawodnikId int foreign key references Zawodnik(Id),
);

Create table Zawodnik(
	Id int identity(1,1) primary key,
	Imię varchar(50),
	Nazwisko varchar(50),
	DataUrodzenia datetime not null,
	Pozycja varchar(50) ,
	KlubId int foreign key references Klub(Id),
	AgentId int foreign key references Agent(Id)
);

Create table WartoscZawodnika(
	Id int identity(1,1) primary key,
	WartoscRynkowa int,
	DataWystawienia datetime not null,
	ZawodnikId int foreign key references Zawodnik(Id)
);
