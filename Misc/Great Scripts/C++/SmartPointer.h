
// Smart Pointer Class
// by Kyle Furey

#pragma once

// Include this heading to use the class
#include "SmartPointer.h"

// Smart Pointer Class
template <class DataType> class smart_ptr
{
public:

	// The stored pointer
	DataType* ptr = nullptr;

	// Constructor
	explicit smart_ptr(DataType* newPtr = nullptr)
	{
		ptr = newPtr;
	}

	// Deconstructor
	~smart_ptr()
	{
		// Deletes the pointer
		delete ptr;
	}

	// Setting pointer function
	void equals(DataType& data)
	{
		ptr = &data;
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

	// Setting pointer operator
	void operator=(DataType& data)
	{
		ptr = &data;
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
public:

	// The stored pointer
	DataType* pointer = nullptr;

	// Constructor
	explicit SmartPointer(DataType* newPointer = nullptr)
	{
		pointer = newPointer;
	}

	// Deconstructor
	~SmartPointer()
	{
		// Deletes the pointer
		delete pointer;
	}

	// Setting pointer function
	void Equals(DataType& data)
	{
		pointer = &data;
	}

	// Dereferencing pointer function
	DataType& Dereference()
	{
		return *pointer;
	}

	// Arrow pointer function
	DataType* Arrow()
	{
		return pointer;
	}

	// Setting pointer operator
	void operator=(DataType& data)
	{
		pointer = &data;
	}

	// Dereferencing pointer operator
	DataType& operator*()
	{
		return *pointer;
	}

	// Arrow pointer operator
	DataType* operator->()
	{
		return pointer;
	}
};
