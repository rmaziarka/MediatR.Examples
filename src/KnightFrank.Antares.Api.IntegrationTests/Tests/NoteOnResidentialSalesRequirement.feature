Feature: Note on residential sales requirement

@ignore
Scenario: Save note to residential sales requirement
	Given User fill text box in note form for residential sales requirement
		| NoteId | NoteDescription |
		| 1      | TestNote        |
	When User retreive data from DB
	Then the note should be ssame as
		| NoteId | NoteDescription |
		| 1      | TestNote        |

Scenario: Save note to non existing residential sales requirement
	Given User fill text box in note form for residential sales requirement
		| NoteId | NoteDescription |
		| 2      | TestNote 2      |
		And this requirement is not exist in DB
	When User retreive data from DB
	Then the note should not be saved in DB due to mising requirement
		And error message should be displayed ? - ask dev how they check error message

Scenario: Get list of all notes from DB for specific residential sales requirement
	Given At least 3 notes exist in DB for chosen requirement
		| NoteId | NoteDescription |
		| 1      | TestNote        |
		| 2      | TestNote 2      |
		| 3      | TestNote 3      |
	When User retreive data from DB
	Then Note list should be same as
		| NoteId | NoteDescription |
		| 1      | TestNote        |
		| 2      | TestNote 2      |
		| 3      | TestNote 3      |
