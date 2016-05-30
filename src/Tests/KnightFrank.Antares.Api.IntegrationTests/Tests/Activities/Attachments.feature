Feature: Attachments

@Attachment
Scenario: Upload attachment
Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| ActivityStatus       | PreAppraisal     |
			| Division             | Residential      |
			| ActivityDocumentType | TermsOfBusiness  |
			| ActivityUserType     | LeadNegotiator   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
	When User uploads attachment for latest activity id for TermsOfBusiness with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get OK http status code

@Attachment
Scenario Outline: Upload attachment with invalid data
Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| ActivityStatus       | PreAppraisal     |
			| Division             | Residential      |
			| ActivityDocumentType | <documentType>   |
			| ActivityUserType     | LeadNegotiator   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
	When User uploads attachment for <activityId> activity id for <documentType> with following data
		| FileName | Size | ExternalDocumentId                   |
		| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	Then User should get <statusCode> http status code

	Examples:
	| documentType    | activityId                           | statusCode |
	| Offices         | latest                               | BadRequest |
	| TermsOfBusiness | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 | BadRequest |
	| TermsOfBusiness |                                      | NotFound   |

@Activity
Scenario: Get Activity with attachment
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode         | enumTypeItemCode |
			| ActivityStatus       | PreAppraisal     |
			| Division             | Residential      |
			| ActivityDocumentType | TermsOfBusiness  |
			| ActivityUserType     | LeadNegotiator   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And Attachment for TermsOfBusiness with following data exists in database
			| FileName | Size | ExternalDocumentId                   |
			| abc.pdf  | 1024 | ba3b115b-4a5f-42c9-8e0f-25b7ed903b00 |
	When User gets activity with latest id
	Then User should get OK http status code
		And Retrieved activity should have expected attachments 
