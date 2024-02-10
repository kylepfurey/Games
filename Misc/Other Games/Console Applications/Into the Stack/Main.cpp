#include <iostream>
#include <string>

using namespace std;

void title()
{
	// Clears the console
	system("CLS");

	// Title text
	cout << "\nINTO THE  S T A C K\n";
	cout << "by Kyle Furey\n\n\n";
}

int main(int argc, char* argv[])
{
	// Randomize the random seed
	srand(time(0));
	// SOURCE: https://en.cppreference.com/w/cpp/numeric/random/srand

	// Used for input when no input is needed
	string input = "";
	string word = "";

	// Stores array for word
	char wordArray[6] = { '\0', '\0', '\0', '\0', '\0', '\0' };

	while (true)
	{
		// Refresh screen
		title();

		// Prompt 1
		cout << "Give me a five letter word . . .\n\n";

		while (word.length() == 0)
		{
			// Receives and stores the user's input
			cout << "\nYOU: ";
			cin >> word;

			// Length check
			if (word.length() > 5)
			{
				cout << "\n\nI said five please.\n\n";
				word = "";
			}
		}

		int length = word.length();

		// Convert string to char array
		for (int i = 0; i < word.length(); i++)
		{
			wordArray[i] = word[i];
		}

		// Initalize pointer
		char* wordPtr = &wordArray[0];

		// Exit check
		if (word == "exit")
		{
			break;
		}

		// Refresh screen
		title();

		// Length check
		if (word.length() != 5)
		{
			cout << ". . . t h a t ' s  n o t  f i v e  b u t  o k a y .\n\n";
		}

		// Informational text
		cout << "Your magical word is " << wordArray << "!\n\n";

		cout << "This is actually an array of characters!\n\n";

		cout << "I reserved some space for your word so that it may be stored in the STACK!\n\n";

		cout << "We are going to make and use a pointer to store your word.\n\n";

		// Informational text
		cout << ". . . done!\n\n";

		cout << "Yaay! I'm so excited!\n\n";

		cout << "We have made a pointer at " << &wordPtr << " for your character array \"" << wordArray << "\" of length " << length << ".\n\n";

		cout << "Hooray!\n\n";

		// Receives the user's input
		cout << "\nYOU: ";
		cin >> input;

		// Exit check
		if (input == "exit")
		{
			break;
		}

		// Refresh screen
		title();

		// Informational text
		cout << "Yup!\n\n";

		cout << "Just you and " << wordArray << ".\n\n";

		// Return capitalized string using toupper function
		// SOURCE: https://en.cppreference.com/w/cpp/string/byte/toupper#:~:text=int%20toupper(%20int%20ch%20)%3B,the%20currently%20installed%20C%20locale.
		for (int i = 0; i < length - 1; i++)
		{
			cout << (char)toupper(wordPtr[i]) << ", ";
		}

		// Informational text
		cout << (char)toupper(wordPtr[length - 1]) << ".\n\n";

		cout << (char)toupper(wordPtr[0]);
		cout << (char)toupper(wordPtr[0]);
		cout << (char)toupper(wordPtr[0]);

		for (int i = 1; i < length; i++)
		{
			cout << (char)tolower(wordPtr[i]);
			cout << (char)tolower(wordPtr[i]);
			cout << (char)tolower(wordPtr[i]);
		}

		cout << ".\n\nHow delightful!\n\n";

		cout << "Nothing could possibly go wrong with " << wordArray << ".\n\n";

		cout << "Just a simple array of " << length << " chara- \n\n";

		cout << ". . . w h a t  a r e  y o u  d o i n g ?\n\n";

		// Receives the user's input
		cout << "\nINCREMENT WORD? ";
		cin >> input;

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}

		// Refresh screen
		title();

		// Informational text
		cout << "COMPUTER: *";

		for (int i = 0; i < length; i++)
		{
			cout << (char)tolower(wordPtr[i]);
		}

		// Track where we are in the stack relative to the array
		int count = 0;

		// Find the next null terminator byte
		count = 0;
		while (*wordPtr != '\0')
		{
			wordPtr++;
			count++;
		}

		// Remove the null terminator byte
		*wordPtr = (char)rand() % 4294967296;	// <- is the total number of possible characters in c++
		// SOURCE: https://cplusplus.com/reference/cstdlib/rand/;

		// Move the pointer back to the start of the array
		wordPtr -= count;

		// Informational text
		cout << "Ptr now set to " << wordArray << ". Null terminator byte overwritten.\n\n";

		cout << "WHAT THE FART?!\n\n";

		cout << "YOU CAN'T JUST EXTEND YOUR ARRAY LIKE THAT!\n\n";

		cout << "WHAT DOES " << wordArray << " EVEN MEAN?!\n\n";

		cout << "YOU MUST STOP!\n\n";

		// Receives the user's input
		cout << "\nINCREMENT WORD? ";
		cin >> input;

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}

		// Refresh screen
		title();

		// Find the next null terminator byte
		count = 0;
		while (*wordPtr != '\0')
		{
			wordPtr++;
			count++;
		}

		// Remove the null terminator byte
		*wordPtr = (char)rand() % 4294967296;	// <- is the total number of possible characters in c++
		// SOURCE: https://cplusplus.com/reference/cstdlib/rand/;

		// Move the pointer back to the start of the array
		wordPtr -= count;

		// Informational text
		cout << "COMPUTER: Your word is now " << wordArray << ".\n\n";

		cout << "OH RAM!\n\n";

		cout << "IF YOU KEEP EXTENDING THE ARRAY, YOUR WORD WILL BE RUINED!\n\n";

		cout << "OH YOUR BEAUTIFUL WORD! OH " << wordArray << "!\n\n";

		cout << "HOW TARNISHED IT HAS BECOME.\n\n";

		// Receives the user's input
		cout << "\nINCREMENT WORD? ";
		cin >> input;

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}

		// Refresh screen
		title();

		// Find the next null terminator byte
		count = 0;
		while (*wordPtr != '\0')
		{
			wordPtr++;
			count++;
		}

		// Remove the null terminator byte
		*wordPtr = (char)rand() % 4294967296;	// <- is the total number of possible characters in c++
		// SOURCE: https://cplusplus.com/reference/cstdlib/rand/;

		// Move the pointer back to the start of the array
		wordPtr -= count;

		// Informational text
		cout << "COMPUTER: Your word is now " << wordArray << ".\n\n";

		cout << "no seriously this probably isn't good for your computer bro\n\n";

		cout << "the program will crash if you keep doing this and it will be your fault lmao\n\n";

		cout << "probably memory leak city over here or something\n\n";

		// Receives the user's input
		cout << "\nINCREMENT WORD? ";
		cin >> input;

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}

		// Refresh screen
		title();

		// Find the next null terminator byte
		count = 0;
		while (*wordPtr != '\0')
		{
			wordPtr++;
			count++;
		}

		// Remove the null terminator byte
		*wordPtr = (char)rand() % 4294967296;	// <- is the total number of possible characters in c++
		// SOURCE: https://cplusplus.com/reference/cstdlib/rand/;

		// Move the pointer back to the start of the array
		wordPtr -= count;

		// Ending text
		cout << "COMPUTER: Your word is now " << wordArray << ".\n\n";

		cout << "your funeral bud\n\n";

		cout << "you can always type no or exit to leave when you're done\n\n";

		cout << "peace out\n\n";

		// Receives the user's input
		cout << "\nINCREMENT WORD? ";
		cin >> input;

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}

		// Print garbage
		while (input != "exit")
		{
			// Refresh screen
			title();

			// Find the next null terminator byte
			count = 0;
			while (*wordPtr != '\0')
			{
				wordPtr++;
				count++;
			}

			// Remove the null terminator byte
			*wordPtr = (char)rand() % 4294967296;	// <- is the total number of possible characters in c++
			// SOURCE: https://cplusplus.com/reference/cstdlib/rand/;

			// Move the pointer back to the start of the array
			wordPtr -= count;

			// Print out what we think our array is now
			cout << "COMPUTER: Your word is now " << wordArray << ".\n\n";

			// Receives the user's input
			cout << "\nINCREMENT WORD? ";
			cin >> input;

			// Exit check
			if (input == "exit" || input == "no")
			{
				break;
			}
		}

		// Exit check
		if (input == "exit" || input == "no" || input == "n")
		{
			break;
		}
	}

	cout << "\n\nProgram closed. Thank you!\n\n";
}
