#Подключаем библиотеки

#Формы
from tkinter import *
import tkinter as tk
from tkinter import messagebox
# Графики
import matplotlib
import math as mt
matplotlib.use("TkAgg")
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg, NavigationToolbar2Tk
from matplotlib.figure import Figure
import numpy as np





#Создаем окно
window = tk.Tk();

# График
f = Figure(figsize=(5,5), dpi=100)
que_din = f.add_subplot(111)
que_din.plot([1,2,3,4,5,6,7,8],[5,6,1,3,8,9,3,5])

#Добавляем график на форму
canvas = FigureCanvasTkAgg(f, window);
canvas.get_tk_widget().grid(row=1, column=1);

#Ввод значений
input_parameters = tk.LabelFrame(window, text="Параметры",width=50,height=50);

txt1 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Кол-во итераций' ,relief='ridge')
txt1.grid(row=0,column=0);
eiter = tk.Entry(input_parameters,width=10);
eiter.grid(row=0,column=1);

txt2 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Шаг по времени' ,relief='ridge');
txt2.grid(row=1,column=0);
etd = tk.Entry(input_parameters,width=10);
etd.grid(row=1,column=1);

txt3 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Генерация заявок: a' ,relief='ridge');
txt3.grid(row=2,column=0);
ea = tk.Entry(input_parameters,width=10);
ea.grid(row=2,column=1);

txt4 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Генерация заявок: b' ,relief='ridge');
txt4.grid(row=3,column=0);
eb = tk.Entry(input_parameters,width=10);
eb.grid(row=3,column=1);

txt5 = tk.Label(input_parameters,width=32,height=1,bg='#ffffff',text='Лямбда (Пуассон) обслуживание заявок' ,relief='ridge');
txt5.grid(row=4,column=0);
el = tk.Entry(input_parameters,width=10);
el.grid(row=4,column=1);

result = tk.LabelFrame(window,width=50,height=50);
txt6 = tk.Label(result,width=32,height=1,bg='#ffffff',text='Предельный объем очереди' ,relief='ridge');
txt7 = tk.Label(result,width=32,height=1,bg='#ffffff',text=' ' );
txt6.grid(row=0,column=0);
txt7.grid(row=1,column=0);
result.grid(row=0,column=1);


input_parameters.grid(row=0,column=0);

def solve(td=1):
    try:
        td = float(etd.get()); #шаг по времени
    except:
        td = 1; 
    try:
        #Стартовые значения
        que = 0; #Очередь
       
        ch_flg = 0; # флаг загруженности канала


        
        iter = int(eiter.get()); #Количество итераций

        #Для равномерного распределения
        a = float(ea.get());
        b = float(eb.get());
        #Для пуассона
        l = float(el.get());

    except:
        answer = messagebox.showerror(title="Ошибка", message="Данные введены некорректно!")
    i = 0;
    max_que = 0;
    que_list = [];
    while(i < iter):
        # Генерация заявки и отправка ее в очередь или в канал обслуживания
        new_apps = round(np.random.uniform(a,b)*td);

    
        if ch_flg == 0 and new_apps > 0:
             ch_flg +=1;
             que +=new_apps-1;
             if que > max_que:
                 max_que = que;
        else:
             que +=new_apps;
             
             if que > max_que:
                 max_que = que;

        productivity = round(np.random.poisson(l)*td);
        #Обслужили ли заявку
    
        if ch_flg == 1 and productivity > 0:
            ch_flg -=1;
            if que > 0 and que > productivity-1:
                que -=productivity-1;
            else:
                que = 0;
 

        #Перенос заявки из очереди в канал
        if ch_flg == 0 and que > 0:
            ch_flg +=1;
            que -=1;

        que_list.append(que);
        i +=1;

    f = Figure(figsize=(5,5), dpi=100);
    que_din = f.add_subplot(111);
    que_din.plot(que_list);
    canvas = FigureCanvasTkAgg(f, window);
    canvas.get_tk_widget().grid(row=1, column=1);
    window.update();
    txt7['text'] = max_que;

bt = tk.Button(window, width=10, height=2, text ="Рассчитать", command=solve); 
bt.grid(row=1,column=0);






#Запуск формы
window.mainloop();
