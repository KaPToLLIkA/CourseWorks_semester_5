#pragma once
#include <vector>
#include <string>
#include <list>

template<class T>
void heap_sort(std::vector<T>& data);

template<class T>
void radix_sort(std::vector<T>& data, size_t(*get_key)(T& value));

template<class T>
void quick_sort(std::vector<T>& data);

namespace local {
	template<class T>
	void quick_sort_recursive(size_t l, size_t r, std::vector<T>& data);

	template<class T>
	inline void quick_sort_recursive(size_t l, size_t r, std::vector<T>& data)
	{
		size_t i = l, j = r;
		T pivot = data[(l + r) / 2];

		while (i <= j) 
		{
			while (data[i] < pivot)
			{
				++i;
			}

			while (data[j] > pivot)
			{
				--j;
			}

			if (i <= j) 
			{
				std::swap(data[i++], data[j--]);
			}
		}

		if (l < j) 
		{
			quick_sort_recursive(l, j, data);
		}
		if (r > i) 
		{
			quick_sort_recursive(i, r, data);
		}
	}

} // end "local" namespace

template<class T>
inline void heap_sort(std::vector<T>& data)
{
	int64_t size = static_cast<int64_t>(data.size());

	for (int64_t j = 0; j < size; j++)
	{
		for (int64_t i = size / 2 - 1 - j / 2; i > -1; i--)
		{
			if (2 * i + 2 <= size - 1 - j)
			{
				if (data[2 * i + 1] > data[2 * i + 2])
				{
					if (data[i] < data[2 * i + 1])
					{
						std::swap(data[i], data[2 * i + 1]);
						
					}
				}
				else 
				{
					if (data[i] < data[2 * i + 2])
					{
						std::swap(data[i], data[2 * i + 2]);

					}
				}
			}
			else 
			{
				if (2 * i + 1 <= size - 1 - j)
				{
					if (data[i] < data[2 * i + 1])
					{
						std::swap(data[i], data[2 * i + 1]);
					}
				}
			}
				
		}
		std::swap(data[0], data[size - 1 - j]);
	}
}

template<class T>
inline void radix_sort(std::vector<T>& data, size_t(*get_key)(T& value))
{
	if (data.size() <= 1) 
	{
		return;
	}

	size_t max_r = std::to_string(UINT64_MAX).length();
	size_t base = 10;
	std::vector<std::list<T*>> stacks(base);
	std::vector<T> tmp(data.size());

	size_t k = 1;
	for (size_t r = 0; r < max_r; ++r) 
	{

		for (size_t i = 0; i < data.size(); ++i) 
		{
			stacks[get_key(data[i]) / k % 10].push_back(&data[i]);
		}
		k *= 10;

		if (stacks[0].size() == data.size()) 
		{
			return;
		}

		size_t i = 0;
		for (auto& e : stacks) 
		{
			while (!e.empty()) 
			{
				tmp[i++] = *e.front();
				e.pop_front();
			}
		}

		data = tmp;
	}

}

template<class T>
inline void quick_sort(std::vector<T>& data)
{
	using namespace local;

	if (data.size() <= 1)
	{
		return;
	}

	quick_sort_recursive(0, data.size() - 1, data);
}


