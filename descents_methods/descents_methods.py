#Подключаем библиотеки

#Формы
from tkinter import *
import tkinter as tk
from tkinter import messagebox

# Графики
import matplotlib.pyplot as plt
import matplotlib
import math as mt
from math import *
matplotlib.use("TkAgg")
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg, NavigationToolbar2Tk
from matplotlib.figure import Figure
import numpy as np

# Мат вычисления
from scipy.optimize import minimize_scalar
import scipy as sp
from math import fabs


#Создаем окно
root = tk.Tk();

#Тестовая функция
#def func(x1,x2):
#    return (2*(x1**2)-2*x1*x2+(x2**2)+2*x1-2*x2 )
fun = "2*(x1**2)-2*x1*x2+(x2**2)+2*x1-2*x2";

exec("def func(x1,x2): return 2*(x1**2)-2*x1*x2+(x2**2)+2*x1-2*x2");

#def func(x1,x2,fun=fun): 
#    print(fun)
#    fun1 = fun.replace('x1',str(x1))
#    fun1 = fun1.replace('x2',str(x2))
#    res = float(exec(fun1))
#    return res

#Покоординатный спуск    (Необходимо добавить возможность выбора метода одномерной оптимизации)
def coordinate_descent(func,x,eps):
        x0 = x;
        e = eps;
        cnt = 0;
        while(True):
            print(cnt)
            index = cnt % 2;
            #Запоминаем значение на предыдущем шаге
            samp1 = x0.copy()
            # Поочередно фиксируем аргументы
            if index == 0:
                print(1)
                func2 = lambda x :func(x,x0[1])
            else:
                print(2)
                func2 = lambda x :func(x0[0],x)
            #Одномерная оптимизация
            res = minimize_scalar(func2)
            print(res)
            x0[index] = res.x
            #Проверяем условие останова
            if  fabs(func(*x0)-func(*samp1)) < e:
                return samp1, cnt
            #На случай если метод будет не сходиться
            if cnt > 2000:
                return xk_1, cnt;
            cnt+=1

#Градиентный спуск - Добавить другие варианты шага
def gradient_descent(func,x0,step,eps,method='const'):
    xk = np.array(x0)
    e = eps;
    cnt = 0
    z_grad = step;
    while(True):
        x = xk[0]
        y = xk[1]
        
        #Выбираем метод шага 
        if method == 'decr':
            z_grad = z_grad/(cnt+1)
            
        #Запоминаем прошлый шаг
        xk_1 = xk.copy()
        
        #Аппроксимируем частные производные через центральные разности
        diff_x = (func(x+1,y)-func(x-1,y))/2*1
        diff_y = (func(x,y+1)-func(x,y-1))/2*1
        
        #Шаг по антиградиенту
        xk = xk-z_grad*np.array([diff_x,diff_y])
        
        #Проверяем условие останова
        if fabs(func(*xk)-func(*xk_1)) < e:
            return xk_1, cnt;
        #На случай если метод будет не сходиться
        if cnt > 2000:
            return xk_1, cnt;
        cnt+=1


#Значения
u = np.linspace(-10, 10, 100)
v = np.linspace(-10, 10, 100)
X,Y = np.meshgrid(u, v)
Z = func(X,Y)

n = Z.shape[0]*Z.shape[1]

# График

#f = Figure(figsize=(5,5), dpi=100)
#que_din = f.add_subplot(111)
#que_din.plot([1,2,3,4,5,6,7,8],[5,6,1,3,8,9,3,5])


from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt



fig = plt.figure(figsize=(10,10))
ax = fig.add_subplot(111, projection='3d')

#x = np.linspace(-10,10,1000)
#y = np.linspace(-10,10,1000)
#z = np.array([ (x[i]**2)/8 + (y[i]**2)/18 for i in range(len(x))])



ax.scatter(X.reshape(n), Y.reshape(n), Z.reshape(n), c='b', marker='1')

ax.set_xlabel('X Label')
ax.set_ylabel('Y Label')
ax.set_zlabel('Z Label')


#Ввод значений
interface = tk.LabelFrame(root,width=90,height=160);

description = tk.LabelFrame(interface, text="Инструкция",width=90,height=40);
txt0 = tk.Label(description,width=32,height=20,bg='#ffffff',text='В функции должны находиться\n переменные x1 и x2\nДля обозначении степени\n использовать ** вместо ^\n Для покоординатного спуска\nиспользуется последовательное\n фиксирование координат\nПогрешность и начальная точка\nзадается для двух методов\nостальные параметры\nдля градиентного спуска' ,relief='ridge')
txt0.grid(row=0,column=0);

input_parameters = tk.LabelFrame(interface, text="Параметры",width=50,height=50);

txt1 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Функция от двух переменных' ,relief='ridge')
txt1.grid(row=0,column=0);
eiter = tk.Entry(input_parameters,width=10);
eiter.grid(row=0,column=1);

txt2 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Начальный шаг для грд. спуска' ,relief='ridge');
txt2.grid(row=1,column=0);
etd = tk.Entry(input_parameters,width=10);
etd.grid(row=1,column=1);

txt3 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Абсолютная погрешность вычислений' ,relief='ridge');
txt3.grid(row=2,column=0);
ea = tk.Entry(input_parameters,width=10);
ea.grid(row=2,column=1);

