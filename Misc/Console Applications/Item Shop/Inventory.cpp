#include "Inventory.h"
#include "Item.h"
#include <iostream>
#include <string>

// Inventory Class Functions
Inventory::Inventory()
{
	// Constructor
	for (int i = 0; i < 5; i++)
	{
		inventory[i].type = None;
		inventory[i].cost = 0;
	}
}

Inventory::~Inventory()
{
	// Deconstructor

}

bool Inventory::AddItem(Item &item)
{
	// Adds an item to an available spot
	for (int i = 0; i < 5; i++)
	{
		if (inventory[i].type == None)
		{
			inventory[i] = item;
			return true;
		}
	}

	return false;
}

bool Inventory::RemoveItem(Item &item)
{
	// Removes a corresponding item to an available spot
	for (int i = 0; i < 5; i++)
	{
		if (inventory[i].type == item.type)
		{
			inventory[i].type = None;
			inventory[i].cost = 0;
			return true;
		}
	}

	return false;
}

void Inventory::DisplayInventory()
{
	std::cout << "1. " << inventory[0].Item::Name() << " = " << inventory[0].Item::Cost() << " Gold" << std::endl;
	std::cout << "2. " << inventory[1].Item::Name() << " = " << inventory[1].Item::Cost() << " Gold" << std::endl;
	std::cout << "3. " << inventory[2].Item::Name() << " = " << inventory[2].Item::Cost() << " Gold" << std::endl;
	std::cout << "4. " << inventory[3].Item::Name() << " = " << inventory[3].Item::Cost() << " Gold" << std::endl;
	std::cout << "5. " << inventory[4].Item::Name() << " = " << inventory[4].Item::Cost() << " Gold" << std::endl;
}
