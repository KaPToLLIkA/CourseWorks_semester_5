#pragma once
#include <vector>
#include <algorithm>

#define NPOS -1

template <class T>
size_t fibonacci_search(std::vector<T>& array, T key);

template <class T>
size_t binary_search(std::vector<T>& array, T key);

template <class T>
size_t interpolation_search(std::vector<T>& array, T key);

template<class T>
inline size_t fibonacci_search(std::vector<T>& array, T key)
{
	if (array.empty()) 
	{
		return NPOS;
	}

	std::sort(array.begin(), array.end());
	// first fibonacci numbers in range(0, INT64_MAX)
	size_t arr_sz = 93;
	int64_t first_fib_nums[] = {
		0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 
		1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 
		196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887, 
		9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141, 
		267914296, 433494437, 701408733, 1134903170, 1836311903, 2971215073, 
		4807526976, 7778742049, 12586269025, 20365011074, 32951280099, 
		53316291173, 86267571272, 139583862445, 225851433717, 365435296162, 
		591286729879, 956722026041, 1548008755920, 2504730781961, 
		4052739537881, 6557470319842, 10610209857723, 17167680177565, 
		27777890035288, 44945570212853, 72723460248141, 117669030460994, 
		190392490709135, 308061521170129, 498454011879264, 806515533049393, 
		1304969544928657, 2111485077978050, 3416454622906707, 5527939700884757, 
		8944394323791464, 14472334024676221, 23416728348467685, 37889062373143906, 
		61305790721611591, 99194853094755497, 160500643816367088, 259695496911122585, 
		420196140727489673, 679891637638612258, 1100087778366101931, 
		1779979416004714189, 2880067194370816120, 4660046610375530309, 
		7540113804746346429, 
	};

	int64_t k = 0;
	for (size_t i = 0; i < arr_sz; ++i)
	{
		if (first_fib_nums[i] - 1 >= static_cast<int64_t>(array.size()))
		{
			break;
		}
		k = i;
	}
	
	int64_t m = first_fib_nums[k + 1] - 1 - static_cast<int64_t>(array.size());
	int64_t current_index = first_fib_nums[k] - m;
	int64_t p = first_fib_nums[k - 1], q = first_fib_nums[k - 2];

	while(true) 
	{
		if (current_index < 1 || key > array[current_index - 1])
		{
			if (p > 1) 
			{
				current_index += q;
				p -= q;
				q -= p;
			}
			else 
			{
				return NPOS;
			}
		}
		else if (key < array[current_index - 1])
		{
			if (q > 0) 
			{
				current_index -= q;
				m = p;
				p = q;
				q = m - q;
			}
			else 
			{
				return NPOS;
			}
		}
		else 
		{
			return static_cast<size_t>(current_index - 1);
		}
	}


	return NPOS;
}

template<class T>
inline size_t binary_search(std::vector<T>& array, T key)
{
	std::sort(array.begin(), array.end());

	size_t l = 0, r = array.size();
	size_t mid = 0;
	
	while (!(l >= r))
	{
		mid = l + (r - l) / 2;

		if (key == array[mid])
		{
			return mid;
		}

		if (key < array[mid])
		{
			r = mid;
		}
		else
		{
			l = mid + 1;
		}
	}

	return NPOS;
}

template<class T>
inline size_t interpolation_search(std::vector<T>& array, T key)
{
	if (array.empty())
	{
		return NPOS;
	}

	std::sort(array.begin(), array.end());

	size_t l = 0, r = array.size() - 1;
	size_t current_index = 0;

	while (array[l] < key && array[r] > key) 
	{
		current_index = l + (r - l) * (key - array[l]) / (array[r] - array[l]);
		if (key > array[current_index]) 
		{
			l = current_index + 1;
		}
		else if (key < array[current_index]) 
		{
			r = current_index - 1;
		}
		else 
		{
			return current_index;
		}
	}

	if (array[r] == key) 
	{
		return r;
	}

	if (array[l] == key)
	{
		return l;
	}

	return NPOS;
}
