﻿Feature: Company UI tests

@Company
Scenario: Create company
	Given User navigates to create contact page
		And User creates contacts on create contact page
			| Title | FirstName | Surname |
			| Sir   | Sean      | Connery |
			| Mrs   | Sarah     | Johns   |
		And User navigates to create company page
	When User fills in company details on create company page
		| Name         |
		| Knight Frank |
		And User selects contacts on create company page
			| FirstName | Surname |
			| Sean      | Connery |
			| Sarah     | Johns   |
	Then list of company contacts should contain following contacts
		| FirstName | Surname |
		| Sean      | Connery |
		| Sarah     | Johns   |
	When User clicks save button on create company page
	Then New company should be created