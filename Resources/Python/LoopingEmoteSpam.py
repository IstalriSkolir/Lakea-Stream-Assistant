import os, random
from time import sleep
from Utils.common_functions import RESOURCE_FOLDER
from Utils.obs import create_source, get_source_id, set_source_transform, set_source_activity, remove_source_from_scene
from Utils.obs_caller import play_looping_prank_animation

SCENE = "Canvas"
EMOTES_FOLDER = f"{RESOURCE_FOLDER}Emotes\\"

TIME_BETWEEN_EMOTES = 0.175
MIN_EMOTES = 25
MAX_EMOTES = 50
MIN_X_POSITION = 0
MAX_X_POSITION = 1920
MIN_Y_POSITION = 0
MAX_Y_POSITION = 1080
MIN_SCALE = 0.75
MAX_SCALE = 1.25

def start():
    emotes_list = os.listdir(EMOTES_FOLDER)
    source_ids = create_emote_sources(emotes_list)
    play_looping_prank_animation()
    remove_emotes(source_ids)

def create_emote_sources(emotes_list: list) -> list:
    emote_count = random.randint(MIN_EMOTES, MAX_EMOTES)
    source_ids = []
    for x in range(emote_count):
        emote = random.choice(emotes_list)
        source_data = get_source_data(f"Emote_{x + 1}", f"{EMOTES_FOLDER}{emote}")
        create_source(source_data["source_name"], "image_source", SCENE, source_data["source_file"], False)      
        source_id = get_source_id(source_data["source_name"], SCENE)
        set_source_transform(source_id, SCENE, source_data["position"])
        set_source_activity(source_id, SCENE, True)
        source_ids.append(source_id)
        sleep(TIME_BETWEEN_EMOTES)
    return source_ids

def remove_emotes(source_ids: list):
    for x in range(len(source_ids)):
        remove_source_from_scene(source_ids[x], SCENE)

def get_source_data(source_name: str, path: str):
    scale = random.uniform(MIN_SCALE,MAX_SCALE)
    return {
        "source_name": source_name,
        "source_kind": "image_source",
        "source_file": {
            "file": path
        },
        "position": {
            "positionX": random.randrange(MIN_X_POSITION, MAX_X_POSITION),
            "positionY": random.randrange(MIN_Y_POSITION, MAX_Y_POSITION),
            "scaleX": scale,#random.uniform(MIN_SCALE,MAX_SCALE),
            "scaleY": scale,#random.uniform(MIN_SCALE,MAX_SCALE),
            "rotation": random.randrange(0, 360)
        }
    }



if __name__ == '__main__':
    start()
