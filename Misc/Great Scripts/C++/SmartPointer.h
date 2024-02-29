
// Smart Pointer Class
// by Kyle Furey

#pragma once

// Include this heading to use the class
#include "SmartPointer.h"

// Smart Pointer Class
template <class DataType> class smart_ptr
{
private:

	// The stored pointer
	DataType* ptr = nullptr;

public:

	// Constructor
	explicit smart_ptr(DataType* newPtr = nullptr)
	{
		// Set the pointer
		ptr = newPtr;
	}

	// Deconstructor
	~smart_ptr()
	{
		// Deletes the pointer
		delete ptr;

		// Set the pointer to null
		ptr = nullptr;
	}

	// Dereferencing pointer function
	DataType& dereference()
	{
		return *ptr;
	}

	// Arrow pointer function
	DataType* arrow()
	{
		return ptr;
	}

	// Dereferencing pointer operator
	DataType& operator*()
	{
		return *ptr;
	}

	// Arrow pointer operator
	DataType* operator->()
	{
		return ptr;
	}
};

// Smart Pointer Class
template <class DataType> class SmartPointer
{
private:

	// The stored pointer
	DataType* ptr = nullptr;

public:

	// Constructor
	explicit SmartPointer(DataType* newPtr = nullptr)
	{
		// Set the pointer
		ptr = newPtr;
	}

	// Deconstructor
	~SmartPointer()
	{
		// Deletes the pointer
		delete ptr;

		// Set the pointer to null
		ptr = nullptr;
	}

	// Dereferencing pointer function
	DataType& Dereference()
	{
		return *ptr;
	}

	// Arrow pointer function
	DataType* Arrow()
	{
		return ptr;
	}

	// Dereferencing pointer operator
	DataType& operator*()
	{
		return *ptr;
	}

	// Arrow pointer operator
	DataType* operator->()
	{
		return ptr;
	}
};
