#pragma once
#include <iostream>

// Monster Base Class
class monster
{
public:
	// Public variables and functions

	// The name of this monster
	std::string name = "Monster";

	// Monster's health
	float health = 10;

	// Monster's attack
	float attack = 5;

	// Monster's defense
	float defense = 0;

	// Whether this monster drops an item on death
	bool drops_item = false;

	// Whether this is a boss enemy
	bool is_boss = false;

	// Constructor
	monster() { }

	// Destructor
	~monster() { }

	// Damage method
	float& damage(float damage)
	{
		std::cout << name << " TOOK " << damage << " DAMAGE!\n\n";

		health -= damage - defense < 1 ? 1 : damage - defense;

		return health;
	}
};
