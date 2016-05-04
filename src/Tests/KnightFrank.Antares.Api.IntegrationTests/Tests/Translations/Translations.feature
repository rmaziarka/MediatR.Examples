Feature: Translations

@Translations
Scenario: Gets translations for resource by isoCode
	Given There is a Locale for xx country code
		And Country code xx is present in database
		And User creates following translations for countries in database
			| Code | TranslateValue |
			| xx   | Polska         |
		And User creates follwoing characteristics in database
			| Code  | DisplayText | enabled |
			| code  | 1           | 1       |
			| code1 | 1           | 1       |
		And User creates following translations for characteristics in database
			| Code  | TranslateValue   |
			| code  | charakterystyka  |
			| code1 | charakterystyka1 |
	When User retrieves translations for xx isocode
	Then Translations are as expected
