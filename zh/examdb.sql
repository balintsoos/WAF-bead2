CREATE DATABASE ExamDB;
GO

USE ExamDB;
GO

CREATE TABLE Student(
	UserId VARCHAR(20) PRIMARY KEY
);

CREATE TABLE Teacher(
	UserId VARCHAR(20) PRIMARY KEY
);

CREATE TABLE Course(
	Id INTEGER PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Task(
	Id INTEGER PRIMARY KEY IDENTITY(1,1),
	CourseId INTEGER NOT NULL,
	Question VARCHAR(400) NOT NULL,
	Active TINYINT NOT NULL,
	Answer1 VARCHAR(400) NOT NULL,
	Answer2 VARCHAR(400) NOT NULL,
	Answer3 VARCHAR(400) NOT NULL,
	GoodAnswer INTEGER NOT NULL,
	CONSTRAINT TaskToCourse 
        FOREIGN KEY (CourseId) 
        REFERENCES Course (Id),
);

CREATE TABLE Exam(
	Id INTEGER PRIMARY KEY IDENTITY(1,1),
	StudentId VARCHAR(20) NOT NULL,
	StartTime DATETIME NOT NULL,
	EndTime DATETIME NOT NULL,
	T1 INTEGER NOT NULL,
	A1 INTEGER NOT NULL,
	T2 INTEGER NOT NULL,
	A2 INTEGER NOT NULL,
	T3 INTEGER NOT NULL,
	A3 INTEGER NOT NULL,
	T4 INTEGER NOT NULL,
	A4 INTEGER NOT NULL,
	CONSTRAINT ExamToStudent 
        FOREIGN KEY (StudentId) 
        REFERENCES Student (UserId),
	CONSTRAINT ExamToTask1 
        FOREIGN KEY (T1) 
        REFERENCES Task (Id),
	CONSTRAINT ExamToTask2 
        FOREIGN KEY (T2) 
        REFERENCES Task (Id),
	CONSTRAINT ExamToTask3 
        FOREIGN KEY (T3) 
        REFERENCES Task (Id),
	CONSTRAINT ExamToTask4 
        FOREIGN KEY (T4) 
        REFERENCES Task (Id),
);
GO

INSERT INTO Teacher VALUES('theBOSS');

INSERT INTO Student VALUES('AAAAAA');
INSERT INTO Student VALUES('BBBBBB');
INSERT INTO Student VALUES('CCCCCC');

INSERT INTO Course(Name) VALUES('�ltal�nos ismeretek');
INSERT INTO Course(Name) VALUES('T�rt�nelem');

INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Melyik nehezebb, 1 KG vas, vagy 1KG pihe?', 1,'A vas.','A pihe.','Egyform�n nehezek.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Melyik orsz�gban dobtak le el�sz�r atombomb�t?', 1,'Hiroshima','Chernobil','Jap�n',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Melyik napon �nneplik az amerikaiak j�lius 4-�t?', 1,'J�lius 4.','Minden �vben m�s napra esik.','Nem �nneplik.',1);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'H�ny napb�l �ll egy sz�k��v?', 1,'361','366','418',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'H�ny ember �l a f�ld�n?', 1,'6 478 566 217.','7 milli�rd.','150 milli� a f�ld�n �s m�g t�bb a vil�gban.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi a guillotine?', 1,'Francia kiv�gz�eszk�z.','Olasz t�szta.','Nem l�tezik olyan.',1);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'H�ny s�rga lap van a b�r�n�l egy �tlagos m�rk�z�sen?', 1,'11','22','1',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Az els� amerikai �rhaj�s magyar volt.', 1,'Igaz.','Hamis.','Nem eld�nthet�.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Az internetet az �kori kelt�k is haszn�lt�k?', 1,'Igen.','Nem.','Igen, de nem �gy nevezt�k.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Mi a Don-kanyar?', 1,'Aut�verseny szakkifejez�s.','Olasz �t.','A II. vil�gh�bor� egyik fontos csatatere.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi a Nemzeti 11?', 1,'Az EU tag�llamai.','A fociv�logatott.','�r�k csoportja.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi az a perpetum mobile?', 1,'�rint�k�perny�s mobiltelefon.','Egy �j mobilt�rsas�g.','�r�kmozg�.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'H�ny 0 van a milli�rdban?', 1,'8','9','7',2);

INSERT INTO Exam(StudentId, StartTime, EndTime, T1, A1, T2, A2, T3, A3, T4, A4) VALUES('AAAAAA','2017-05-22 10:01','2017-05-22 10:32',5,3,1,0,4,1,11,3);
INSERT INTO Exam(StudentId, StartTime, EndTime, T1, A1, T2, A2, T3, A3, T4, A4) VALUES('AAAAAA','2017-05-22 10:41','2017-05-22 11:01',4,2,10,1,11,2,5,3);
GO