#include "entry.h"

uint64_t entry::g_id = 0;

entry::entry(uint64_t id): id(id)
{
	for (size_t i = 0; i < sizeof(data); ++i) {
		data[i] = static_cast<char>(rand() % 20 + 60);
	}

	data[sizeof(data) - 1] = '\0';
}

entry entry::new_entry_with_arithmetically_progressive_id()
{
	entry::g_id += 3;
	return entry(entry::g_id);
}

entry entry::new_entry_with_serial_id()
{
	return entry(entry::g_id++);
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

bool entry::operator==(const entry& o)
{
	return this->id == o.id;
}

uint64_t entry::operator-(const entry& o)
{
	return this->id - o.id;
}
