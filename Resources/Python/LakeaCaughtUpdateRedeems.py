import sys
from time import sleep
from Utils.sensitive_data import REDEEM_IDS, REDEEM_SETTINGS
from Utils.common_dicts import get_update_channel_redeem_dict_from_dict
from Utils.common_functions import reset_all_twitch_redeems
from Utils.lakea import send_to_websocket

def start(mode: str):
    if mode == "LAKEACAUGHT":
        lakea_caught()
    elif mode == "LAKEARELEASED":
        reset_all_twitch_redeems()

def lakea_caught():
    catch_lakea_redeem_id = REDEEM_IDS["CATCHLAKEA"]
    redeem = REDEEM_SETTINGS[catch_lakea_redeem_id]
    redeem.update({"IsEnabled": False})
    data = get_update_channel_redeem_dict_from_dict(catch_lakea_redeem_id, redeem)
    send_to_websocket(data, "UpdateChannelRedeem")
    sleep(0.2)
    for obj in REDEEM_EDITS:
        redeem = REDEEM_SETTINGS[obj['id']]
        redeem.update({"Title": obj['name']})
        redeem.update({"Cost": obj['cost']})
        redeem.update({"Prompt": obj['description']})
        data = get_update_channel_redeem_dict_from_dict(obj['id'], redeem)
        send_to_websocket(data, "UpdateChannelRedeem")
        sleep(0.25)

REDEEM_EDITS = [
    {
        "name": "Feed The Fire - Lakeas Caught!",
        "id": REDEEM_IDS["FEEDTHEFIRE"],
        "cost": 10,
        "description": "Lakea is caught, she can't help you with this right now!"
    },
    {
        "name": "Feed The Fire X3 - Lakeas Caught!",
        "id": REDEEM_IDS["FEEDTHEFIREX3"],
        "cost": 10,
        "description": "Lakea is caught, she can't help you with this right now!"
    },
    {
        "name": "Chop Firewood - Lakeas Caught!",
        "id": REDEEM_IDS["CHOPFIREWOOD"],
        "cost": 10,
        "description": "Lakea is caught, she can't help you with this right now!"
    },
    {
        "name": "Train With Lakea - Lakeas Caught!",
        "id": REDEEM_IDS["TRAINWITHLAKEA"],
        "cost": 10,
        "description": "Lakea can hardly train you when she's tied up like that!"
    },
    {
        "name": "Wolf Howl - Lakeas Caught!",
        "id": REDEEM_IDS["WOLFHOWL"],
        "cost": 10,
        "description": "I don't think Lakea is interested in getting wolves to howl right now..."
    },
    {
        "name": "Fight A Weak Monster - Lakeas Caught!",
        "id": REDEEM_IDS["FIGHTWEAKMONSTER"],
        "cost": 10,
        "description": "Lakea can't help you fight while caught!"
    },
    {
        "name": "Fight A Normal Monster - Lakeas Caught!",
        "id": REDEEM_IDS["FIGHTNORMALMONSTER"],
        "cost": 10,
        "description": "Lakea can't help you fight while caught!"
    },
    {
        "name": "Display Random Art - Lakeas Caught!",
        "id": REDEEM_IDS["DISPLAYRANDOMART"],
        "cost": 10,
        "description": "How's she meant to display the art when your all holding her captive?"
    },
    {
        "name": "Fight A Random Monster - Lakeas Caught!",
        "id": REDEEM_IDS["FIGHTRANDOMMONSTER"],
        "cost": 10,
        "description": "Lakea can't help you fight while caught!"
    },
    {
        "name": "Fight A Hard Monster - Lakeas Caught!",
        "id": REDEEM_IDS["FIGHTHARDMONSTER"],
        "cost": 10,
        "description": "Lakea can't help you fight while caught!"
    },
    {
        "name": "Boss Battle - Lakeas Caught!",
        "id": REDEEM_IDS["BOSSBATTLE"],
        "cost": 10,
        "description": "Sure, you tie Lakea to a tree then decide you want to fight her... scared much?"
    },
    {
        "name": "Boop Me - Lakeas Caught!",
        "id": REDEEM_IDS["BOOPME"],
        "cost": 10,
        "description": "Lakea can't help you boop me while caught!"
    },
    {
        "name": "Throw A Dagger - Lakeas Caught!",
        "id": REDEEM_IDS["THROWADAGGER"],
        "cost": 10,
        "description": "Lakea would probably prefer to keep her dagger right now. Since, you know, it would help her out of her tight spot..."
    },
    {
        "name": "Tea Time - Lakeas Caught!",
        "id": REDEEM_IDS["TEATIME"],
        "cost": 10,
        "description": "Lakea can't make tea while caught!"
    },
    {
        "name": "Catch Me - Lakeas Caught!",
        "id": REDEEM_IDS["CATCHME"],
        "cost": 10,
        "description": "I'm sure Lakea would love to trade places right now, you should have caught me before going after her!"
    },
    {
        "name": "Collar - Lakeas Caught!",
        "id": REDEEM_IDS["COLLAR"],
        "cost": 10,
        "description": "...lets not discuss this one while Lakea's tied up"
    },
    {
        "name": "Moustached Wolf - Lakeas Caught!",
        "id": REDEEM_IDS["MOUSTACHEDWOLF"],
        "cost": 10,
        "description": "Lakea is caught, she can't help you with this right now!"
    }
]



if __name__ == "__main__":
    mode = sys.argv[1]
    start(mode)