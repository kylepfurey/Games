#include <iostream>
#include <string>
#include "Binary.h"
#include "Macro.h"

using namespace std;

string Operator(int input)
{
	switch (input)
	{

	default: cout << "Null operator!"; break;

	case 1: return "+";

	case 2: return "-";

	case 3: return "*";

	case 4: return "/";

	case 5: return "%";

	case 6: return "^";

	}
}

int main(int argc, char* argv[])
{
	while (true)
	{
		// TITLE

		// Title text
		TITLE;

		// Prompt
		cout << "1. Start" << endl;
		cout << "2. Exit\n\n";
		cout << "INPUT: ";

		// Get player's input
		int input = 0;

		while (true)
		{
			cin >> input;

			if (input == 1 || input == 2)
			{
				break;
			}

			cout << "\nThat is not a valid selection.\n\n\nINPUT: ";
		}

		// Close program
		if (input == 2)
		{
			break;
		}


		// OPERATOR

		// Title text
		TITLE;

		// Prompt
		cout << "Please select an operator!" << endl;
		cout << "1. Addition" << endl;
		cout << "2. Subtraction" << endl;
		cout << "3. Multiplication" << endl;
		cout << "4. Divison" << endl;
		cout << "5. Modulo" << endl;
		cout << "6. Power" << endl;
		cout << "7. Exit\n\n";
		cout << "INPUT: ";

		// Get player's input
		input = 0;

		while (true)
		{
			cin >> input;

			if (input > 0 && input < 8)
			{
				break;
			}

			cout << "\nThat is not a valid selection.\n\n\nINPUT: ";
		}

		// Close game
		if (input == 7)
		{
			break;
		}


		// NUMBER 1

		// Title text
		TITLE;

		// Prompt
		cout << "Your equation:\n\n";
		cout << "_ " << Operator(input) << " _\n\n";
		cout << "Enter your left integer!\n\n";
		cout << "INPUT: ";

		// Get player's input
		int left = 0;

		cin >> left;


		// NUMBER 2

		// Title text
		TITLE;

		// Prompt
		cout << "Your equation:\n\n";
		cout << left << " " << Operator(input) << " _\n\n";
		cout << "Enter your right integer!\n\n";
		cout << "INPUT: ";

		// Get player's input
		int right = 0;

		cin >> right;


		// OUTPUT

		// Title text
		TITLE;

		// Prompt
		cout << "Your equation:\n\n";

		switch (input)
		{

		default: cout << "Null operator!"; break;

		case 1: cout << left << " + " << right << " = " << Binary().Add(left, right); break;

		case 2: cout << left << " - " << right << " = " << Binary().Subtract(left, right); break;

		case 3: cout << left << " * " << right << " = " << Binary().Multiply(left, right); break;

		case 4: cout << left << " / " << right << " = " << Binary().Divide(left, right); break;

		case 5: cout << left << " % " << right << " = " << Binary().Modulo(left, right); break;

		case 6: cout << left << " ^ " << right << " = " << Binary().Power(left, right); break;

		}

		cout << "\n\nINPUT: ";

		string end;

		cin >> end;
	}

	// End text
	END;

	return 0;
}
