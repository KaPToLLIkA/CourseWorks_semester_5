#pragma once
#include <stdint.h>
#include <cmath>
 

class entry
{
private:
	static uint64_t g_id;
	uint64_t id;
	char data[200 - sizeof(id)];

public:
	explicit entry(uint64_t id);

	static entry new_entry_with_arithmetically_progressive_id();
	static entry new_entry_with_serial_id();

	uint64_t get_id();
	char* get_data();

	bool operator<(const entry& o);
	bool operator>(const entry& o);
	bool operator==(const entry& o);
	uint64_t operator-(const entry& o);
};


