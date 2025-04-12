import sys, string, random
from datetime import datetime
from Utils.twitch import send_twitch_message
from Utils.monster_battle_classes import get_current_boss, get_monster_dict_by_id, fight
from Utils.common_functions import update_boss_health_bar, get_from_common_storage, update_common_storage

ACCOUNT_IDS = {
    "246286679": "BladedPelt",
    "187517232": "hetvideorestaurant"
}

def start(account_id, display_name):
    if account_id in ACCOUNT_IDS.keys() and check_first_time_today(account_id, display_name):
        monster = get_monster()
        boss = get_current_boss()
        boss = fight(boss, monster)
        boss.save_as_current_boss()
        update_boss_health_bar()
        send_chat_message(boss, monster, display_name)

def check_first_time_today(account_id: str, display_name: str) -> bool:
    current_date = datetime.today().strftime('%Y-%m-%d')
    key = f"{account_id}SUMMONFAMILIAR"
    last_used = get_from_common_storage(key)
    if current_date == last_used:
        send_twitch_message(f"You've already used your summon familiar spell today {display_name}, you need to wait a day before you can use it again!")
        return False
    else:
        update_common_storage(key, current_date)
        return True

def get_monster() -> object:
    monsters = get_monster_dict_by_id()
    monster_key = random.choice(list(monsters.keys()))
    return monsters[monster_key]

def send_chat_message(boss: object, monster: object, display_name: str):
    if boss.name == "LAKEA_MOONLIGHT":
        send_twitch_message(f"You summoned a {string.capwords(monster.name)} to attack me? You have some nerve {display_name}!")
    else:
        send_twitch_message(f"You summoned a {string.capwords(monster.name)} to attack {string.capwords(boss.name)}? You have some nerve {display_name}!")



if __name__ == '__main__':
    account_id = sys.argv[1]
    display_name = sys.argv[2]
    start(account_id, display_name)
