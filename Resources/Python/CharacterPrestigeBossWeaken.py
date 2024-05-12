import os

BOSS_FOLDER = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Bosses\\"
#BOSS_FOLDER = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Bosses\\"
HEALTH_DECREASE = 50

def start():
    weaken_boss(f"{BOSS_FOLDER}0000-LAKEA_MOONLIGHT.txt")
    weaken_boss(f"{BOSS_FOLDER}0001-LOOPING_COIL.txt")
    weaken_boss(f"{BOSS_FOLDER}0002-MATERIES_COIL.txt")

def weaken_boss(path):
    boss_dict = get_boss_info(path)
    boss_dict = decrease_boss_starting_health(boss_dict)
    save_boss_info(boss_dict, path)

def get_boss_info(path):
    boss_dict = {}
    if(os.path.isfile(path)):
        reader = open(path)
        lines = reader.read().splitlines()
        reader.close()
        for line in lines:
            parts = line.split(":", 1)
            boss_dict.update({parts[0]: parts[1]})
    else:
        boss_dict.update({"ERROR": "NOBOSSFILE"})
    return boss_dict

def decrease_boss_starting_health(boss_dict):
    current_hp = int(boss_dict["CURRENT_HP"])
    current_hp -= HEALTH_DECREASE
    if current_hp < 1:
        current_hp = 1
    boss_dict.update({"CURRENT_HP": str(current_hp)})
    return boss_dict

def save_boss_info(boss_dict, path):
    writer = open(path, "w")
    count = len(boss_dict.keys())
    for key, value in boss_dict.items():
        if(count > 1):
            writer.write(f"{key}:{value}\n")
        else:
            writer.write(f"{key}:{value}")
        count -= 1
    writer.close()

start()