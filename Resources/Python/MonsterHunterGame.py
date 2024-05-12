import math, _thread
from PIL import Image, ImageDraw, ImageFont
from Utils.monster_battle_classes import get_monster_dict_by_id
from Utils.common_functions import get_from_common_storage, update_common_storage
from Utils.twitch import send_twitch_message, create_connection, get_details_from_message
from Utils.obs import create_source, get_source_id, remove_source_from_scene
from time import sleep

SCENE = "Canvas"
MONSTER_DICT = {}
HIGH_SCORE = 0
CURRENT_SCORE = 0
SOURCE_ID = 0
KEEP_ALIVE = True

def monster_hunter_start():
    run_setup()
    start_timer()

def run_setup():
    global MONSTER_DICT, HIGH_SCORE
    MONSTER_DICT = create_monster_dict()
    HIGH_SCORE = int(get_from_common_storage("MONSTERHUNTERHIGHSCORE"))
    update_progress_bar(0)
    create_obs_source()
    send_twitch_message(f"Monster Hunter Time! Hunt as many monsters as you can in 60 seconds, rangers! The stronger the monster, the more points you earn for the community! Try to beat the current high score of {HIGH_SCORE}!")
    _thread.start_new_thread(socket_loop, ())

def create_monster_dict():
    monster_dict = get_monster_dict_by_id()
    new_monster_dict = {}
    for _, value in monster_dict.items():
        monster_name = value.name.lower().replace("_", "")
        new_monster_dict.update({monster_name: value})
    return new_monster_dict

def update_progress_bar(current_score):
    start_point = (100, 133)
    end_point = (1590, 133)
    percent = current_score / HIGH_SCORE
    if percent >= 1:
        percent = 1
    total_distance = math.dist(start_point, end_point)
    distance = total_distance * percent
    progress_point = (start_point[0] + distance, start_point[1])    
    im = Image.new('RGBA', (1920, 1080), (0, 0, 0, 0))
    draw = ImageDraw.Draw(im)
    large_font = ImageFont.truetype("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\Fonts\\Cataneo_BT_Bold.ttf", 40)
    small_font = ImageFont.truetype("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\Fonts\\Cataneo_BT_Bold.ttf", 25)
    if percent < 1:
        draw.line((start_point, progress_point), fill=(20, 199, 163), width=15)
    else:
        draw.line((start_point, progress_point), fill=(32, 199, 20), width=15)
    draw.line(((start_point[0] - 2.5, start_point[1] - 15), (start_point[0] - 2.5, start_point[1] + 15)), fill=(163, 26, 26), width=5)
    draw.line(((end_point[0], end_point[1] - 15), (end_point[0], end_point[1] + 15)), fill=(163, 26, 26), width=5)
    draw.text((start_point[0] - 90, start_point[1] - 25), f"{CURRENT_SCORE}", (255, 255, 255), large_font)
    draw.text((end_point[0] + 15, end_point[1] - 25), f"{HIGH_SCORE}", (255, 255, 255), large_font)
    draw.text((end_point[0], end_point[1] + 30), "High Score", (255, 255, 255), small_font)
    im.save("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\Outputs\\ProgressBar.png", quality=95)

def create_obs_source():
    global SOURCE_ID
    data = {
        "source_name": "ProgressBar",
        "source_kind": "image_source",
        "source_file": {
            "file": 
            "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\Outputs\\ProgressBar.png"
        },
    }
    create_source(data["source_name"], data['source_kind'], SCENE, data["source_file"], True)
    SOURCE_ID = get_source_id(data["source_name"], SCENE)

def start_timer():
    timer = 60
    step = 3
    while timer > 0:
        sleep(step)
        timer -= step
        update_progress_bar(CURRENT_SCORE)
        match timer:
            case 30:
                send_twitch_message("Half of your time is gone rangers! 30 seconds left to accrue more points!")
            case 15:
                send_twitch_message("Only 15 seconds left rangers! Hurry and hunt those monsters!")
    end_script()

def end_script():
    global KEEP_ALIVE
    KEEP_ALIVE = False
    if CURRENT_SCORE > HIGH_SCORE:
        send_twitch_message(f"Congratulations, rangers! You set a new high score of {CURRENT_SCORE}, great work!")
        update_common_storage("MONSTERHUNTERHIGHSCORE", str(CURRENT_SCORE))
    else:
        send_twitch_message(f"Nice try rangers, however you didn't succeed in beating the high score! You finished with a score of {CURRENT_SCORE}, better luck next time!")
    sleep(5)
    remove_source_from_scene(SOURCE_ID, SCENE)

def socket_loop():
    sock = create_connection()
    while KEEP_ALIVE is True:
        resp = sock.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            sock.send("PONG\n".encode('utf-8'))
        else:
            details = get_details_from_message(resp)
            check_message(details)

def check_message(details):
    global CURRENT_SCORE
    monster = {}
    if details["username"] == "lakeamoonlight":
        message = details["message"].lower().replace(" ", "")
        if "fought" in message and "won" in message:
            for name in MONSTER_DICT.keys():
                if name in message:
                    monster = MONSTER_DICT[name]
                    break
    if monster != {}:
        points = calcualte_points(monster.level)
        CURRENT_SCORE += points

def calcualte_points(monster_level):
    points = math.floor((180 - (monster_level * 0.15)) * (monster_level * 0.01) - ((monster_level * 1.5) - 0.75))
    return points



if __name__ == "__main__":
    monster_hunter_start()
