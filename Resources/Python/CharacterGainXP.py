from Utils.monster_battle_classes import character
from Utils.twitch import send_twitch_message
import os, random

#CHARACTER_PATH = "X:\\1-APPLICATIONDATA\\DEVENV\\Creatures\\Characters\\"
CHARACTER_PATH = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Creatures\\Characters\\"

def random_character_gain_xp(value):
    char = get_random_character()
    xp_gain = get_xp_gain_from_value(value)
    char.increase_xp(xp_gain)
    char.save_character()
    send_twitch_message(f"Looping gave @{char.name} some combat tips and they earned {xp_gain} XP!")

def get_random_character():
    files = os.listdir(CHARACTER_PATH)
    length = len(files)
    ran = random.randrange(0, length)
    character_id = files[ran][:-4]
    char = character(character_id)
    return char

def get_xp_gain_from_value(value):
    if "|" not in value:
        return int(value)
    else:
        parts = value.split("|")
        min_xp = int(parts[0])
        max_xp = int(parts[1])
        xp_gain = random.randint(min_xp, max_xp)
        if xp_gain % 5 != 0:
            remainder = xp_gain % 5
            xp_gain -= remainder
        return xp_gain