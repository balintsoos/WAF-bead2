CREATE DATABASE LibraryDB;
GO

USE LibraryDB;
GO

CREATE TABLE Book(
	ISBN VARCHAR(20) PRIMARY KEY,
	Title VARCHAR(300) NOT NULL,
	Author VARCHAR(300) NOT NULL,
	Year INTEGER NOT NULL
);

CREATE TABLE Copy(
	CopyId INTEGER PRIMARY KEY IDENTITY(1,1),
	ISBN VARCHAR(20) NOT NULL,
	[Condition] VARCHAR(50) NULL,
	CONSTRAINT CopyToBook
    FOREIGN KEY (ISBN) 
    REFERENCES Book (ISBN),
);

CREATE TABLE Rent(
	RentId INTEGER PRIMARY KEY IDENTITY(1,1),
	CopyId INTEGER NOT NULL,
	Name VARCHAR(100) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	Phone VARCHAR(30) NOT NULL,
	StartDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	ReturnDate DATE NULL,
  CONSTRAINT RentToCopy 
    FOREIGN KEY (CopyId) 
    REFERENCES Copy (CopyId),
);
GO

INSERT INTO  Book (ISBN, Title, Author, Year) VALUES ('0-201-70073-5', 'The C++ programming language', 'Stroustrup Bjarne', 2004)
INSERT INTO  Book (ISBN, Title, Author, Year) VALUES ('963-463-833-3', 'Bevezetés a programozáshoz', 'Fóthi Ákos', 2005)
INSERT INTO  Book (ISBN, Title, Author, Year) VALUES ('963-9301-46-9', 'Programozási nyelvek', 'Nyékyné Gaizler Judit', 2003)
INSERT INTO  Book (ISBN, Title, Author, Year) VALUES ('978-963-463-729-5', 'Bevezetés a matematikába', 'Járai Antal', 2012)
INSERT INTO  Book (ISBN, Title, Author, Year) VALUES ('978-963-9863-17-0', 'Introducing .NET 4.0 with Visual Studio 2010', 'Mackey Alex', 2010)

INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-463-729-5', 'new')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-463-729-5', 'good')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-463-833-3', 'good')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-463-833-3 ', 'new')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-463-833-3', 'fair')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('0-201-70073-5', 'fair')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-9863-17-0', 'good')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-9863-17-0', 'poor')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-9301-46-9', 'new')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-9301-46-9', 'good')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('963-9301-46-9', 'poor')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-463-729-5', 'fair')
INSERT INTO  Copy (ISBN, [Condition]) VALUES ('978-963-463-729-5', 'new')

INSERT INTO  Rent (CopyID, Name, Email, Phone, StartDate, EndDate, ReturnDate) VALUES (2, 'Gipsz Jakab', 'gipsz.jakab@elte.hu', '0612345678', CAST('2017-06-12' AS Date), CAST('2017-06-25' AS Date), NULL)
INSERT INTO  Rent (CopyID, Name, Email, Phone, StartDate, EndDate, ReturnDate) VALUES (9, 'Teszt Elek', 'teszt.elek@elte.hu', '0619876543', CAST('2017-05-09' AS Date), CAST('2017-06-08' AS Date), CAST('2017-06-10' AS Date))
INSERT INTO  Rent (CopyID, Name, Email, Phone, StartDate, EndDate, ReturnDate) VALUES (13, 'Gipsz Jakab', 'gipsz.jakab@elte.hu', '0612345678', CAST('2017-05-31' AS Date), CAST('2017-06-13' AS Date), NULL)
INSERT INTO  Rent (CopyID, Name, Email, Phone, StartDate, EndDate, ReturnDate) VALUES (9, 'Minden Áro', 'minden.aron@.elte.hu', '0611357902', CAST('2017-04-03' AS Date), CAST('2017-04-16' AS Date), CAST('2017-04-15' AS Date))
INSERT INTO  Rent (CopyID, Name, Email, Phone, StartDate, EndDate, ReturnDate) VALUES (6, 'Teszt Elek', 'teszt.elek@elte.hu', '0619876543', CAST('2017-05-20' AS Date), CAST('2017-06-18' AS Date), CAST('2017-06-12' AS Date))
GO