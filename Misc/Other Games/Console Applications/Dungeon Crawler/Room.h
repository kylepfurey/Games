#pragma once

// Room type enum
enum room_type { nothing, chest, wall, fairy, mimic, knight };

// Room class
class room
{
public:

	// Type of room
	room_type type = nothing;

	// Adjacent room directions
	room* up_room = nullptr;
	room* down_room = nullptr;
	room* left_room = nullptr;
	room* right_room = nullptr;

	// Constructor
	room() { }

	explicit room(room_type type_of_room)
	{
		type = type_of_room;

		up_room = nullptr;
		down_room = nullptr;
		left_room = nullptr;
		right_room = nullptr;
	}

	// Destructor
	~room() { }
};
