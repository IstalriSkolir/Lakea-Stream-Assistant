import sys

def start():
    message = "TEST COMPLETE:"
    if(len(sys.argv) > 0):
        for arg in sys.argv:
            message = message + " " + arg
    with open("TEST.txt", 'w') as f:
        f.write(message)

start()