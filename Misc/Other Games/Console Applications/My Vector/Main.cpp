#include <iostream>
#include <string>
#include "Vector.h" // NOT <vector>, made by Kyle Furey

using namespace std;

// Macros
#define TITLE system("CLS"); cout << "\nMy Vector\nby Kyle Furey\n\n\n";
#define NUMBER cout << "INPUT: "; cin >> number; cout << endl;
#define EXIT(condition) if (condition) { break; }
#define INPUT cout << "INPUT: "; cin >> input; cout << endl;

int main(int argc, char* argv[])
{
	// Custom vector class
	vector<string> myVector = vector<string>();

	// Selection variables
	int number = 0;
	string input = "";

	// Program loop
	while (true)
	{
		TITLE;

		cout << "Your vector:\n\n";

		if (myVector.size() > 0)
		{
			myVector.print();
		}
		else
		{
			cout << "NONE\n";
		}

		cout << "\nSize:       " << myVector.size();
		cout << "\nCapacity:   " << myVector.capacity();
		cout << "\nExpansions: " << myVector.expansions() << "\n\n\n";

		cout << "What would you like to do with your vector?\n\n";
		cout << "1.  Push Back\n";
		cout << "2.  Push\n";
		cout << "3.  Pop Back\n";
		cout << "4.  Pop\n";
		cout << "5.  Insert\n";
		cout << "6.  Replace\n";
		cout << "7.  Erase\n";
		cout << "8.  Clear\n";
		cout << "9.  Reverse\n";
		cout << "10. Swap\n";
		cout << "11. Shift\n";
		cout << "12. Shrink To Fit\n";
		cout << "13. Exit\n\n";

		NUMBER;

		EXIT(number == 13);

		int temp = 0;

		switch (number)
		{
		case 1:

			cout << "\nAdd what new element?\n\n";

			INPUT;

			myVector.push_back(input);

			break;

		case 2:

			cout << "\nAdd what new element?\n\n";

			INPUT;

			myVector.push(input);

			break;

		case 3:

			myVector.pop_back();

			break;

		case 4:

			myVector.pop();

			break;

		case 5:

			cout << "\nInsert a new element where?\n\n";

			NUMBER;

			cout << "\nWith what new element?\n\n";

			INPUT;

			myVector.insert(number, input);

			break;

		case 6:

			cout << "\nReplace which index?\n\n";

			NUMBER;

			cout << "\nWith what new element?\n\n";

			INPUT;

			myVector.fill(number, input);

			break;

		case 7:

			cout << "\nErase which index?\n\n";

			NUMBER;

			myVector.erase(number);

			break;

		case 8:

			myVector.clear();

			break;

		case 9:

			myVector.reverse();

			break;

		case 10:

			cout << "\nWhich indicies are we swapping?\n\n";

			NUMBER;

			temp = number;

			NUMBER;

			myVector.swap(temp, number);

			break;

		case 11:

			cout << "\nShift your vector by how much?\n\n";

			NUMBER;

			myVector.shift_right(number);

			break;

		case 12:

			myVector.shrink_to_fit();

			break;
		}
	}

	cout << "\nProgram closed! Goodbye!\n";

	return 0;
}
