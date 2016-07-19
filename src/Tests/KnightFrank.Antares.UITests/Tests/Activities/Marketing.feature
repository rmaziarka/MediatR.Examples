Feature: Marketing UI tests

@Activity
Scenario: Set marketing details on residential letting activity
	Given Contacts are created in database
		| Title | FirstName | LastName  |
		| Lady  | Ellen     | DeGeneres |
		And Property with Residential division and Flat type is defined
		And Property attributes details are defined
			| MinArea | MaxArea | MinLandArea | MaxLandArea |
			| 2000    | 3000    | 4000        | 5000        |
		And Property characteristics are defined
		And Property in GB is created in database
			| PropertyNumber | PropertyName      | Line2   | Line3 | Postcode | City      | County     |
			| 24             | One Latin Culture | Hope St |       | L1 9BX   | Liverpool | Merseyside |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 01-05-2016   | 375000   |
	When User navigates to view property page with id
		And User clicks add activites button on view property page
		And User selects activity status and type on create actvity page
			| Type                | Status             |
			| Open Market Letting | To Let Unavailable |
		And User fills in basic information on create activity page
			| Source     | SourceDescription | PitchingThreats |
			| Magazine A | Text              | Text            |
		And User fills in attendees on create activity page
			| Attendees       | InvitationText |
			| Ellen DeGeneres | Text           |
		And User clicks save button on create activity page
	Then View activity page should be displayed
	When User switches to marketing tab on view activity page
	Then Marketing description details on marketing tab on view activity page are same as the following
		| Strapline | FullDescription | LocationDescription |
		| -         | -               | -                   |
		And Advertising details on marketing tab on view activity page are same as the following
			| PublishToWeb | AdvertisingNote | PrPermitted | PrContent |
			| No           | -               | No          | -         |
		And Sales boards details on marketing tab on view activity page are same as the following
			| Type | Status | BoardUpToDate | SpecialInstructions |
			| None | -      | No            | -                   |
	When User clicks edit marketing button on marketing tab on view activity page
		And User fills in marketing description on marketing tab on view activity page
			| Strapline  | FullDescription  | LocationDescription  |
			| Strap line | Full description | Location description |
		And User fills in advertising on marketing tab on view activity page
			| PublishToWeb | AdvertisingNote  | PrPermitted | PrContent  |
			| No           | Advertising note | No          | Pr content |
		And User fills in sales boards on marketing tab on view activity page
			| Type | Status | BoardUpToDate | SpecialInstructions  |
			|      | To Let | No            | Special instructions |
		And User clicks save marketing button on marketing tab on view activity page
	Then Marketing description details on marketing tab on view activity page are same as the following
		| Strapline  | FullDescription  | LocationDescription  |
		| Strap line | Full description | Location description |
		And Advertising details on marketing tab on view activity page are same as the following
			| PublishToWeb | AdvertisingNote  | PrPermitted | PrContent  |
			| No           | Advertising note | No          | Pr content |
		And Sales boards details on marketing tab on view activity page are same as the following
			| Type | Status | BoardUpToDate | SpecialInstructions  |
			| None | To Let | No            | Special instructions |

@Activity
Scenario: Set marketing details on residential sale activity
	Given Contacts are created in database
		| Title | FirstName | LastName |
		| Lady  | Sigourney | Weaver   |
		And Property with Residential division and Maisonette type is defined
		And Property attributes details are defined
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms |
			| 2           | 4           | 1             | 2             | 2            | 2            |
		And Property in GB is created in database
			| PropertyNumber | PropertyName                     | Line2   | Line3 | Postcode | City      | County     |
			| 7A             | St Petersburg Russian Restaurant | York St |       | L1 5BN   | Liverpool | Merseyside |
		And Property ownership is defined
			| PurchaseDate | BuyPrice |
			| 10-10-2015   | 80000    |
		And Long leasehold Sale activity with For Sale Unavailable status is defined
	When User navigates to view activity page with id
		And User switches to marketing tab on view activity page
	When User clicks edit marketing button on marketing tab on view activity page
		And User fills in marketing description on marketing tab on view activity page
			| Strapline  | FullDescription  | LocationDescription  |
			| Strap line | Full description | Location description |
		And User fills in advertising on marketing tab on view activity page
			| PublishToWeb | AdvertisingNote  | PrPermitted | PrContent  | PortalToMarketOn |
			| Yes          | Advertising note | Yes         | Pr content | portal2.co.uk    |
		And User fills in sales boards on marketing tab on view activity page
			| Type | Status   | BoardUpToDate | SpecialInstructions  |
			| Flag | For Sale | Yes           | Special instructions |
		And User clicks save marketing button on marketing tab on view activity page
	Then Marketing description details on marketing tab on view activity page are same as the following
		| Strapline  | FullDescription  | LocationDescription  |
		| Strap line | Full description | Location description |
		And Advertising details on marketing tab on view activity page are same as the following
			| PublishToWeb | AdvertisingNote  | PrPermitted | PrContent  | PortalToMarketOn |
			| Yes          | Advertising note | Yes         | Pr content | portal2.co.uk    |
		And Sales boards details on marketing tab on view activity page are same as the following
			| Type | Status   | BoardUpToDate | SpecialInstructions  |
			| Flag | For Sale | Yes           | Special instructions |
