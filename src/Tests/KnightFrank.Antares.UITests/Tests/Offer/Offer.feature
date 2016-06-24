Feature: Offer UI tests

@Requirement
@Offer
Scenario: Create residential sales offer on requirement
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Sir   | John      | Soane    |
		| Sir   | Robert    | McAlpine |
		| Sir   | Edward    | Graham   |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 10          | 12          | 2             | 4             | 8            | 10           | 20000   | 25000   | 30000       | 50000       | 10                  | 20                  |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2                | Postcode | City   | County        |
			| 13             | John Soane’s house | Lincoln’s Inn Fields | WC2A 3BP | London | London county |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 10-01-1998   | 100000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice  | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 90000000 | 120000000 | 5           | 15          | 2                 | 4                 | 2            | 10           | 15               | 25               | 20000   | 25000   | 30000       | 50000       | Note        |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
	Then Activity details on view requirement page are same as the following
		| Details                                     |
		| John Soane’s house, 13 Lincoln’s Inn Fields |
	When User fills in offer details on view requirement page
		| Status | Offer  | SpecialConditions |
		| New    | 100000 | Text              |
		And User clicks save offer button on view requirement page
	Then New offer should be created and displayed on view requirement page
		And Offer details on 1 position on view requirement page are same as the following
			| Details                                     | Offer      | Status |
			| John Soane’s house, 13 Lincoln’s Inn Fields | 100000 GBP | NEW    |
	When User clicks 1 offer details on view requirement page
	Then Offer details on view requirement page are same as the following
		| Details                                     | Status | Offer      | SpecialConditions | Negotiator |
		| John Soane’s house, 13 Lincoln’s Inn Fields | New    | 100000 GBP | Text              | John Smith |
	When User clicks view activity from offer on view requirement page
	Then View activity page should be displayed
		And Offer should be displayed on view activity page
		And Offer details on 1 position on view activity page are same as the following
			| Details                                    | Offer      | Status |
			| John Soane, Robert McAlpine, Edward Graham | 100000 GBP | NEW    |
	When User clicks 1 offer details on view activity page
	Then Offer details on view activity page are same as the following
		| Details                                    | Status | Offer      | SpecialConditions | Negotiator |
		| John Soane, Robert McAlpine, Edward Graham | New    | 100000 GBP | Text              | John Smith |

@Requirement
@Offer
Scenario: Update residential sales offer on requirement
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Dr    | Indiana   | Jackson  |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 1             | 1            | 2            | 1000    | 3000    | 1400        | 5000        | 1                   | 2                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2     | Postcode | City  | County         |
			| 22             | House        | Eltham Dr | LS6 2TU  | Leeds | West Yorkshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2013   | 10000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 1           | 3           | 1                 | 2                 | 1            | 3            | 1                | 3                | 900     | 3500    | 1200        | 6000        | Note        |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Status |
			| New    |
	When User navigates to view requirement page with id
		And User clicks edit offer button for 1 offer on view requirement page
	Then Activity details on view requirement page are same as the following
		| Details             |
		| House, 22 Eltham Dr |
	When User fills in offer details on view requirement page
		| Status   | Offer | SpecialConditions  |
		| Accepted | 2000  | Special conditions |
		And User clicks save offer button on view requirement page
		And User clicks 1 offer details on view requirement page
	Then Offer details on view requirement page are same as the following
		| Details             | Status   | Offer    | SpecialConditions  | Negotiator |
		| House, 22 Eltham Dr | Accepted | 2000 GBP | Special conditions | John Smith |

@Offer
Scenario: View offer details page
	Given Contacts are created in database
		| Title  | FirstName | LastName |
		| Madame | Judith    | Greciet  |
		| Chef   | Julius    | Chaloff  |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 4           | 5           | 2             | 4             | 1            | 2            | 2000    | 2500    | 3000        | 5000        | 1                   | 1                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName    | Line2   | Postcode | City      | County     |
			| 34             | Greciet’s house | Bixteth | L3 9BA   | Liverpool | Merseyside |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-01-2015   | 100000   |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 90000    | 120000   | 3           | 4           | 2                 | 4                 | 2            | 2            | 1                | 2                | 2000    | 2500    | 3000        | 5000        | Note        |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
		And User fills in offer details on view requirement page
			| Status    | Offer | SpecialConditions     |
			| Withdrawn | 95000 | My special conditions |
		And User clicks save offer button on view requirement page
	Then New offer should be created and displayed on view requirement page
	When User clicks 1 offer details on view requirement page
		And User clicks details offer link on view requirement page
	Then View offer page should be displayed
		And Offer header details on view offer page are same as the following
			| Details                        | Status    |
			| Judith Greciet, Julius Chaloff | Withdrawn |
		And Offer activity details on view offer page are same as the following
			| Details                     |
			| Greciet’s house, 34 Bixteth |
		And Offer requirement details on view offer page are same as the following
			| Details                        |
			| Judith Greciet, Julius Chaloff |
		And Offer details on view offer page are same as the following
			| Status    | Offer     | SpecialConditions     | Negotiator |
			| Withdrawn | 95,000.00 | My special conditions | John Smith |
	When User clicks activity details on view offer page
	Then Activity details on view offer page are same as the following 
		| Status        | Negotiator | Vendor                        | Type          |
		| Pre-appraisal | John Smith | Judith Greciet;Julius Chaloff | Freehold Sale |
	When User clicks view activity link from activity on view offer page
	Then View activity page should be displayed
	When User goes back to previous page
		And User clicks requirement details button on view offer page
	Then View requirement page should be displayed

