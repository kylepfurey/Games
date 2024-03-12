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

	vector<string> names;

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
		//cout << "2. Remove a name" << endl;
		//cout << "3. Remove a number" << endl;
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

		bool success = false;

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

			names.push_back(name);

			break;

		case 2:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a name or number to remove:" << endl;

			// Input
			INPUT;

			// Search for that number
			for (int i = 0; i < names.size(); i++)
			{
				if (phoneNumbers[names[i]] == input)
				{
					success = true;

					// Remove that name
					phoneNumbers.erase(names[i]);

					names.erase(names.begin() + i);

					cout << "\n\nRemoved the name and number of phone number " << input << "." << endl;

					break;
				}
			}

			if (!success)
			{
				// Search for that name
				if (phoneNumbers.count(input))
				{
					// Remove that name
					phoneNumbers.erase(input);

					for (int i = 0; i < names.size(); i++)
					{
						if (names[i] == input)
						{
							names.erase(names.begin() + i);

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

			/*
		case 3:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a name to remove:" << endl;

			// Input
			INPUT;

			// Search for that name
			if (phoneNumbers.count(input))
			{
				// Remove that name
				phoneNumbers.erase(input);

				for (int i = 0; i < names.size(); i++)
				{
					if (names[i] == input)
					{
						names.erase(names.begin() + i);

						break;
					}
				}

				cout << "\n\nRemoved the name and number of \"" << input << "\"." << endl;
			}
			else
			{
				cout << "\n\nCould not find a matching name!" << endl;
			}

			// Input
			INPUT;

			break;

		case 4:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a number to remove:" << endl;

			// Input
			INPUT;

			// Search for that number
			for (int i = 0; i < names.size(); i++)
			{
				if (phoneNumbers[names[i]] == input)
				{
					success = true;

					// Remove that name
					phoneNumbers.erase(names[i]);

					names.erase(names.begin() + i);

					cout << "\n\nRemoved the name and number of phone number " << input << "." << endl;

					break;
				}
			}

			if (!success)
			{
				cout << "\n\nCould not find a matching number!" << endl;
			}

			// Input
			INPUT;

			break;
			*/

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
			for (int currentName = 0; currentName < phoneNumbers.size(); currentName++)
			{
				cout << (currentName + 1) << ". \"" << names[currentName] << "\"  -  " << phoneNumbers[names[currentName]] << endl;
			}

			// Input
			INPUT;

			break;

		case 4:

			// Clear the names and numbers
			phoneNumbers.clear();

			names.clear();

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
