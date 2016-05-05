Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	When User retrieves url for activity attachment upload for <activityDocumentType> activity document type
	Then User should get <statusCode> http status code

	Examples: 
	| activityDocumentType | statusCode |
	| invalidCode          | BadRequest |
	| MarketingSignOff     | OK         |
