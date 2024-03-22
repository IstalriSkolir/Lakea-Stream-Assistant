import os
import sys
import random

CHARACTER_FOLDER = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Characters\\"
#CHARACTER_FOLDER = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Characters\\"

def start():
    account_id = sys.argv[1]
    account_name = sys.argv[2]
    level_ups = int(sys.argv[3])
    character_dict = get_character_info(CHARACTER_FOLDER + account_id + ".txt")
    char = character(character_dict, account_id, account_name)
    for x in range(level_ups):
        next_level = calculate_next_level(char.level)
        char = character_level_up(char, next_level)
    save_character_data(char, CHARACTER_FOLDER + account_id + ".txt")

def get_character_info(character_path):
    character_dict = {}
    if(os.path.isfile(character_path)):
        reader = open(character_path)
        lines = reader.read().splitlines()
        reader.close()
        for line in lines:
            parts = line.split(":", 1)
            character_dict.update({parts[0]: parts[1]})
    else:
        character_dict.update({"ERROR": "NOCHARACTERFILE"})
    return character_dict

def calculate_next_level(current_level):
    next_level = 0
    for x in range(current_level):
        next_level += (x + 1) * 30
    return next_level

def character_level_up(char, next_level):
    char.xp = next_level
    char.level += 1
    if char.level <= 100:
        for x in range(2):
            ran = random.randint(1,3)
            match ran:
                case 1:
                    char.st += 1
                case 2:
                    char.de += 1
                case 3:
                    char.co += 1
        co_mod = char.co // 3
        char.hp += random.randint(1, co_mod)
    return char

def save_character_data(char, path):
    writer = open(path, "w")
    writer.write("NAME:" + char.name + "\n")
    writer.write("ID:" + str(char.id) + "\n")
    writer.write("LEVEL:" + str(char.level) + "\n")
    writer.write("XP:" + str(char.xp) + "\n")
    writer.write("HP:" + str(char.hp) + "\n")
    writer.write("STR:" + str(char.st) + "\n")
    writer.write("DEX:" + str(char.de) + "\n")
    writer.write("CON:" + str(char.co) + "\n")
    writer.write("DEATHS:" + char.deaths + "\n")
    writer.write("MONSTERS_KILLED:" + char.monsters_killed + "\n")
    writer.write("BOSSES_FOUGHT:" + char.bosses_fought + "\n")
    writer.write("BOSSES_BEATEN:" + char.bosses_beaten + "\n")
    writer.write("MONSTER_WIN_RATE:" + char.monster_win_rate + "\n")
    writer.write("PRESTIGE:" + char.prestige + "\n")
    writer.close()

class character:
    name = ""
    id = ""
    level = -1
    xp = -1
    hp = -1
    st = -1
    de = -1
    co = -1
    deaths = ""
    monsters_killed = ""
    bosses_fought = ""
    bosses_beaten = ""
    monster_win_rate = ""
    prestige = ""

    def __init__(self, character_dict, account_id, account_name) -> None:
        if not ("ERROR" in character_dict):
            self.name = character_dict["NAME"]
            self.id = character_dict["ID"]
            self.level = int(character_dict["LEVEL"])
            self.xp = int(character_dict["XP"])
            self.hp = int(character_dict["HP"])
            self.st = int(character_dict["STR"])
            self.de = int(character_dict["DEX"])
            self.co = int(character_dict["CON"])
            self.deaths = character_dict["DEATHS"]
            self.monsters_killed = character_dict["MONSTERS_KILLED"]
            self.bosses_fought = character_dict["BOSSES_FOUGHT"]
            self.bosses_beaten = character_dict["BOSSES_BEATEN"]
            self.monster_win_rate = character_dict["MONSTER_WIN_RATE"]
            self.prestige = character_dict["PRESTIGE"]
        else:
            self.name = account_name
            self.id = account_id
            self.level = 1
            self.xp = 0
            self.hp = 20
            self.st = 9
            self.de = 9
            self.co = 9
            self.deaths = "0"
            self.monsters_killed = "0"
            self.bosses_fought = "0"
            self.bosses_beaten = "0"
            self.monster_win_rate = "0"
            self.prestige = "0"

start()