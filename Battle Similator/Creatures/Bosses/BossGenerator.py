import random
import os

name = ""
level = 1
hp = 20
st = 9
de = 9
co = 9

print("\nBoss Generator\n")

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
            save_boss_data()
        value = get_numeric_input("'1' to generate a new Boss, '2' to exit: ")
        while value < 1 or value > 2:
            print(f"{value} is not valid input, please try again\n\n")
            value = get_numeric_input("'1' to keep, '2' to regenerate ability points")
        if(value == 2):
            end = True
        elif(value == 1):
            reset_abilities()

def enter_name_and_level():
    global name, level
    name = input("\nEnter Boss name: ")
    level = get_numeric_input("Enter Boss level: ")

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

def save_boss_data():
    global name, level, hp, st, de, co
    boss_id = get_boss_id()
    name = name.upper()
    file_name = boss_id + "-" + name
    update_boss_lists(file_name)
    with open(file_name + ".txt", 'w') as f:
        f.write(f"NAME:{name}\n")
        f.write(f"ID:{boss_id}\n")
        f.write(f"LEVEL:{level}\n")
        f.write(f"HP:{hp}\n")
        f.write(f"STR:{st}\n")
        f.write(f"DEX:{de}\n")
        f.write(f"CON:{co}")

def update_boss_lists(boss_file_name):
    if(os.path.exists("BOSSLIST.txt")):
        file = open("BOSSLIST.txt", "a")
        file.write("\n" + boss_file_name)
        file.close()
    else:
        with open("BOSSLIST.txt", "w") as f:
            f.write(boss_file_name)          

def get_boss_id():
    all_files = os.listdir()
    boss_files = []
    for string in all_files:
        boss_id = string[:4]
        if(boss_id.isnumeric()):
            boss_files.append(int(boss_id))
    list_length = len(boss_files)
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