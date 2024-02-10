#pragma once
#include <iostream>
#include <string>

// All types of items
static enum itemType { None, Apple, Banana, Sword, Shield, Bow, Armor, Staff };
// SOURCE: https://www.programiz.com/cpp-programming/enumeration

// Inventory Class
class Item
{
public:

	// Public variables and functions
	Item();										// Constructor
	~Item();									// Deconstructor
	std::string Name();							// Returns this item's name based on its type
	int Cost();									// Returns this item's logical cost

	itemType type = None;						// The type of item of this instance
	int cost = 0;								// The cost of this item in the shop
	bool resell = false;						// Whether this item has been sold

private:

	// Private variables and functions

	// Made you look AGAIN :)
};
