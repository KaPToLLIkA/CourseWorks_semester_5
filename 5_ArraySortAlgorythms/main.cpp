#include <iostream>
#include <iomanip>

#include "entry.h"
#include "tests.h"
#include "sort_algorythms.h"

int main() {
#ifdef _DEBUG
	//test_quick_sort();
	//test_heap_sort();
	//test_radix_sort();
#endif // _DEBUG
	uint64_t sz = 1;
	uint64_t k[] = { 4 };
	uint64_t z[] = { 100000 };
	uint64_t w = 20;

	//quick sort
	std::cout << "QUICK SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				double start = getCPUTime();
				quick_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}
	std::cout << "QUICK SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//sorted
			std::vector<entry> v1;
			for (uint64_t t = 0; t < x * z[i]; ++t) 
			{
				v1.push_back(entry(t));
			}

			RESET_OP_COUNTER
				double start = getCPUTime();
				quick_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}

	std::cout << "QUICK SORT END\n\n\n";





	//quick sort
	std::cout << "HEAP SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				double start = getCPUTime();
				heap_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}
	std::cout << "HEAP SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//sorted
			std::vector<entry> v1;
			for (uint64_t t = 0; t < x * z[i]; ++t)
			{
				v1.push_back(entry(t));
			}

			RESET_OP_COUNTER
				double start = getCPUTime();
				heap_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}

	std::cout << "HEAP SORT END\n\n\n";





	//quick sort
	std::cout << "RADIX SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				double start = getCPUTime();
				radix_sort(v1, get_key);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}
	std::cout << "RADIX SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << ":"
		<< std::setw(w) << "TIME" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//sorted
			std::vector<entry> v1;
			for (uint64_t t = 0; t < x * z[i]; ++t)
			{
				v1.push_back(entry(t));
			}

			RESET_OP_COUNTER
				double start = getCPUTime();
				radix_sort(v1, get_key);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << ":"
				<< std::setw(w) << getCPUTime() - start << "\n";
		}
	}

	std::cout << "RADIX SORT END\n\n\n";

	return 0;
}