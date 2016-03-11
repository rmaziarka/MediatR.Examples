Feature: Residential sales requirements 

@ignore	
Scenario: Save requirement to DB with contact and all detailed fields fullfiled
	Given Details of requirement are provided
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceprionRooms | MaxReceprionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea |
			| 1000000  | 4000000  | 1	        | 5			  | 0				  | 2				  | 1			 | 3			   | 1			   | 2			      | 1200	| 2000	  | 10000	    | 20000		  |
	When user retreive the data form DB
	Then requirement should be same as
			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceprionRooms | MaxReceprionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea |
			| 1000000  | 4000000  | 1		    | 5			  | 0				  | 2				  | 1			 | 3			| 1				   | 2				  | 1200	| 2000	  | 10000	    | 20000		  |

Scenario: Negative - Try save requirement to DB without contact and all detailed fields fullfiled
	Given Details of requirement are provided
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms | MinReceprionRooms | MaxReceprionRooms | MinBathrooms | MaxBathrooms | MinParkingSpaces | MaxParkingSpaces | MinArea | MaxArea | MinLandArea | MaxLandArea |
			| 1000000  | 4000000  | 1		    | 5			  | 0				  | 2				  | 1			 | 3			| 1			       | 2				  | 1200	| 2000	  | 10000	    | 20000	      |
	When user retreive the data from DB
	Then requirement should not be saved in DB
		And error message should be displayed - ask dev

Scenario: Save requirements with location and all valid fields
	Given user fills all fields for property location
 			| Street name | Postcode | Town   |
			| Marsh Rd	  | HA5 5NQ  | London |
	When user retreive the data form DB
	Then requirment should be save in DB
		And should be same as
			| Street name | Postcode | Town   |
			| Marsh Rd	  | HA5 5NQ  | London |

Scenario: Negative - try save requirement with location without contact field
	Given user fills all fields for property location
			| Street name | Postcode | Town	  |
			| Marsh Rd	  | HA5 5NQ	 | London |
		But applicant is not chosen from contact list
	When user retreive the data from DB
	Then requirement should not be saved to the DB 
		And error message should be displayed - ask dev