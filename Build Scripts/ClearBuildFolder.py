import sys
import os
import shutil

current_dir = os.getcwd()

def start():
    app = sys.argv[1]
    mode = sys.argv[2]
    build_dir = ""
    if(app == "Lakea Stream Assistant" and mode == "Debug"):
        build_dir = current_dir + "/../Lakea Stream Assistant/bin/Debug/net6.0"
    elif(app == "Lakea Stream Assistant" and mode == "Release"):
        build_dir = current_dir + "/../Lakea Stream Assistant/bin/Release/net6.0"
    elif(app == "Battle Similator" and mode == "Debug"):
        build_dir = current_dir + "/../Battle Similator/bin/Debug/net6.0"
    elif(app == "Battle Similator" and mode == "Release"):
        build_dir = current_dir + "/../Battle Similator/bin/Release/net6.0"
    print(f"Clearing Directory {build_dir}")
    shutil.rmtree(build_dir)



start()