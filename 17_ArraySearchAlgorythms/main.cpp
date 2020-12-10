#include <iostream>
#include <vector>


#include "entry.h"
#include "search_algorythms.h"
#include "test_functions.h"



int main() {
	//tests section
#ifdef _DEBUG
	basic_binary_search_test();
	basic_fibonacci_search_test();
	basic_interpolation_search_test();
#endif // _DEBUG

	//example of usage
	//how to get execution time

	//ex 1
	//middle time of 100 tests

	{
		double total_time = 0;
		// create dataset, use new_vector_for_<name_of_algorythm>(size_of_data_set)
		std::vector<entry> data = new_vector_for_binary_s(1000 * 1000);

		entry target(401319);

		int number_of_tests = 100;
		for (int i = 0; i < number_of_tests; ++i) 
		{
			std::cout << "Test: " << i << std::endl;
			double start = getCPUTime();
			binary_search(data, target);
			total_time += getCPUTime() - start;
		}
		std::cout << "Middle time: " << total_time / number_of_tests << "\n";
	}


	//ex 2
	//simple test

	{
		std::vector<entry> data = new_vector_for_fibonacci_s(1000 * 1000);
		entry target(401319);

		double start = getCPUTime();
		fibonacci_search(data, target);
		std::cout << "Execution time: " << getCPUTime() - start << "\n";

	}
}