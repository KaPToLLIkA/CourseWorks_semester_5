#pragma once
#include <stdint.h>

#define ENABLE_GLOBAL_COUNTER

#ifdef ENABLE_GLOBAL_COUNTER
namespace counter {
	class operations_counter
	{
	private:
		static operations_counter* inst;
		uint64_t copy = 0, compare = 0;

		explicit operations_counter();
	public:
		static operations_counter* get_i();

		void reset();
		uint64_t get_copy_op();
		uint64_t get_compare_op();

		void inc_copy();
		void inc_compare();
	};

} // end "counter" namespace

#define FIX_COPY_OP (counter::operations_counter::get_i()->inc_copy());
#define FIX_COMPARE_OP (counter::operations_counter::get_i()->inc_compare());
#define RESET_OP_COUNTER (counter::operations_counter::get_i()->reset());
#define GET_COPY_OP_COUNT (counter::operations_counter::get_i()->get_copy_op())
#define GET_COMPARE_OP_COUNT (counter::operations_counter::get_i()->get_compare_op())

#else

#define FIX_COPY_OP
#define FIX_COMPARE_OP
#define RESET_OP_COUNTER
#define GET_COPY_OP_COUNT 0
#define GET_COMPARE_OP_COUNT 0

#endif // ENABLE_GLOBAL_COUNTER




