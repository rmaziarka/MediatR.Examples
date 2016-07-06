Feature: Offer UI tests

@Requirement
@Offer
Scenario: Create residential letting offer on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Lady  | Lori      | Petty   |
		| Lady  | Emilia    | Clarke  |
		| Lady  | Margot    | Robbie  |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 3           | 2             | 3             | 2            | 3            | 20000   | 25000   | 30000       | 50000       | 2                   | 3                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2     | Line3 | Postcode | City      | County           |
			| 8              | Lori Petty’s house | George St |       | NN16 0AW | Kettering | Northamptonshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-01-2000   | 1000000  |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
	#Then Activity details on view requirement page are same as the following
	#	| Details                         |
	#	| Lori Petty’s house, 8 George St |
	When User fills in letting offer details on view requirement page
		| Status | OfferPerWeek | SpecialConditions |
		| New    | 1000         | Text              |
		And User clicks save offer button on view requirement page
	Then New offer should be created and displayed on view requirement page
		And Letting offer details on 1 position on view requirement page are same as the following
			| Details                                                            | OfferPerWeek | Status |
			| 8 Lori Petty’s house George St NN16 0AW Kettering Northamptonshire | 1000         | NEW    |
	When User clicks 1 offer details on view requirement page
	Then Letting offer details on view requirement page are same as the following
		| Details                                                            | Status | OfferPerWeek | SpecialConditions | Negotiator |
		| 8 Lori Petty’s house George St NN16 0AW Kettering Northamptonshire | New    | 1000         | Text              | John Smith |
	#   -------TODO when view on activity is done-------
	#When User clicks view activity from offer on view requirement page
	#Then View activity page should be displayed
	#	And Offer should be displayed on view activity page
	#	And Offer details on 1 position on view activity page are same as the following
	#		| Details                                  | OfferPerWeek | Status |
	#		| Lori Petty, Emilia Clarke, Margot Robbie | 1000         | NEW    |
	#When User clicks 1 offer details on view activity page
	#Then Offer details on view activity page are same as the following
	#	| Details                                  | Status | OfferPerWeek | SpecialConditions | Negotiator |
	#	| Lori Petty, Emilia Clarke, Margot Robbie | New    | 1000         | Text              | John Smith |

@Requirement
@Offer
Scenario: Update residential letting offer on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Lady  | Vivien    | Leigh   |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 2           | 4           | 2             | 4             | 2            | 4            | 10000   | 30000   | 14000       | 50000       | 2                   | 4                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2         | Line3 | Postcode | City     | County        |
			| 1              | Vivien Leigh | Crutchley Ave |       | B78 3JT  | Tamworth | Staffordshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2010   | 99999    |
		And Property Open Market Letting activity is defined
		And Requirement for GB is created in database
			| Type                | Description |
			| Residential Letting | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type                | Status |
			| Residential Letting | New    |
	When User navigates to view requirement page with id
		And User clicks edit offer button for 1 offer on view requirement page
	Then Activity details on view requirement page are same as the following
		| Details                                                     |
		| 1 Vivien Leigh Crutchley Ave B78 3JT Tamworth Staffordshire |
	When User fills in letting offer details on view requirement page
		| Status   | OfferPerWeek | SpecialConditions  |
		| Accepted | 2000         | Special conditions |
		And User clicks save offer button on view requirement page
		And User clicks 1 offer details on view requirement page
	Then Letting offer details on view requirement page are same as the following
		| Details                                                     | Status   | OfferPerWeek | SpecialConditions  | Negotiator |
		| 1 Vivien Leigh Crutchley Ave B78 3JT Tamworth Staffordshire | Accepted | 2000         | Special conditions | John Smith |

