import time as t
import sys
import os
import cProfile
import numpy as np


def std_mul(m1,m2,a,b,c,cont):
    mr = cont;
    i = 0
    while i < a:
        j = 0
        while j < b:
            k = 0
            while k < c:
                mr[i][j] = mr[i][j] + m1[i][k]*m2[k][j];
                k = k + 1;
            j = j+1;
        i = i+1;
    return mr



def vinograd(m1,m2,a,b,c,cont):
    mr = cont;
    d = int(b/2);
    row = [];
    i = 0;
    while i < a:
        row.append(m1[i][0] * m1[i][1]);  
        j = 1;
        while j < d:
            row[i] = row[i] + m1[i][2*j] * m1[i][2*j+1];
            j = j+1;
        i = i + 1;

    i = 0;
    column = [];
    while i < c:
        column.append(m2[0][i] * m2[1][i]);  
        j = 1;
        while j < d:
            column[i] = column[i] + m2[2*j][i] * m2[2*j+1][i];
            j = j+1;
        i = i + 1;

    i = 0
    while i < a:
        j = 0
        while j < c:
            k = 0
            mr[i][j] = -row[i] - column[j];
            while k < d:
                mr[i][j] = mr[i][j] + (m1[i][2*k]+m2[2*k+1][j])*(m1[i][2*k+1] + m2[2*k][j]);
                k = k + 1;
            j = j+1;
        i = i+1;

    f = b % 2 
    if (f):
        i = 0
        while i < a:
            j = 0
            while j < c: 
                mr[i][j] = mr[i][j] + m1[i][b-1]*m2[b-1][j];
                j = j+1;
            i = i+1; 
    return mr


#Размерность
n = 100


mat1 = np.random.randint(0, 5, (n, n))
mat2 = np.random.randint(0, 5, (n, n))
matres = np.zeros((n,n))   


r1 = std_mul(mat1,mat2,n,n,n,matres)

matres = np.zeros((n,n))
r2 = vinograd(mat1,mat2,n,n,n,matres)


for i in range(7):
    if i == 0:
        continue;
    n=i*100;
    print(n)
    mat1 = np.random.randint(0, 5, (n, n))
    mat2 = np.random.randint(0, 5, (n, n))
    matres = np.zeros((n,n))
    cProfile.run('std_mul(mat1,mat2,n,n,n,matres)') 
    matres = np.zeros((n,n))
    cProfile.run('vinograd(mat1,mat2,n,n,n,matres)')


for i in range(7):
    if i == 0:
        continue;
    n=i*100+1;
    print(n)
    mat1 = np.random.randint(0, 5, (n, n))
    mat2 = np.random.randint(0, 5, (n, n))
    matres = np.zeros((n,n))
    cProfile.run('std_mul(mat1,mat2,n,n,n,matres)') 
    matres = np.zeros((n,n))
    cProfile.run('vinograd(mat1,mat2,n,n,n,matres)')
