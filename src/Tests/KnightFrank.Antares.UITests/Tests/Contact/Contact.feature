Feature: Contact UI tests

@Contact
Scenario: Create contact
    Given User navigates to preferences page
		And User selects Mr John Smith format on preferences page
	When User saves preferences on preferences page
		And User navigates to create contact page
		And User fills in contact details on create contact page
			| FirstName | LastName  | Title |
			| Alan      | Macarthur | Mr    |
		And User selects secondary negotiatiors on create contact page
			| FirstName | LastName |
			| John      | Doe      |
			| Thomas    | Miller   |
		And User clicks save contact button on create contact page
	Then View contact page should be displayed
		And Contact details on view contact page are same as the following
			| FirstName | LastName  | Title |
			| Alan      | Macarthur | Mr    |
		And Mailings salutations details on view contact page are same as the following
			| DefaultMailingSalutation | MailingFormalSalutation | MailingSemiformalSalutation | MailingInformalSalutation | MailingPersonalSalutation | MailingEnvelopeSalutation |
			| Semiformal               | Sir                     | Mr Macarthur                | Alan                      |                           | Mr Alan Macarthur         |
		And Primary negotiator is John Smith on view contact page
		And Secondary negotiators on view contact page are the same as following
			| Name          |
			| John Doe      |
			| Thomas Miller |

@Contact 
Scenario: Edit contact
	Given Contact is created in database
		    | FirstName | LastName | Title | MailingFormalSalutation | MailingSemiformalSalutation | MailingInformalSalutation | MailingPersonalSalutation | MailingEnvelopeSalutation |
		    | Frank     | Miguel   | Mr    | Sir                     | Mr Miguel                   | Frank                     |                           | Mr Frank Miguel           |
	When User navigates to view contact page with id
		And User clicks edit button on view contact page
	Then Edit contact page should be displayed
	When User fills in contact details on edit contact page
			| FirstName | LastName  | Title |
			| Joseph    | McGregor  | Sir   |
		And User selects primary negotiator to Adam Williams on edit contact page
		And User removes 2 secondary negotiator on edit contact page
		And User removes 1 secondary negotiator on edit contact page
		And User selects secondary negotiatiors on edit contact page
			| FirstName | LastName |
			| John      | Doe      |
			| Thomas    | Miller   |
		And User clicks save button on edit contact page
	Then View contact page should be displayed
		And Contact details on view contact page are same as the following
			| FirstName | LastName  | Title |
			| Joseph    | McGregor  | Sir   |
		And Mailings salutations details on view contact page are same as the following
			| DefaultMailingSalutation | MailingFormalSalutation | MailingSemiformalSalutation | MailingInformalSalutation | MailingPersonalSalutation | MailingEnvelopeSalutation |
			| Formal                   | Sir McGregor            | Sir McGregor                | Joseph                    |                           | Sir Joseph McGregor       |
		And Primary negotiator is Adam Williams on view contact page
		And Secondary negotiators on view contact page are the same as following
			| Name          |
			| John Doe      |
			| Thomas Miller |
	When User clicks edit button on view contact page
		Then Edit contact page should be displayed
	When User sets 2 secondary negotiator as lead negotiator on edit contact page
	And User clicks save button on edit contact page
	Then View contact page should be displayed
	    And Primary negotiator is Thomas Miller on view contact page
		And Secondary negotiators on view contact page are the same as following
			| Name          |
			| Adam Williams |
			| John Doe		| 

@Contact
Scenario Outline: Contact salutations 
    Given User navigates to preferences page
		And User selects Mr John Smith format on preferences page
	When User saves preferences on preferences page
		And User navigates to create contact page
		And User fills in contact details on create contact page
			| FirstName   | LastName   | Title   |
			| <FirstName> | <LastName> | <Title> |
	Then Check Mailings Salutations
		| MailingFormalSalutation   | MailingSemiformalSalutation   | MailingInformalSalutation   | MailingPersonalSalutation   | MailingEnvelopeSalutation        |
		| <MailingFormalSalutation> | <MailingSemiformalSalutation> | <MailingInformalSalutation> | <MailingPersonalSalutation> | <MailingEnvelopeSalutationPref1> |
	When User navigates to preferences page
		And User selects John Smith, Esq format on preferences page
		And User saves preferences on preferences page
		And User navigates to create contact page
		And User fills in contact details on create contact page
			| FirstName   | LastName   | Title   |
			| <FirstName> | <LastName> | <Title> |
	Then Check Mailings Salutations
		| MailingFormalSalutation   | MailingSemiformalSalutation   | MailingInformalSalutation   | MailingPersonalSalutation   | MailingEnvelopeSalutation        |
		| <MailingFormalSalutation> | <MailingSemiformalSalutation> | <MailingInformalSalutation> | <MailingPersonalSalutation> | <MailingEnvelopeSalutationPref2> |

	Examples:
		| FirstName | LastName | Title | MailingFormalSalutation | MailingSemiformalSalutation | MailingInformalSalutation | MailingPersonalSalutation | MailingEnvelopeSalutationPref1 | MailingEnvelopeSalutationPref2 |
		| Alan      | McArthur | Mr    | Sir                     | Mr McArthur                 | Alan                      |                           | Mr Alan McArthur               | Alan McArthur, Esq             |
		#| Alan      | McArthur | Sir   | Sir McArthur            | Sir McArthur                | Alan                      |                           | Sir Alan McArthur              | Sir Alan McArthur              |
		#| Anna      | Doe      | Mrs   | Madam                   | Mrs Doe                     | Anna                      |                           | Mrs Anna Doe                   | Mrs Anna Doe                   |
		#| Anna      | Doe      | Ms    | Madam                   | Ms Doe                      | Anna                      |                           | Ms Anna Doe                    | Ms Anna Doe                    |
		#| Anna      | Doe      | Miss  | Madam                   | Miss Doe                    | Anna                      |                           | Miss Anna Doe                  | Miss Anna Doe                  |
		#| A         | Doe      | Miss  | Madam                   | Miss Doe                    | Miss Doe                  |                           | Miss A Doe                     | Miss A Doe                     |