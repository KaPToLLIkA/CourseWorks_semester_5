#include "entry.h"

#define MAX_ID UINT64_MAX
#define MIN_ID 0
#define __MAX(x, y) (x < y ? y : x)
#define __MIN(x, y) (x < y ? x : y)

void entry::rand_fill_data()
{
	for (size_t i = 0; i < sizeof(data); ++i) {
		data[i] = static_cast<char>(rand() % 20 + 60);
	}
	data[sizeof(data) - 1] = '\0';
}

entry::entry(uint64_t id) : id(id)
{	
	rand_fill_data();
}

entry::entry(uint64_t max_id, uint64_t min_id) :
	id(rand() % (__MAX(max_id, min_id) + 1 - __MIN(max_id, min_id)) + __MIN(max_id, min_id))
{
	rand_fill_data();
}

entry::entry() :
	id(rand() % (MAX_ID + 1 - MIN_ID) + MIN_ID)
{
	rand_fill_data();
}


uint64_t entry::get_id()
{
	return this->id;
}

char* entry::get_data()
{
	return this->data;
}

bool entry::operator<(const entry& o)
{
	return this->id < o.id;
}

bool entry::operator>(const entry& o)
{
	return this->id > o.id;
}

bool entry::operator<=(const entry& o)
{
	return this->id <= o.id;
}

bool entry::operator>=(const entry& o)
{
	return this->id >= o.id;
}

bool entry::operator==(const entry& o)
{
	return this->id == o.id;
}

#undef MAX_ID
#undef MIN_ID
#undef __MAX
#undef __MIN