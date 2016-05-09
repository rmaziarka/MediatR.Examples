Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	When User retrieves url for activity attachment upload for <activityDocumentTypeCode> activity document type
	Then User should get <statusCode> http status code

	Examples: 
	| activityDocumentTypeCode | statusCode |
	| invalidCode              | BadRequest |
	| MarketingSignOff         | OK         |
