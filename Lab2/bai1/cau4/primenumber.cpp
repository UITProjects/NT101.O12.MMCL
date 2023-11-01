#include<iostream>
using namespace std;
#include<bitset>



bitset<81> divisor_increment(bitset<81> current_bitset) {
	for(int i = 80 ;i>=0 ;i--)
		if (current_bitset[i] == 1) {

			if (i == 0) {
				current_bitset.reset(0);
				current_bitset.set(1);
				return current_bitset;
			}
			else {
				int count = 0;
				int reset_bit_if_one = i ;
				for (int j = reset_bit_if_one-1; j>=0;j--)
					if (current_bitset[j] == 1) {
						count++;
					}
					

				if (count == reset_bit_if_one)
				{
	
					for (int j = i; j >= 0; j--)
						current_bitset.reset(j);

					current_bitset.set(i + 1);
					return current_bitset;
				}
				
			
			}

		}
	current_bitset.set(0);
	return current_bitset;
}


int main() {
	bitset<81> test = bitset<81>();
	test.reset();

	

	
	for (int i = 0;i < 500;i++)
	{
		test = divisor_increment(test);
		cout << test.to_string() << endl;
	}
	



}
