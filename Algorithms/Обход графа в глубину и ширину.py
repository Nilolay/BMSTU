from collections import deque

n = int(input('Введите кол-во вершин'))
m = int(input('Введите кол-во ребер'))
graph = {i:set() for i in range(n)}
print('Введите ребра - вершина1 пробел вершина2')
for i in range(m):
    ver0,ver1 = map(int,input().split())
    graph[ver0].add(ver1)
    graph[ver1].add(ver0)


def bfs(graph):

    start = 0
    dist[start] = 0
    que = deque([start])
    visited = [0]

    while que:
        cur = que.popleft()
        for nvert in graph[cur]:
            if nvert not in visited:
                visited.append(nvert)
                que.append(nvert)
    return visited



def dfs(graph, node, visited):
    if node not in visited:
        visited.append(node)
        for n in graph[node]:
            dfs(graph,n, visited)
    return visited

bfs_visited = bfs(graph)
dfs_visited = dfs(graph,0, [])

print('Обход графа в ширину',bfs_visited)
print('Обход графа в глубину',dfs_visited)
