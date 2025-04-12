import _thread, math, random, string, sys
from Utils.obs import create_source, set_source_transform, get_source_id, set_source_activity, remove_source_from_scene, set_source_settings
from Utils.twitch import create_twitch_connection, send_twitch_message, get_details_from_message
from Utils.monster_battle_classes import monster
from Utils.common_functions import RESOURCE_FOLDER
from Utils.common_dicts import get_create_channel_redeem_dict, get_event_item_dict, get_event_details_dict, get_event_target_dict, get_event_target_args_dict
from Utils.rope_drawer import draw_rope
from Utils.lakea import send_to_websocket
from PIL import Image, ImageDraw, ImageFont
from time import sleep

TIMER_FULL = 900 # 15 minutes
STEP = 5
RESCUED = False

SOCK = {}
SCENE = "Canvas"
IMAGE_SOURCE = "image_source"
KEEP_ALIVE = True
STREAMER_NAME = ""
STREAMER_PRONOUNS = ""
STREAMER_IMAGES = []
CURRENT_IMAGE = -1
STEP_IMAGE = -1.0
ENEMY = ""
ENEMY_ID = ""
ENEMY_COUNT = -1
MESSAGE_TRACKER = 0.7
CHANNEL_REDEEM_ID = ""

HANGING_ROPES_SOURCE_ID = -1
ROPE_SOURCE_ID = -1
BARREL_SOURCE_ID = -1
STREAMER_SOURCE_ID = -1
CAGE_SOURCE_ID = -1
FLAME_SOURCE_ID = -1
EXPLOSION_SOURCE_ID = -1
GRAVE_SOURCE_ID = -1

def socket_loop():
    while KEEP_ALIVE is True:
        resp = SOCK.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            SOCK.send("PONG\n".encode('utf-8'))
        else:
            details = get_details_from_message(resp)
            check_message(details)

def start_timer():
    global KEEP_ALIVE
    _thread.start_new_thread(socket_loop, ())
    timer = TIMER_FULL
    while timer > 0 and RESCUED == False:
        sleep(STEP)
        timer -= STEP
        percent = timer / TIMER_FULL
        draw_rope_length(percent, True)
        set_streamer_image(percent)
        send_progress_messages(percent)
    KEEP_ALIVE = False
    monster_battle_reset()
    if RESCUED == True:
        streamer_rescued()
    else:
        streamer_not_rescue()

def send_progress_messages(percent):
    global MESSAGE_TRACKER
    if percent <= MESSAGE_TRACKER:
        match MESSAGE_TRACKER:
            case 0.7:
                send_twitch_message(f"{STREAMER_NAME} still needs rescuing! Don't forget to save {STREAMER_PRONOUNS}!")
                MESSAGE_TRACKER = 0.4
            case 0.4:
                send_twitch_message(f"Time is running out for {STREAMER_NAME} rangers! Save {STREAMER_PRONOUNS} before its too late!")
                MESSAGE_TRACKER = 0.1
            case 0.1:
                send_twitch_message(f"{STREAMER_NAME} has only a few minutes left, hurry up rangers!")
                MESSAGE_TRACKER = -1

def check_message(details):
    global KEEP_ALIVE, ENEMY_COUNT, RESCUED
    if details["username"] == "lakeamoonlight":
        message = details["message"]
        if "fought" in message and "won" in message:
            message = message.lower()
            enemy = ENEMY.lower()
            if enemy in message:
                ENEMY_COUNT -= 1
                if ENEMY_COUNT <= 0:
                    RESCUED = True

def streamer_rescued():
    remove_source_from_scene(CAGE_SOURCE_ID, SCENE)
    remove_source_from_scene(FLAME_SOURCE_ID, SCENE)
    remove_source_from_scene(HANGING_ROPES_SOURCE_ID, SCENE)
    send_twitch_message(f"Great job rangers! You rescued {STREAMER_NAME}!")
    sleep(10)
    remove_source_from_scene(BARREL_SOURCE_ID, SCENE)
    remove_source_from_scene(ROPE_SOURCE_ID, SCENE)
    remove_source_from_scene(STREAMER_SOURCE_ID, SCENE)
    remove_source_from_scene(EXPLOSION_SOURCE_ID, SCENE)
    remove_source_from_scene(GRAVE_SOURCE_ID, SCENE)

