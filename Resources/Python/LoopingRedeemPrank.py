import sys, time
from Utils.sensitive_data import REDEEM_IDS, REDEEM_SETTINGS
from Utils.common_dicts import get_update_channel_redeem_dict_from_dict
from Utils.lakea import send_to_websocket
from Utils.twitch import send_twitch_message, send_twitch_message_looping

PROPERTY_BUFFER = {}

def start(data: dict):
    data["target"] = check_target_value_for_conversion(data["target"])
    data_to_send = create_data_to_send(data, True)
    send_to_websocket(data_to_send, "UpdateChannelRedeem")
    send_twitch_message_looping(data["looping_message"])
    time.sleep(int(data["time"]))
    send_twitch_message(data["lakea_message"])
    data_to_send = create_data_to_send(data, False)
    send_to_websocket(data_to_send, "UpdateChannelRedeem")

def check_target_value_for_conversion(data: dict) -> dict:
    if data["property"] == "Cost" or data["property"] == "MaxPerStream" or data["property"] == "MaxPerUserPerStream" or data["property"] == "GlobalCooldownSeconds":
        data["value"] = int(data["value"])
    elif data["property"] == "IsEnabled" or data["property"] == "IsUserInputRequired" or data["property"] == "IsMaxPerStreamEnabled" or data["property"] == "IsMaxPerUserPerStreamEnabled" or data["property"] == "IsGlobalCooldownEnabled" or data["property"] == "ShouldRedemptionsSkipRequestQueue":
        prop = data["property"].upper()
        if prop == "TRUE":
            data["value"] = True
        else:
            data["value"] = False
    return data

def create_data_to_send(data: dict, edit: bool) -> dict:
    global PROPERTY_BUFFER
    redeem_id = REDEEM_IDS[data["redeem"]]
    redeem_data = REDEEM_SETTINGS[redeem_id]
    if edit:
        PROPERTY_BUFFER.update({"Key": data['target']['property']})
        PROPERTY_BUFFER.update({"Value": redeem_data[data['target']['property']]})
        redeem_data.update({data['target']['property']: data['target']['value']})
    else:
        redeem_data.update({PROPERTY_BUFFER['Key']: PROPERTY_BUFFER['Value']})
    return get_update_channel_redeem_dict_from_dict(redeem_id, redeem_data)



if __name__ == "__main__":
    data = {
        "redeem": sys.argv[1],
        "time": sys.argv[4],
        "looping_message": sys.argv[5],
        "lakea_message": sys.argv[6],
        "target": {
            "property": sys.argv[2],
            "value": sys.argv[3]
        }
    }
    start(data)