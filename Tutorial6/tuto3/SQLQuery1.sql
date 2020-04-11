-- drop tables
drop table student;
drop table enrollment;
drop table studies;

-- tables
-- Table: Enrollment
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

-- Table: Student
CREATE TABLE Student (
    IndexNumber nvarchar(100)  NOT NULL,
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY  (IndexNumber)
);

-- Table: Studies
CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);

Insert Into Studies(IdStudy, Name) values(
	31, 'IT'
);
Insert Into Studies(IdStudy, Name) values(
	32, 'ART'
);
Insert Into Studies(IdStudy, Name) values(
	33, 'MEDIA'
);
Insert Into Studies(IdStudy, Name) values(
	34, 'IT'
);

---///////////////////////////////////////////////////////////////////////----

Insert Into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)values
(
11,2,(Select IdStudy from Studies Where Studies.IdStudy = 31) , '2013-11-11'
);

Insert Into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)values
(
12,3,(Select IdStudy from Studies Where Studies.IdStudy = 32) , '2015-11-13'
);

Insert Into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)values
(
13,6,(Select IdStudy from Studies Where Studies.IdStudy = 33) , '2015-11-13'
);


Insert Into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)values
(
14,1,(Select IdStudy from Studies Where Studies.IdStudy = 34) , '2013-10-19'
);
---///////////////////////////////////////////////////////////////////////----


Insert Into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(
21, 'Bob1','Johnson1','1955-11-13', (Select IdEnrollment From Enrollment Where Enrollment.IdEnrollment = 11)

);

Insert Into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(
22, 'Bob2','Johnson2','1956-11-13', (Select IdEnrollment From Enrollment Where Enrollment.IdEnrollment = 12)

);


Insert Into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(
23, 'Bob3','Johnson3','1957-11-13', (Select IdEnrollment From Enrollment Where Enrollment.IdEnrollment = 13)

);

Insert Into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(
24, 'Bob4','Johnson4','1952-11-13', (Select IdEnrollment From Enrollment Where Enrollment.IdEnrollment = 14)

);
select * from studies;
select * from enrollment;
select * from student;