def streamer_not_rescue():
    remove_source_from_scene(ROPE_SOURCE_ID, SCENE)
    remove_source_from_scene(BARREL_SOURCE_ID, SCENE)
    remove_source_from_scene(STREAMER_SOURCE_ID, SCENE)
    remove_source_from_scene(CAGE_SOURCE_ID, SCENE)
    remove_source_from_scene(FLAME_SOURCE_ID, SCENE)
    remove_source_from_scene(HANGING_ROPES_SOURCE_ID, SCENE)
    set_source_activity(EXPLOSION_SOURCE_ID, SCENE, True)
    send_twitch_message(f"Oh for gods sake rangers! How could you let that happen to {STREAMER_NAME}!?!")
    sleep(2)
    set_source_activity(GRAVE_SOURCE_ID, SCENE, True)
    remove_source_from_scene(EXPLOSION_SOURCE_ID, SCENE)
    sleep(10)
    remove_source_from_scene(GRAVE_SOURCE_ID, SCENE)

def start(value):
    global SOCK, STREAMER_NAME, STREAMER_PRONOUNS, STREAMER_IMAGES, ENEMY, ENEMY_ID, ENEMY_COUNT, STEP_IMAGE, CURRENT_IMAGE
    SOCK = create_twitch_connection()
    parts = value.split("|")
    STREAMER_NAME = parts[2]
    STREAMER_IMAGES = parts[3].split(",")
    STEP_IMAGE = 1 / len(STREAMER_IMAGES)
    CURRENT_IMAGE = 0
    STREAMER_PRONOUNS = parts[4]
    enemy_parts = parts[5].split("-")
    ENEMY_ID = enemy_parts[0]
    ENEMY = string.capwords(enemy_parts[1].replace("_", " "))
    if "-" in parts[6]:
        numbers = parts[6].split("-")
        ENEMY_COUNT = random.randint(int(numbers[0]), int(numbers[1]))
    else:
        ENEMY_COUNT = int(parts[6])
    draw_rope_length(1)
    monster_rating = monster_battle_setup()
    create_obs_sources(STREAMER_IMAGES[CURRENT_IMAGE], monster_rating)
    send_twitch_message(f"{STREAMER_NAME} has be kidnapped by {ENEMY}s! Go rescue {STREAMER_PRONOUNS} before the {ENEMY}s decide to get rid of {STREAMER_PRONOUNS}!")
    start_timer()

def set_streamer_image(percent):
    global CURRENT_IMAGE
    progress = 1 - percent
    threshold = (CURRENT_IMAGE + 1) * STEP_IMAGE
    if progress >= threshold:
        CURRENT_IMAGE += 1
        if CURRENT_IMAGE < len(STREAMER_IMAGES):
            input_settings = {
                "file": f"{RESOURCE_FOLDER}{STREAMER_IMAGES[CURRENT_IMAGE]}"
            }
            set_source_settings("StreamerImage", input_settings)

def draw_rope_length(percent, move_fire = False):
    point_1 = (1750, 225)
    point_2 = (1600, 135)
    point_3 = (27, 135)
    distance_1 = math.dist(point_1, point_2)
    distance_2 = math.dist(point_2, point_3)
    total_distance = distance_1 + distance_2
    actual_distance = total_distance * percent
    im = Image.new('RGBA', (1920, 1080), (0, 0, 0, 0))
    if actual_distance >= distance_1:
        new_distance = actual_distance - distance_1
        new_point = (point_2[0] - new_distance, point_2[1])
        im = draw_rope(im, point_1, point_2, 8.0)
        im = draw_rope(im, point_2, new_point, 8.0)
        if move_fire == True:
            flame_position = {
                "positionX": (new_point[0] - 65),
                "positionY": (new_point[1] - 60)
            }
            set_source_transform(FLAME_SOURCE_ID, SCENE, flame_position)
    else:
        line_angle_radians = math.atan2(point_2[1] - point_1[1], point_2[0] - point_1[0])
        x_offset = actual_distance * math.cos(line_angle_radians)
        y_offset = actual_distance * math.sin(line_angle_radians)
        new_point = (point_1[0] + x_offset, point_1[1] + y_offset)
        im = draw_rope(im, point_1, new_point, 8.0)
        if move_fire == True:
            flame_position = {
                "positionX": (new_point[0] - 65),
                "positionY": (new_point[1] - 60)
            }
            set_source_transform(FLAME_SOURCE_ID, SCENE, flame_position)
    draw = ImageDraw.Draw(im)
    font = ImageFont.truetype(f"{RESOURCE_FOLDER}Fonts\\Cataneo_BT_Bold.ttf", 35)
    draw.text((1710, 310), f"{ENEMY_COUNT} {ENEMY}s\nRemaining!", (255, 255, 255), font)
    im.save(f"{RESOURCE_FOLDER}Ropes.png", quality=95)

