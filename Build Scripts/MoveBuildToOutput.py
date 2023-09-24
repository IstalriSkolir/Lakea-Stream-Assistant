import os
import shutil

current_dir = os.getcwd()

def start():
    print("Moving Build Folder to Output Directory...")
    version = get_project_version()
    source = current_dir + "/../Lakea Stream Assistant/bin/Debug/net6.0"
    destination = current_dir + "/../Output/Lakea Stream Assistant " + version
    if(os.path.isdir(destination)):
        shutil.rmtree(destination)
    shutil.copytree(source, destination)

def get_project_version():
    version_source = current_dir + "/../Lakea Stream Assistant/VersionIncrementer.cs"
    lines = []
    with open (version_source) as f:
        lines = f.readlines()
    assembly_line = ""
    for line in lines:
        if("AssemblyVersion" in line):
            assembly_line = line
    parts = assembly_line.split("\"")
    numbers = parts[1].split(".")
    version = numbers[0] + "." + numbers[1] + "." + numbers[2]
    return version



start()