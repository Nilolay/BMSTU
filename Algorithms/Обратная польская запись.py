from math import sin

statement = input()

tokens = list(statement)

print("Введенное выражение")
print(statement)

#Расправляемся с синусом

while 's' in tokens:
    
    ind = tokens.index('s')
    
    for i in range(3):
        tokens.pop(ind)
    tokens.insert(ind,'sin')


operations = ['+','-','*','/','sin']

dict_oper = {'+':0,
             '-':0,
             '*':1,
             '/':1,
             'sin':2}

opz = []
st_ops = []
pr_up = []

num = ''
prior = 0
for i in tokens:
    #print(num)
    if (i in operations):
        if num != '':   
            opz.append(num)
        num = ''
        if len(st_ops) != 0 and dict_oper[st_ops[-1]] + pr_up[-1] >= dict_oper[i] + prior:
            opz.append(st_ops.pop(-1))
            pr_up.pop(-1)
        st_ops.append(i)
        pr_up.append(prior)
    elif i == '(':
        prior += 5
    elif i == ')':
        prior -= 5
        opz.append(num)
        opz.append(st_ops.pop(-1))
        pr_up.pop(-1)
        num = ''
    else:
        num = num + i

if num != '':   
    opz.append(num)
        
st_ops.reverse()
opz = opz + st_ops

print("Обратная польская нотация")
print(opz)

#Вычисление выражения

accum = []

for i in opz:
    if i in operations:
        if i != 'sin':
            b = accum.pop(-1)
            a = accum.pop(-1)
            accum.append(eval(str(a)+i+str(b)))
        else:
            c = accum.pop(-1)
            accum.append(eval(str(i)+'('+str(c)+')'))
    else:
        accum.append(i)

print("Результат")
print(accum[0])
