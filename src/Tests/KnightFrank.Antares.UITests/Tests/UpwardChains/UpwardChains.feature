Feature: Upward chains

@UpwardChains
Scenario: Create upward chains transactions for residetial sale activity
	Given Contacts are created in database	
		| Title | FirstName | LastName  |
		| Sir   | Burt      | Lancaster |
		| Lady  | Kirk      | Douglas   |
		And Company is created in database
			| Name          |
			| Chain Company |
		And Contacts are created in database
			| Title | FirstName | LastName |
			| Sir   | Peter     | Sellers  |
			| Sir   | Henry     | Fonda    |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 2           | 3           | 2             | 3             | 2            | 3            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2   | Line3 | Postcode | City  | County         |
			| 32             | Docs Flat    | Dock St |       | LS10     | Leeds | West Yorkshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2010   | 1500000  |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type             | Status |
			| Residential Sale | New    |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 4           | 5           | 2             | 3             | 3            | 4            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2         | Line3 | Postcode | City  | County         |
			| 4              | Sunrise Villa | Sunnyview Ave |       | LS11 8QY | Leeds | West Yorkshire |
	When User navigates to view offer page with id
		And User clicks add upward chain button on view offer page
		And User fills in chain transaction details on view offer page
			| EndOfChain | Vendor          |
			| false      | Abraham Lincoln |
		And User selects property in chain transaction on view offer page
	Then Chain property details on view offer page are same as the following
		| Property                                                    |
		| 4 Sunrise Villa Sunnyview Ave LS11 8QY Leeds West Yorkshire |
	When User selects knight frank agent in chain transaction on view offer page
		| KnightFrankAgent |
		| John Smith       |
	Then Chain knight frank agent details on view offer page are same as the following
		| KnightFrankAgent |
		| John Smith       |
	When User selects solicitor in chain transaction on view offer page
		| Solicitor      | SolicitorCompany |
		| Burt Lancaster | Chain Company    |
	Then Chain solicitor details on view offer page are same as the following
		| Solicitor      | SolicitorCompany |
		| Burt Lancaster | Chain Company    |
	When User selects progress details in chain transaction on view offer page
		| Mortgage | Survey | Searches | Enquiries | ContractAgreed |
		|          |        |          |           |                |
		And User clicks save chain button on view offer page

@ignore
@UpwardChains
Scenario: Test
	Given Contacts are created in database	
		| Title | FirstName | LastName  |
		| Sir   | Burt      | Lancaster |
		| Lady  | Kirk      | Douglas   |
		And Company is created in database
			| Name          |
			| Chain Company |
		And Contacts are created in database
			| FirstName | LastName | Title |
			| Peter     | Sellers  | Sir   |
			| Henry     | Fonda    | Sir   |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 2           | 3           | 2             | 3             | 2            | 3            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2   | Line3 | Postcode | City  | County         |
			| 32             | Docs Flat    | Dock St |       | LS10     | Leeds | West Yorkshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2010   | 1500000  |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type             | Status |
			| Residential Sale | New    |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 4           | 5           | 2             | 3             | 3            | 4            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2         | Line3 | Postcode | City  | County         |
			| 4              | Sunrise Villa | Sunnyview Ave |       | LS11 8QY | Leeds | West Yorkshire |
		And Upward chain is created in database
			| EndOfChain | Vendor     | KnightFrankAgent |
			| false      | James Dean | true             |
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 4           | 5           | 2             | 3             | 3            | 4            |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2         | Line3 | Postcode | City  | County         |
			| 5              | Sunrise Villa | Sunnyview Ave |       | LS11 8QY | Leeds | West Yorkshire |
		And Upward chain is created in database
			| EndOfChain | Vendor     | KnightFrankAgent |
			| true       | James Door | true             |
	When User navigates to view offer page with id
