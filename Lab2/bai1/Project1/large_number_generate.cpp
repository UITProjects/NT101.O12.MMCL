#include <iostream>
#include <iostream>
#include<string>
#include <bitset>
#include <random>

using namespace std;

bitset<81> large_number_generate() {
    bitset<81>large_number_bits;

    random_device dev;
    mt19937 rng(dev());
    std::uniform_int_distribution<std::mt19937::result_type> dist6(0, 1);


    for (int i = 0; i < 81;i++)
        if (dist6(rng) == 1)
            large_number_bits.set(i);
        else
            continue;
            
    return large_number_bits;
}


bool min_bits(bitset<81> a, bitset<81> b) {
    for (int i = 80; i >= 0;i--)
        if (
            (a[i] == 1 && b[i] == 1)
            || a[i] == 0 && b[i] == 0 
            )


            continue;
        else if (a[i] == 1 && b[i] == 0)
            return false;
        else
            return true;

}

bitset<81> minus_bits(bitset<81> a, bitset <81> b) {
    bitset<81> result;
    result.reset();
    bool borrow = false;
    for (int i = 0; i < 81 ;i++) {


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



bitset<81> gcd(bitset<81> a, bitset<81> b) {
    int count = 0;
    

    while (!(a==b)) {
        if (b == 0)
            return b;
        else if (a == 0)
            return b;

        if (min_bits(a, b))
            b = minus_bits(b, a);
        else
            a = minus_bits(a, b);
        count++;
        cout << count << endl;
    }
    
   
    return a;
    
}


int main() {

    bitset<81> a =  large_number_generate();
    bitset<81> b =  large_number_generate();

   // bitset<81> a = bitset<81>("0111111111111100"); // 33554432
  //  bitset<81> b = bitset<81>("10000000000000000000000000"); // 32764
  //  gcd(a, b);  4: 100
    bitset<81> gcd_result = gcd(a, b);
    cout << "gcd: " << endl;
    cout << gcd_result.to_string() << endl;
    cout << "a: " << endl;
    cout << a.to_string() << endl ;
    cout << "b: " << endl;
    cout << b.to_string() << endl;
    return 0;
}