@Offer
Scenario: Update new residential sales offer
	Given Contacts are created in database
		| Title | FirstName | LastName     |
		| Lady  | Sarah     | McCorquodale |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 4           | 6           | 2             | 3             | 3            | 5            | 10000   | 31000   | 14000       | 51000       | 3                   | 4                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2        | Postcode | City     | County  |
			| 84             | Sarah McCorquodale | Granville Rd | CA2 7BA  | Carlisle | Cumbria |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 10-12-2000   | 100000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 100000   | 500000   | 3           | 6           | 2                 | 4                 | 2            | 6            | 1                | 6                | 9000    | 32000   | 12000       | 60000       | Note        |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Status |
			| New    |
	When User navigates to view offer page with id
		And User clicks edit offer button on view offer page
		And User fills in offer details on edit offer page
			| Status | Offer  | SpecialConditions     |
			| New    | 450000 | My special conditions |
		And User clicks save offer on edit offer page
	Then Offer updated success message should be displayed
		And Offer details on view offer page are same as the following
			| Status | Offer      | SpecialConditions     | Negotiator |
			| New    | 450,000.00 | My special conditions | John Smith |
		And Offer header details on view offer page are same as the following
			| Details            | Status |
			| Sarah McCorquodale | New    |
		And Offer activity details on view offer page are same as the following
			| Details                             |
			| Sarah McCorquodale, 84 Granville Rd |
		And Offer requirement details on view offer page are same as the following
			| Details            |
			| Sarah McCorquodale |

@Offer
Scenario: Create and update accepted residential sales offer
	Given Contacts are created in database
		| Title | FirstName | Surname   |
		| Sir   | Steve     | Harris    |
		| Sir   | Dave      | Murray    |
		| Sir   | Adrian    | Smith     |
		| Sir   | Bruce     | Dickinson |
		And Company is created in database
			| Name        | WebsiteUrl             | ClientCarePageUrl      |
			| Objectivity | https://www.google.com | https://www.google.com |
		And Contacts are created in database
			| Title | FirstName | Surname |
			| Sir   | Mark      | Walport |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 5           | 6           | 3             | 5             | 2            | 3            | 2500    | 3000    | 3500        | 5500        | 2                   | 2                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName  | Line2    | Postcode | City    | County        |
			| 21             | Moselle House | Derby Rd | WD17 2LW | Watford | Hertfordshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-01-2015   | 100000   |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceptionRooms | MaxReceptionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea | Description |
			| 90000    | 120000   | 4           | 7           | 3                 | 5                 | 2            | 4            | 1                | 2                | 2000    | 3500    | 3000        | 6000        | Note        |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
		And User fills in offer details on view requirement page
			| Status   | Offer  | SpecialConditions     |
			| Accepted | 110000 | My special conditions |
		And User clicks save offer button on view requirement page
	Then New offer should be created and displayed on view requirement page
	When User clicks details offer button for 1 offer on view requirement page
	Then View offer page should be displayed
		And Offer progress summary details on view offer page are same as the following
			| MortgageStatus | MortgageSurveyStatus | AdditionalSurveyStatus | SearchStatus | Enquiries   | ContractApproved |
			| Unknown        | Unknown              | Unknown                | Not started  | Not started | false            |
	When User clicks edit offer button on view offer page
		And User fills in offer details on edit offer page
			| Status   | Offer  | SpecialConditions |
			| Accepted | 120000 | Text              |
		And User fills in offer progress summary on edit offer page
			| MortgageStatus | MortgageSurveyStatus | AdditionalSurveyStatus | SearchStatus | Enquiries | ContractApproved |
			| Agreed         | Complete             | Complete               | Complete     | Complete  | true             |
		And User fills in offer mortgage details on edit offer page
			| MortgageLoanToValue | Broker       | BrokerCompany | Lender      | LenderCompany | Surveyor     | SurveyorCompany |
			| 100                 | Steve Harris | Objectivity   | Dave Murray | Objectivity   | Adrian Smith | Objectivity     |
		And User fills in offer additional details on edit offer page
			| AdditionalSurveyor | AdditionalSurveyorCompany | Comment  |
			| Bruce Dickinson    | Objectivity               | Approved |
	Then Following company contacts should be displayed on edit offer page
		| Broker       | BrokerCompany | Lender      | LenderCompany | Surveyor     | SurveyorCompany | AdditionalSurveyor | AdditionalSurveyorCompany |
		| Steve Harris | Objectivity   | Dave Murray | Objectivity   | Adrian Smith | Objectivity     | Bruce Dickinson    | Objectivity               |
	When User clicks save offer on edit offer page
	Then Offer updated success message should be displayed
		And Offer details on view offer page are same as the following
			| Status   | Offer      | SpecialConditions | Negotiator |
			| Accepted | 120,000.00 | Text              | John Smith |
		And Offer progress summary details on view offer page are same as the following
			| MortgageStatus | MortgageSurveyStatus | AdditionalSurveyStatus | SearchStatus | Enquiries | ContractApproved |
			| Agreed         | Complete             | Complete               | Complete     | Complete  | true             |
		And Offer mortgage details details on view offer page are same as the following
			| MortgageLoanToValue | Broker       | BrokerCompany | Lender      | LenderCompany | Surveyor     | SurveyorCompany |
			| 100                 | Steve Harris | Objectivity   | Dave Murray | Objectivity   | Adrian Smith | Objectivity     |
		And Offer additional details on view offer page are same as the following
			| AdditionalSurveyor | AdditionalSurveyorCompany | Comment  |
			| Bruce Dickinson    | Objectivity               | Approved |
