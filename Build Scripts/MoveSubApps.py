import shutil
import sys
import os

def start():
    mode = sys.argv[1]
    current_dir = os.getcwd()
    source = current_dir
    destination = current_dir
    if(mode == "Debug"):
        source = current_dir + "/../Battle Similator/bin/Debug/net6.0"
        destination = current_dir + "/../Lakea Stream Assistant/bin/Debug/net6.0/Applications/Battle Simulator"
    elif(mode == "Release"):
        source = current_dir + "/../Battle Similator/bin/Release/net6.0"
        destination = current_dir + "/../Lakea Stream Assistant/bin/Release/net6.0/Applications/Battle Simulator"
    shutil.copytree(source, destination)



start()