Feature: Navigation drawer UI tests

Scenario: Navigate through navigation drawer and check application 
	Given User navigates to create contact page
	When User opens navigation drawer menu
	Then Drawer menu should be displayed
	When User selects Contacts menu item
		And User clicks create button in drawer submenu
	Then Drawer Contacts submenu should be displayed
		And Contact form on create contact page should be displayed
	When User selects Companies menu item
		And User clicks create button in drawer submenu
	Then Drawer Companies submenu should be displayed
		And Company form on create company page should be diaplyed
	When User selects Properties menu item
		And User clicks create button in drawer submenu
	Then Drawer Properties submenu should be displayed
		And Property form on create property page should be displayed
	#When User clicks search link in drawer submenu
	#Then Search form on search property page should be displayed
	When User selects Requirements menu item
		And User clicks create button in drawer submenu
	Then Drawer Requirements submenu should be displayed
		And Requirement form on create requirement page should be displayed
	When User closes navigation drawer menu
		And User opens navigation drawer menu
	Then Drawer Requirements submenu should be displayed
		And Requirement form on create requirement page should be displayed
