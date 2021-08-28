// Integr.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include "Header.h"


int _tmain(int argc, _TCHAR* argv[])
{
	int i,j,k,m,n,l, j1;
	//Baza_Integ_virt fp, * fv;
	double s, xp;
	pointf* tv, * t0, * te;

	Baza_tab* af;
	af = new Baza_tab(-9, 9, 1000);
	(*af).tabul(f2);
	tv = (*af).interval_nul();
	cout << "Interval 0 f kol " << (*af).kol_in << endl;

	for (j = 0; j < (*af).kol_in; j++)
	{
		cout << tv[j].get_x() << " " << tv[j].get_y() << endl;
	}
	//double yc[]={11,13,14,16,21,7,4,9};
	//double kc[]={10,3,17,21,29,2,15,8};
	//pointf *bl, p;
	//double a,b,s; int **c;
	//Baza_tab v = Baza_tab(0,9,100);
	//pointf *tw, *tv0, *tw2; 
	//pointf tw3;
	////n=50;
	//tw=v.tabul(f1);
	//cout << " tab f1 " << endl;
	//for (i  = 0; i  < v.get_n() ; i ++)
	//{
	//	cout << i <<'\t' << tw[i].get_x()<<'\t'<< tw[i].get_y() << endl;
	//}
	//
	//tv0 = v.interval_nul();
	//cout << "interval_nul f1" << endl;
	//for (i  = 0; i  < v.kol_in ; i ++)
	//{
	//	cout << i+'\t' + tv0[i].get_x()+'\t'+ tv0[i].get_y() << endl;
	//}
	//system("pause");
	//system("cls");
	//Baza_tab vt = Baza_tab(0,9,100);
	//pointf *tw1; 
	//tw1=vt.tabul(f2);
	//tw2=vt.interval_nul();
	//
	//cout << " tab f2 " << endl;
	//for (i  = 0; i  < vt.get_n() ; i ++)
	//{
	//	cout << i <<'\t' << tw1[i].get_x()<<'\t'<< tw1[i].get_y() << endl;
	//}

	//system("pause");
	//
	//system("cls");
	//system("pause");

	//Baza_Integ_virt* df;
	////df = new Int_trap_virt(0, 1.57075, f1);
	////cout << "Int_trap_virt " << df->tabul(0.00001) << " n = " << df->get_n() << endl;
	//system("pause");
	//system("cls");
	////tw2 = ;



	Baza_Integ_virt *zpf[30];
	double x, a, b;
	for (j = 0, j1=0; j < (*af).kol_in; j++)
	{
		cout << tv[j].get_x() << ' ' << tv[j].get_y() << endl;
		zpf[j1] = new Met_xord_virt(tv[j].get_x(), tv[j].get_y(),f2);
		s = zpf[j1]->poisk(0.001);
		cout << "korni met hord" << s << "F(x)" << f2(s) << "n=" << (*zpf[0]).get_n() << "a=" << (*zpf[0]).get_a() << endl;

		zpf[j1+1] = new Met_newton_virt(tv[j].get_x(), tv[j].get_y(), f2);
		s = zpf[j1+1]->poisk(0.001);
		cout << "korni met newton" << s << "F(x)" << f2(s) << "n=" << (*zpf[j1+1]).get_n() << endl;

		zpf[j1+2] = new Pol_del_virt(tv[j].get_x(), tv[j].get_y(), f2);
		s = zpf[j1+2]->poisk(0.001);
		cout << "korni Pol del virt" << s << "F(x)" << f2(s) << "n=" << (*zpf[j1 + 2]).get_n() << endl;
		j1 += 3;
	}
	
	
	system("pause");
	system("cls");
	m = j1;

	Poisk_korni** zp;
	zp = new Poisk_korni * [j1];

	for (i = 0; i < m; i++)
	{
		zp[i] = new Poisk_korni(zpf[i]);
		cout << "Massiv poisk korni" << endl;
		

	}

	for (l = 0, k = 0; l < m; l++)
	{
		cout << zp[l]->poisk(0.000001) << " F(x) = " << f2(zp[l]->poisk(0.000001)) << endl;
	}

	/*v.~Baza_tab();
	vt.~Baza_tab();*/

	return 0;
};

/*//  определяет точку входа для консольного приложения. 
// 

#include "stdafx.h" 
#include <iostream> 
using namespace std; 



class Baza_Integ_virt { 
public: 

virtual double integral(double eps) = 0; 
Baza_Integ_virt() { } 
~Baza_Integ_virt() { } 
virtual int get_n() = 0; 
}; 

typedef double (*fun)(double x); 
double f1(double x) { return cos(x); } 

class Int_trap_virt : public Baza_Integ_virt { 




protected: 
double a, b; 
fun f; 
long int n; 

public: 

Int_trap_virt() : Baza_Integ_virt() { } 
  
Int_trap_virt(double av, double bv, fun ff) : Baza_Integ_virt() { 
  a = av; 
  b = bv; 
  f = ff; 
  n = 2300; } 
float get_a() { return a; } 
float get_b() { return b; } 
int get_n() { return n; } 
double integral(double eps) { 
  double s, sp, h, y, x, d; 
  long int i; 
  sp = 0; 
  d = 1; 
  while (fabs(d) > eps) { 

   for (h = (b - a) / n, s = (f(a) + f(b)) * (h/2), i = 1, x = a; i < n - 1; i++) { 
    s += (f(x) * h); 
    x += h; 
   } 
   d = s - sp; 
   sp = s; 
   n *= 5; 
  } 
  return s; 
} 

  
~Int_trap_virt() { std :: cout « "dest"; } 
}; 

int _tmain(int argc, _TCHAR* argv[]) 
{ 
Baza_Integ_virt *df; 
df = new Int_trap_virt(0, 1.57075, f1); 
cout « "Int_trap_virt " « df -> integral(0.00001) « " n = " « df -> get_n() « endl; 
// cin » i; 
return 0; 
}*/