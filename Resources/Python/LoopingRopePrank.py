import random, os
from PIL import Image
from time import sleep
from Utils.rope_drawer import draw_rope
from Utils.common_functions import TEMP_STORAGE
from Utils.obs_caller import play_looping_prank_animation
from Utils.obs import create_source, get_source_id, remove_source_from_scene

TIME_BETWEEN_ROPES = 0.160
REMOVAL_DELAY = 3
MIN_ROPES = 25
MAX_ROPES = 40
MIN_HEIGHT = -50
MAX_HEIGHT = 1130
MIN_THICKNESS = 8
MAX_THICKNESS = 35

SCENE = "Canvas"

def start():
    ropes = create_rope_sources()
    play_looping_prank_animation()
    sleep(REMOVAL_DELAY)
    remove_sources_and_images(ropes)

def create_rope_sources() -> list:
    rope_count = random.randint(MIN_ROPES, MAX_ROPES)
    rope_data = []
    for x in range(rope_count):
        image_path = generate_rope_image(x)
        source_data = get_source_data(f"LOOPINGROPEPRANK{x}", image_path)
        create_source(source_data["source_name"], "image_source", SCENE, source_data["source_file"], True)
        source_id = get_source_id(source_data["source_name"], SCENE)
        rope_data.append({
            "source_id": source_id,
            "image_path": image_path
        })
        sleep(TIME_BETWEEN_ROPES)
    return rope_data

def generate_rope_image(count: int) -> str:
    im = Image.new('RGBA', (1920, 1080), (0, 0, 0, 0))
    start_height = random.randint(MIN_HEIGHT, MAX_HEIGHT)
    end_height = random.randint(MIN_HEIGHT, MAX_HEIGHT)
    thickness = random.randint(MIN_THICKNESS, MAX_THICKNESS)
    path = f"{TEMP_STORAGE}LOOPINGROPEPRANK{str(count)}.png"
    im = draw_rope(im, (-50, start_height), (1970, end_height), thickness)
    im.save(path, quality=95)
    return path

def get_source_data(source_name: str, path: str) -> dict:
    return {
        "source_name": source_name,
        "source_file": {
            "file": path
        },       
    }

def remove_sources_and_images(ropes: dict):
    for x in range(len(ropes)):
        reversed_index = len(ropes) - (x + 1)
        remove_source_from_scene(ropes[x]["source_id"], SCENE)
        os.remove(ropes[x]["image_path"])



if __name__ == '__main__':
    start()