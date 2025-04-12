import json
from time import sleep
from Utils.common_functions import RESOURCE_FOLDER
from Utils.twitch import send_twitch_message
from Utils.monster_battle_classes import character, get_character_dict_by_id

DATA_PATH = f"{RESOURCE_FOLDER}Subathon2024Data\\PointsTrackerData.json"

def start():
    characters = get_character_dict_by_id()
    level_order = sort_by_level(characters)
    i = 1

def sort_by_level(characters: dict) -> list:
    descending_order = []
    while len(characters) > 0:
        char_key = ""
        char = character("1")
        char.level = -1
        for key, value in characters.items():
            if value.level > char.level:
                char = value
                char_key = key
        descending_order.append(char)
        characters.pop(char_key)
    return descending_order



if __name__ == '__main__':
    pass
    start()
