#!/bin/bash
# if not installed run:
# sudo apt install mono-complete
mcs -out:chess ../Program.cs && ./chess --Unix
