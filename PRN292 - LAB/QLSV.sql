create database QLSV
go
use QLSV
go
create table SV(
	SVId int identity(1,1) ,
	MSSV nvarchar(8) primary key not null,
	SVName nvarchar(max),
	Gender bit,
	Birthday datetime,
	DTB float,
	IDLop int not null,
	Tel nvarchar(max),
	CMND bit,
	HocBa bit,
	THPT bit
)

create table Lop(
	IDLop int primary key not null,
	NameLop nvarchar(max)
)

AlTER TABLE SV
ADD FOREIGN KEY (IDLop) REFERENCES Lop(IDLop)