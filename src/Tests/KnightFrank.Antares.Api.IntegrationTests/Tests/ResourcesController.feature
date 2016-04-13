Feature: Resources Controller	

@Resources
Scenario: Get countries for Property of EnumTypeItem and for default location
	Given There is an AddressForm for GB country code
		And There exists AddressForm for Property EnumTypeItem
	When User retrieves countries for Property EnumTypeItem
	Then User should get OK http status code
		And Result contains single item with GB isoCode
