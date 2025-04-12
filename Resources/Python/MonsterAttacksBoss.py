import sys
from Utils.common_functions import update_boss_health_bar
from Utils.monster_battle_classes import monster, boss, get_current_boss, fight
from Utils.twitch import send_twitch_message

def start(monster_details: str):
    bos = get_current_boss()
    if bos != None:
        mon = monster(monster_details)
        bos = fight(bos, mon)
        bos.save_as_current_boss()
        update_boss_health_bar()
        send_chat_message(bos, mon)

def send_chat_message(bos: boss, mon: monster):
    monster_name = mon.name.lower().title().replace("_", " ")
    if bos.name == "LAKEA_MOONLIGHT":
        send_twitch_message(f"I was just attacked by a {monster_name}! The nerve!")
    else:
        full_name_parts = bos.name.split("_")
        first_name = full_name_parts[0].lower().title()
        send_twitch_message(f"{first_name} was attacked by a {monster_name}!")



if __name__ == '__main__':
    #monster_details = "0014-GOBLIN"
    monster_details = sys.argv[1]
    start(monster_details)
