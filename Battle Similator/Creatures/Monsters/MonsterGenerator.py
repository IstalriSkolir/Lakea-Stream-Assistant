import random
import os

name = ""
level = 1
hp = 20
st = 9
de = 9
co = 9

print("\nMonster Generator\n")

def start():
    end = False
    while(end is False):
        enter_name_and_level()
        generate_abilities()
        global value
        value = get_numeric_input("'1' to save, '2' to clear: ")
        while value < 1 or value > 2:
            print(f"{value} is not valid input, please try again\n\n")
            value = get_numeric_input("'1' to keep, '2' to regenerate ability points")
        if(value == 1):
            save_monster_data()
        value = get_numeric_input("'1' to generate a new monster, '2' to exit: ")
        while value < 1 or value > 2:
            print(f"{value} is not valid input, please try again\n\n")
            value = get_numeric_input("'1' to keep, '2' to regenerate ability points")
        if(value == 2):
            end = True
        elif(value == 1):
            reset_abilities()

def enter_name_and_level():
    global name, level
    name = input("\nEnter monster name: ")
    level = get_numeric_input("Enter monster level: ")

def generate_abilities():
    global name, level, hp, st, de, co
    regen = True
    while regen:
        for lvl in range(level - 1):
            for point in range(2):
                ran = random.randint(1,3)
                match ran:
                    case 1:
                        st += 1
                    case 2:
                        de += 1
                    case 3:
                        co += 1
            co_mod = co // 3
            hp += random.randint(1, co_mod)
        print(f"NAME:{name}\nLEVEL:{level}\nHP:{hp}\nSTR:{st}\nDEX:{de}\nCON:{co}\n")
        global value
        value = get_numeric_input("'1' to keep, '2' to regenerate ability points: ")
        while value < 1 or value > 2:
            print(f"{value} is not valid input, please try again\n\nNAME:{name}\nLEVEL:{level}\nHP:{hp}\nSTR:{st}\nDEX:{de}\nCON:{co}\n")
            value = get_numeric_input("'1' to keep, '2' to regenerate ability points")
        if(value == 1):
            regen = False       
        elif(value == 2):
            hp = 20
            st = 9
            de = 9
            co = 9

def save_monster_data():
    global name, level, hp, st, de, co
    monster_id = get_monster_id()
    name = name.upper()
    file_name = monster_id + "-" + name
    update_monster_lists(file_name)
    with open(file_name + ".txt", 'w') as f:
        f.write(f"NAME:{name}\n")
        f.write(f"ID:{monster_id}\n")
        f.write(f"LEVEL:{level}\n")
        f.write(f"HP:{hp}\n")
        f.write(f"STR:{st}\n")
        f.write(f"DEX:{de}\n")
        f.write(f"CON:{co}")

def update_monster_lists(monster_file_name):
    global level
    list_file_name = ""
    if(level > 5 and level <= 15):
        list_file_name = "WEAKMONSTERS.txt"
    elif(level > 15 and level <= 30):
        list_file_name = "NORMALMONSTERS.txt"
    elif(level > 30 and level <= 50):
        list_file_name = "HARDMONSTERS.txt"
    else:
        list_file_name = "MISCMONSTERS.txt"
    if(os.path.exists(list_file_name)):
        file = open(list_file_name, "a")
        file.write("\n" + monster_file_name)
        file.close()
    else:
        with open(list_file_name, 'w') as f:
            f.write(monster_file_name)
    if(os.path.exists("RANDOMMONSTERS.txt")):
        file2 = open("RANDOMMONSTERS.txt", "a")
        file2.write("\n" + monster_file_name)
        file2.close()
    else:
        with open("RANDOMMONSTER.txt", 'w') as f:
            file2.write(monster_file_name)            

def get_monster_id():
    all_files = os.listdir()
    monster_files = []
    for string in all_files:
        monster_id = string[:4]
        if(monster_id.isnumeric()):
            monster_files.append(int(monster_id))
    list_length = len(monster_files)
    raw_id = str(list_length)
    if(len(raw_id) == 1):
        return "000" + raw_id
    elif(len(raw_id) == 2):
        return "00" + raw_id
    elif(len(raw_id) == 3):
        return "0" + raw_id

def reset_abilities():
    global hp, st, de, co
    hp = 20
    st = 9
    de = 9
    co = 9

def get_numeric_input(output):
    global value
    value = input(output)
    while not value.isnumeric():
        print(f"\n{value} is not a number, please try again\n")
        value = input(output)
    return int(value)

start()