@Requirement
@Offer
Scenario: Create residential sale offer on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname  |
		| Sir   | John      | Soane    |
		| Sir   | Robert    | McAlpine |
		| Sir   | Edward    | Graham   |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 10          | 12          | 2             | 4             | 8            | 10           | 20000   | 25000   | 30000       | 50000       | 10                  | 20                  |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2                | Line3 | Postcode | City   | County        |
			| 13             | John Soane’s house | Lincoln’s Inn Fields |       | WC2A 3BP | London | London county |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 10-01-1998   | 100000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
	#Then Activity details on view requirement page are same as the following
	#	| Details                                                                  |
	#	| 13 John Soane’s house Lincoln’s Inn Fields WC2A 3BP London London county |
	When User fills in sale offer details on view requirement page
		| Status | Offer  | SpecialConditions |
		| New    | 100000 | Text              |
		And User clicks save offer button on view requirement page
	Then New offer should be created and displayed on view requirement page
		And Sale offer details on 1 position on view requirement page are same as the following
			| Details                                                                  | Offer  | Status |
			| 13 John Soane’s house Lincoln’s Inn Fields WC2A 3BP London London county | 100000 | NEW    |
	When User clicks 1 offer details on view requirement page
	Then Sale offer details on view requirement page are same as the following
		| Details                                                                  | Status | Offer  | SpecialConditions | Negotiator |
		| 13 John Soane’s house Lincoln’s Inn Fields WC2A 3BP London London county | New    | 100000 | Text              | John Smith |
	#   -------TODO when view on activity is done-------
	#When User clicks view activity from offer on view requirement page
	#Then View activity page should be displayed
	#	And Offer should be displayed on view activity page
	#	And Offer details on 1 position on view activity page are same as the following
	#		| Details                                    | Offer  | Status |
	#		| John Soane, Robert McAlpine, Edward Graham | 100000 | NEW    |
	#When User clicks 1 offer details on view activity page
	#Then Offer details on view activity page are same as the following
	#	| Details                                    | Status | Offer  | SpecialConditions | Negotiator |
	#	| John Soane, Robert McAlpine, Edward Graham | New    | 100000 | Text              | John Smith |

@Requirement
@Offer
Scenario: Update residential sale offer on requirement
	Given Contacts are created in database
		| Title | FirstName | Surname |
		| Dr    | Indiana   | Jackson |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 1             | 1            | 2            | 1000    | 3000    | 1400        | 5000        | 1                   | 2                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName | Line2     | Line3 | Postcode | City  | County         |
			| 22             | House        | Eltham Dr |       | LS6 2TU  | Leeds | West Yorkshire |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-12-2013   | 10000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type             | Status |
			| Residential Sale | New    |
	When User navigates to view requirement page with id
		And User clicks edit offer button for 1 offer on view requirement page
	Then Activity details on view requirement page are same as the following
		| Details                                         |
		| 22 House Eltham Dr LS6 2TU Leeds West Yorkshire |
	When User fills in sale offer details on view requirement page
		| Status   | Offer | SpecialConditions  |
		| Accepted | 2000  | Special conditions |
		And User clicks save offer button on view requirement page
		And User clicks 1 offer details on view requirement page
	Then Sale offer details on view requirement page are same as the following
		| Details                                         | Status   | Offer | SpecialConditions  | Negotiator |
		| 22 House Eltham Dr LS6 2TU Leeds West Yorkshire | Accepted | 2000  | Special conditions | John Smith |

#SAME TEST FOR LETTING OFFER
@Offer
Scenario: View residential sale offer details page
	Given Contacts are created in database
		| Title  | FirstName | Surname |
		| Madame | Judith    | Greciet |
		| Chef   | Julius    | Chaloff |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 4           | 5           | 2             | 4             | 1            | 2            | 2000    | 2500    | 3000        | 5000        | 1                   | 1                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName    | Line2   | Line3 | Postcode | City      | County     |
			| 34             | Greciet’s house | Bixteth |       | L3 9BA   | Liverpool | Merseyside |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-01-2015   | 100000   |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |     
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
		And User fills in sale offer details on view requirement page
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
			| Details                                                |
			| 34 Greciet’s house Bixteth L3 9BA Liverpool Merseyside |
		And Offer requirement details on view offer page are same as the following
			| Details                        |
			| Judith Greciet, Julius Chaloff |
		And Offer details on view offer page are same as the following
			| Status    | Offer | SpecialConditions     | Negotiator |
			| Withdrawn | 95000 | My special conditions | John Smith |
	When User clicks activity details on view offer page
	Then Activity details on view offer page are same as the following 
		| Status        | Negotiator | Vendor                        | Type          |
		| Pre-appraisal | John Smith | Judith Greciet;Julius Chaloff | Freehold Sale |
	When User clicks view activity link from activity on view offer page
	Then View activity page should be displayed
	When User goes back to previous page
		And User clicks requirement details button on view offer page
	Then View requirement page should be displayed

