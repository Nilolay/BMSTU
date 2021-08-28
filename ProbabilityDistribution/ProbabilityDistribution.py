from tkinter import *
import tkinter as tk
import matplotlib
import math as mt
matplotlib.use("TkAgg")
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg, NavigationToolbar2Tk
from matplotlib.figure import Figure
import numpy as np
from decimal import *
import decimal as d






#Равномерное распределение
def uniform_dens(a,b,x):
    if x > b or x < a:
        res = 0
    else:
        res = 1/(b-a)
    return res

def uniform_destrib(a,b,x):
    if x < a:
        res = 0;
    elif x > b:
        res = 1;
    else:
        res = (x-a)/(b-a);
    return res;

#Распределение Пуассона

def poisson_dens(l,k):
    if k < 0:
        return Decimal(0)
    return Decimal(l**k)*Decimal(mt.exp(Decimal(-l)))/Decimal(mt.factorial(k))

def poisson_destrib(l,x):
    accum = Decimal(0);
    #prob_distr = [];
    for i in range(x):
       accum = accum + Decimal(poisson_dens(l,i));
       #prob_distr.append(accum);
    return accum;

# Отрисовка Формы

window=Tk();

btn=Button(window, text="Start", fg='blue');
btn.place(x=580, y=100);

txtfld=Entry(window, text="p", bd=5);
txtfld.place(x=580, y=150);

txtfld1=Entry(window, text="n", bd=5);
txtfld1.place(x=580, y=200);





# График
f = Figure(figsize=(5,5), dpi=100)
a = f.add_subplot(111)
a.plot([1,2,3,4,5,6,7,8],[5,6,1,3,8,9,3,5])

#Добавляем график на форму
canvas = FigureCanvasTkAgg(f, window);
canvas.get_tk_widget().grid(row=0, column=0);

# Параметры окна
window.title('Probobility distribution')
window.geometry("1000x600+10+20")

#Функции для кнопки
def graph(self=window):
    x = np.linspace(0,50);
    a = float(txtfld.get());
    b = float(txtfld1.get());
    y = [uniform_dens(a,b,h) for h in x]
    # График
    f = Figure(figsize=(5,5), dpi=100)
    a = f.add_subplot(111)
    a.plot(x,y)
    #Добавляем график на форму
    canvas = FigureCanvasTkAgg(f, window);
    canvas.get_tk_widget().grid(row=0, column=0);
    btn.bind('<Button-1>', graph);
    txtfld1.place(x=580, y=200);
    window.update();

def graph1(self=window):
    x = np.linspace(0,50);
    a = float(txtfld.get());
    b = float(txtfld1.get());
    y = [uniform_destrib(a,b,h) for h in x]
    # График
    f = Figure(figsize=(5,5), dpi=100)
    a = f.add_subplot(111)
    a.plot(x,y)
    #Добавляем график на форму
    canvas = FigureCanvasTkAgg(f, window);
    canvas.get_tk_widget().grid(row=0, column=0);
    btn.bind('<Button-1>', graph1);
    txtfld1.place(x=580, y=200);
    window.update();

def graph2(self=window):
    x = [i for i in range(50)];
    a = Decimal(txtfld.get());
    y = [poisson_dens(a,h) for h in x]
    # График
    f = Figure(figsize=(5,5), dpi=100)
    a = f.add_subplot(111)
    a.plot(x,y)
    #Добавляем график на форму
    canvas = FigureCanvasTkAgg(f, window);
    canvas.get_tk_widget().grid(row=0, column=0);
    btn.bind('<Button-1>', graph2);
    txtfld1.place_forget();
    window.update();

def graph3(self=window):
    x = [i for i in range(50)];
    a = Decimal(txtfld.get());
    y = [poisson_destrib(a,h) for h in x]
    # График
    f = Figure(figsize=(5,5), dpi=100)
    a = f.add_subplot(111)
    a.plot(x,y)
    #Добавляем график на форму
    canvas = FigureCanvasTkAgg(f, window);
    canvas.get_tk_widget().grid(row=0, column=0);
    btn.bind('<Button-1>', graph3);
    txtfld1.place_forget()
    window.update();

#События
btn.bind('<Button-1>', graph1)

main_menu = tk.Menu(window);
window.config(menu=main_menu);

first_item = Menu(main_menu);

main_menu.add_cascade(label='Равномерное распределение',menu=first_item)
first_item.add_command(label='Плотность',command=graph);
first_item.add_command(label='Функция распределения',command=graph1);

second_item = Menu(main_menu);
main_menu.add_cascade(label='Распределение Пуассона',menu=second_item)
second_item.add_command(label='Плотность',command=graph2);
second_item.add_command(label='Функция распределения',command=graph3);

# Запуск
window.mainloop()


