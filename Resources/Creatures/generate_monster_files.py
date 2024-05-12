import os, random

WORKING_FOLDER = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures"

def start():
    existing_monsters = get_existing_monster_data()
    new_monsters = get_new_monster_data()
    update_monsters = get_monsters_to_update(existing_monsters, new_monsters)
    existing_ids = get_existing_ids()
    generated_monsters = generate_monsters(update_monsters, existing_ids)
    write_monsters_to_file(generated_monsters)
    write_monster_list_files(generated_monsters)

def get_existing_monster_data():
    existing_monsters = {}
    if os.path.isdir(f"{WORKING_FOLDER}\\Monsters"):
        for file in os.listdir(f"{WORKING_FOLDER}\\Monsters"):
            id_string = file[:4]
            if id_string.isnumeric():
                monster_file = open(f"{WORKING_FOLDER}\\Monsters\\{file}")
                lines = monster_file.read().splitlines()
                monster = {}
                for line in lines:
                    parts = line.split(":")
                    monster.update({parts[0]: parts[1]})
                existing_monsters.update({monster["NAME"]: monster})
    else:
        os.mkdir(f"{WORKING_FOLDER}\\Monsters")
    return existing_monsters

def get_new_monster_data():
    new_monsters = []
    file = open(f"{WORKING_FOLDER}\\Monsters.csv")
    lines = file.read().splitlines()
    lines.pop(0)
    for line in lines:
        if line != "":
            parts = line.split(",")
            monster = {
                "NAME": parts[0].upper(),
                "RATING": parts[1],
                "LEVEL": int(parts[2])
            }
            new_monsters.append(monster)
    return new_monsters

def get_monsters_to_update(existing_monsters, new_monsters):
    update_monsters = []
    existing_monster_keys = existing_monsters.keys()
    for monster in new_monsters:
        monster_name = monster["NAME"].upper()
        if monster_name not in existing_monster_keys:
            update_monsters.append(monster)
        else:
            existing_level = int(existing_monsters[monster_name]["LEVEL"])
            new_level = int(monster["LEVEL"])
            if new_level != existing_level:
                monster.update({"ID": existing_monsters[monster_name]["ID"]})
                update_monsters.append(monster)
    return update_monsters

def get_existing_ids():
    ids = []
    for file in os.listdir(f"{WORKING_FOLDER}\\Monsters"):
        id_string = file[:4]
        if id_string.isnumeric():
            id = int(id_string)
            ids.append(id)
    return ids

def generate_monsters(update_monsters, existing_ids):
    generated_monsters = []
    for monster in update_monsters:
        new_monster = Monster(monster["NAME"], int(monster["LEVEL"]))
        if "ID" in monster:
            new_monster.id = monster["ID"]
        else:
            new_id = generate_id(existing_ids)
            existing_ids.append(int(new_id))
            new_monster.id = new_id
        new_level = 1
        while new_level < new_monster.level:
            new_level += 1
            for point in range(2):
                ran = random.randint(1,3)
                match ran:
                    case 1:
                        new_monster.st += 1
                    case 2:
                        new_monster.de += 1
                    case 3:
                        new_monster.co += 1
            co_mod = new_monster.co // 3
            hp_increase = random.randint(1, co_mod)
            new_monster.hp += hp_increase
            new_monster.hp_max += hp_increase
        generated_monsters.append(new_monster)
    return generated_monsters

def generate_id(existing_ids):
    new_id = 0
    done = False
    while done is False:
        if new_id not in existing_ids:
            done = True
        else:
            new_id += 1
    id_string = str(new_id)
    length = len(id_string)
    if length == 1:
        return f"000{id_string}"
    elif length == 2:
        return f"00{id_string}"
    elif length == 3:
        return f"0{id_string}"
    else:
        return id_string

def write_monsters_to_file(monsters):
    for monster in monsters:
        monster_name = monster.name.replace(" ", "_")
        file_name = f"{monster.id}-{monster_name}"
        monster_file = open(f"{WORKING_FOLDER}\\Monsters\\{file_name}.txt", "w")
        monster_file.write(f"NAME:{monster_name}\n")
        monster_file.write(f"ID:{monster.id}\n")
        monster_file.write(f"LEVEL:{monster.level}\n")
        monster_file.write(f"HP:{monster.hp_max}\n")
        monster_file.write(f"STR:{monster.st}\n")
        monster_file.write(f"DEX:{monster.de}\n")
        monster_file.write(f"CON:{monster.co}")

def write_monster_list_files(monsters):
    weak_monsters = []
    normal_monsters = []
    hard_monsters = []
    random_monsters = []
    for monster in monsters:
        match monster.rating:
            case "WEAK":
                weak_monsters.append(monster)
                random_monsters.append(monster)
            case "NORMAL":
                normal_monsters.append(monster)
                random_monsters.append(monster)
            case "HARD":
                hard_monsters.append(monster)
                random_monsters.append(monster)
            case "MISC":
                random_monsters.append(monster)
    create_monster_list_file(f"{WORKING_FOLDER}\\Monsters\\WEAKMONSTERS.txt", weak_monsters)
    create_monster_list_file(f"{WORKING_FOLDER}\\Monsters\\NORMALMONSTERS.txt", normal_monsters)
    create_monster_list_file(f"{WORKING_FOLDER}\\Monsters\\HARDMONSTERS.txt", hard_monsters)
    create_monster_list_file(f"{WORKING_FOLDER}\\Monsters\\RANDOMMONSTERS.txt", random_monsters)

def create_monster_list_file(path, list):
    file = open(path, "w")
    for index in range(len(list)):
        monster_name = list[index].name.replace(" ", "_")
        if index == 0:
            file.write(f"{list[index].id}-{monster_name}")
        else:
            file.write(f"\n{list[index].id}-{monster_name}")
    file.close()

class Monster:
    name = ""
    id = ""
    level = 1
    hp = 20
    hp_max = 20
    st = 9
    de = 9
    co = 9
    rating = ""

    def __init__(self, name, level):
        self.name = name
        self.level = level
        if self.level <= 10:
            self.rating = "WEAK"
        elif self.level <= 30:
            self.rating = "NORMAL"
        elif self.level <= 50:
            self.rating = "HARD"
        else:
            self.rating = "MISC"



start()