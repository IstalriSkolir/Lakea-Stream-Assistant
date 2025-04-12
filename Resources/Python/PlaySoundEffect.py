import sys, math, random
from mutagen.mp3 import MP3
from time import sleep
from Utils.obs import create_source, get_source_id, set_source_activity, set_audio_monitoring, delete_input
from Utils.common_functions import RESOURCE_FOLDER

BUFFER_TIME = 1
SCENE = "Canvas"

def start():
    file_path = get_effect_file(sys.argv[1])
    audio_length = get_audio_length(file_path)
    source_name = create_obs_source(file_path, f"{sys.argv[1]}SA")
    wait(audio_length)
    delete_input(source_name)

def get_effect_file(effect: str) -> str:
    value = SOUNDS_DICT[effect]
    if type(value) is list:
        return random.choice(value)
    else:
        return value

def get_audio_length(file_path: str) -> int:
    audio = MP3(file_path)
    return math.ceil(audio.info.length)

def create_obs_source(file_path, source_name):
    source_file = {
        "local_file": file_path
    }
    create_source(source_name, "ffmpeg_source", SCENE, source_file, False)
    source_id = get_source_id(source_name, SCENE)
    set_audio_monitoring(source_name, "OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT")
    set_source_activity(source_id, SCENE, True)
    return source_name


def wait(audio_length: int):
    sleep_time = audio_length + BUFFER_TIME
    sleep(sleep_time)

SOUNDS_DICT = {
    "ROPENET": f"{RESOURCE_FOLDER}SoundEffects\\RopeNet.mp3",
    "ARROWVOLLEY": f"{RESOURCE_FOLDER}SoundEffects\\ArrowVolley.mp3",
    "FIRESPIRAL": f"{RESOURCE_FOLDER}SoundEffects\\FireSpiral.mp3",
    "RANGERREBELLION": f"{RESOURCE_FOLDER}SoundEffects\\RangerRebellion.mp3",
    "ARROWIMPACTWOOD": f"{RESOURCE_FOLDER}SoundEffects\\ArrowImpactWood.mp3",
    "WOLFHOWL": [
        f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl1.mp3",
        f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl2.mp3",
        f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl3.mp3"
        #f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl4.mp3",
        #f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl5.mp3",
        #f"{RESOURCE_FOLDER}SoundEffects\\WolfHowl6.mp3"
    ]
}



if __name__ == '__main__':
    start()
