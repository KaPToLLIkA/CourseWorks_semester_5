#pragma once
#if defined(_WIN32)
#include <Windows.h>
#include <psapi.h>

#elif defined(__unix__) || defined(__unix) || defined(unix) || (defined(__APPLE__) && defined(__MACH__))
#include <unistd.h>
#include <sys/resource.h>
#include <sys/times.h>
#include <time.h>

#else
#error "Unable to define getCPUTime( ) for an unknown OS."
#endif



double getCPUTime();

#if defined(_WIN32)
double getAllocatedMemorySize(double k);
#endif

#ifdef _DEBUG
#include <iostream>

#include "sort_algorythms.h"
#include "entry.h"

void test_quick_sort();


#endif // _DEBUG
