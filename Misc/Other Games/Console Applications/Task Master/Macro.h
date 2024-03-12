
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

#define TITLE CLEAR; cout << "\nTask Master!" << endl; cout << "by Kyle Furey\n\n\n";\

#define SELECTION selection = 0; cout << "\nINPUT: "; cin >> selection;\

// SOURCE: https://stackoverflow.com/questions/5838711/stdcin-input-with-spaces
#define INPUT input = ""; cout << "\nINPUT: "; cin >> input;\

#define END cout << "\n\nProgram closed! Goodbye!" << endl;\
