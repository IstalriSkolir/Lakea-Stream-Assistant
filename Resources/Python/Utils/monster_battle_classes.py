import os
import random

#BOSS_PATH = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Bosses\\"
BOSS_PATH = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Bosses\\"

#MONSTER_PATH = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Monsters\\"
MONSTER_PATH = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Monsters\\"

#CHARACTER_PATH = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Characters\\"
CHARACTER_PATH = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Characters\\"

def get_monster_dict_by_id():
    monster_dict = {}
    for file in os.listdir(MONSTER_PATH):
        mon = monster(file)
        monster_dict.update({mon.id: mon})
    return monster_dict

def get_monster_dict_by_name():
    monster_dict = {}
    for file in os.listdir(MONSTER_PATH):
        mon = monster(file)
        monster_dict.update({mon.name: mon})
    return monster_dict

def get_character_dict():
    character_dict = {}
    for file in os.listdir(CHARACTER_PATH):
        cha = character(file)
        character_dict.update({cha.id: cha})
    return character_dict

class boss:
    name = ""
    id = ""
    level = -1
    hp = -1
    hp_max = -1
    st = -1
    de = -1
    co = -1
        
    def __init__(self, boss):
        file = open(f"{BOSS_PATH}{boss}.txt")
        lines = file.read().splitlines()
        for line in lines:
            parts = line.split(':', 1)
            match parts[0]:
                case "NAME":
                    self.name = parts[1]
                case "ID":
                    self.id = parts[1]
                case "LEVEL":
                    self.level = int(parts[1])
                case "CURRENT_HP":
                    self.hp = int(parts[1])
                case "MAX_HP":
                    self.hp_max = int(parts[1])
                case "MAX_HP":
                    self.hp_max = int(parts[1])
                case "STR":
                    self.st = int(parts[1])
                case "DEX":
                    self.de = int(parts[1])
                case "CON":
                    self.co = int(parts[1])
                case _:
                    pass

class monster:
    name = ""
    id = ""
    level = -1
    hp = -1
    hp_max = -1
    st = -1
    de = -1
    co = -1
    rating = ""

    def __init__(self, monster):
        file = {}
        if monster[-4:] == ".txt":
            file = open(f"{MONSTER_PATH}{monster}")
        else:
            file = open(f"{MONSTER_PATH}{monster}.txt")
        lines = file.read().splitlines()
        for line in lines:
            parts = line.split(':', 1)
            match parts[0]:
                case "NAME":
                    self.name = parts[1]
                case "ID":
                    self.id = parts[1]
                case "LEVEL":
                    self.level = int(parts[1])
                case "HP":
                    self.hp = int(parts[1])
                    self.hp_max = int(parts[1])
                case "STR":
                    self.st = int(parts[1])
                case "DEX":
                    self.de = int(parts[1])
                case "CON":
                    self.co = int(parts[1])
        if self.level <= 10:
            self.rating = "WEAK"
        elif self.level <= 30:
            self.rating = "NORMAL"
        elif self.level <= 50:
            self.rating = "HARD"
        else:
            self.rating = "MISC"

class character:
    name = ""
    id = ""
    level = -1
    xp = -1
    hp = -1
    hp_max = -1
    st = -1
    de = -1
    co = -1
    deaths = -1
    monsters_killed = -1
    bosses_fought = -1
    bosses_beaten = -1
    monster_win_rate = -1.0
    prestige = -1
        
    def __init__(self, id):
        file = {}
        if id[-4:] == ".txt":
            file = open(f"{CHARACTER_PATH}\\{id}")
        else:
            file = open(f"{CHARACTER_PATH}\\{id}.txt")
        lines = file.read().splitlines()
        for line in lines:
            parts = line.split(':', 1)
            match parts[0]:
                case "NAME":
                    self.name = parts[1]
                case "ID":
                    self.id = parts[1]
                case "LEVEL":
                    self.level = int(parts[1])
                case "XP":
                    self.xp = int(parts[1])
                case "HP":
                    self.hp = int(parts[1])
                    self.hp_max = int(parts[1])
                case "STR":
                    self.st = int(parts[1])
                case "DEX":
                    self.de = int(parts[1])
                case "CON":
                    self.co = int(parts[1])
                case "DEATHS":
                    self.deaths = int(parts[1])
                case "MONSTERS_KILLED":
                    self.monsters_killed = int(parts[1])
                case "BOSSES_FOUGHT":
                    self.bosses_fought = int(parts[1])
                case "BOSSES_BEATEN":
                    self.bosses_beaten = int(parts[1])
                case "MONSTER_WIN_RATE": 
                    self.monster_win_rate = float(parts[1])
                case "PRESTIGE":
                    self.prestige = int(parts[1])
                case _:
                    pass

    def save_character(self):
        writer = open(f"{CHARACTER_PATH}\\{self.id}.txt", "w")
        writer.write("NAME:" + self.name + "\n")
        writer.write("ID:" + str(self.id) + "\n")
        writer.write("LEVEL:" + str(self.level) + "\n")
        writer.write("XP:" + str(self.xp) + "\n")
        writer.write("HP:" + str(self.hp_max) + "\n")
        writer.write("STR:" + str(self.st) + "\n")
        writer.write("DEX:" + str(self.de) + "\n")
        writer.write("CON:" + str(self.co) + "\n")
        writer.write("DEATHS:" + str(self.deaths) + "\n")
        writer.write("MONSTERS_KILLED:" + str(self.monsters_killed) + "\n")
        writer.write("BOSSES_FOUGHT:" + str(self.bosses_fought) + "\n")
        writer.write("BOSSES_BEATEN:" + str(self.bosses_beaten) + "\n")
        writer.write("MONSTER_WIN_RATE:" + str(self.monster_win_rate) + "\n")
        writer.write("PRESTIGE:" + str(self.prestige))
        writer.close()

    def increase_xp(self, xp_gain):
        self.xp += xp_gain
        next_level = self._calculate_next_level()
        if self.xp >= next_level:
            while self.xp >= next_level:
                self._level_up()
                next_level = self._calculate_next_level()
        
    def _level_up(self):
        self.level += 1
        if self.level <= 100:
            for x in range(2):
                ran = random.randint(1,3)
                match ran:
                    case 1:
                        self.st += 1
                    case 2:
                        self.de += 1
                    case 3:
                        self.co += 1
            co_mod = self.co // 3
            self.hp_max += random.randint(1, co_mod)
            self.hp = self.hp_max
        
    def _calculate_next_level(self):
        next_level = 0
        for x in range(self.level):
            next_level += (x + 1) * 30
        return next_level