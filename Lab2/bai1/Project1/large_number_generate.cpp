#include <iostream>
#include <iostream>
#include<string>
#include <bitset>
#include <random>

using namespace std;

bitset<128> large_number_generate() {
    bitset<128>large_number_bits;

    random_device dev;
    mt19937 rng(dev());
    std::uniform_int_distribution<std::mt19937::result_type> dist6(0, 1);

    for (int i = 0; i < 82; i++)
        large_number_bits.set(i);

    for (int i = 82; i < 128;i++)
        if (dist6(rng) == 1)
            large_number_bits.set(i);
        else
            continue;

    cout << large_number_bits.to_string() << endl;
    return large_number_bits;
}


int main() {

    large_number_generate();
    large_number_generate();

    return 0;
}
