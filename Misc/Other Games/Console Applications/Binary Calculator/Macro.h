
// Template Macro Library Script
// by Kyle Furey

#pragma once
#include <iostream>
#include <string>
#include <vector>

// Include this heading to use the library
#include "Macro.h"

#define MACRO cout << "Macro inserted!" << endl;\

#define CLEAR system("CLS");\

#define TITLE CLEAR; cout << "\nBinary Calculator!" << endl; cout << "by Kyle Furey\n\n";\

#define END cout << "\n\nProgram closed! Goodbye!" << endl;\

