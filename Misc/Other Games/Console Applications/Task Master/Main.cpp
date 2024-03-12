#include <iostream>
#include <string>
#include <vector>
#include "Queue.h"
#include "Macro.h"

using namespace std;

int main(int argc, char* argv[])
{
	int selection = 0;

	string input = "";

	priority_queue<string> tasks;

	int tasksCompleted = 0;

	// Program loop
	while (true)
	{
		// Title text
		TITLE;

		// Prompt
		cout << "Current Task: " << (tasks.is_empty() ? "None" : "\"" + tasks.peek() + "\"") << endl;
		cout << "\nPlease select an action:" << endl;
		cout << endl;
		cout << "1. Add a new task" << endl;
		cout << "2. Remove your next task" << endl;
		cout << "3. View your task list" << endl;
		cout << "4. Clear your task list" << endl;
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

		// Options
		switch (selection)
		{
		case 1:

			// Title text
			TITLE;

			// Prompt
			cout << "Please enter a new task." << endl;

			// Input
			INPUT;

			// Prompt
			cout << "\n\nPlease give this task a priority." << endl;

			// Input
			SELECTION;

			// Enqueue the new task
			tasks.enqueue(input, -selection);

			break;

		case 2:

			// Title text
			TITLE;

			// Prompt
			if (tasks.is_empty())
			{
				cout << "You have no tasks remaining. Great job!" << endl;

				// Input
				INPUT;
			}
			else
			{
				// Increment tasks completed
				tasksCompleted++;

				// Retrieve the priority
				int priority = -tasks.highest_priority();

				// Relay what task was completed
				cout << "You finished \"" << tasks.dequeue() << "\" with a priority of " << priority << "." << endl;

				// Relay what number task this was
				if (tasksCompleted == 1)
				{
					cout << "\You have completed your first task! Nice work!" << endl;;
				}
				else
				{
					cout << "\nYou have completed " << tasksCompleted << " total tasks. Nice work!" << endl;
				}

				// Check if this is the last task
				if (tasks.is_empty())
				{
					cout << "\nYou completed your final task. I knew you could do it.\n\nGo treat yourself!" << endl;
				}
				else
				{
					// Relay the next task
					if (tasks.size() == 1)
					{
						cout << "\nYou have one remaining task left.\n\nYour final task is \"" << tasks.peek() << ".\"\n\nYou got this!" << endl;
					}
					else
					{
						cout << "\mYou have " << tasks.size() << " total tasks remaining.\n\nYour next task is \"" << tasks.peek() << "\".\n\nKeep up the great work!" << endl;
					}
				}

				// Input
				INPUT;
			}

			break;

		case 3:

			// Title text
			TITLE;

			cout << "Current Task: " << (tasks.is_empty() ? "None" : "\"" + tasks.peek() + "\"") << endl;

			// Prompt
			if (tasks.is_empty())
			{
				cout << "\nYou have no tasks remaining. Great job!" << endl;
			}
			else
			{
				// Check if this is the last task
				if (tasks.size() == 1)
				{
					cout << "\nYou have 1 task remaining:" << endl;
				}
				else
				{
					cout << "\nYou have " << tasks.size() << " total tasks remaining:" << endl;
				}

				cout << endl;

				// Relay all tasks
				for (int currentTask = 0; currentTask < tasks.queue.size(); currentTask++)
				{
					cout << (currentTask + 1) << ". \"" << tasks.queue[currentTask] << "\" with a priority of " << -tasks.priority[currentTask] << "." << endl;
				}
			}

			// Input
			INPUT;

			break;

		case 4:

			// Clear the task list
			tasks.clear();

			// Title text
			TITLE;

			// Relay that the clear was successful
			cout << "Your task list was cleared." << endl;

			// Input
			INPUT;

			break;
		}
	}

	// Ending text
	END;

	return 0;
}
