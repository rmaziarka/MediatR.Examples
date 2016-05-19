Feature: Negotiators
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Update Activity with negotiators
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin |
		| jdoe                 |
		| jrambo               |	
	When User updates activity status with defined negotiators
	Then User should get OK http status code
		And Retrieved activity should be same as in database

Scenario: Update Activity only with lead negotiator
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin |	
	When User updates activity status with defined negotiators
	Then User should get OK http status code
		And Retrieved activity should be same as in database


Scenario Outline: Check improper values
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin <LeadNegotiator> exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin  |
		| <SecondaryLegotiator> |
	When User updates activity status with defined negotiators
	Then User should get BadRequest http status code

		Examples: 
		| LeadNegotiator                       | SecondaryLegotiator |
		| jsmith                               | jsmith              |
		| 91AC6B12-020A-11E6-8D22-5E5517507C66 | jsmith              |
