#pragma once
#if defined(_WIN32)
#include <Windows.h>

#elif defined(__unix__) || defined(__unix) || defined(unix) || (defined(__APPLE__) && defined(__MACH__))
#include <unistd.h>
#include <sys/resource.h>
#include <sys/times.h>
#include <time.h>

#else
#error "Unable to define getCPUTime( ) for an unknown OS."
#endif

/*
 * Author:  David Robert Nadeau
 * Site:    http://NadeauSoftware.com/
 * License: Creative Commons Attribution 3.0 Unported License
 *          http://creativecommons.org/licenses/by/3.0/deed.en_US
 */

 /**
  * Returns the amount of CPU time used by the current process,
  * in seconds, or -1.0 if an error occurred.
  */

double getCPUTime();

#include <vector>
#include <iostream>
#include "entry.h"
#include "search_algorythms.h"


std::vector<entry> new_vector_for_interpol_s(size_t size);

std::vector<entry> new_vector_for_binary_s(size_t size);

std::vector<entry> new_vector_for_fibonacci_s(size_t size);


#ifdef _DEBUG

void basic_binary_search_test();

void basic_fibonacci_search_test();

void basic_interpolation_search_test();

#endif // _DEBUG
