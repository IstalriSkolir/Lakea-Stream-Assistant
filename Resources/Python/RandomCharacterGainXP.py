import random, _thread
from time import sleep
from Utils.monster_battle_classes import character, get_character_dict_by_name
from Utils.twitch import create_twitch_connection, send_twitch_message, send_twitch_message_looping, get_details_from_message

WAIT_TIMER = 60
KEEP_ALIVE = True
CHATTER_LIST = []
CHATTERS_IGNORE = [
    "tmi.twitch.tv 001 lakeamoonlight ",
    "lakeamoonlight",
    "materiescoil",
    "kofistreambot"
]

def random_character_gain_xp(value: str):
    xp_gain = get_xp_gain_from_value(value)
    character_dict = get_character_dict_by_name(True)
    _thread.start_new_thread(socket_loop, ())
    send_twitch_message_looping("mmmm, I wonder who's active...")
    timer()
    char = get_char(character_dict)
    char.increase_xp(xp_gain)
    char.save_character()
    send_twitch_message(f"Looping gave @{char.name} some combat tips and they earned {xp_gain} XP!")

def get_xp_gain_from_value(value: str) -> int:
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

def timer():
    global KEEP_ALIVE
    sleep(WAIT_TIMER)
    KEEP_ALIVE = False

def get_char(character_dict: dict) -> character:
    if len(CHATTER_LIST) > 0 :
        chatter = random.choice(CHATTER_LIST)
        return character_dict[chatter]
    else:
        keys = list(character_dict.keys())
        key = random.choice(keys)
        return character_dict[key]

def socket_loop():
    sock = create_twitch_connection()
    while KEEP_ALIVE == True:
        resp = sock.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            sock.send("PONG\n".encode('utf-8'))
        else:
            details = get_details_from_message(resp)
            check_list(details)

def check_list(message):
    global CHATTER_LIST
    chatter = message["username"]
    if chatter not in CHATTER_LIST and chatter not in CHATTERS_IGNORE:
        CHATTER_LIST.append(chatter)