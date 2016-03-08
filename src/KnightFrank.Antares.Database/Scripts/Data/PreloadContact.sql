MERGE dbo.Contact c
USING (VALUES
	('John', 'Smith', 'test 1'),
	('Alan', 'Rolfe', 'test 2')
) as d(FirstName, Surname, Title)
ON c.FirstName = d.FirstName AND c.Surname = d.Surname
WHEN MATCHED THEN
	UPDATE SET		
		c.Title = d.Title
WHEN NOT MATCHED BY TARGET THEN
	INSERT(		
		FirstName, 
		Surname, 
		Title
	) VALUES (		
		d.FirstName, 
		d.Surname, 
		d.Title
	);
GO
