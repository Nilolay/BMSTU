// Array_c++.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "title.h"

using namespace std;

int main()
{
	int i, j, k, n, m;
	Array *a, *b;
	Array ar1, ar2, ar3, ar, ar4;
	n = 2;
	
	ar4.setfile((char *)"dat1.txt");
	cout << ar4;

	ar1 = Array(n);
	cin >> ar1;

	cout << "  mas  " << endl;

	for (size_t i = 0; i < ar1.getsize(); i++)
	{
		cout << ar[i] << "   " << endl;
		
	}
	cout << ar1;
	ar2 = Array(ar1);
	cout << ar2;
	ar3 = ar2 + ar1;
	ar = ar3;
	cout << "  ar  " << endl;
	cout << ar;
	cout << ar3;
	cout << "  dobav end mas  " << endl;
	ar1 += 9;
	ar1 += 4;
	cout << ar1;
	
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