def create_obs_sources(streamer_image, monster_rating):
    global HANGING_ROPES_SOURCE_ID, ROPE_SOURCE_ID, BARREL_SOURCE_ID, STREAMER_SOURCE_ID, CAGE_SOURCE_ID, FLAME_SOURCE_ID, EXPLOSION_SOURCE_ID, GRAVE_SOURCE_ID
    im = Image.new('RGBA', (1920, 1080), (0, 0, 0, 0))
    im = draw_rope(im, (1630, -50), (1730, 30), 12.0)
    im = draw_rope(im, (2000, -75), (1900, 30), 12.0)
    im = draw_rope(im, (1750, 190), (1750, 300), 8.0)
    im = draw_rope(im, (1875, 190), (1875, 300), 8.0)
    draw = ImageDraw.Draw(im)
    font = ImageFont.truetype(f"{RESOURCE_FOLDER}Fonts\\Cataneo_BT_Bold.ttf", 20)
    draw.text((1705, 395), f"({monster_rating})", (255, 255, 255), font)
    im.save(f"{RESOURCE_FOLDER}HangingRopes.png", quality=95)    
    ropes_image_data = get_default_image_data("Ropes", "Ropes.png")
    barrel_image_data = get_default_image_data("TNTBarrel", "TNTBarrel.png")
    hanging_ropes_image_data = get_default_image_data("HangingRopes", "HangingRopes.png")
    streamer_image_data = get_default_image_data("StreamerImage", streamer_image)
    cage_image_data = get_default_image_data("Cage", "Cage.png")
    flame_image_data = get_default_image_data("Flame", "Flame.gif")
    grave_image_data = get_default_image_data("Grave", "GraveStone.png")
    explosion_image_data = get_default_image_data("Explosion", "Explosion.png")
    ROPE_SOURCE_ID = create_full_obs_source(ropes_image_data, False)
    BARREL_SOURCE_ID = create_full_obs_source(barrel_image_data, False)
    HANGING_ROPES_SOURCE_ID = create_full_obs_source(hanging_ropes_image_data, False)
    STREAMER_SOURCE_ID = create_full_obs_source(streamer_image_data, False)
    CAGE_SOURCE_ID = create_full_obs_source(cage_image_data, False)
    FLAME_SOURCE_ID = create_full_obs_source(flame_image_data, False)
    GRAVE_SOURCE_ID = create_full_obs_source(grave_image_data, False)
    EXPLOSION_SOURCE_ID = create_full_obs_source(explosion_image_data, False)
    set_source_activity(HANGING_ROPES_SOURCE_ID, SCENE, True)
    set_source_activity(ROPE_SOURCE_ID, SCENE, True)
    set_source_activity(BARREL_SOURCE_ID, SCENE, True)
    set_source_activity(STREAMER_SOURCE_ID, SCENE, True)
    set_source_activity(CAGE_SOURCE_ID, SCENE, True)
    set_source_activity(FLAME_SOURCE_ID, SCENE, True)

def create_full_obs_source(details, active):
    create_source(details["source_name"], details["source_kind"], SCENE, details["source_file"], False)
    streamer_image_source_id = get_source_id(details["source_name"], SCENE)
    set_source_transform(streamer_image_source_id, SCENE, details["position"])
    if active == True:
        set_source_activity(streamer_image_source_id, SCENE, True)
    return streamer_image_source_id

