#include <iostream>

#include "entry.h"
#include "tests.h"

int main() {
#ifdef _DEBUG
	test_quick_sort();
	test_heap_sort();
	test_radix_sort();
#endif // _DEBUG
	

	return 0;
}