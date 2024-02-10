#include "Item.h"
#include <iostream>
#include <string>


// Item Class Functions
Item::Item()
{
	// Constructor
	type = None;
	cost = 0;
	resell = false;
}

Item::~Item()
{
	// Deconstructor

}

// Returns the name of an item based on its type
std::string Item::Name()
{
	switch (type)
	{

	default:

		break;

	case None:

		return "No Item Found";

	case Apple:

		return "Apple        ";

	case Banana:

		return "Banana       ";

	case Sword:

		return "Sword        ";

	case Shield:

		return "Shield       ";

	case Bow:

		return "Bow          ";

	case Armor:

		return "Armor        ";

	case Staff:

		return "Staff        ";
	}
}

// Checks the logical cost of an item
int Item::Cost()
{
	if (type == None)
	{
		return 0;
	}

	if (resell && cost > 1)
	{
		return cost - 1;
	}

	return cost;
}