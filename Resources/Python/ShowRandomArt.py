from Utils.obs import create_source, get_source_id, set_source_transform, set_source_activity, remove_source_from_scene
from Utils.twitch import send_twitch_message
from time import sleep
import random

SCENE = "Canvas"
VIEW_TIME = 30

def start():
    art = get_random_art_object()
    source_id = create_art_scene_item(art)
    send_twitch_message(art["chat_message"])
    sleep(VIEW_TIME)
    remove_source_from_scene(source_id, SCENE)

def get_random_art_object():
    keys = list(ART_DICT.keys())
    key = random.choice(keys)
    return ART_DICT[key]

def create_art_scene_item(art):
    create_source("RandomArt", "image_source", SCENE, art["source_file"], False)
    source_id = get_source_id("RandomArt", SCENE)
    set_source_transform(source_id, SCENE, art["transform"])
    set_source_activity(source_id, SCENE, True)
    return source_id

ART_DICT = {
    1: {
        "chat_message": "Materies may act like a daft sod sometimes, but he's not one you want to cross blades with in anger! This amazing artwork of Materies was done by Liminal Gender! Go check out their Twitter: https://twitter.com/liminal_gender",
        "transform": {
            "positionX": 0,
            "positionY": 415,
            "scaleX": 0.31,
            "scaleY": 0.31
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\1.jpeg"
        }
    },
    2: {
        "chat_message": "It's about time we got some art of me, my turn to get some attention! This beautiful artwork was done by Gretactic! Go check out their Twitter: https://twitter.com/Gretactic",
        "transform": {
            "positionX": 0,
            "positionY": 533,
            "scaleX": 0.365,
            "scaleY": 0.365
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\2.png"
        }
    },
    3: {
        "chat_message": "Loopings always up to trouble of some kind, it's good that he's so skilled in stealth or someone would have knocked some sense into him by now! This amazing artwork was done by Gretactic! Go check out their Twitter: https://twitter.com/Gretactic",
        "transform": {
            "positionX": 0,
            "positionY": 533,
            "scaleX": 0.365,
            "scaleY": 0.365
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\3.png"
        }
    },
    4: {
        "chat_message": "I love the peace and tranquility of the forests, you could never get the same sense of peace and freedom in a city. This beautiful artwork was done by Gretactic! Go check out their Twitter: https://twitter.com/Gretactic",
        "transform": {
            "positionX": 0,
            "positionY": 564,
            "scaleX": 0.495,
            "scaleY": 0.495
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\4.png"
        }
    },
    5: {
        "chat_message": "It's not often these two stop from their adventures, this serene artwork of Materies and Looping was done by Gretactic! Go check out their Twitter: https://twitter.com/Gretactic",
        "transform": {
            "positionX": 0,
            "positionY": 552,
            "scaleX": 0.54,
            "scaleY": 0.54
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\5.png"
        }
    },
    6: {
        "chat_message": "This was the first piece of art that Materies ever commissioned, for D&amp;D with Looping Coil! This piece was done by Augmented Waffles! Go check out their Instagram: https://www.instagram.com/augmentedwaffles/",
        "transform": {
            "positionX": 0,
            "positionY": 444,
            "scaleX": 0.177,
            "scaleY": 0.177
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\6.png"
        }
    },
    7: {
        "chat_message": "From Materies's travels, sights like these are part of the reason he does what he does. This stellar artwork was a YCH done by XelArtz! Go check out their Twitter: https://twitter.com/xel_artz",
        "transform": {
            "positionX": 0,
            "positionY": 559,
            "scaleX": 0.242,
            "scaleY": 0.242
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\7.png"
        }
    },
    8: {
        "chat_message": "Materies should know better than to stand on a open cliff in a storm, but he never listens to his own advice! This stormy artwork was a YCH done by XelArtz! Go check out their Twitter: https://twitter.com/xel_artz",
        "transform": {
            "positionX": 0,
            "positionY": 559,
            "scaleX": 0.242,
            "scaleY": 0.242
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\8.png"
        }
    },
    9: {
        "chat_message": "The great Crab War of Whiterun, that was a messy time during the adventures in Skyrim! This amazing piece was done by Meta_The_Cat!",
        "transform": {
            "positionX": 0,
            "positionY": 590,
            "scaleX": 0.55,
            "scaleY": 0.55
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\9.png"
        }
    },
    10: {
        "chat_message": "Materies out on patrol in the woodlands, keeping the woodlands safe! This incredible piece was done by Fram! Go check out her Instagram: https://www.instagram.com/insta_fram__",
        "transform": {
            "positionX": 0,
            "positionY": 535,
            "scaleX": 0.35,
            "scaleY": 0.35
        },
        "source_file": {
            "file": "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\RandomArt\\10.png"
        }
    }
}



start()