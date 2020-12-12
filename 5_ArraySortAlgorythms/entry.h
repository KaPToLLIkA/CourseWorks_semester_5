#pragma once
#include <stdint.h>
#include <cmath>


class entry
{
private:

	uint64_t id = 0;
	char data[200 - sizeof(id)] = { '\0' };

	void rand_fill_data();
public:
	explicit entry(uint64_t id);
	explicit entry(uint64_t max_id, uint64_t min_id);
	explicit entry();

	uint64_t get_id();
	char* get_data();

	bool operator<(const entry& o);
	bool operator>(const entry& o);
	bool operator<=(const entry& o);
	bool operator>=(const entry& o);
	bool operator==(const entry& o);
};
