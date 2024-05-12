from Utils.obs import create_source, get_source_id, set_source_transform, set_source_activity, remove_source_from_scene
from time import sleep
import random

SCENE = "Canvas"
ACTIVE_TIME = 60

def start():
    clip_name = random.choice(CLIPS)
    clip = get_clip_object(clip_name)
    source_id = create_clip_scene_item(clip)
    sleep(ACTIVE_TIME)
    remove_source_from_scene(source_id, SCENE)

def get_clip_object(clip_name):
    return {
        "transform": {
            "positionX": 0,
            "positionY": 615,
            "scaleX": 0.65,
            "scaleY": 0.65            
        },
        "source_file": {
            "local_file": f"X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomClip\\{clip_name}.mp4"
        }
    }

def create_clip_scene_item(clip):
    create_source("RandomClip", "ffmpeg_source", SCENE, clip["source_file"], False)
    source_id = get_source_id("RandomClip", SCENE)
    set_source_transform(source_id, SCENE, clip["transform"])
    set_source_activity(source_id, SCENE, True)
    return source_id

CLIPS = [
    "A Sticky Situation",
    "Amogus",
    "AND THEN ALONG CAME ZEUS",
    "Distracted By The Ears",
    "Distracted Driving",
    "He Likes What",
    "Mistakes Were Made",
    "Prof Found Quicksand",
    "The Long No",
    "To Catch A Blorb",
    "Cats Always Land On Their Feet",
    "Shiny In Twitch Plays",
    "Giant Twink"
]

if __name__ == '__main__':
    start()
