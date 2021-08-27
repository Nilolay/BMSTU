class My_integral():
    def __init__(self):
        pass
    
    def integral(self,f,a,b,eps,method='lp'):

        if (( f(b)-f(a) )/eps) % int(( f(b)-f(a) )/eps) > 0:
            s = 1
        else:
            s = 0
        num = int(( f(b)-f(a) )/eps) + s;
        i = 0;
        accum = 0;
        if method == 'lp':
            print('lp')
            while i < num:
                #print(i);
                #print(num);
                if a+(i+1)*eps >= b:
                    #print(a+i*eps)
                    accum = accum + (a+i*eps - (b))*f(a+i*eps)
                    break;
                else:
                    #print(a+i*eps)
                    accum = accum + eps*f(a+i*eps)
                i = i + 1;
                #print (accum)
        if method == 'tr':
            #print('tr')
            i = 1;
            while i < num:
                if a+(i+1)*eps >= b:
                    accum = accum + (a+i*eps - (b))*(f(a+(i-1)*eps)+f(b))/2
                    break;
                else:
                    accum = accum + eps*(f(a+i*eps)+f(a+(i-1)*eps))/2
                i = i+1;
        return accum;


def func(x):
    return x*8+4;

integ = My_integral()

integ.integral(func,2,6,0.000001,method='tr')
