Feature: Negotiators

Scenario: Update Activity with negotiators
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin |
		| jdoe                 |
		| jrambo               |	
	When User updates activity with defined negotiators
	Then User should get OK http status code
		And Retrieved activity should be same as in database

Scenario: Update Activity with lead negotiator only
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin |	
	When User updates activity with defined negotiators
	Then User should get OK http status code
		And Retrieved activity should be same as in database

Scenario Outline: Update activity with invalid negotiators data
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin <LeadNegotiator> exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin  |
		| <SecondaryNegotiator> |
	When User updates activity with defined negotiators
	Then User should get BadRequest http status code

	Examples: 
	| LeadNegotiator                       | SecondaryNegotiator |
	| jsmith                               | jsmith              |
	| 91AC6B12-020A-11E6-8D22-5E5517507C66 | jsmith              |
