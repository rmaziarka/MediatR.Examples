Feature: Upward chains

@UpwardChains
Scenario: Create upward chains transactions for residetial sale activity
	Given Contacts are created in database	
		| Title | FirstName | LastName  |
		| Sir   | Burt      | Lancaster |
		| Sir   | Kirk      | Douglas   |
		And Company is created in database
			| Name          |
			| Chain Company |
		And Contacts are created in database
			| Title | FirstName | LastName |
			| Sir   | Peter     | Sellers  |
			| Sir   | Henry     | Fonda    |
		And Property with Residential division and Flat type is defined
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
		And User clicks save chain transaction button on view offer page
	Then Property details on view offer page are same as the following
		| Property                                       |
		| 32 Docs Flat Dock St LS10 Leeds West Yorkshire |
		And Chain transaction cards details on view offer page are same as the following
			| Property                       | Mortgage | Survey  | Searches    | Enquiries   | ContractAgreed |
			| Sunrise Villa, 4 Sunnyview Ave | Unknown  | Unknown | Outstanding | Outstanding | Outstanding    |
	When User clicks 1 chain transaction details on view offer page
	Then Chain transaction details on view offer page are same as the following
		| EndOfChain | Property                                                    | Vendor          | KnightFrankAgent | Solicitor      | SolicitorCompany | Mortgage | Survey  | Searches    | Enquiries   | ContractAgreed |
		| false      | 4 Sunrise Villa Sunnyview Ave LS11 8QY Leeds West Yorkshire | Abraham Lincoln | John Smith       | Burt Lancaster | Chain Company    | Unknown  | Unknown | Outstanding | Outstanding | Outstanding    |
	When Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2        | Line3 | Postcode | City       | County        |
			| 17             | Sundown Flat | New Canal St |       | B5       | Birmingham | West Midlands |
			And User clicks add upward chain button on view offer page
		And User fills in chain transaction details on view offer page
			| EndOfChain | Vendor        |
			| true       | Ulysses Grant |
		And User selects property in chain transaction on view offer page
		And User selects 3rd party agent in chain transaction on view offer page
			| OtherAgent     | OtherAgentCompany |
			| Burt Lancaster | Chain Company     |
	Then Chain 3rd party agent details on view offer page are same as the following
		| OtherAgent     | OtherAgentCompany |
		| Burt Lancaster | Chain Company     |
	When User selects solicitor in chain transaction on view offer page
		| Solicitor    | SolicitorCompany |
		| Kirk Douglas | Chain Company    |
		And User selects progress details in chain transaction on view offer page
			| Mortgage | Survey   | Searches | Enquiries | ContractAgreed |
			| Complete | Complete | Complete | Complete  | Complete       |
		And User clicks save chain transaction button on view offer page
	Then Add upward chain button should not be displayed on view offer page
		And Property details on view offer page are same as the following
			| Property                                       |
			| 32 Docs Flat Dock St LS10 Leeds West Yorkshire |
		And Chain transaction cards details on view offer page are same as the following
			| Property                       | Mortgage | Survey   | Searches    | Enquiries   | ContractAgreed |
			| Sundown Flat, 17 New Canal St  | Complete | Complete | Complete    | Complete    | Complete       |
			| Sunrise Villa, 4 Sunnyview Ave | Unknown  | Unknown  | Outstanding | Outstanding | Outstanding    |
	When User clicks 1 chain transaction details on view offer page
	Then Chain transaction details on view offer page are same as the following
		| EndOfChain | Property                                                 | Vendor        | Buyer           | OtherAgent     | OtherAgentCompany | Solicitor    | SolicitorCompany | Mortgage | Survey   | Searches | Enquiries | ContractAgreed |
		| true       | 17 Sundown Flat New Canal St B5 Birmingham West Midlands | Ulysses Grant | Abraham Lincoln | Burt Lancaster | Chain Company     | Kirk Douglas | Chain Company    | Complete | Complete | Complete | Complete  | Complete       |

@UpwardChains
Scenario: Update upward chains transactions for residetial sale activity
	Given Contacts are created in database	
		| Title | FirstName | LastName  |
		| Dr    | Calvin    | Coolidge  |
		| Dr    | Theodore  | Roosevelt |
		| Dr    | William   | McKinley  |
		And Company is created in database
			| Name          |
			| Chain Company |
		And Contacts are created in database
			| FirstName | LastName | Title |
			| Woodrow   | Wilson   | Sir   |
			| William   | Taft     | Sir   |
		And Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2            | Line3 | Postcode | City     | County         |
			| 1              | Wardley House | Little Horton Ln |       | BD5 0AG  | Bradford | West Yorkshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-1990   | 15000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type             | Status |
			| Residential Sale | New    |
		And Property with Residential division and House type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2   | Line3 | Postcode | City     | County         |
			| 1              | West Villa   | City Rd |       | BD8 8ER  | Bradford | West Yorkshire |
		And Upward chain is created in database
			| EndOfChain | Vendor     | KnightFrankAgent |
			| false      | James Dean | true             |
		And Property with Residential division and Flat type is defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2     | Line3 | Postcode | City  | County         |
			| 3              | East Villa   | Sayner Ln |       | LS10 1LS | Leeds | West Yorkshire |
	When User navigates to view offer page with id
		And User clicks edit chain button for 1 chain on view offer page
		And User fills in chain transaction details on view offer page
			| EndOfChain | Vendor        |
			| true       | Ulysses Grant |
		And User selects property in chain transaction on view offer page
		And User selects 3rd party agent in chain transaction on view offer page
			| OtherAgent         | OtherAgentCompany |
			| Theodore Roosevelt | Chain Company     |
		And User selects solicitor in chain transaction on view offer page
			| Solicitor       | SolicitorCompany |
			| Calvin Coolidge | Chain Company    |
		And User selects progress details in chain transaction on view offer page
			| Mortgage     | Survey   | Searches | Enquiries | ContractAgreed |
			| Not required | Complete | Complete | Complete  | Complete       |
		And User clicks save chain transaction button on view offer page
	Then Property details on view offer page are same as the following
		| Property                                                         |
		| 1 Wardley House Little Horton Ln BD5 0AG Bradford West Yorkshire |
		And Chain transaction cards details on view offer page are same as the following
			| Property                | Mortgage     | Survey   | Searches | Enquiries | ContractAgreed |
			| East Villa, 3 Sayner Ln | Not required | Complete | Complete | Complete  | Complete       |
	When User clicks 1 chain transaction details on view offer page
	Then Chain transaction details on view offer page are same as the following
		| EndOfChain | Property                                             | Vendor        | OtherAgent         | OtherAgentCompany | Solicitor       | SolicitorCompany | Mortgage     | Survey   | Searches | Enquiries | ContractAgreed |
		| true       | 3 East Villa Sayner Ln LS10 1LS Leeds West Yorkshire | Ulysses Grant | Theodore Roosevelt | Chain Company     | Calvin Coolidge | Chain Company    | Not required | Complete | Complete | Complete  | Complete       |
		And Add upward chain button should not be displayed on view offer page
	