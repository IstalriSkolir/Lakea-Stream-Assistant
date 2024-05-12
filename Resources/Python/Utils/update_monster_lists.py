from Utils.monster_battle_classes import monster
import os

MONSTER_FOLDER = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Monsters\\"

WEAK_MONSTERS = []
NORMAL_MONSTERS = []
HARD_MONSTERS = []
ALL_MONSTERS = []

def update_monster_lists():
    all_files = os.listdir(MONSTER_FOLDER)
    for file in all_files:
        monster_id = file[:4]
        if monster_id.isnumeric():
            mon = monster(file[:-4])
            add_to_list(mon, file[:-4])
    save_monster_files()

def add_to_list(mon, file_name):
    global WEAK_MONSTERS, NORMAL_MONSTERS, HARD_MONSTERS, ALL_MONSTERS
    match mon.rating:
        case "WEAK":
            WEAK_MONSTERS.append(file_name)
        case "NORMAL":
            NORMAL_MONSTERS.append(file_name)
        case "HARD":
            HARD_MONSTERS.append(file_name)
        case _:
            pass
    ALL_MONSTERS.append(file_name)

def save_monster_files():
    weak_file = open("{MONSTER_FOLDER}WEAKMONSTERS.txt", "w")
    for index in range(len(WEAK_MONSTERS)):
        print(f"Weak Monster: {WEAK_MONSTERS[index]}")
        if(index == 0):
            weak_file.write(WEAK_MONSTERS[index])
        else:
            weak_file.write("\n" + WEAK_MONSTERS[index])
    weak_file.close()
    normal_file = open("{MONSTER_FOLDER}NORMALMONSTERS.txt", "w")
    for index in range(len(NORMAL_MONSTERS)):
        print(f"Normal Monster: {NORMAL_MONSTERS[index]}")
        if(index == 0):
            normal_file.write(NORMAL_MONSTERS[index])
        else:
            normal_file.write("\n" + NORMAL_MONSTERS[index])
    normal_file.close()
    hard_file = open("{MONSTER_FOLDER}HARDMONSTERS.txt", "w")
    for index in range(len(HARD_MONSTERS)):
        print(f"Hard Monster: {HARD_MONSTERS[index]}")
        if(index == 0):
            hard_file.write(HARD_MONSTERS[index])
        else:
            hard_file.write("\n" + HARD_MONSTERS[index])
    hard_file.close()
    all_file = open("{MONSTER_FOLDER}RANDOMMONSTERS.txt", "w")
    for index in range(len(ALL_MONSTERS)):
        print(f"Random Monster: {ALL_MONSTERS[index]}")
        if(index == 0):
            all_file.write(ALL_MONSTERS[index])
        else:
            all_file.write("\n" + ALL_MONSTERS[index])
    all_file.close()