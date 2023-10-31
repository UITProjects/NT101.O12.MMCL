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


bool min_bits(bitset<128> a, bitset<128> b) {
    for (int i = 127; i >= 0;i--)
        if (a[i] == 1 && b[i] == 1)
            continue;
        else if (a[i] == 1 && b[i] == 0)
            return false;
        else
            return true;

}

bitset<128> minus_bits(bitset<128> a, bitset <128> b) {
    bitset<128> result;
    result.reset();
    bool borrow = false;
    for (int i = 0; i < 128 ;i++) {


        if (borrow) {
            if (a[i] == 1 && b[i] == 1)
                result.set(i);
            else if (a[i] == 1 && b[i] == 0)
            {
                result.reset(i);
                borrow = false;
            }
            else if (a[i] == 0 && b[i] == 1)
                result.reset(i);
            else if (a[i] == 0 && b[i] == 0)
                result.set(i);

            continue;
        }



        if (a[i] == 1 && b[i] == 1)
            result.reset(i);
        else if (a[i] == 1 && b[i] == 0)
            result.set(i);
        else if (a[i] == 0 && b[i] == 1) {
            result.set(i);
            borrow = true;
        }
        



    }


        return result;


}

bitset<128> gcd(bitset<128> a, bitset<128> b) {
    bitset<128> result;
    if (min_bits(a, b))
       result =  minus_bits(b, a);
    else result =  minus_bits(a, b); 
    cout << result.to_string();

    return result;
    
}


int main() {

    bitset<128> a =  large_number_generate();
    bitset<128> b =  large_number_generate();
    gcd(a, b);
    return 0;
}
