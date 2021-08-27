import cProfile
import random as rand

graph = [[0,30,70,10,20],
         [30,0,20,50,10],
         [70,20,0,40,30],
         [10,50,40,0,30],
         [20,10,30,30,0]]

legacy = [[0,2,1,2,2],
          [2,0,4,3,1],
          [1,4,0,4,3],
          [2,3,4,0,2],
          [2,1,3,2,0]]

#Муравьиный алгоритм
def ant_method(graph,pher,days,Q=10,p=0.5):
    glob = 0
    while glob < days:
        black_list = [0]
        path = [0]
        #print(black_list)
        #Старт с 1 вершины
        cur = 0
        
        pre =    [[0,2,1,2,2],
                  [2,0,4,3,1],
                  [1,4,0,4,3],
                  [2,3,4,0,2],
                  [2,1,3,2,0]]

        i = 0
        while i < len(graph):
            j = 0
            while j < len(graph):
                if graph[i][j] == 0:
                    pre[i][j] = 0
                else:
                    pre[i][j] = legacy[i][j]/graph[i][j]
                j += 1
            i += 1

        while len(black_list) < len(graph):
            a = pre[cur].copy()
            prob = []
            prob_index = []
            itr = 0
            black_list.sort()
            for i in black_list:
                a.pop(i-itr)
                print(a)
                itr +=1

            poi = 0
            accum = 0
            while poi < len(pre[cur]):  
                if not (poi in black_list):
                    accum = accum + pre[cur][poi]/sum(a)
                    prob.append(accum)
                    prob_index.append(poi)
                else:
                    pass
                poi += 1

            coin = round(rand.uniform(0,100),2)    

            j = 0
            while j < len(prob):
                if coin < prob[j]:
                    cur = prob_index[j]
                    black_list.append(prob_index[j])
                    path.append(prob_index[j])
                    break;
                j += 1

        poi1 = 0
        dt = 0
        way = 0
        row = []
        col = []
        while poi1 < len(path):   
            if poi1 == 0:
                way = graph[0][path[poi1]]
            else:
                way = way + graph[path[poi1-1]][path[poi1]]
            poi1 +=1

        dt = Q/way

        i = 0
        while i < len(pher):
            j = 0
            while j < len(pher):
                pher[i][j] = pher[i][j]*(1-p)
                if pher[i][j] == 0:
                    pher[i][j] = 0.0001

                j += 1
            i += 1
        
        poi2 = 0
        while poi2 < len(path):   
            if poi2 == 0:
                pher[0][path[poi2]] = pher[0][path[poi2]] + dt
                pher[path[poi2]][0] = pher[path[poi2]][0] + dt
            else:
                pher[path[poi2-1]][path[poi2]] = pher[path[poi2-1]][path[poi2]] + dt
                pher[path[poi2]][path[poi2-1]] = pher[path[poi2]][path[poi2-1]] + dt
            poi2 +=1
    
        glob += 1    
    return path,pher

p, mat = ant_method(graph,legacy,15)


cProfile.run('ant_method(graph,legacy,20)')


#Алгоритм перебора находим все перестановки потом по ним суммы длин путей
def swap(l,a,b):
    tmp = l[a]
    l[a] = l[b]
    l[b] = tmp

def permute(a,l,r,result):
    if l==r:
        #print(a)
        result.append(a.copy())
    else:
        i = l
        while i <= r:
            swap(a,l, i)
            permute(a,l+1,r,result)
            swap(a,l, i)
            i += 1

def naive(graph):
    result = []
    vert = list(range(1,len(graph)+1))
    n = len(graph)-1
    permute(vert,1,n,result=result)
    lpath = []
    for i in result:
        j = 0
        accum = 0
        while j < 4:
            accum += graph[i[j]-1][i[j+1]-1]
            j += 1
        lpath.append(accum)
    path = result[lpath.index(min(lpath))]
    leng = min(lpath)
    return path,leng

naive(graph)

print("На матрице размерностью пять алгоритм перебора работает быстрее\nНо при увеличении размерности матрицы начинает проигрывать")
cProfile.run('naive(graph)')  
