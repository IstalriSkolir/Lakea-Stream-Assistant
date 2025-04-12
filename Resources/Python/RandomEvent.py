import os, sys, random
from RandomCharacterGainXP import random_character_gain_xp
from StreamOnFire import stream_on_fire_start
from Utils.common_functions import RESOURCE_FOLDER, PYTHON_FOLDER
from Utils.obs_caller import obs_set_source_visability, obs_create_source
from Utils.twitch_caller import twitch_caller_send_message, twitch_caller_looping_send_message, lakea_and_looping_conversation

EVENT_PERCENT = 10 # 1-100, how likely a random event is to occur

def start():
    if(len(sys.argv) > 1):
        if sys.argv[1] == "ENVIRONMENTRESET":
            remove_lock()
        elif sys.argv[1].isnumeric():
            if check_lock() is False:
                run_event(int(sys.argv[1]))
                remove_lock()
        elif sys.argv[1] == "TRUE":
            if check_lock() is False:
                run_event()
                remove_lock()
        elif sys.argv[1] == "RANDOMTEST":
            random_test(int(sys.argv[2]))
    else:
        if check_lock() is False:
            run_script()
        
def check_lock() -> bool:
     if os.path.isfile("./RandomEvent.lock") is False:
        lock = open("RandomEvent.lock", "w")
        lock.close()
        return False
     else:
        return True

def run_script():
    ran = random.randrange(1, 101)
    if ran <= EVENT_PERCENT:
        #run_event()
        try:
            run_event()
        except Exception as error:
            print(str(error))
    remove_lock()
    
def remove_lock():
    if os.path.exists("RandomEvent.lock"):
        os.remove("RandomEvent.lock")
    
def get_event(is_random: bool, event_number = -1) -> dict:
    file = open(f"{RESOURCE_FOLDER}RandomEvents.txt")
    events = file.read().splitlines()
    file.close()
    line = "#"
    ran = 0
    if is_random is True:
        events_dict = create_event_dict(events)
        keys = list(events_dict.keys())
        key = random.choice(keys)
        array = events_dict[key]
        ran = random.randrange(0, len(array))
        line = f"{key}:{array[ran]}"
    else:
        eve_num = event_number - 1
        line = events[eve_num]
    parts = line.split(':', 1)
    event = {
        "type": parts[0],
        "value": parts[1]
    }
    return event

def create_event_dict(lines: list) -> dict:
    events_dict = {}
    events_type =  []
    for line in lines:
        if line[0] != '#':
            parts = line.split(':', 1)
            if parts[0] in events_type:
                array = events_dict[parts[0]]
                array.append(parts[1])
                events_dict.update({parts[0]: array})
            else:
                events_type.append(parts[0])
                events_dict.update({parts[0]: [parts[1]]})
    return events_dict

def run_event(event_number = -1):
    event = {}
    if event_number == -1:
        event = get_event(True)
    else:
        event = get_event(False, event_number)
    match event["type"]:
        case "CAPTURESTREAMERGAME":
            run_script_with_args(event["value"])
        case "LAKEALOOPINGCONVERSATION":
            lakea_and_looping_conversation(event["value"])
        case "LOOPINGEMOTESPAM":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "LOOPINGREDEEMPRANK":
            run_script_with_args(event["value"])
        case "LOOPINGROPEPRANK":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "LOOPINGROTATEGAME":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "MONSTERATTACKBOSS":
            run_script_with_args(event["value"])
        case "MONSTERHUNTERGAME":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "OBSACTIVATESOURCE":
            obs_set_source_visability(event["value"], True)
        case "OBSCREATESOURCE":
            obs_create_source(event["value"], "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\")
        case "PLAYSOUNDEFFECT":
            run_script_with_args(event["value"])
        case "RUNPYTHONSCRIPT":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "SENDRIDDLECHAT":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "SENDTWITCHMESSAGE":
            twitch_caller_send_message(event["value"])
        case "SENDTWITCHMESSAGELOOPING":
            twitch_caller_looping_send_message(event["value"])
        case "SHOWRANDOMART":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "SHOWRANDOMCLIP":
            os.system(f"python {PYTHON_FOLDER}{event['value']}.py")
        case "STREAMONFIRE":
            stream_on_fire_start(event['value'])
        case "RANDOMCHARACTERXP":
            random_character_gain_xp(event["value"])
        case _:
            pass

def run_script_with_args(details):
    parts = details.split("|")
    script = parts.pop(0)
    command = f"python {PYTHON_FOLDER}{script}.py"
    for arg in parts:
        command = f"{command} {arg}"
    os.system(command)

def random_test(number: int):
    file = open(f"{RESOURCE_FOLDER}RANDOMTESTOUTPUT.txt", "w")
    for x in range(number):
        line = get_event(True)
        print(f"{x + 1}. {line}")
        file.write(f"{line}\n")
    file.close()



if __name__ == '__main__':
    start()
