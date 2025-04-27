USE soup_DB 
GO 

CREATE TABLE [USER](
	[ID] int IDENTITY PRIMARY KEY, 
	[Name] nvarchar(20) NOT NULL UNIQUE, 
	[Password] nvarchar(20) NOT NULL,
)
GO

/*CREATE TRIGGER trg_PreventUserDelete
ON [USER]
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR('Deletion from USER table is not allowed.', 16, 1);
    ROLLBACK TRANSACTION; 
END;
GO*/

CREATE TABLE [PROJECT](
	[ID] int IDENTITY PRIMARY KEY, 
	[Name] nvarchar(50) NOT NULL UNIQUE, 
	[Description] nvarchar(200) NOT NULL, 
	[Creator] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL,  
	[IsComplete] bit NOT NULL DEFAULT 0,   
	[DateBegan] datetime NOT NULL, 
	[DateFinished] datetime NULL, 
	[DateDeadline] datetime NULL,
	[isPrivate] bit NOT NULL DEFAULT 0 
) 
GO

CREATE TABLE [REPOSITORY](
	[ID] int PRIMARY KEY FOREIGN KEY REFERENCES [PROJECT]([ID]) NOT NULL, 
	[GithubName] nvarchar(200) NOT NULL, 
	[GithubCreator] nvarchar(200) NOT NULL 
)

/*CREATE TRIGGER trg_PreventProjectDelete
ON [PROJECT]
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR('Deletion from PROJECT table is not allowed.', 16, 1);
    ROLLBACK TRANSACTION; 
END;
GO*/ 


CREATE TABLE [TEAM](
	[ID] int IDENTITY PRIMARY KEY NOT NULL,  
	[UserID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL, 
	[ProjectID] int FOREIGN KEY REFERENCES [PROJECT]([ID]) ON DELETE NO ACTION NOT NULL, 
	[Role] nvarchar(50) CHECK ([Role] IN (
		'������������ �������', 
		'������������ ������ �������',   
			'���-��������', 
			'����������� ��������', 	
			'3D-��������', 
			'UI/UX ��������',  
		'������������ ������ ����������',  
			'����������', 
			'Fullstack-�����������',  
			'Front end-�����������', 
			'Back end-�����������', 
			'����������� ��� ������', 
		'������������ ������ ��������� � ������������', 
			'DevOps-�������', 
			'�����������', 
		'������������ ������ �������������� ������������',  
			'���������� �� ������������', 
			'��������� �������������', 
		'������������ ������ ���������', 
			'������-��������', 
			'��������� ��������' 
		)) NOT NULL, 
	[Level] int CHECK ([Level] BETWEEN 0 AND 2) NOT NULL, 
	
	UNIQUE([UserID], [ProjectID], [Role]), 
)
GO

CREATE UNIQUE INDEX IX_UC_SingleManagers 
ON [TEAM] ([ProjectID], [Role])
WHERE [Role] IN (
	'������������ �������', 
    '������������ ������ �������', 
    '������������ ������ ����������',  
    '������������ ������ ��������� � ������������', 
    '������������ ������ �������������� ������������',  
    '������������ ������ ���������'
);

/*CREATE TABLE DomRoles (
    DomRole NVARCHAR(255),
    SubRole NVARCHAR(255)
);

INSERT INTO DomRoles (DomRole, SubRole)
VALUES 
    ('������������ �������', '������������ ������ �������'),
    ('������������ �������', '������������ ������ ����������'),
    ('������������ �������', '������������ ������ ��������� � ������������'),
    ('������������ �������', '������������ ������ �������������� ������������'),
    ('������������ �������', '������������ ������ ���������'),
    ('������������ ������ �������', '���-��������'), 
    ('������������ ������ �������', '����������� ��������'), 
    ('������������ ������ �������', '3D-��������'), 
    ('������������ ������ �������', 'UI/UX ��������'), 
    ('������������ ������ ����������', '����������'),
    ('������������ ������ ����������', 'Fullstack-�����������'),
    ('������������ ������ ����������', 'Front end-�����������'),
    ('������������ ������ ����������', 'Back end-�����������'),
    ('������������ ������ ����������', '����������� ��� ������'), 
    ('������������ ������ ��������� � ������������', 'DevOps-�������'),
    ('������������ ������ ��������� � ������������', '�����������'),
    ('������������ ������ �������������� ������������', '���������� �� ������������'), 
    ('������������ ������ �������������� ������������', '��������� �������������'),
    ('������������ ������ ���������', '������-��������'), 
    ('������������ ������ ���������', '��������� ��������');
GO

CREATE FUNCTION dbo.GetDomRole(@SubRole NVARCHAR(255))
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @DomRole NVARCHAR(255);
    
    SELECT @DomRole = DomRole
    FROM DomRoles
    WHERE SubRole = @SubRole;

    RETURN @DomRole;
END;
GO */


CREATE TABLE [NOTIFICATION](
	[ID] int IDENTITY PRIMARY KEY NOT NULL, 
	[ProjectID] int FOREIGN KEY REFERENCES [PROJECT]([ID]) ON DELETE NO ACTION NOT NULL, 
	[SenderID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL, 
	[ReceiverID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL,
		[Role] nvarchar(50) CHECK ([Role] IN (
		'������������ �������', 
		'������������ ������ �������',   
			'���-��������', 
			'����������� ��������', 	
			'3D-��������', 
			'UI/UX ��������',  
		'������������ ������ ����������',  
			'����������', 
			'Fullstack-�����������',  
			'Front end-�����������', 
			'Back end-�����������', 
			'DevOps-�������', 	
			'����������� ��� ������', 
		'������������ ������ ��������� � ������������', 
			'DevOps-�������', 
			'�����������', 
		'������������ ������ �������������� ������������',  
			'���������� �� ������������', 
			'��������� �������������', 
		'������������ ������ ���������', 
			'������-��������', 
			'��������� ��������' 
		)) NOT NULL, 
	[Type] nvarchar(20) CHECK ([Type] IN ('invite', 'request'))NOT NULL 
)
GO


CREATE TABLE [TASK](
	[ID] int IDENTITY PRIMARY KEY NOT NULL, 
	[ProjectID] int FOREIGN KEY REFERENCES [PROJECT]([ID]) ON DELETE NO ACTION NOT NULL, 
	[CreatorID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL,
	[AssigneeID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL,
	[Name] nvarchar(50) NOT NULL,  
	[Description] nvarchar(500) NULL, 
	[IsComplete] bit NOT NULL DEFAULT 0  
)
GO


CREATE TABLE [ACTION](
	[ID] int IDENTITY PRIMARY KEY NOT NULL, 
	[ProjectID] int FOREIGN KEY REFERENCES [PROJECT]([ID]) ON DELETE NO ACTION NOT NULL, 
	[ActorID] int FOREIGN KEY REFERENCES [USER]([ID]) ON DELETE NO ACTION NOT NULL, 
	[TaskID] int FOREIGN KEY REFERENCES [TASK]([ID]) ON DELETE NO ACTION NOT NULL, 
	[Description] nvarchar(500) NOT NULL, 
	[Commit] nvarchar (255) NULL, 
	[Date] datetime NOT NULL
)
