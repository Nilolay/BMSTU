# Fog! 
 
import sys, time 
from math import * 
from OpenGL.GL import * 
from OpenGL.GLU import * 
from OpenGL.GLUT import * 
 
viewRotX = 0 
viewRotY = 0 
viewDistance = 25 


def draw():  
    glClearColor(0.5, 0.8, 1.0, 0.0)  
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)  
    glEnable(GL_DEPTH_TEST)    
    glMatrixMode(GL_PROJECTION)  
    glLoadIdentity()  
    gluPerspective(50.0, 1.75, 1.0, 100.0)  
    glMatrixMode(GL_MODELVIEW) 
 
    glLoadIdentity()    
    glTranslatef(0.0, 0.0, -viewDistance)  
    glRotatef(viewRotX, 1.0, 0.0, 0.0)  
    glRotatef(viewRotY, 0.0, 1.0, 0.0)    
    glEnable(GL_FOG)  
    glFogi(GL_FOG_MODE, GL_LINEAR)  
    glFogfv(GL_FOG_COLOR, [0.5, 0.8, 1, 0])  
    glFogf(GL_FOG_START, 5.0)  
    glFogf(GL_FOG_END, 40.0) 
 
    defineLight()  
    glEnable(GL_LIGHTING)  
    global floorMeshDList  
    glCallList(floorMeshDList)  
    drawTeapot()  
    drawPedestal()  
    drawBall()  
    glDisable(GL_LIGHTING) 
 
    glutSwapBuffers()


def defineLight():  
    glEnable(GL_LIGHT0)  
    glLightfv(GL_LIGHT0, GL_DIFFUSE, [1, 1, 1, 1])  
    glLightfv(GL_LIGHT0, GL_POSITION, [-1, 2, 1])
    

def drawFloorMesh(columns,rows):  
    green = [ 0, 0.8, 0.1, 1 ]  
    black = [ 0, 0, 0, 1 ]  
    glMaterialfv(GL_FRONT, GL_AMBIENT, green)  
    glMaterialfv(GL_FRONT, GL_DIFFUSE, green)  
    glMaterialfv(GL_FRONT, GL_SPECULAR, black)  
    glNormal3f(0.0, 1.0, 0.0)  
    for j in range(0,rows):   
        glBegin(GL_TRIANGLE_STRIP)   
        for i in range(0,columns):    
            x = ((float(i)) / columns) * 40.0 - 20.0    
            z = ((float(j)) / rows) * 40.0 - 20.0    
            glVertex3f(x, -2.0, z)    
            z = ((float(j)+1) / rows) * 40.0 - 20.0    
            glVertex3f(x, -2.0, z)   
        glEnd()


def drawTeapot():  
    red = [ 0.8, 0, 0, 1 ]  
    white = [ 1, 1, 1, 1 ]  
    glMaterialfv(GL_FRONT, GL_AMBIENT, red)  
    glMaterialfv(GL_FRONT, GL_DIFFUSE, red)  
    glMaterialfv(GL_FRONT, GL_SPECULAR, white)  
    glMaterialf(GL_FRONT, GL_SHININESS, 90.0)  
    glPushMatrix()  
    glTranslatef(2.0, 4.0, -1.0)  
    glRotatef(currentTime() * 30.0, 0.0, 1.0, 0.0)  
    global teapotDList  
    glCallList(teapotDList)  
    glPopMatrix() 



quadric = None 
def drawPedestal():  
    global quadric  
    grey = [ 0.6, 0.6, 0.6, 1 ]  
    white = [ 1, 1, 1, 1 ]  
    if not quadric:   
        quadric = gluNewQuadric()  
    glMaterialfv(GL_FRONT, GL_AMBIENT, grey)  
    glMaterialfv(GL_FRONT, GL_DIFFUSE, grey)  
    glMaterialfv(GL_FRONT, GL_SPECULAR, white)  
    glMaterialf(GL_FRONT, GL_SHININESS, 30.0) 

    glPushMatrix()  
    glTranslatef(2.0, -2.0, -1.0)  
    glRotatef(-90.0, 1.0, 0.0, 0.0)  
    gluCylinder(quadric, 4.0, 4.0, 4.0, 16, 1)  
    glTranslatef(0.0, 0.0, 4.0)  
    gluDisk(quadric, 0.0, 4.0, 16, 1)  
    glPopMatrix()


def drawBall():  
    cyan = [ 0, 1, 1, 1 ]  
    black = [ 0, 0, 0, 1 ]  
    glMaterialfv(GL_FRONT, GL_AMBIENT, cyan)  
    glMaterialfv(GL_FRONT, GL_DIFFUSE, cyan)  
    glMaterialfv(GL_FRONT, GL_SPECULAR, black)  
    glPushMatrix()  
    glTranslatef(-10.0, fabs(sin(currentTime())) * 3.0, -15.0)  
    glutSolidSphere(2.0, 16, 8)  
    glPopMatrix() 
 
 
def drawBitmapString(text, font=GLUT_BITMAP_TIMES_ROMAN_24):  
    for c in text:   
        glutBitmapCharacter(font, ord(c)) 


startTime = time.time() 

def currentTime():  
    return time.time() - startTime 
 
 
def keyboard(k, x, y):  
    global viewDistance  
    if k == b'\x1b':   
        sys.exit(0)  
    elif k == b'm':   
        viewDistance -= 1  
    elif k == b'n':   
        viewDistance += 1 



def specialkey(k, x, y):  
    global viewRotX, viewRotY, viewDistance 
    if k == GLUT_KEY_LEFT:   
        viewRotY += 3  
    elif k == GLUT_KEY_RIGHT:   
        viewRotY -= 3  
    elif k == GLUT_KEY_UP:   
        viewRotX += 3  
    elif k == GLUT_KEY_DOWN:   
        viewRotX -= 3
    


glutInit([]) 
glutInitDisplayMode(GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH) 
glutInitWindowSize(700,400) 
glutCreateWindow(b"fog")  #sys.argv[0]
glutDisplayFunc(draw) 
glutKeyboardFunc(keyboard) 
glutSpecialFunc(specialkey) 
glutIdleFunc(glutPostRedisplay) 
glEnable(GL_DEPTH_TEST) 
glEnable(GL_LIGHTING) 
glEnable(GL_LIGHT0) 
 
floorMeshDList = glGenLists(1) 
glNewList(floorMeshDList, GL_COMPILE) 
drawFloorMesh(100,100) 
glEndList() 
 
teapotDList = glGenLists(1) 
glNewList(teapotDList, GL_COMPILE) 
glutSolidTeapot(2) 
glEndList() 
 
glutMainLoop()