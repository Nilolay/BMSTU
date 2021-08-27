class raz_matrix:
        
    def __init__(self):
        self.M = 0
        self.N = 0
        self.AN = [];
        self.IC = [];
        self.JR = [];


    def pack(self,mat,n,m):
        i = 0;
        cnt = 0;
        self.M = m;
        self.N = n;
        while i < m:
            j = 0;
            k = 0;
            while j < n:
                if mat[i][j] !=0:
                    if k ==0:
                        k = 1;
                        self.JR.append(cnt);
                    self.AN.append(mat[i][j]);
                    self.IC.append(j);
                    cnt = 1 + cnt;
                j = j +1;
            i = i+1;
            if k ==0:
                self.JR.append(cnt); 
        self.JR.append(cnt);
        
    def unpack(self):
        res = []
        i = 0;
        while i < self.M:
            j = 0;
            cur1 = 0;
            col = [];
            an,al = self.get_row(i);
            while j < self.N:
                if cur1 >= len(an):
                    col.append(0);
                    j = j + 1;
                    continue;
                if j < al[cur1]:
                    col.append(0);
                if j == al[cur1]:
                    col.append(an[cur1]);
                    cur1 = cur1 + 1;
                j = j + 1;
            res.append(col);
            i = i+1;
        return res;
     
    def initialize(self,BN,BC,JR,m,n):
        self.M = m;
        self.N = n;
        self.AN = BN;
        self.IC = BC;
        self.JR = JR;
    
    def show_matrix(self):
        print (self.AN)
        print (self.IC)
        print (self.JR)
    
    def get_row(self,i):
        row = [];
        lnk = [];
        ind = self.JR[i];
        while ind < len(self.AN) and ind < self.JR[i+1]:
            row.append(self.AN[ind]);
            lnk.append(self.IC[ind]);
            ind = ind + 1;
        return row, lnk;
    
    def get_column(self,a):
        column = [];
        lnk = [];
        i = 0;
        
        while i < len(self.AN):
            j = 0;
            if self.IC[i] == a:
                column.append(self.AN[i]);
                while self.JR[j] <= i:
                    j = j + 1;
                lnk.append(j-1);
            i = i + 1;
        return column, lnk;
    
    def __add__(self, mat):
        i = 0;
        cn = [];
        cl = [];
        jr = [];
        while i < self.M:
            j = 0;
            k = 0;
            an, al = self.get_row(i);
            bn, bl = mat.get_row(i);
            cur1 = 0;
            cur2 = 0;
            while cur1 < len(an) and cur2 < len(bn):
                if al[cur1]==bl[cur2]:
                    if an[cur1]+bn[cur2] == 0:
                        cur1 = cur1 + 1;
                        cur2 = cur2 + 1;
                        continue;
                    else:
                        cn.append(an[cur1]+bn[cur2]);
                        cl.append(al[cur1]);
                        cur1 = cur1 + 1;
                        cur2 = cur2 + 1;
                        if k==0:
                            k=1;
                            jr.append(len(cn)-1);
                        continue;
                if al[cur1]<bl[cur2]:
                    cn.append(an[cur1]);
                    cl.append(al[cur1]);
                    if k==0:
                            k=1;
                            jr.append(len(cn)-1);
                    cur1 = cur1 + 1;
                    continue;
                if al[cur1]>bl[cur2]:
                    cn.append(an[cur2]);
                    cl.append(al[cur2]);
                    if k==0:
                            k=1;
                            jr.append(len(cn)-1);
                    cur2 = cur2 + 1;
                    continue;
            if cur1 < len(an) and not (cur2 < len(bn)):
                while cur1 < len(an):
                    cn.append(an[cur1]);
                    cl.append(al[cur1]);
                    if k==0:
                            k=1;
                            jr.append(len(cn)-1);
                    cur1 = cur1 + 1;
            if not (cur1 < len(an)) and cur2 < len(bn):
                while cur2 < len(bn):
                    cn.append(bn[cur2]);
                    cl.append(bl[cur2]);
                    if k==0:
                            k=1;
                            jr.append(len(cn)-1);
                    cur2 = cur2 + 1;
            if k ==0:
                jr.append(len(cn));
            i = i+1;
        jr.append(len(cn));
        result = raz_matrix();
        result.initialize(cn,cl,jr,self.M,self.N);
        return result;    
    
    def __mul__(self, mat):
        i = 0;
        cn = [];
        cl = [];
        jr = [];
        while i < self.M:
            j = 0;
            k = 0;
            while j < self.N:
                an,al = self.get_row(i);
                bn,bl = mat.get_column(j);
                cur1 = 0;
                cur2 = 0;
                res = 0;
                while cur1 < len(an) and cur2 < len(bn):
                    if al[cur1]==bl[cur2]:
                        res = res + an[cur1]*bn[cur2];
                        print(res);
                        cur1 = cur1 + 1;
                        cur2 = cur2 + 1;
                        continue;
                    if al[cur1]<bl[cur2]:
                        cur1 = cur1 + 1;
                        continue;
                    if al[cur1]>bl[cur2]:
                        cur2 = cur2 + 1;
                        continue;
                if res != 0:
                    cn.append(res);
                    cl.append(j);
                    if k == 0:
                        k=1;
                        jr.append(len(cn)-1);
                j = j + 1;
            if k ==0:
                jr.append(len(cn));
            i = i+1;
        jr.append(len(cn));
        result = raz_matrix();
        result.initialize(cn,cl,jr,self.M,self.N);
        return result;  
    #def sum(self,mat,n,m):



m = raz_matrix()
ma = raz_matrix()


m1 = [[0,0,3],[2,1,0],[1,0,0]]
m2 = [[0,0,-3],[1,0,3],[1,0,0]]
m.pack(m1,3,3)
ma.pack(m2,3,3)
m.show_matrix()
ma.show_matrix()

res = m + ma;
res1 = m*ma

res.show_matrix()
res1.show_matrix()
