#pragma once
#include <vector>

template<class T>
void heap_sort(std::vector<T>& data);

template<class T>
void root_sort(std::vector<T>& data);

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

}

template<class T>
inline void root_sort(std::vector<T>& data)
{
}

template<class T>
inline void quick_sort(std::vector<T>& data)
{
	using namespace local;

	if (data.empty()) 
	{
		return;
	}

	quick_sort_recursive(0, data.size() - 1, data);
}


