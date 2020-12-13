#include <string>

int main() {
	int c = 0;
	for (int i = 1; i <= 1000000; ++i) {
		std::string a = std::to_string(i);
		if (a[0] == '1') {
			++c;
		}
	}

	c;
	return 0;
}