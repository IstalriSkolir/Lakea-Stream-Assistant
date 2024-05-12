from Utils.obs import get_source_id, set_source_activity, create_source, set_source_transform, remove_source_from_scene
from time import sleep

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
