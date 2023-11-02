#include <iostream>
#include <cmath>
#include <string>
#include <random>
#include <bits/stdc++.h>
using namespace std;

int64_t gcd(int64_t a, int64_t b) {
    if (b == 0) {
        return a;
    }
    return gcd(b, a % b);
}

//Tìm a^x mod m = 1
int64_t modInverse(int64_t a, int64_t m) {
    a = a % m;
    for (int x = 1; x < m; x++) {
        if ((a * x) % m == 1) {
            return x;
        }
    }
    return 1;
}

// hàm tính a^k mod n
long long power_mod(long long a, long long k, long long n) {
    long long c = 0;
    long long d = 1;

    for (int i = k; i >= 1; i--) {
        if (((k >> (i - 1)) & 1) == 1) {
            c = c * 2 + 1;
            d = (d * d * a) % n;
        }
        else {
            c = c * 2;
            d = (d * d) % n;
        }
    }
    return d;
}

// hàm kiểm tra n có phải là số nguyên tố với độ chính xác xác suất p
bool isPrime(int64_t n, int p) {
    // kiểm tra các trường hợp đặc biệt
    if (n <= 1) return false;
    if (n <= 3) return true;

    // tìm số d và s sao cho n-1 = d*2^s
    int64_t d = n - 1;
    int s = 0;
    while (d % 2 == 0) {
        d /= 2;
        s++;
    }

    // thực hiện p lần kiểm tra Miller-Rabin
    random_device rd;
    mt19937 gen(rd());
    uniform_int_distribution<int64_t> dis(2, n - 2);
    for (int i = 0; i < p; i++) {
        int64_t a = dis(gen);
        int64_t x = power_mod(a, d, n);
        if (x == 1 || x == n - 1) continue;
        for (int j = 0; j < s - 1; j++) {
            x = (x * x) % n;
            if (x == 1) return false;
            if (x == n - 1) break;
        }
        if (x != n - 1) return false;
    }
    return true;
}

long long int generatePrime() {
    int64_t num = rand() % 100 + 1;
    while (!isPrime(num,10)) {
        num = rand() % 100 + 1;
    }
    return num;
}

long long int generateKeyRandom(long long int p, long long int q) {
    int64_t n = p * q;
    int64_t phi = (p - 1) * (q - 1);
    int64_t e = rand() % (phi - 2) + 2;
    while (gcd(e, phi) != 1) {
        e = rand() % (phi - 2) + 2;
    }
    int64_t d = modInverse(e, phi);
    return e;
}

long long int generateKey(int64_t p, int64_t q) {
    int64_t n = p * q;
    int64_t phi = (p - 1) * (q - 1);
    int64_t e = 0;
    do {
        cout << "Enter a value e (1 < e < phi(n), gcd(e, phi(n)) = 1): ";
        cin >> e;
    } while (gcd(e, phi) != 1);
    int64_t d = modInverse(e, phi);
    return e;
}

// Function to generate all prime
// numbers less than n
bool SieveOfEratosthenes(int n, bool isPrime[])
{
	// Initialize all entries of boolean array
	// as true. A value in isPrime[i] will finally
	// be false if i is Not a prime, else true
	// bool isPrime[n+1];
	isPrime[0] = isPrime[1] = false;
	for (int i = 2; i <= n; i++)
		isPrime[i] = true;

	for (int p = 2; p * p <= n; p++) {
		// If isPrime[p] is not changed, then it is
		// a prime
		if (isPrime[p] == true) {
			// Update all multiples of p
			for (int i = p * 2; i <= n; i += p)
				isPrime[i] = false;
		}
	}
}

string findPrimePair(int n)
{
	int flag = 0;

	// Generating primes using Sieve
	bool isPrime[n + 1];
	SieveOfEratosthenes(n, isPrime);

	// Traversing all numbers to find first
	// pair
	for (int i = 2; i < n; i++) {
		int x = n / i;

		if (isPrime[i] && isPrime[x] and x != i
			and x * i == n) {
			cout << "p = " << i << ", " << "q = " << x;
			flag = 1;
			return "y";
		}
	}

	if (!flag)
		return "x";
}

int main() {

    int64_t p, q, publicKey;
    while (1)
    {
        int choice;
        cout << "(1)Random p,q/ (2)Enter p,q/  (3)Set the key pair as large as possible:" << endl;
        cin >> choice;
        if (choice == 1)
        {
            srand(time(0));
            p = generatePrime();
            q = generatePrime();
            publicKey = generateKeyRandom(p, q);
            cin.ignore();
            cout << "p: " << p<<endl;
            cout << "q: " << q<<endl;
            break;
        }
        else if (choice == 2)
        { 
            do{
                cout << "p= ";
                cin >> p;
                cin.ignore();
            } while (!isPrime(p, 10));

            do {
                cout << "q= ";
                cin >> q;
            } while (!isPrime(q, 10));
            cin.ignore();
            publicKey = generateKey(p, q);
            cin.ignore();
            break;
        }
        else if (choice == 3)
        {
            int n = 21024;
	        for (int i = n; i > 0; i--) {
	            if (findPrimePair(i) == "x")
	                continue;
	            else
	                break;
	        }
	        return 0;
        }
    }
    int64_t n = p * q;
    int64_t phi = (p - 1) * (q - 1);
    int64_t privateKey = modInverse(publicKey, phi);
    

    cout << "Public key: " << "(" << publicKey << "," << n << ")"<<endl;
    cout << "Private key: " << "(" << privateKey << "," << n << ")" << endl;
    return 0;
}
