import sys
import os
import shutil

current_dir = os.getcwd()
build_dir = ""
mode = sys.argv[1]

def start():
    global current_dir, build_dir, mode
    build_dir = current_dir + f"/bin/{mode}/net6.0/"
    print(f"Creating Creature Directories in the {mode} Directory...")
    os.mkdir(build_dir + "Output")
    os.makedirs(build_dir + "Creatures")
    os.mkdir(build_dir + "Creatures/Characters")
    os.mkdir(build_dir + "Creatures/Monsters")
    os.mkdir(build_dir + "Creatures/Bosses")
    copy_characters()
    copy_monsters()
    copy_bosses();

def copy_characters():
    global current_dir, build_dir, mode
    source = current_dir + "/Creatures/Characters/"
    destination = build_dir + "/Creatures/Characters/"
    all_files = os.listdir(source)
    for file in all_files:
        print(f"Copying {file} to the {mode} Build Directory...")
        shutil.copy(source + file, destination + file)

def copy_monsters():
    global current_dir, build_dir, mode
    source = current_dir + "/Creatures/Monsters/"
    destination = build_dir + "/Creatures/Monsters/"
    all_files = os.listdir(source)
    for file in all_files:
        if(file == "WEAKMONSTERS.txt" or file == "NORMALMONSTERS.txt" or file == "HARDMONSTERS.txt" or file == "RANDOMMONSTERS.txt"):
            print(f"Copying {file} to the {mode} Build Directory...")
            shutil.copy(source + file, destination + file)
        elif(len(file) >= 4):
            monster_id = file[:4]
            if(monster_id.isnumeric()):
                print(f"Copying {file} to the {mode} Build Directory...")
                shutil.copy(source + file, destination + file)

def copy_bosses():
    global current_dir, build_dir, mode
    source = current_dir + "/Creatures/Bosses/"
    destination = build_dir + "/Creatures/Bosses/"
    all_files = os.listdir(source)
    for file in all_files:
        print(f"\n\n{source}\n{destination}\n{file}\n\n")
        if(file == "BOSSLIST.txt" or file == "BOSSPROFILEPICPATHS.txt"):
            print(f"Copying {file} to the {mode} Build Directory...")
            shutil.copy(source + file, destination + file)
        elif(len(file) >= 4):
            boss_id = file[:4]
            if(boss_id.isnumeric()):
                print(f"Copying {file} to the {mode} Build Directory...")
                shutil.copy(source + file, destination + file)



start()