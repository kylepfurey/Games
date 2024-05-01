#include <iostream>
#include <string>
#include <vector>
#include <map>
#include "Macro.h"

using namespace std;

int main(int argc, char* argv[])
{
	int selection = 0;

	string input = "";

	map<string, string> phoneNumbers;

	int tasksCompleted = 0;

	// Program loop
	while (true)
	{
		// Title text
		TITLE;

		// Prompt
		cout << "Please select an action:" << endl;
		cout << endl;
		cout << "1. Add a new name and number" << endl;
		cout << "2. Remove a name or number" << endl;
		cout << "3. View your phone numbers" << endl;
		cout << "4. Clear all names and numbers" << endl;
		cout << "5. Close the program" << endl;

		// Selection
		while (true)
		{
			SELECTION;

			if (selection > 0 && selection < 6)
			{
				break;
			}
		}

		// Exit check
		if (selection == 5)
		{
			break;
		}

		string name = "";

		int currentName = 0;

		// Options
		switch (selection)
		{
		case 1:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a new name." << endl;

			// Input
			INPUT;

			name = input;

			// Prompt
			cout << "\n\nPlease enter this person's phone number." << endl;

			// Input
			INPUT;

			// Insert this name and number
			phoneNumbers.insert(std::pair<string, string>(name, input));

			break;

		case 2:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a name or number to remove:" << endl;

			// Input
			INPUT;

			// Search for that number
			for (auto i = phoneNumbers.begin(); i != phoneNumbers.end(); i++)
			{
				if (i->first == input)
				{
					currentName = true;

					// Remove that name
					phoneNumbers.erase(i);

					cout << "\n\nRemoved the name and number of phone number " << input << "." << endl;

					break;
				}
			}

			if (!currentName)
			{
				// Search for that name
				if (phoneNumbers.count(input))
				{
					// Remove that name
					phoneNumbers.erase(input);

					for (auto i = phoneNumbers.begin(); i != phoneNumbers.end(); i++)
					{
						if (i->first == input)
						{
							// Remove that name
							phoneNumbers.erase(i);

							break;
						}
					}

					cout << "\n\nRemoved the name and number of \"" << input << "\"." << endl;
				}
				else
				{
					cout << "\n\nCould not find a matching name or number!" << endl;
				}
			}

			// Input
			INPUT;

			break;

		case 3:

			// Title text
			TITLE;

			// Relay the total number of phone numbers
			if (phoneNumbers.size() == 0)
			{
				cout << "You have no phone numbers." << endl;
			}
			else if (phoneNumbers.size() == 1)
			{
				cout << "You have 1 phone number:" << endl;
			}
			else
			{
				cout << "You have " << phoneNumbers.size() << " phone numbers:" << endl;
			}

			cout << endl;

			// Relay all names and numbers
			for (auto i = phoneNumbers.begin(); i != phoneNumbers.end(); i++)
			{
				cout << (currentName + 1) << ". \"" << i->first << "\"  -  " << i->second << endl;

				currentName++;
			}

			// Input
			INPUT;

			break;

		case 4:

			// Clear the names and numbers
			phoneNumbers.clear();

			// Title text
			TITLE;

			// Relay that the clear was successful
			cout << "Your names and numbers were cleared." << endl;

			// Input
			INPUT;

			break;
		}
	}

	// Ending text
	END;

	return 0;
}
