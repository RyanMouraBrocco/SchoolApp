CREATE DATABASE SchoolApp_Classroom;
USE SchoolApp_Classroom;
CREATE TABLE Classroom
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	RoomNumber NVARCHAR(15) NOT NULL,
	TeacherId INT NOT NULL,
	SubjectId INT NOT NULL,
	CreatorId INT NOT NULL,
	CreationDate DATETIME NOT NULL,
	UpdaterId INT NULL,
	UpdateDate DATETIME NULL,
	Deleted BIT NOT NULL
);
CREATE TABLE Classroom_Student
(
	Id 	INT IDENTITY NOT NULL PRIMARY KEY,
	ClassroomId INT NOT NULL,
	StudentId INT NOT NULL
);
CREATE TABLE Owner_Type
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	CreatorId INT NOT NULL,
	CreationDate DATETIME NOT NULL,
	UpdaterId INT NULL,
	UpdateDate DATETIME NULL,
	Deleted BIT NOT NULL
);
CREATE TABLE Student
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	BirthDate DATETIME NOT NULL,
	DocumentId NVARCHAR(100) NOT NULL,
	Sex INT NOT NULL,
	CreatorId INT NOT NULL,
	CreationDate DATETIME NOT NULL,
	UpdaterId INT NULL,
	UpdateDate DATETIME NULL,
	Deleted BIT NULL
);
CREATE TABLE Owner_Student
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	OwnerId INT NOT NULL,
	StudentId INT NOT NULL,
	OwenerTypeId INT NOT NULL
);
CREATE TABLE Subject
(
	Id INT IDENTITY NOT NULL PRIMARY KEY,
	AccountId INT NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	CreatorId INT NOT NULL,
	CreationDate DATETIME NOT NULL,
	UpdaterId INT NULL,
	UpdateDate DATETIME NULL,
	Deleted BIT NULL
);

-- crete foreign key
ALTER TABLE Classroom ADD CONSTRAINT [FK_Classroom_SubjectId] FOREIGN KEY (SubjectId) REFERENCES [Subject](Id)
ALTER TABLE Classroom_Student ADD CONSTRAINT [FK_Classroom_Student_ClassroomId] FOREIGN KEY (ClassroomId) REFERENCES [Classroom](Id)
ALTER TABLE Classroom_Student ADD CONSTRAINT [FK_Classroom_Student_StudentId] FOREIGN KEY (StudentId) REFERENCES [Student](Id)
ALTER TABLE Owner_Student ADD CONSTRAINT [FK_Owner_Student_StudentId] FOREIGN KEY (StudentId) REFERENCES [Student](Id)


-- create indexes
CREATE NONCLUSTERED INDEX IX_Classroom_AccountId ON [Classroom](AccountId);
CREATE NONCLUSTERED INDEX IX_Classroom_TeacherId ON [Classroom](TeacherId);
CREATE NONCLUSTERED INDEX IX_Owner_Type_AccountId ON [Owner_Type](AccountId);
CREATE NONCLUSTERED INDEX IX_Student_AccountId ON [Student](AccountId);
CREATE NONCLUSTERED INDEX IX_Owner_Student_OwnerId ON [Owner_Student](OwnerId);
CREATE NONCLUSTERED INDEX IX_Subject_AccountId ON [Subject](AccountId);
