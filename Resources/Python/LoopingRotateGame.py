from time import sleep
from Utils.obs import get_source_id, set_source_transform
from Utils.obs_caller import play_looping_prank_animation

SCENE = "Game Scene - Solo"
SOURCE = "PCGame"
ROTATE_TIMER = 8

DEFAULT_POSITION = {
    "positionX": 0,
    "positionY": 0,
    "rotation": 0
}

ROTATED_POSITION = {
    "positionX": 1920,
    "positionY": 1080,
    "rotation": 180
}

def start():
    source_id = get_source_id(SOURCE, SCENE)
    set_source_transform(source_id, SCENE, ROTATED_POSITION)
    play_looping_prank_animation()
    sleep(6)
    set_source_transform(source_id, SCENE, DEFAULT_POSITION)


if __name__ == '__main__':
    start()