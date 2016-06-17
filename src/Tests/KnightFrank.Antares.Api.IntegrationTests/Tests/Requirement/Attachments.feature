Feature: Attachments

@Requirements
Scenario: Upload attachment
	Given Contacts exists in database
		| Title  | FirstName | Surname |
		| Mister | Tomasz    | Bien    |
		And Requirement exists in database
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode            | enumTypeItemCode |
			| RequirementDocumentType | TermsOfBusiness  |
	When User uploads requirement attachment for latest requirement id for TermsOfBusiness with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get OK http status code

@Requirements
Scenario Outline: Upload attachment with invalid data
	Given Contacts exists in database
		| Title  | FirstName | Surname |
		| Mister | Tomasz    | Bien    |
		And Requirement exists in database
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode            | enumTypeItemCode |
			| RequirementDocumentType | TermsOfBusiness  |
			| ActivityDocumentType    | Offices          |
	When User uploads requirement attachment for <requirementId> requirement id for <documentType> with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get <statusCode> http status code

	Examples:
	| documentType    | requirementId                        | statusCode |
	| Offices         | latest                               | BadRequest |
	| TermsOfBusiness | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 | BadRequest |
	| TermsOfBusiness |                                      | NotFound   |

@Requirements
Scenario: Get requirement with attachment
	Given Contacts exists in database
		| Title  | FirstName | Surname |
		| Mister | Tomasz    | Bien    |
		And Requirement exists in database
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode            | enumTypeItemCode |
			| RequirementDocumentType | TermsOfBusiness  |
		And Requirement attachment for TermsOfBusiness with following data exists in database
			| FileName | Size | ExternalDocumentId                   |
			| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	When User retrieves requirement for latest id
	Then User should get OK http status code
		And Retrieved requirement should have expected attachments 
