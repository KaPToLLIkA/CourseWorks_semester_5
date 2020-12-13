#include "operations_counter.h"

#ifdef ENABLE_GLOBAL_COUNTER

namespace counter {
	operations_counter* operations_counter::inst = nullptr;

	operations_counter* operations_counter::get_i()
	{
		if (inst == nullptr)
		{
			inst = new operations_counter();
		}
		return inst;
	}

	void operations_counter::reset()
	{
		this->copy = this->compare = 0;
	}

	uint64_t operations_counter::get_copy_op()
	{
		return this->copy;
	}

	uint64_t operations_counter::get_compare_op()
	{
		return this->compare;
	}

	void operations_counter::inc_copy()
	{
		++this->copy;
	}

	void operations_counter::inc_compare()
	{
		++this->compare;
	}

	operations_counter::operations_counter()
	{
	}

} // end "counter" namespace

#endif // ENABLE_GLOBAL_COUNTER