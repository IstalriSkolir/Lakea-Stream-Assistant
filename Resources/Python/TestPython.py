from Utils.obs import create_source, set_source_transform, get_source_id, set_source_activity
from CaptureStreamerGame import capture_streamer_start
from Utils.obs_caller import obs_create_source
from StreamOnFire import stream_on_fire_start
from MonsterHunterGame import monster_hunter_start
import random

def start4():
    file = open("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomEvents3.txt")
    events = file.read().splitlines()
    file.close()
    ran = random.randrange(0, len(events))
    parts = events[ran].split(':', 1)
    event = {
        "type": parts[0],
        "value": parts[1]
    }
    capture_streamer_start(event["value"])

def start3():
    file = open("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomEvents2.txt")
    events = file.read().splitlines()
    file.close()
    ran = random.randrange(0, len(events))
    parts = events[ran].split(':', 1)
    event = {
        "type": parts[0],
        "value": parts[1]
    }
    stream_on_fire_start(event["value"])

def start2():
    print("Creating source...")
    scene_name = "Canvas"
    input_name = "TestSource"
    source_kind = "image_source"
    scene_item_enabled = False
    input_settings = {
        "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\WinryGModcheck112.gif",
    }
    scene_item_transform = {
        "positionX": 1000,
        "positionY": 500,
        "scaleX": 2,
        "scaleY": 2
    }

    create_source(input_name, source_kind, scene_name, input_settings, scene_item_enabled)
    source_id = get_source_id(input_name, scene_name)
    set_source_transform(source_id, scene_name, scene_item_transform)
    set_source_activity(source_id, scene_name, True)

def start():
    with open("TEST.txt", 'a') as f:
        f.write(f"TEST COMPLETE\n")

start4()