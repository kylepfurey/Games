
// List of Monsters

#pragma once
#include "MonsterBase.h"

// Monster Template Class
class monster_example : public monster
{
public:
	// Public variables and functions

	// Constructor
	monster_example()
	{
		name = "Monster";
		health = 10;
		attack = 5;
		defense = 0;
		drops_item = false;
		is_boss = false;
	}

	// Destructor
	~monster_example() { }
};

// Fairy Monster Class
class foo_fairy : public monster
{
public:
	// Public variables and functions

	// Constructor
	foo_fairy()
	{
		name = "FOO FAIRY";
		health = 30;
		attack = 5;
		defense = 0;
		drops_item = false;
		is_boss = false;
	}

	// Destructor
	~foo_fairy() { }
};

// Mimic Monster Class
class malloc_mimic : public monster
{
public:
	// Public variables and functions

	// Constructor
	malloc_mimic()
	{
		name = "MALLOC MIMIC";
		health = 50;
		attack = 15;
		defense = 5;
		drops_item = true;
		is_boss = false;
	}

	// Destructor
	~malloc_mimic() { }
};

// Knight Monster Class
class null_knight : public monster
{
public:
	// Public variables and functions

	// Constructor
	null_knight()
	{
		name = "NULL REFERENCE KNIGHT";
		health = 75;
		attack = 20;
		defense = 15;
		drops_item = false;
		is_boss = true;
	}

	// Deructor
	~null_knight() { }
};
