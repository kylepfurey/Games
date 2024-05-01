
// List Of Items

#pragma once
#include "ItemBase.h"
#include "MonsterBase.h"

// Item Template Class
class item_example : public item
{
public:
	// Public variables and functions

	// Constructor
	item_example()
	{
		name = "New Item";
		is_consumable = false;
		stat = 0;
		cost = 10;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~item_example() { }

	// Use item
	item& use(monster* target) override
	{
		return *this;
	}
};

// Apple Item Class
class apple : public item
{
public:
	// Public variables and functions

	// Constructor
	apple()
	{
		name = "APPLE";
		is_consumable = true;
		stat = 25;
		cost = 5;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~apple() { }

	// Use item
	item& use(monster* target) override
	{
		heal(stat);

		return *this;
	}
};

// Potion Item Class
class potion : public item
{
public:
	// Public variables and functions

	// Constructor
	potion()
	{
		name = "POTION";
		is_consumable = true;
		stat = 50;
		cost = 10;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~potion() { }

	// Use item
	item& use(monster* target) override
	{
		heal(stat);

		return *this;
	}
};

// Totem Item Class
class totem : public item
{
public:
	// Public variables and functions

	// Constructor
	totem()
	{
		name = "TOTEM";
		is_consumable = true;
		stat = 100;
		cost = 20;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~totem() { }

	// Use item
	item& use(monster* target) override
	{
		heal(stat);

		return *this;
	}
};

// Sword Item Class
class sword : public item
{
public:
	// Public variables and functions

	// Constructor
	sword()
	{
		name = "SWORD";
		is_consumable = false;
		stat = 15;
		cost = 10;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~sword() { }

	// Use item
	item& use(monster* target) override
	{
		target->damage(stat);

		return *this;
	}
};

// Axe Item Class
class axe : public item
{
public:
	// Public variables and functions

	// Constructor
	axe()
	{
		name = "AXE";
		is_consumable = false;
		stat = 30;
		cost = 20;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~axe() { }

	// Use item
	item& use(monster* target) override
	{
		target->damage(stat);

		return *this;
	}
};

// Trident Item Class
class trident : public item
{
public:
	// Public variables and functions

	// Constructor
	trident()
	{
		name = "TRIDENT";
		is_consumable = false;
		stat = 60;
		cost = 40;
		resold = false;

		resold_cost = cost / 2;
	}

	// Destructor
	~trident() { }

	// Use item
	item& use(monster* target) override
	{
		target->damage(stat);

		return *this;
	}
};
