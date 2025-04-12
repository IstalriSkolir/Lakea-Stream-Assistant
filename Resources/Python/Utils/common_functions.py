import os
from time import sleep
from pathlib import Path
from Utils.common_dicts import get_event_details_dict, get_event_item_dict
from Utils.lakea import send_to_websocket
from Utils.sensitive_data import REDEEM_SETTINGS

COMMON_STORAGE_FILE = "PATH"
PYTHON_FOLDER = "PATH"
RESOURCE_FOLDER = "PATH"
TEMP_STORAGE = "PATH"
BATTLE_SIM_PATH = "PATH"

def get_from_common_storage(key: str) -> str:
    file = open(COMMON_STORAGE_FILE, 'r')
    lines = file.read().splitlines()
    file.close
    value = ""
    for line in lines:
        parts = line.split(':', 1)
        if parts[0] == key:
            value = parts[1]
            break
    return value

def update_common_storage(key, value):
    file = open(COMMON_STORAGE_FILE, 'r')
    lines = file.read().splitlines()
    file.close()
    for index in range(len(lines)):
        if lines[index] != "":
            parts = lines[index].split(':', 1)
            if parts[0] == key:
                lines[index] = f"{key}:{value}"
                break
    file = open(COMMON_STORAGE_FILE, 'w')
    for line in lines:
        if line != "":
            file.write(f"{line}\n")
    file.close()

def update_boss_health_bar():
    event_details = get_event_details_dict("Update Boss Healthbar Websocket", "Lakea", "Lakea_Web_Socket", "Boss_Healthbar_Websocket")
    event = get_event_item_dict("Lakea_Web_Socket", event_details, {})
    send_to_websocket(event, "RunEvent")

def check_for_script_lock(script: str) -> bool:
     if os.path.isfile(f"{TEMP_STORAGE}{script}.lock") is False:
        Path(f"{TEMP_STORAGE}{script}.lock").touch()
        return False
     else:
        return True   

def remove_script_lock(script: str):
    if os.path.exists(f"{TEMP_STORAGE}{script}.lock"):
        os.remove(f"{TEMP_STORAGE}{script}.lock")

def reset_all_twitch_redeems():
    for key, value in REDEEM_SETTINGS.items():
        update_redeem = {
            "RedeemID": key,
            "RedeemData": value
        }
        send_to_websocket(update_redeem, "UpdateChannelRedeem")
        sleep(0.25)
