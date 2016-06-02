Feature: Negotiators

Scenario: Update Activity with negotiators
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith and today plus 1 next call date exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin |
		| jdoe                 |
		| jrambo               |
	When User updates activity with defined negotiators
	Then User should get OK http status code
		And Activity details should be the same as already added

Scenario: Update Activity with lead negotiator and next call date
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith and today plus 1 next call date exists in database
	When User updates activity with defined negotiators
	Then User should get OK http status code
		And Activity details should be the same as already added

Scenario: Update Activity with lead negotiator only
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin jsmith and today plus 1 next call date exists in database
	When User updates activity with defined negotiators
	Then User should get OK http status code
		And Activity details should be the same as already added

Scenario Outline: Update activity with invalid negotiators data
	Given Activity exists in database
		And Lead negotiator with ActiveDirectoryLogin <LeadNegotiator> and today plus <nextCallDate> next call date exists in database
		And Following secondary negotiators exists in database
		| ActiveDirectoryLogin  |
		| <SecondaryNegotiator> |
	When User updates activity with defined negotiators
	Then User should get BadRequest http status code

	Examples: 
	| LeadNegotiator | SecondaryNegotiator | nextCallDate |
	| jsmith         | jsmith              | 1            |
	| invalid        | jsmith              | 1            |
	| jsmith         | jsmith              | -1           |

Scenario: Update only last call date
	Given Activity exists in database
	When User updates last call date by adding 2 days for valid user
	Then User should get OK http status code
		And Last call date should be updated in data base

Scenario Outline: Update last call date for invalid data
	Given Activity exists in database
	When User updates last call date by adding <nextCallDate> days for <user> user
	Then User should get BadRequest http status code

	Examples: 
	 | nextCallDate | user    |
	 | null         | valid   |
	 | -2           | valid   |
	 | 2            | invalid |
