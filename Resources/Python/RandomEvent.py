import os, sys, random
from CharacterGainXP import random_character_gain_xp
from CaptureStreamerGame import capture_streamer_start
from StreamOnFire import stream_on_fire_start
from Utils.obs_caller import obs_set_source_visability, obs_create_source
from Utils.twitch_caller import twitch_caller_send_message

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
    else:
        if check_lock() is False:
            run_script()
        
def check_lock():
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
    
def get_event(is_random, event_number = -1):
    file = open("X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomEvents.txt")
    events = file.read().splitlines()
    file.close()
    length = len(events)
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

def create_event_dict(lines):
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
            capture_streamer_start(event["value"])
        case "MONSTERHUNTERGAME":
            os.system(f"python {event['value']}.py")
        case "OBSACTIVATESOURCE":
            obs_set_source_visability(event["value"], True)
        case "OBSCREATESOURCE":
            obs_create_source(event["value"], "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\")
        case "RUNPYTHONSCRIPT":
            os.system(f"python {event['value']}.py")
        case "SENDTWITCHMESSAGE":
            twitch_caller_send_message(event["value"])
        case "SHOWRANDOMART":
            os.system(f"python {event['value']}.py")
        case "SHOWRANDOMCLIP":
            os.system(f"python {event['value']}.py")
        case "STREAMONFIRE":
            stream_on_fire_start(event['value'])
        case "RANDOMCHARACTERXP":
            random_character_gain_xp(event["value"])
        case _:
            pass



start()