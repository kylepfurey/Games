#pragma once
#include "MonsterBase.h"

// Item Base Class
class item
{
public:
	// Public variables and functions

	// The name of this item
	std::string name = "New Item";

	// Whether this item is consumable
	bool is_consumable = false;

	// Generic item stat
	float stat = 0;

	// Cost
	int cost = 10;

	// Resell value
	bool resold = false;
	int resold_cost = cost / 2;

	// Constructor
	item() { }

	// Destructor
	~item() { }

	// Use item
	virtual item& use(monster* target)
	{
		return *this;
	}

	// Logical equals operator (needed for std::remove)
	bool operator==(const item& comparison)
	{
		return *this == comparison;
	}
};