def monster_battle_setup():
    global CHANNEL_REDEEM_ID
    channel_redeem_dict = get_create_channel_redeem_dict(
        f"Rescue {STREAMER_NAME}!",
        f"Rescue {STREAMER_NAME} from the {ENEMY}s before its too late!",
        25,
        colour="#FF0000",
        max_per_user_per_stream_enabled=True,
        max_per_user_per_stream_amount=3
    )
    response = send_to_websocket(channel_redeem_dict, "CreateChannelRedeem")
    CHANNEL_REDEEM_ID = response["Data"][0]["Id"]
    lakea_event_details = get_event_details_dict("Rescue Streamer Minigame", "Twitch", "Twitch_Redeem", CHANNEL_REDEEM_ID)
    lakea_event_arg_1 = get_event_target_args_dict("Type", "SPECIFICMONSTER")
    lakea_event_arg_2 = get_event_target_args_dict("Arg1", f"{ENEMY_ID}-{ENEMY}")
    lakea_event_target = get_event_target_dict("Battle_Simulator", "Battle_Simulator_Encounter", [lakea_event_arg_1, lakea_event_arg_2])
    lakea_event = get_event_item_dict(CHANNEL_REDEEM_ID, lakea_event_details, lakea_event_target)
    send_to_websocket(lakea_event, "AddEvent")
    channel_redeem_dict.update({"IsEnabled": True})
    update_redeem = {
        "RedeemID": CHANNEL_REDEEM_ID,
        "RedeemData": channel_redeem_dict
    }
    send_to_websocket(update_redeem, "UpdateChannelRedeem")
    monster_name = ENEMY.upper().replace(" ", "_")
    mon = monster(f"{ENEMY_ID}-{monster_name}")
    return_string = ""
    match mon.rating:
        case "WEAK":
            return_string = f"Weak Monster - lvl {str(mon.level)}"
        case "NORMAL":
            return_string = f"Normal Monster - lvl {str(mon.level)}"
        case "HARD":
            return_string = f"Hard Monster - lvl {str(mon.level)}"
        case "MISC":
            return_string = f"Random Monster - lvl {str(mon.level)}"
        case _:
            return_string = "Something Broke!"
    return return_string

def monster_battle_reset():
    data = {
        "Key": CHANNEL_REDEEM_ID,
        "EventItem": {
            "EventDetails": {
                "Source": "Twitch",
                "Type": "Twitch_Redeem"
            }
        }
    }
    send_to_websocket(CHANNEL_REDEEM_ID, "DeleteChannelRedeem", to_json=False)
    #send_to_websocket(data, "RemoveEvent")

def get_default_image_data(image, file):
    match image:
        case "StreamerImage":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 1730,
                    "positionY": 25,
                    "scaleX": 1.5,
                    "scaleY": 1.5
                }
            }
        case "Cage":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 1725,
                    "positionY": 20,
                    "scaleX": 0.36,
                    "scaleY": 0.36
                }
            }
        case "TNTBarrel":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 1727,
                    "positionY": 150,
                    "scaleX": 0.275,
                    "scaleY": 0.275
                }
            }
        case "Flame":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": -35,
                    "positionY": 75,
                    "scaleX": 0.25,
                    "scaleY": 0.25
                }
            }
        case "Explosion":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 1580,
                    "positionY": -80,
                    "scaleX": 1,
                    "scaleY": 1
                }
            }
        case "Grave":
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 1730,
                    "positionY": 40,
                    "scaleX": 0.165,
                    "scaleY": 0.165
                }
            }            
        case _:
            return {
                "source_name": image,
                "source_kind": "image_source",
                "source_file": {
                    "file": f"{RESOURCE_FOLDER}{file}"
                },
                "position": {
                    "positionX": 0,
                    "positionY": 0,
                }
            }
        


if __name__ == '__main__':
    event = ""
    for arg in sys.argv:
        event = f"{event}|{arg}"    
    start(event)
