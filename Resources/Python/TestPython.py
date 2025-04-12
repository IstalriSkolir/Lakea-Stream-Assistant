from Utils.monster_battle_classes import get_character_dict_by_id
from Utils.common_functions import TEMP_STORAGE
from Utils.rope_drawer import draw_rope
import os, _thread, sys
from time import sleep
from PIL import Image, ImageDraw
from Utils.twitch import send_twitch_message

def start():
    characters = get_character_dict_by_id()
    for key, value in characters.items():
        print(f"ID:{key} - {value.name}")
    i = 0

def start2():
    loop = 4
    os.system("python PlayVoiceLine.py LAKEACAPTURETAKEN")
    for x in range(loop):
        _thread.start_new_thread(run_voice_line, ())
        sleep(1)
    os.system("python PlayVoiceLine.py LAKEACAPTUREFREED")
    #sleep(10)

def run_voice_line():
    os.system("python PlayVoiceLine.py LAKEACAPTURERETORT")

def start3():
    progress = sys.argv[1]
    escape_dc = sys.argv[2]
    send_twitch_message(f"Progress: {progress}, Escape DC: {escape_dc}")

start3()