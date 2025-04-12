from Utils.obs import get_source_id, set_source_activity, create_source, set_source_transform, remove_source_from_scene
from Utils.common_functions import RESOURCE_FOLDER
from time import sleep

CANVAS = "Canvas"
LOOPING_PRANK_ANIMATION_PATH = f"{RESOURCE_FOLDER}\\Animations\\LoopingLaughSingleRun.gif"
LAKEA_CAUGHT_ANIMATION = f"{RESOURCE_FOLDER}\\Animations\\LakeaTiedUp.gif"
LAKEA_CAUGHT_TALKING_ANIMATION = f"{RESOURCE_FOLDER}\\Animations\\LakeaTiedUpTalking.gif"

def obs_set_source_visability(value, visibility):
    parts = value.split("|")
    source_name = parts[0]
    scene = parts[1]
    reset = int(parts[2])
    source_id = get_source_id(source_name, scene)
    set_source_activity(source_id, scene, visibility)
    if reset > 0:
        sleep(reset)
        visibility = not visibility
        set_source_activity(source_id, scene, visibility)

def obs_create_source(value, path):
    parts = value.split("|")
    source_name = parts[0]
    source_file = f"{path}{parts[1]}"
    source_kind = parts[2]
    scene = parts[3]
    points_parts = parts[4].split("-", 1)
    point = [
        int(points_parts[0]),
        int(points_parts[1])
    ]
    scale = float(parts[5])
    reset = int(parts[6])
    input_settings = _create_input_settings(source_file, source_kind)
    #input_settings = {
    #    "file": source_file
    #}
    scene_item_transform = {
        "positionX": point[0],
        "positionY": point[1],
        "scaleX": scale,
        "scaleY": scale
    }
    create_source(source_name, source_kind, scene, input_settings, False)
    source_id = get_source_id(source_name, scene)
    set_source_transform(source_id, scene, scene_item_transform)
    set_source_activity(source_id, scene, True)
    if reset > 0:
        sleep(reset)
        remove_source_from_scene(source_id, scene)

def play_looping_prank_animation():
    input_settings = _create_input_settings(LOOPING_PRANK_ANIMATION_PATH, "ffmpeg_source")
    scene_item_transform = {
        "positionX": 0,
        "positionY": 640,
        "scaleX": 0.55,
        "scaleY": 0.55
    }
    create_source("Looping_Prank_Animation", "ffmpeg_source", CANVAS, input_settings, False)
    source_id = get_source_id("Looping_Prank_Animation", CANVAS)
    set_source_transform(source_id, CANVAS, scene_item_transform)
    set_source_activity(source_id, CANVAS, True)
    sleep(8)
    remove_source_from_scene(source_id, CANVAS)

def lakea_captured_line(line_type: str):
    if line_type == "LAKEACAPTURETAKEN":
        silent_input_settings = _create_input_settings(LAKEA_CAUGHT_ANIMATION, "ffmpeg_source")
        talking_input_settings = _create_input_settings(LAKEA_CAUGHT_TALKING_ANIMATION, "ffmpeg_source")
        silent_input_settings.update({"looping": True})
        silent_input_settings.update({"restart_on_activate": False})
        talking_input_settings.update({"looping": True})
        talking_input_settings.update({"restart_on_activate": False})
        scene_item_transform = {
            "positionX": 1650,
            "positionY": 100,
            "scaleX": 0.175,
            "scaleY": 0.175
        }
        create_source("Lakea_Struggle_Animation", "ffmpeg_source", CANVAS, silent_input_settings, False)
        create_source("Lakea_Talking_Animation", "ffmpeg_source", CANVAS, talking_input_settings, False)
        struggle_source_id = get_source_id("Lakea_Struggle_Animation", CANVAS)
        talking_source_id = get_source_id("Lakea_Talking_Animation", CANVAS)
        set_source_transform(struggle_source_id, CANVAS, scene_item_transform)
        set_source_transform(talking_source_id, CANVAS, scene_item_transform)
        set_source_activity(talking_source_id, CANVAS, True)
    elif line_type == "LAKEACAPTURERETORT":
        struggle_source_id = get_source_id("Lakea_Struggle_Animation", CANVAS)
        talking_source_id = get_source_id("Lakea_Talking_Animation", CANVAS)
        set_source_activity(talking_source_id, CANVAS, True)
        set_source_activity(struggle_source_id, CANVAS, False)
    elif line_type == "LAKEACAPTURERETORTENDED" or line_type == "LAKEACAPTURETAKENENDED":
        struggle_source_id = get_source_id("Lakea_Struggle_Animation", CANVAS)
        talking_source_id = get_source_id("Lakea_Talking_Animation", CANVAS)
        set_source_activity(struggle_source_id, CANVAS, True)
        set_source_activity(talking_source_id, CANVAS, False)
    elif line_type == "LAKEACAPTUREFREED":
        struggle_source_id = get_source_id("Lakea_Struggle_Animation", CANVAS)
        talking_source_id = get_source_id("Lakea_Talking_Animation", CANVAS)
        remove_source_from_scene(struggle_source_id, CANVAS)
        remove_source_from_scene(talking_source_id, CANVAS)

def _create_input_settings(source_file, source_kind):
    match source_kind:
        case "image_source":
            return {
                "file": source_file
            }
        case "ffmpeg_source":
            return {
                "local_file": source_file
            }
        case _:
            print("Unrecognised Source Kind")
