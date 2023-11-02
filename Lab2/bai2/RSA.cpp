#include <bits/stdc++.h>

using namespace std;

long int p, q, n, y, e, d, i, j, m, c, flag;
char oMsg[100];
char eMsg[100];
char dMsg[100];

bool isPrime = true;


bool SieveOfEratosthenes(int n, bool isPrime[])
{
	isPrime[0] = isPrime[1] = false;
	for (int i = 2; i <= n; i++)
		isPrime[i] = true;

	for (int p = 2; p * p <= n; p++) {
		if (isPrime[p] == true) {
			for (int i = p * 2; i <= n; i += p)
				isPrime[i] = false;
		}
	}
}

pair<int, int> findPrimePair(int n)
{
    int flag = 0;
    bool isPrime[n + 1];
    SieveOfEratosthenes(n, isPrime);

    for (int i = 2; i < n; i++) {
        int x = n / i;

        if (isPrime[i] && isPrime[x] and x != i
            and x * i == n) {
            flag = 1;
            return make_pair(i, x);
        }
    }

    if (!flag)
        return make_pair(-1, -1);
}
bool prime (long int num)
{
    for (int i = 2 ; i <= (num/2); i++) {
        if (num % i == 0)
           return false;
    }
    return true;
}
long int calculateD(long int e)
{
    long int k = 1;
    while (1) {
        k = k + y;
        if (k % e == 0)
        {
            return (k / e);
        }
    }
}
void calculateE()
{
    for (j = 2; j < y; j++) {
        if (y % j == 0) 
            continue;
        isPrime = prime(j);
        if (isPrime && j != p && j!= q) {
            e = j;
            flag = calculateD(e);
            if (flag > 0) {
                d = flag;
                break;
            }
        }
    }
}
int main()
{
    int size_n;
    cout << "size_n input: ";
    cin >> size_n;
    cout << "\n";
    pair<int, int> primePair;
    for (int i = size_n ; i > 0; i--) {
        primePair = findPrimePair(i);
        if (primePair.first != -1 && primePair.second != -1)
            break;
    }
    p = primePair.first;
    q = primePair.second;
    n = p*q;
    y = (p-1)*(q-1);
    calculateE();
    cout << "Input message ";
    cin >> oMsg;
    cout << "p\tq\tn\ty\tm" << endl;
    cout << p << "\t" << q << "\t" << n << "\t" << y << "\t" << oMsg << "\t" << endl;
    cout << "e = " << e << endl;
    cout << "d = " << d << endl;
    int a;
    for (a = 0; oMsg[a] != '\0'; a++) {
        // enc algo 
    m = oMsg[a];
    long int k = 1;
    for (i = 0; i < e; i++) {
        k = k*m;
        k = k%n;
    }
    c = k;
    eMsg[a] = c; 
    // dec algo
    k = 1;
    for (i = 0; i < d; i++) {
        k = k*c;
        k = k%n;
    }
    m = k;
    dMsg[a] = m; 
    }
    eMsg[a] = -1;
    dMsg[a] = -1;
        cout << "The encrypted message is ";

    for (i = 0; eMsg[i] != -1; i++) {
        cout <<eMsg[i];
    }
    cout << "\nThe Decrypted message is ";
    for (i = 0; dMsg[i] != -1; i++) {
        cout << dMsg[i];
    }
    cout <<endl;
    return 0;
    
}