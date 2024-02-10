#pragma once
#include "Inventory.h"
#include "Item.h"
#include <iostream>
#include <string>

// Item Class
class Inventory
{
public:

	// Public variables and functions
	Inventory();								// Constructor
	~Inventory();								// Deconstructor
	bool AddItem(Item &item);					// Adds the given item
	bool RemoveItem(Item &item);					// Removes the given item
	void DisplayInventory();					// Displays the current inventory

	Item inventory[5];							// The current inventory
	int gold = 50;								// The current gold count

private:

	// Private variables and functions

	// Made you look :)
};