txt4 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Значение для инициализации X0' ,relief='ridge');
txt4.grid(row=3,column=0);
eb = tk.Entry(input_parameters,width=10);
eb.grid(row=3,column=1);

txt5 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Вид шага по антиградиенту' ,relief='ridge');
txt5.grid(row=4,column=0);
el = tk.Entry(input_parameters,width=10);
el.grid(row=4,column=1);

result = tk.LabelFrame(interface, text="Рассчет по методу Градиентного спуска", width=50,height=50);
txt6 = tk.Label(result,width=32,height=1,bg='#ffffff',text='Оптимальное значение функции' ,relief='ridge');
txt7 = tk.Label(result,width=32,height=1,bg='#ffffff',text=' ' );
txt8 = tk.Label(result,width=32,height=1,bg='#ffffff',text='Номер итерации' ,relief='ridge');
txt9 = tk.Label(result,width=32,height=1,bg='#ffffff',text=' ' );
txt10 = tk.Label(result,width=32,height=1,bg='#ffffff',text='Точка оптимума Xk' ,relief='ridge');
txt11 = tk.Label(result,width=32,height=1,bg='#ffffff',text=' ' );
txt6.grid(row=0,column=0);
txt7.grid(row=1,column=0);
txt8.grid(row=2,column=0);
txt9.grid(row=3,column=0);
txt10.grid(row=4,column=0);
txt11.grid(row=5,column=0);
result.grid(row=0,column=1);

result_coordinate = tk.LabelFrame(interface, text="Рассчет по методу Покоординатного спуска",width=50,height=50);
txt12 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text='Оптимальное значение функции' ,relief='ridge');
txt13 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text=' ' );
txt14 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text='Номер итерации' ,relief='ridge');
txt15 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text=' ' );
txt16 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text='Точка оптимума Xk' ,relief='ridge');
txt17 = tk.Label(result_coordinate,width=32,height=1,bg='#ffffff',text=' ' );
txt12.grid(row=0,column=0);
txt13.grid(row=1,column=0);
txt14.grid(row=2,column=0);
txt15.grid(row=3,column=0);
txt16.grid(row=4,column=0);
txt17.grid(row=5,column=0);
#result_coordinate.grid(row=0,column=1);

#Принемаем новую функцию
def solve():
    try:
        fun = eiter.get(); 
        fun = fun.replace("x1","2");
        fun = fun.replace("x2","2");
        eval(fun);
        fun = eiter.get();
    except:
        answer = messagebox.showerror(title="Ошибка", message="Функция введена некорректно!")
        fun = "2*(x1**2)-2*x1*x2+(x2**2)+2*x1-2*x2";
    print(fun);
    exec("def func(x1,x2): return " + fun, globals());
    print(func(6,2))
    #Значения для графика
    u = np.linspace(-15, 15, 100)
    v = np.linspace(-15, 15, 100)
    X,Y = np.meshgrid(u, v)
    Z = func(X,Y)
    n = Z.shape[0]*Z.shape[1]

    #Перерисовываем график функции

    fig = plt.figure(figsize=(10,10))
    ax = fig.add_subplot(111, projection='3d')
    ax.scatter(X.reshape(n), Y.reshape(n), Z.reshape(n), c='b', marker='1')

    ax.set_xlabel('X Label')
    ax.set_ylabel('Y Label')
    ax.set_zlabel('Z Label')

    #Добавляем график на форму
    canvas = FigureCanvasTkAgg(fig, root);

    #Параметры окна
    canvas.get_tk_widget().grid(row=1, column=1);

    try:
        er = float(ea.get())
    except:
        answer = messagebox.showerror(title="Ошибка", message="Точность задана не верно!")
        er = 10**(-5);
    print(er);

    try:
        stp = float(etd.get())
    except:
        stp = 0.25;

    try:
        mth = el.get()
    except:
        mth = 'const';

    try:
        start_x = [float(x) for x in eb.get().split(' ')]
    except:
        start_x = [0.0,0.0];

    #Оптимизация Градиентный спуск
    xopt, itercnt = gradient_descent(func,start_x,stp,er,method=mth)
    func_value = func(*xopt);
    txt7['text'] = func_value;
    txt9['text'] = itercnt;
    txt11['text'] = xopt;

    #Оптимизация Покоординатный спуск
    xopt_coord, itercnt_coord = coordinate_descent(func,start_x,er)
    func_value_coord = func(*xopt);
    txt13['text'] = func_value_coord;
    txt15['text'] = itercnt_coord;
    txt17['text'] = xopt_coord;

    root.update()


#Кнопка
bt = tk.Button(interface, width=5, height=2, text ="Solve", command=solve);

#Добавляем график на форму
canvas = FigureCanvasTkAgg(fig, root);

#Параметры окна
canvas.get_tk_widget().grid(row=1, column=1);
interface.grid(row=1, column=3, padx=60);

description.grid(row=0,column=1,pady=10);
bt.grid(row=4,column=1,pady=10);
input_parameters.grid(row=1,column=1,pady=10);
result.grid(row=2,column=1,pady=10);
result_coordinate.grid(row=3,column=1);

root.title('Descent methods')

#root.overrideredirect(True)
root.geometry("{0}x{1}+0+0".format(root.winfo_screenwidth(), root.winfo_screenheight()))

#Запуск формы
root.mainloop()
