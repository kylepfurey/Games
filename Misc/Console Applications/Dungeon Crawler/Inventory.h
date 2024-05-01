#pragma once
#include "Player.h"
#include "ItemBase.h"
#include "MonsterBase.h"

// Starting statistics
#define DEFAULT_ITEM sword
#define DEFAULT_MAX_SLOTS 5
#define DEFAULT_MONEY 0
#define DEFAULT_SCORE 0
#define DEFAULT_HIGH_SCORE 0

// Inventory Class
class inventory
{
public:
	// Public variables and functions

	// Items
	std::vector<item*> items;
	int max_slots;
	
	// Default item
	DEFAULT_ITEM starting_item;

	// Money
	int money;

	// Score
	int score;
	int high_score;

	// Clearing inventory
	void clear_items()
	{
		items.clear();

		max_slots = DEFAULT_MAX_SLOTS;
	}

	// Restarting inventory
	void reset()
	{
		player_health = DEFAULT_HEALTH;

		clear_items();

		add_item(&starting_item);

		max_slots = DEFAULT_MAX_SLOTS;

		money = DEFAULT_MONEY;

		// Update high score
		if (score > high_score)
		{
			high_score = score;
		}

		score = DEFAULT_SCORE;
	}

	// Constructor
	inventory()
	{
		reset();

		high_score = DEFAULT_HIGH_SCORE;
	}

	// Destructor
	~inventory() { }

	// Get an iterator to an item
	std::vector<item*>::iterator get_iterator(item* found_item)
	{
		for (std::vector<item*>::iterator i = items.begin(); i < items.end(); i++)
		{
			if (*i == found_item)
			{
				return i;
			}
		}
	}

	// Adding a new item
	bool add_item(item* new_item)
	{
		if (items.size() < max_slots)
		{
			items.push_back(new_item);

			return true;
		}

		return false;
	}

	// Removing an item
	std::vector<item*>& remove_item(item* removed_item)
	{
		items.erase(get_iterator(removed_item));

		return items;
	}

	// Consuming a item
	std::vector<item*>& consume_item(item* consumed_item, monster* target)
	{
		consumed_item->use(target);

		return remove_item(consumed_item);
	}

	// Using a item
	std::vector<item*>& use_item(item* used_item, monster* target)
	{
		if (used_item->is_consumable)
		{
			consume_item(used_item, target);
		}
		else
		{
			used_item->use(target);
		}

		return items;
	}

	// Buying a new item
	bool buy_item(item* new_item, int& seller_item)
	{
		if (items.size() < max_slots)
		{
			if (new_item->resold)
			{
				if (money >= new_item->resold_cost)
				{
					money -= new_item->resold_cost;

					seller_item += new_item->resold_cost;

					add_item(new_item);

					return true;
				}
			}
			else
			{
				if (money >= new_item->cost)
				{
					money -= new_item->cost;

					seller_item += new_item->cost;

					new_item->resold = true;

					add_item(new_item);

					return true;
				}
			}
		}

		return false;
	}

	// Selling an item
	bool sell_item(item* sold_item, int& seller_money)
	{
		if (sold_item->resold)
		{
			if (seller_money >= sold_item->resold_cost)
			{
				seller_money -= sold_item->resold_cost;

				money += sold_item->resold_cost;

				remove_item(sold_item);

				return true;
			}
		}
		else
		{
			if (seller_money >= sold_item->cost)
			{
				seller_money -= sold_item->cost;

				money += sold_item->cost;

				remove_item(sold_item);

				return true;
			}
		}

		return false;
	}

	// Prints the list of items
	void print_items()
	{
		for (int i = 0; i < items.size(); i++)
		{
			std::cout << i + 1 << ". " << items[i]->name << "\n";
		}
	}
};
