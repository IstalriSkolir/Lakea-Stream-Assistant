import sys, json, random
from Utils.bluesky import get_bluesky_client_lakea
from Utils.common_functions import RESOURCE_FOLDER
from atproto import client_utils

JSON_FOLDER = f"{RESOURCE_FOLDER}BlueSky\\"
IMAGES_FOLDER = f"{JSON_FOLDER}Images\\"

def start(mode: str, args: list):
    client = get_bluesky_client_lakea()
    if mode in MODES:
        MODES[mode](client, args)

def boss_beaten_mode(client: object, args: list):
    post_data = get_random_post("BossBeatenPosts")
    post_data = build_post(post_data)
    client.send_post(post_data["Post"]) 

    #client.send_image(text=post_data["Post"], image=post_data["Image"], image_alt=post_data["ImageAltText"])

def monster_hunter_new_high_score_mode(client: object, args: list):
    print("Test 2")

def ranger_rebellion_mode(client: object, args: list):
    print("Test 3")

def get_random_post(file: str) -> dict:
    post_list = get_json_data(file)
    return random.choice(post_list)

def get_json_data(file: str) -> list:
    data = {}
    with open(f"{JSON_FOLDER}{file}.json") as json_file:
        data = json.load(json_file)
    return data["data"]

def build_post(post_data: dict) -> dict:
    post = client_utils.TextBuilder()
    image_data = None
    if "Image" in post_data:
        image_data = open(f"{IMAGES_FOLDER}{post_data['Image']}", "rb").read()
    for part in post_data["Message"]:
        match part["Type"]:
            case "Text":
                post = post.text(part["Text"])
            case "Link":
                post = post.link(part["Text"], part["URL"])
            case _:
                pass
    post_json = {
        "Post": post,
        "Image": image_data,
        "ImageAltText": post_data["ImageAltText"] if "ImageAltText" in post_data else "N/A"
    }
    return post_json

MODES = {
    "BOSS_BEATEN": boss_beaten_mode,
    "MONSTER_HUNTER_NEW_HIGH_SCORE": monster_hunter_new_high_score_mode,
    "RANGER_REBELLION": ranger_rebellion_mode
}



if __name__ == '__main__':
    mode = sys.argv[1]
    args = []
    for x in range(len(sys.argv)):
        if x > 1:
            args.append(sys.argv[x])
    start(mode, args)