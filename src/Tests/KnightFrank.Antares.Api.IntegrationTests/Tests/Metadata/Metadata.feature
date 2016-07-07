Feature: Metadata

# ------- activity -------

@Metadata
Scenario: Get activity attributes
	When User gets activity attributes
	Then User should get OK http status code

@Metadata
Scenario Outline: Get activity attributes with invalid data
	When User gets activity attributes with invalid <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data         | statusCode |
	| propertyType | BadRequest |
	| activityType | BadRequest |
	| pageType     | BadRequest |

	
@Metadata
Scenario Outline: Get activity attributes with empty data
	When User gets activity attributes with empty <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data         | statusCode |
	| propertyType | BadRequest |
	| activityType | BadRequest |
	| pageType     | BadRequest |

# ------- end activity -------

# ------- offer -------

@Metadata
Scenario: Get offer attributes
	When User gets offer attributes
	Then User should get OK http status code

@Metadata
Scenario Outline: Get offer attributes with invalid data
	When User gets offer attributes with invalid <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data            | statusCode |
	| requirementType | BadRequest |
	| offerType       | BadRequest |
	| pageType        | BadRequest |

	
@Metadata
Scenario Outline: Get offer attributes with empty data
	When User gets offer attributes with empty <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data            | statusCode |
	| requirementType | BadRequest |
	| offerType       | BadRequest |
	| pageType        | BadRequest |

# ------- end offer -------

# ------- requirement -------

@Metadata
Scenario: Get requirement attributes
	When User gets requirement attributes
	Then User should get OK http status code

@Metadata
Scenario Outline: Get requirement attributes with invalid data
	When User gets requirement attributes with invalid <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data            | statusCode |
	| requirementType | BadRequest |
	| pageType        | BadRequest |

	
@Metadata
Scenario Outline: Get requirement attributes with empty data
	When User gets requirement attributes with empty <data> data
	Then User should get <statusCode> http status code

	Examples: 
	| data            | statusCode |
	| requirementType | BadRequest |
	| pageType        | BadRequest |

# ------- end requirement -------