from Utils.obs import create_source, get_source_id, set_source_transform, set_source_activity, remove_source_from_scene
from Utils.twitch import create_connection, send_twitch_message, get_details_from_message
from time import sleep
import random, _thread

FLAMES = {}
SOCK = {}
SCENE = "Canvas"
SOOT_IMAGE_ID = -1
KEEP_ALIVE = True

def stream_on_fire_start(value):
    global FLAMES, SOCK
    details = get_value_details(value)
    FLAMES = create_flames(details)
    FLAMES = create_obs_sources(FLAMES)
    SOCK = create_connection()
    send_twitch_message(f"The streams set on fire! Send messages with the word 'water' in to put the fire out!")
    _thread.start_new_thread(socket_loop, ())
    while KEEP_ALIVE is True:
        sleep(1)
    send_twitch_message(f"Excellent job rangers! The stream is safe, at least until one of you idiots over feeds the fire again...")

def socket_loop():
    while KEEP_ALIVE is True:
        resp = SOCK.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            SOCK.send("PONG\n".encode('utf-8'))
        else:
            details = get_details_from_message(resp)
            check_message(details)

def check_message(details):
    message = details["message"]
    if "water" in message.lower():
        try:
            reduce_flame()
        except:
            pass

def reduce_flame():
    global FLAMES, KEEP_ALIVE
    key = random.choice(list(FLAMES.keys()))
    flame = FLAMES[key]
    new_scale = flame["position"]["scaleX"] - 0.25
    if new_scale <= 0.1:
        remove_source_from_scene(flame["source_id"], SCENE)
        FLAMES.pop(key)
        if len(FLAMES) == 0:
            KEEP_ALIVE = False
    else:
        flame["position"].update({"scaleX": new_scale})
        flame["position"].update({"scaleY": new_scale})
        set_source_transform(flame["source_id"], SCENE, flame["position"])
        FLAMES.update({key: flame})

def get_value_details(value):
    value_parts = value.split("|")
    flame_count = 0
    flame_min_scale = 0
    flame_max_scale = 0
    if "-" not in value_parts[0]:
        flame_count = int(value_parts[0])
    else:
        parts = value_parts[0].split("-")
        min = parts[0]
        max = parts[1]
        flame_count = random.randint(int(min), int(max))
    if "-" not in value_parts[1]:
        flame_min_scale = float(value_parts[1])
        flame_max_scale = float(value_parts[1])
    else:
        parts = value_parts[1].split("-")
        flame_min_scale = float(parts[0])
        flame_max_scale = float(parts[1])
    details = {
        "count": flame_count,
        "scale": {
            "min": flame_min_scale,
            "max": flame_max_scale
        }
    }
    return details

def create_flames(details):
    flames = {}
    for count in range(details["count"]):
        scale = random.uniform(details["scale"]["min"], details["scale"]["max"])
        flame = {
            "source_name": f"Flame{count}",
            "source_kind": "image_source",
            "source_file": {
                "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\Python\\ScriptResources\\Flame2.gif"
            },
            "position": {
                "positionX": random.randint(0, 1664),
                "positionY": random.randint(0, 824),
                "scaleX": scale,
                "scaleY": scale
            }
        }
        flames.update({f"flame_{count}": flame})
    return flames

def create_obs_sources(flames):
    for key in flames.keys():
        flame =  flames[key]
        create_source(flame["source_name"], flame["source_kind"], SCENE, flame["source_file"], False)
        source_id = get_source_id(flame["source_name"], SCENE)
        set_source_transform(source_id, SCENE, flame["position"])
        set_source_activity(source_id, SCENE, True)
        flames[key].update({"source_id": source_id})
    return flames