#SAME TEST FOR LETTING OFFER?
@Offer
Scenario: Update new residential sale offer
	Given Contacts are created in database	
		| Title | FirstName | Surname |
		| Sir   | John      | Adams   |
		| Lady  | Sarah     | Adams   |
		And Company is created in database
			| Name    | WebsiteUrl             | ClientCarePageUrl      |
			| Testing | https://www.google.com | https://www.google.com |
	Given Contacts are created in database
		| Title | FirstName | Surname      |
		| Lady  | Sarah     | McCorquodale |
		And Property with Residential division and House type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 4           | 6           | 2             | 3             | 3            | 5            | 10000   | 31000   | 14000       | 51000       | 3                   | 4                   |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName       | Line2        | Line3 | Postcode | City     | County  |
			| 84             | Sarah McCorquodale | Granville Rd |       | CA2 7BA  | Carlisle | Cumbria |
		And Property ownership is defined
			| PurchaseDate | BuyPrice  |
			| 10-12-2000   | 100000000 |
		And Property Freehold Sale activity is defined
		And Requirement for GB is created in database
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
		And Offer for requirement is defined
			| Type             | Status |
			| Residential Sale | New    |
	When User navigates to view offer page with id
		And User clicks edit offer button on view offer page
		And User fills in offer details on edit offer page
			| Status | Offer  | SpecialConditions     |
			| New    | 450000 | My special conditions |
		And User selects solicitors on edit offer page
			| Vendor     | VendorCompany | Applicant   | ApplicantCompany |
			| John Adams | Testing       | Sarah Adams | Testing          |
		And User clicks save offer on edit offer page
	Then Offer updated success message should be displayed
		And Offer details on view offer page are same as the following
			| Status | Offer  | SpecialConditions     | Negotiator |
			| New    | 450000 | My special conditions | John Smith |
		And Offer header details on view offer page are same as the following
			| Details            | Status |
			| Sarah McCorquodale | New    |
		And Offer activity details on view offer page are same as the following
			| Details                                                     |
			| 84 Sarah McCorquodale Granville Rd CA2 7BA Carlisle Cumbria |
		And Offer requirement details on view offer page are same as the following
			| Details            |
			| Sarah McCorquodale |
		And Offer solicitors details on view offer page are same as the following
			| Vendor     | VendorCompany | Applicant   | ApplicantCompany |
			| John Adams | Testing       | Sarah Adams | Testing          |

@Offer
Scenario: Create and update accepted residential sale offer
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
			| Type             | Description |
			| Residential Sale | Description |
		And Viewing for requirement is defined
	When User navigates to view requirement page with id
		And User clicks make an offer button for 1 activity on view requirement page
		And User fills in sale offer details on view requirement page
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
			| Status   | Offer  | SpecialConditions | Negotiator |
			| Accepted | 120000 | Text              | John Smith |
		And Offer progress summary details on view offer page are same as the following
			| MortgageStatus | MortgageSurveyStatus | AdditionalSurveyStatus | SearchStatus | Enquiries | ContractApproved |
			| Agreed         | Complete             | Complete               | Complete     | Complete  | true             |
		And Offer mortgage details details on view offer page are same as the following
			| MortgageLoanToValue | Broker       | BrokerCompany | Lender      | LenderCompany | Surveyor     | SurveyorCompany |
			| 100                 | Steve Harris | Objectivity   | Dave Murray | Objectivity   | Adrian Smith | Objectivity     |
		And Offer additional details on view offer page are same as the following
			| AdditionalSurveyor | AdditionalSurveyorCompany | Comment  |
			| Bruce Dickinson    | Objectivity               | Approved |
