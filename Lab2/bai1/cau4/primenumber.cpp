#include<iostream>
using namespace std;
#include<bitset>
#include<vector>
#include<math.h>

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



int compare_bits(vector<bool> a, vector<bool> b) {
	for (int i = a.size() - 1;i >= 0;i--)
		if (a[i] == true && b[i] == true)
			continue;
		else if (a[i] == true && b[i] == false)
			return 1;
		else if (a[i] == false && b[i] == true)
			return -1;

	return 0;
}

void print_vector(vector<bool> current_vector) {
	for (int k = current_vector.size() - 1;k >= 0;k--)
		cout << current_vector[k] ? 1 : 0;
	cout << endl;
}

vector<bool> create_window(vector<bool> the_dividend, int size) {
	vector<bool> window(size);
	int temp = 0;
	for (int i = window.size() - 1;i >= 0; i--) {
		if (the_dividend[the_dividend.size() - 1 - temp])
			window[i] = true;
		else
			window[i] = false;
		temp++;
	}
	return window;
}


vector<bool> minus_bits(vector<bool> a, vector<bool> b) {
	vector<bool> result(a.size());
	bool borrow = false;
	for (int i = 0; i < result.size();i++) {


		if (borrow) {
			if (a[i] == 1 && b[i] == 1)
				result[i] = true;
			else if (a[i] == 1 && b[i] == 0)
			{
				result[i] = false;;
				borrow = false;
			}
			else if (a[i] == 0 && b[i] == 1)
				result[i] = false;
			else if (a[i] == 0 && b[i] == 0)
				result[i] = true;

			continue;
		}



		if (a[i] == 1 && b[i] == 1)
			result[i] = false;
		else if (a[i] == 1 && b[i] == 0)
			result[i] = true;
		else if (a[i] == 0 && b[i] == 1) {
			result[i] = true;
			borrow = true;
		}
	}
	return result;
}



bool divide_bits(bitset<81> a, bitset<81> b) {
	vector<bool> the_dividend;
	vector<bool> divisor;
	vector<bool> result;
	for (int i = 80;i >= 0;i--)
		if (a[i] == 1) {
			the_dividend = vector<bool>(i + 1);
			for (int j = i; j >= 0;j--)
				if (a[j] == 1) {
					the_dividend[j] = true;
				}
				else
					the_dividend[j] = false;


			break;
		}

	for (int i = 80;i >= 0;i--)
		if (b[i] == 1) {
			divisor = vector<bool>(i + 1);
			for (int j = i; j >= 0;j--)
				if (b[j] == 1) {
					divisor[j] = true;
				}
				else
					divisor[j] = false;


			break;
		}
	print_vector(the_dividend);
	print_vector(divisor);






	result = vector<bool>();






	vector<bool> window = create_window(the_dividend, divisor.size());

	
	
	vector<bool> divisor_padding = divisor;
	int diff = window.size() - divisor.size();
	diff = abs(diff);
	if (diff != 0)
		for (int i = 1; i <= diff;i++)
			divisor_padding.push_back(false);






	if (compare_bits(window, divisor_padding) == -1) {
		window = create_window(the_dividend, divisor.size() + 1);
	}
	int window_size = window.size();

	int i = the_dividend.size() - 1;

	bool first_time = true;

	while (1) {
		try {
			diff = window.size() - divisor.size();
			diff = abs(diff);
			if (diff != 0)
				for (int i = 1; i <= diff;i++)
					divisor_padding.push_back(false);

			if (compare_bits(window, divisor_padding) == -1) {

				result.insert(result.begin(), false);


				if (window.at(window.size()-1)==0)
					window.pop_back();
				window.insert(window.begin(), the_dividend.at(i - window_size));
			}
			else {
				result.insert(result.begin(), true);
				vector<bool> minus_result = minus_bits(window, divisor_padding);
				window = minus_result;
				window.pop_back();
				window.insert(window.begin(), the_dividend.at(i-window_size));
			}
		}
		catch (out_of_range e) {
		//	window.insert(window.begin(), the_dividend[0]);
			for (bool check: window)
				if (check) {
					return false;
				}
			return true;
		}
		i--;
	}
	





}



int main() {
	bitset<81> the_dividend = bitset<81>("01010111101");
	bitset<81> divisor = bitset<81>("1");

	
	//cout<< divide_bits(the_dividend, divisor);

	//return 0 ;



	int count = 1;


	do {
		divisor = divisor_increment(divisor);

		if (divide_bits(the_dividend, divisor))
		{
			count += 1;
			divide_bits(the_dividend, divisor);
		}
		cout << "divisor: " << divisor.to_string() << endl;
	} while (the_dividend != divisor);




	cout << endl << endl;
	if (count == 2)
		cout << "La so nguyen to " << endl;
	else
		cout << "Khong la so nguyen to " << endl;


	cout << "So lan chia het: " << count;
	
	
	

}
