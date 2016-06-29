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
		And User clicks save contact button on create contact page
	Then View contact page should be displayed
		And Contact details on view contact page are same as the following
			| FirstName | LastName  | Title |
			| Alan      | Macarthur | Mr    |
		And Mailings salutations details on view contact page are same as the following
			| DefaultMailingSalutation | MailingFormalSalutation | MailingSemiformalSalutation | MailingInformalSalutation | MailingPersonalSalutation | MailingEnvelopeSalutation |
			| Semiformal               | Sir                     | Mr Macarthur                | Alan                      |                           | Mr Alan Macarthur         |

@Contact
Scenario Outline: Contact Salutations 
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
	