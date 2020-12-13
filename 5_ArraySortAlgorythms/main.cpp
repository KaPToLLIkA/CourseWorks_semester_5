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
	uint64_t sz = 2;
	uint64_t k[] = { 9, 10 };
	uint64_t z[] = { 1000, 10000 };
	uint64_t w = 20;

	//quick sort
	std::cout << "QUICK SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				quick_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}
	std::cout << "QUICK SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
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
				quick_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}

	std::cout << "QUICK SORT END\n\n\n";





	//quick sort
	std::cout << "HEAP SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				heap_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}
	std::cout << "HEAP SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
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
				heap_sort(v1);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}

	std::cout << "HEAP SORT END\n\n\n";





	//quick sort
	std::cout << "RADIX SORT RANDOM\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
	for (uint64_t i = 0; i < sz; ++i)
	{
		for (uint64_t x = 1; x <= k[i]; ++x)
		{
			//random
			std::vector<entry> v1(x * z[i]);

			RESET_OP_COUNTER
				radix_sort(v1, get_key);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}
	std::cout << "RADIX SORT SORTED\n";
	std::cout
		<< std::setw(w) << "ITEMS" << ":"
		<< std::setw(w) << "COPY" << ":"
		<< std::setw(w) << "COMPARE" << "\n";
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
				radix_sort(v1, get_key);
			std::cout
				<< std::setw(w) << x * z[i] << ":"
				<< std::setw(w) << GET_COPY_OP_COUNT << ":"
				<< std::setw(w) << GET_COMPARE_OP_COUNT << "\n";
		}
	}

	std::cout << "RADIX SORT END\n\n\n";

	return 0;
}