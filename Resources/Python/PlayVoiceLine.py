import sys, math, random, uuid, os
from mutagen.mp3 import MP3
from time import sleep
from Utils.common_functions import RESOURCE_FOLDER, TEMP_STORAGE, check_for_script_lock, remove_script_lock
from Utils.obs import create_source, get_source_id, set_source_activity, set_audio_monitoring, delete_input
from Utils.obs_caller import lakea_captured_line

SCRIPT_NAME = "PLAYVOICELINE"
BUFFER_TIME = 0.25
SCENE = "Canvas"
SOURCE_NAME = "CHARACTERVOICELINE"

def start():
    line_type = sys.argv[1]
    instance_running = check_for_script_lock(SCRIPT_NAME)
    if instance_running == False:
        run_voice_line(line_type)
        other_files_exist = True
        while other_files_exist == True:
            list_of_files = check_for_other_files()
            if len(list_of_files) > 0:
                play_other_voice_lines(list_of_files)
            else:
                other_files_exist = False
        remove_script_lock(SCRIPT_NAME)
    else:
        existing_voice_line(line_type)
    

def run_voice_line(line_type: str):
    if line_type == "LAKEACAPTURETAKEN" or line_type == "LAKEACAPTURERETORT" or line_type == "LAKEACAPTUREFREED":
        lakea_captured_line(line_type)
    file_path = get_voice_file(line_type)
    audio_length = get_audio_length(file_path)
    create_obs_source(file_path)
    wait(audio_length)
    delete_input(SOURCE_NAME)
    if line_type == "LAKEACAPTURERETORT" or line_type == "LAKEACAPTURETAKEN":
        lakea_captured_line(f"{line_type}ENDED")

def existing_voice_line(line_type: str):
    file_name = f"{SCRIPT_NAME}-{uuid.uuid4()}"
    file = open(f"{TEMP_STORAGE}{file_name}.txt", "w")
    file.write(line_type)
    file.close()

def get_voice_file(type: str) -> str:
    array = VOICE_DICT[type]
    return random.choice(array)

def get_audio_length(file_path: str) -> int:
    audio = MP3(file_path)
    return math.ceil(audio.info.length)

def create_obs_source(file_path: str):
    source_file = {
        "local_file": file_path
    }
    create_source(SOURCE_NAME, "ffmpeg_source", SCENE, source_file, False)
    source_id = get_source_id(SOURCE_NAME, SCENE)
    set_audio_monitoring(SOURCE_NAME, "OBS_MONITORING_TYPE_MONITOR_AND_OUTPUT")
    set_source_activity(source_id, SCENE, True)

def wait(audio_length: int):
    sleep_time = audio_length + BUFFER_TIME
    sleep(sleep_time)

def check_for_other_files() -> list:
    all_files = os.listdir(TEMP_STORAGE)
    voice_files = []
    for file in all_files:
        if SCRIPT_NAME in file and file[-5:] != ".lock":
            voice_files.append(file)
    return voice_files

def play_other_voice_lines(file_list: list):
    x = 0
    for file_name in file_list:
        file = open(f"{TEMP_STORAGE}{file_name}", "r")
        voice_type = file.read()
        file.close()
        os.remove(f"{TEMP_STORAGE}{file_name}")
        run_voice_line(voice_type)

VOICE_DICT = {
    "LAKEABOSSENTER": [
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaEnter1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaEnter2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaEnter3.mp3"
    ],
    "LAKEABOSSDEFEAT": [
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaDefeated1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaDefeated2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLakeaDefeated3.mp3"
    ],
    "LOOPINGBOSSENTER": [
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingEntered1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingEntered2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingEntered3.mp3"
    ],
    "LOOPINGBOSSDEFEAT": [
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingDefeated1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingDefeated2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleLoopingDefeated3.mp3"
    ],
    "MATERIESBOSSENTER": [
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleMateriesEnter1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleMateriesEnter2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\BossBattles\\BossBattleMateriesEnter3.mp3"
    ],
    "LAKEACAPTURETAKEN":[
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaCaptured1.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaCaptured2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaCaptured3.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaCaptured4.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaCaptured4.mp3"
    ],
    "LAKEACAPTURERETORT": [
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort1V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort2V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort3V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort4V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort5V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort6V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort7V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort8V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort9V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaRetort10V2.mp3"
    ],
    "LAKEACAPTUREFREED": [
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaEscape1V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaEscape2V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaEscape3V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaEscape4V2.mp3",
        f"{RESOURCE_FOLDER}VoiceLines\\LakeaCaptured\\LakeaEscape5V2.mp3"
    ]
}



if __name__ == '__main__':
    start()
