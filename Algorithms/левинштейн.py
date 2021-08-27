def lev_distance(s1, s2):
    n, m = len(s1), len(s2)
    if n > m:
        s1, s2 = s2, s1
        n, m = m, n

    current_row = range(n + 1) 
    for i in range(1, m + 1):
        previous_row, current_row = current_row, [i] + [0] * n
        for j in range(1, n + 1):
            add, delete, change = previous_row[j] + 1, current_row[j - 1] + 1, previous_row[j - 1]
            if s1[j - 1] != s2[i - 1]:
                change += 1
            current_row[j] = min(add, delete, change)
        print(current_row)
    return current_row[n]


def dalev_distance(s1, s2):
    n, m = len(s1), len(s2)
    if n > m:
        s1, s2 = s2, s1
        n, m = m, n

    current_row = range(n + 1) 
    for i in range(1, m + 1):
        previous_row, current_row = current_row, [i] + [0] * n
        for j in range(1, n + 1):
            add, delete, change = previous_row[j] + 1, current_row[j - 1] + 1, previous_row[j - 1]
            
            if s1[j - 1] != s2[i - 1]:
                change += 1
            
            if (i < m) and (j < n) and s2[i]==s1[j-1] and s2[i-1] == s1[j] and (s2[i] != s1[j]):
                change -= 1
                
            current_row[j] = min(add, delete, change)
        print(current_row)
    return current_row[n]
