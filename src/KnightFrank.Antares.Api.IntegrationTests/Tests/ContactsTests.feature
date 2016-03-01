Feature: Contacts

Scenario: Retrieve single contact details
    Given User has defined a contact details
    When User retrieves contact details
    Then contact should have following details
        | FirstName | Surname |
        | John      | Doe     |

Scenario: Retrieve all contacts details
    Given User retrieves all contacts details
    Then contacts should have following details
        | FirstName | Surname |
        | John      | Doe     |
        | David     | Dummy   |
