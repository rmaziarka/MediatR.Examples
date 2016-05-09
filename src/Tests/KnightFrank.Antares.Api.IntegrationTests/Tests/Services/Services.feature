Feature: Services

@Attachment
Scenario Outline: Upload attachment for entity with ActivityDocumentType
	Given User gets <activityDocumentTypeCode>  for ActivityDocumentType
	When User retrieves url for activity attachment upload for <entityReferenceId> entity reference id
	Then User should get <statusCode> http status code

	Examples: 
	| entityReferenceId                    | activityDocumentTypeCode | statusCode |
	| 2BA89713-7FF8-4DF5-AD1E-1C1D55DDF510 | invalid			      | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | MarketingSignOff         | BadRequest |
	| 2BA89713-7FF8-4DF5-AD1E-1C1D55DDF510 | MarketingSignOff         | OK         |

@Attachment
Scenario Outline: Download attachment for entity with ActivityDocumentType
	Given User gets <activityDocumentTypeCode>  for ActivityDocumentType
	When User retrieves url for activity attachment download for <entityReferenceId> entity reference id
	Then User should get <statusCode> http status code

	Examples: 
	| entityReferenceId                    | activityDocumentTypeCode | statusCode |
	| 2BA89713-7FF8-4DF5-AD1E-1C1D55DDF510 | invalid			      | BadRequest |
	| 00000000-0000-0000-0000-000000000000 | MarketingSignOff         | BadRequest |
	| 2BA89713-7FF8-4DF5-AD1E-1C1D55DDF510 | MarketingSignOff         | OK         |