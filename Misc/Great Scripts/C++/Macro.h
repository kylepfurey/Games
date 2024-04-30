
// Template Macro Library Script
// by Kyle Furey

#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <map>
#include <algorithm>
#include <fstream>
#include <thread>
#include <future>
#include <coroutine>
#include <functional>
#include <cstdarg>
#include <initializer_list>
#include <chrono>
#include <ctime>
#include <cassert>

// Include this heading to use the library
#include "Macro.h"

// Example macro (use '\' to include the next line)
#define MACRO std::cout << "Macro inserted!" << std::endl;
