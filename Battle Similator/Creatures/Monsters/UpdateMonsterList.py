import os

weak_monsters = []
normal_monsters = []
hard_monsters = []
misc_monsters = []
all_monsters = []

def start():
    all_files = os.listdir()
    for file in all_files:
        monster_id = file[:4]
        if(monster_id.isnumeric()):
            print(f"Sorting {file[:-4]}")
            monster_strength = monster_file(file)
            add_to_lists(file[:-4], monster_strength)
    save_monster_files()     

def monster_file(file):
    lines = open(file).readlines()
    level = 0
    for string in lines:
        parts = string.split(":")
        if(parts[0] == "LEVEL" and parts[1].isnumeric):
            level = int(parts[1])
    if(level > 5 and level <= 15):
        return "WEAK"
    elif(level > 15 and level <= 30):
        return "NORMAL"
    elif(level > 30 and level <= 50):
        return "HARD"
    else:
        return "MISC"
    
def add_to_lists(monster, strength):
    global weak_monsters, normal_monsters, hard_monsters, misc_monsters, all_monsters
    all_monsters.append(monster)
    match(strength):
        case "WEAK":
            weak_monsters.append(monster)
        case "NORMAL":
            normal_monsters.append(monster)
        case "HARD":
            hard_monsters.append(monster)
        case "MISC":
            misc_monsters.append(monster)

def save_monster_files():
    global weak_monsters, normal_monsters, hard_monsters, misc_monsters, all_monsters
    weak_file = open("WEAKMONSTERS.txt", "w")
    for index in range(len(weak_monsters)):
        print(f"Weak Monster: {weak_monsters[index]}")
        if(index == 0):
            weak_file.write(weak_monsters[index])
        else:
            weak_file.write("\n" + weak_monsters[index])
    weak_file.close()
    normal_file = open("NORMALMONSTERS.txt", "w")
    for index in range(len(normal_monsters)):
        print(f"Normal Monster: {normal_monsters[index]}")
        if(index == 0):
            normal_file.write(normal_monsters[index])
        else:
            normal_file.write("\n" + normal_monsters[index])
    normal_file.close()
    hard_file = open("HARDMONSTERS.txt", "w")
    for index in range(len(hard_monsters)):
        print(f"Hard Monster: {hard_monsters[index]}")
        if(index == 0):
            hard_file.write(hard_monsters[index])
        else:
            hard_file.write("\n" + hard_monsters[index])
    hard_file.close()
    misc_file = open("MISCMONSTERS.txt", "w")
    for index in range(len(misc_monsters)):
        print(f"Misc Monster: {misc_monsters[index]}")
        if(index == 0):
            misc_file.write(misc_monsters[index])
        else:
            misc_file.write("\n" + misc_monsters[index])
    misc_file.close()
    all_file = open("RANDOMMONSTERS.txt", "w")
    for index in range(len(all_monsters)):
        print(f"Random Monster: {all_monsters[index]}")
        if(index == 0):
            all_file.write(all_monsters[index])
        else:
            all_file.write("\n" + all_monsters[index])
    all_file.close()



start()