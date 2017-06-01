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

INSERT INTO Course(Name) VALUES('Általános ismeretek');
INSERT INTO Course(Name) VALUES('Történelem');

INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Melyik nehezebb, 1 KG vas, vagy 1KG pihe?', 1,'A vas.','A pihe.','Egyformán nehezek.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Melyik országban dobtak le elõször atombombát?', 1,'Hiroshima','Chernobil','Japán',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Melyik napon ünneplik az amerikaiak július 4-ét?', 1,'Július 4.','Minden évben más napra esik.','Nem ünneplik.',1);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Hány napból áll egy szökõév?', 1,'361','366','418',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Hány ember él a földön?', 1,'6 478 566 217.','7 milliárd.','150 millió a földön és még több a világban.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi a guillotine?', 1,'Francia kivégzõeszköz.','Olasz tészta.','Nem létezik olyan.',1);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Hány sárga lap van a bírónál egy átlagos mérkõzésen?', 1,'11','22','1',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Az elsõ amerikai ûrhajós magyar volt.', 1,'Igaz.','Hamis.','Nem eldönthetõ.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Az internetet az ókori kelták is használták?', 1,'Igen.','Nem.','Igen, de nem így nevezték.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(2, 'Mi a Don-kanyar?', 1,'Autóverseny szakkifejezés.','Olasz út.','A II. világháború egyik fontos csatatere.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi a Nemzeti 11?', 1,'Az EU tagállamai.','A fociválogatott.','Írók csoportja.',2);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Mi az a perpetum mobile?', 1,'Érintõképernyõs mobiltelefon.','Egy új mobiltársaság.','Örökmozgó.',3);
INSERT INTO Task(CourseId, Question, Active, Answer1, Answer2, Answer3, GoodAnswer) VALUES(1, 'Hány 0 van a milliárdban?', 1,'8','9','7',2);

INSERT INTO Exam(StudentId, StartTime, EndTime, T1, A1, T2, A2, T3, A3, T4, A4) VALUES('AAAAAA','2017-05-22 10:01','2017-05-22 10:32',5,3,1,0,4,1,11,3);
INSERT INTO Exam(StudentId, StartTime, EndTime, T1, A1, T2, A2, T3, A3, T4, A4) VALUES('AAAAAA','2017-05-22 10:41','2017-05-22 11:01',4,2,10,1,11,2,5,3);
GO