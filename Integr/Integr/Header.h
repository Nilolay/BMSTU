//#include "stdafx.h"
#include <iostream>
#include "Math.h"


using namespace std;

class pointf
{
	double x,y;
   public:
	pointf(){x=0;y=0;};
	pointf(double x1,double y1) {
	  x = x1;
	  y = y1;
	};
	
	double get_x() { return x; };
	double get_y() { return y; };
	void set(double x1, double y1) {
	   x = x1;
	   y = y1;
	};
	double rast() {
		return sqrt(x*x+y*y);
	};



};


typedef double (*fun) (double x);

double f1 (double x) {return x*sin(x);}
double f2 (double x) {return x*sin(x)+x*cos(x);}

class Baza_tab
{
 float a, b; int n;
 public:
	 pointf *p;
	 pointf *p_in;
	 int kol_in;
	 Baza_tab() {a=0; b=0; n=0; kol_in=0; };
	 Baza_tab(double a1, double b1, int n1) {a=a1; b=b1; n=n1; p = new pointf [n]; kol_in=0; };
	 pointf* tabul( double (*f) (double x1) ) {
	  double h = (b-a)/n;
	  double x2 = a;
	  double y2 = f(x2);
	  for (int i = 0; i < n; i++)
	  {
		  x2+=h; y2=f(x2);
		  p[i]=pointf(x2,y2);
	  }
	  return p;
	  
	 };
	 pointf* interval_nul() {
	   int i,j;
	   pointf* dt;
	   dt = new pointf[n];
	   for (i = 0, j = 0; i < n-2; i++)
	   {  
		   if (p[i].get_y()*p[i+1].get_y()<0)
		   {
			   dt[j]=pointf(p[i].get_x(),p[i+1].get_x());
			   j++;
		   }

	   }
	   p_in = new pointf[j];
	   for (i = 0; i < j; i++)
	   {
		   p_in[i]=dt[i];
	   }
	   kol_in = j;
	   delete[] dt;
	   return p_in;
	   };
	   void Destroy() {
		   if (n>0)
		   {
			   delete[] p; n=0;
		   }
		   if (kol_in>0)
		   {
			   delete[] p_in; kol_in=0;
		   }
	   };
	   float get_a() { return a; };
	   float get_b() { return b; };
	   int get_n() { return n; };
	   ~Baza_tab(){ cout << "destroy" <<endl; Destroy(); };

	   
};

class Baza_Integ_virt {
public:

	//virtual double integral(double eps) = 0;
	Baza_Integ_virt() { }
	~Baza_Integ_virt() { }
	virtual float get_a() = 0;
	virtual double poisk(double eps) = 0;
	//virtual double tabul(double eps) = 0;
	virtual int get_n() = 0;
};

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
		n = 2300;
	}
	double poisk(double eps) { return 0; }
	float get_a() { return a; }
	float get_b() { return b; }
	int get_n() { return n; }
	double tabul(double eps) {
		double s, sp, h, y, x, d;
		long int i;
		sp = 0;
		d = 1;
		while (fabs(d) > eps) {

			for (h = (b - a) / n, s = (f(a) + f(b)) * (h / 2), i = 1, x = a; i < n - 1; i++) {
				s += (f(x) * h);
				x += h;
			}
			d = s - sp;
			sp = s;
			n *= 5;
		}
		return s;
	}


	~Int_trap_virt() {
		std::cout << "dest";
	}
};

class Met_xord_virt : public Baza_Integ_virt {
protected:
	double a, b;
	fun f;
	long int n;

public:

	Met_xord_virt() : Baza_Integ_virt() { }

	Met_xord_virt(double av, double bv, fun ff) : Baza_Integ_virt() {
		a = av;
		b = bv;
		f = ff;
		//n = 2300;
	}
	~Met_xord_virt(){}
	float get_a() { return a; }
	float get_b() { return b; }
	int get_n() { return n; }
	double poisk( double eps ) {
		double d, xt, xp, fa, fb, fx;
		fa = f(a); 
		fb = f(b);
		xp = a - fa * ((b - a) / (fb - fa));
		d = 1;
		n = 1;
		fx = f(xp);
		while (fabs(fx)>eps && fabs(d)>eps && n < 55360) {
			if (fa * fx < 0)
			{
				b = xp;
				fb = fx;
				xt = a - fa * ((b - a) / (fb - fa));
				fx = f(xt);
				d = xt - xp;
				xp = xt;
				n++;
			}
			
			
		}
		return xp;
	}
};

class Met_newton_virt : public Baza_Integ_virt {
protected:
	double a, b;
	fun f;
	long int n;

public:

	Met_newton_virt() : Baza_Integ_virt() { }

	Met_newton_virt(double av, double bv, fun ff) : Baza_Integ_virt() {
		a = av;
		b = bv;
		f = ff;
		
	}
	~Met_newton_virt() {}
	float get_a() { return a; }
	float get_b() { return b; }
	int get_n() { return n; }
	double poisk(double eps) {
		double d, xt, xp, h, fp, pfa, pfb;
		int i; 
		h = 0.1e-11;
		xp = a;
		d = 1;
		n = 0;
		while (fabs(d)>eps&&n<100)
		{
			fp = (f(xp + h) - f(xp)) / h;
			if (fabs(fp)>0.1e-11)
			{
				xt = xp - f(xp) / fp;

			}
			else
			{
				break;
			}
			n++;
			d = (xt - xp);
			xp = xt;
		}
		return xt;
	}
};


class Pol_del_virt : public Baza_Integ_virt {
protected:
	double a, b;
	fun f;
	long int n;

public:

	Pol_del_virt() : Baza_Integ_virt() { }

	Pol_del_virt(double av, double bv, fun ff) : Baza_Integ_virt() {
		a = av;
		b = bv;
		f = ff;

	}
	~Pol_del_virt() {}
	float get_a() { return a; }
	float get_b() { return b; }
	int get_n() { return n; }
	double poisk(double eps) 
	{
		n = 0;
		double c;
		c = (a + b) / 2;
		while (fabs(f(c))>eps && fabs(b-a)>eps && n <50000)
		{
			c = (a + b) / 2;
			if (f(a)*f(c)<0)
			{
				b = c;
			}
			else
			{
				a = c;
			}
			n++;
		}
		return c;
	}



};

class Poisk_korni {
  public:
	  Poisk_korni(Baza_Integ_virt* comp) { p = comp; }
	  ~Poisk_korni() { delete p; }
	  double poisk(double eps) {
		 
		  return p->poisk(eps);
	  }
	  Baza_Integ_virt* p;
};

