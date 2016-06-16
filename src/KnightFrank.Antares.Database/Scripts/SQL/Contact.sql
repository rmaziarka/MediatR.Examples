
CREATE TABLE #TempContact (
	[Id] UNIQUEIDENTIFIER  NOT NULL DEFAULT (newsequentialid()),
	[FirstName] NVARCHAR (MAX) NULL,
	[LastName] NVARCHAR (MAX) NULL,
	[Title] NVARCHAR (MAX) NULL,
	[MailingFormalSalutation] NVARCHAR (MAX) NULL,
	[MailingSemiformalSalutation] NVARCHAR (MAX) NULL,
	[MailingInformalSalutation] NVARCHAR (MAX) NULL,
	[MailingPersonalSalutation] NVARCHAR (MAX) NULL,
	[MailingEnvelopeSalutation] NVARCHAR (MAX) NULL,
	[DefaultMailingSalutationId] UNIQUEIDENTIFIER NULL,
	[EventInviteSalutation] NVARCHAR (MAX) NULL,
	[EventSemiformalSalutation] NVARCHAR (MAX) NULL,
	[EventInformalSalutation] NVARCHAR (MAX) NULL,
	[EventPersonalSalutation] NVARCHAR (MAX) NULL,
	[EventEnvelopeSalutation] NVARCHAR (MAX) NULL,
	[DefaultEventSalutationId] UNIQUEIDENTIFIER NULL
);

ALTER TABLE Contact NOCHECK CONSTRAINT ALL

BULK INSERT #TempContact
    FROM '$(OutputPath)\Scripts\Data\Test\Contact.csv'
        WITH
    (
		FIRSTROW = 2,
		FIELDTERMINATOR = ';',
		ROWTERMINATOR = '\n',
		TABLOCK
    )
    	
MERGE dbo.Contact AS T
	USING #TempContact AS S	
	ON 
	(
        (T.Id = S.Id)
	)
	WHEN MATCHED THEN
		UPDATE SET 
		T.[FirstName] = S.[FirstName],
		T.[LastName] = S.[LastName],
		T.[Title] = S.[Title],
		T.[MailingFormalSalutation] = S.[MailingFormalSalutation],
		T.[MailingSemiformalSalutation] = S.[MailingSemiformalSalutation],
		T.[MailingInformalSalutation] = S.[MailingInformalSalutation],
		T.[MailingPersonalSalutation] = S.[MailingPersonalSalutation],
		T.[MailingEnvelopeSalutation] = S.[MailingEnvelopeSalutation],
		T.[DefaultMailingSalutationId] = S.[DefaultMailingSalutationId],
		T.[EventInviteSalutation] = S.[EventInviteSalutation],
		T.[EventSemiformalSalutation] = S.[EventSemiformalSalutation],
		T.[EventInformalSalutation] = S.[EventInformalSalutation],
		T.[EventPersonalSalutation] = S.[EventPersonalSalutation],
		T.[EventEnvelopeSalutation] = S.[EventEnvelopeSalutation],
		T.[DefaultEventSalutationId] = S.[DefaultEventSalutationId]

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT ([Id], [FirstName], [LastName], [Title], 
			[MailingFormalSalutation], [MailingSemiformalSalutation], [MailingInformalSalutation],
			[MailingPersonalSalutation], [MailingEnvelopeSalutation], [DefaultMailingSalutationId],
			[EventInviteSalutation], [EventSemiformalSalutation], [EventInformalSalutation],
			[EventPersonalSalutation], [EventEnvelopeSalutation], [DefaultEventSalutationId],
			[CreatedDate],[LastModifiedDate],[UserId])
		VALUES ([Id], [FirstName], [LastName], [Title], 
			[MailingFormalSalutation], [MailingSemiformalSalutation], [MailingInformalSalutation],
			[MailingPersonalSalutation], [MailingEnvelopeSalutation], [DefaultMailingSalutationId],
			[EventInviteSalutation], [EventSemiformalSalutation], [EventInformalSalutation],
			[EventPersonalSalutation], [EventEnvelopeSalutation], [DefaultEventSalutationId],
			GETDATE(), GETDATE(), NULL)

    WHEN NOT MATCHED BY SOURCE THEN DELETE;
    
ALTER TABLE Contact WITH CHECK CHECK CONSTRAINT ALL
DROP TABLE #TempContact
