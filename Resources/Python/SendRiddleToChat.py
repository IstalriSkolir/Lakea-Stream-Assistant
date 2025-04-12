import sys, random, _thread
from time import sleep
from Utils.twitch import send_twitch_message, create_twitch_connection, get_details_from_message

WAIT_TIME = 120
CHAT_ANSWERED = False
KEEP_ALIVE = True
RIDDLE = {}
USER = ""        

def start():
    global RIDDLE
    user = ""
    if len(sys.argv) > 1:
        user = sys.argv[1]
    RIDDLE = get_random_riddle()
    if user != "":
        send_twitch_message(f"Here's a riddle for you @{user}. {RIDDLE['riddle']}")
    else:
        send_twitch_message(f"Here's a riddle for you all. {RIDDLE['riddle']}")
    _thread.start_new_thread(socket_loop, ())
    timer = WAIT_TIME
    while timer > 0 and CHAT_ANSWERED == False:
        timer -= 1
        sleep(1)
    if CHAT_ANSWERED == True:
        send_twitch_message(f"@{USER} is correct! The answer was {RIDDLE['answer']}")
    else:
        send_twitch_message(f"No one got the answer, the answer was {RIDDLE['answer']}")

def get_random_riddle() -> dict:
    keys = list(RIDDLES.keys())
    key = random.choice(keys)
    return RIDDLES[key]

def socket_loop():
    global KEEP_ALIVE
    sock = create_twitch_connection()
    while KEEP_ALIVE is True:
        resp = sock.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            sock.send("PONG\n".encode('utf-8'))
        else:
            details = get_details_from_message(resp)
            check_message(details)

def check_message(details: dict):
    global CHAT_ANSWERED, KEEP_ALIVE, USER
    message = details["message"].lower()
    if RIDDLE["check"] in message:
        USER = details["username"]
        CHAT_ANSWERED = True
        KEEP_ALIVE = False

RIDDLES = {
    1: {
        "riddle": "Red as blood, sweet as wine. My heart is hard, but my flash is fine. What am I?",
        "answer": "a cherry",
        "check": "cherry"
    },
    2: {
        "riddle": "I run all day but cannot walk, I have a mouth but cannot talk. What am I?",
        "answer": "a river",
        "check": "river"
    },
    3: {
        "riddle": "I touch the earth, I touch the sky, but if I touch you you'll surely die.",
        "answer": "lightning",
        "check": "lightning"
    },
    4: {
        "riddle": "A sturdy back, legs four, once I lived but no more.",
        "answer": "a chair",
        "check": "chair"
    },
    5: {
        "riddle": "I have an eye but I am blind, I'm cold and hard and never kind. Despite all this tou should still see, there is always a point to me.",
        "answer": "a needle",
        "check": "needle"
    },
    6: {
        "riddle": "My feet are warm, my head is cold, I never move since I'm so old.",
        "answer": "a mountain",
        "check": "mountain"
    },
    7: {
        "riddle": "Ten mens strength, ten mens length, ten men can't break it, yet a young boy walks off with it.",
        "answer": "rope",
        "check": "rope"
    },
    8: {
        "riddle": "It has a golden head, it has a golden tail, and yet it has no body.",
        "answer": "a gold coin",
        "check": "gold coin"
    },
    9: {
        "riddle": "Better old than young; the healther it is, the sameller it will be.",
        "answer": "a wound",
        "check": "wound"
    },
    10: {
        "riddle": "As I was going to St Ives I met a man with 7 wives. Each wife had 7 kids. Each kid had 7 cats. Each cat had 7 kittens. How many were going to St Ives?",
        "answer": "1",
        "check": "1"
    },
    11: {
        "riddle": "Towns without houses, forests without trees, mountains without boulders and waterless seas.",
        "answer": "a map",
        "check": "map"
    },
    12: {
        "riddle": "What is so fragile, even speaking its name will break it?",
        "answer": "silence",
        "check": "silence"
    },
    13: {
        "riddle": "It can pierce the best armour, and make swordscrumble with a rub. Yet for all its power, it can't harm a club.",
        "answer": "rust",
        "check": "rust"
    },
    14: {
        "riddle": "Tall I am young, short I am old, while with life I do glow, the wind is my foe. What am I?",
        "answer": "a candle",
        "check": "candle"
    },
    15: {
        "riddle": "What comes once in a minute, twice in a moment, but never in a thousand years?",
        "answer": "The answer to your riddle was the letter 'm'",
        "check" :  "the letter m"
    },
    16: {
        "riddle": "It is the beginning of eternity, the end of time and space, the beginning of the end and the end of every space.",
        "answer": "letter 'e'!",
        "check": "the letter e"
    },
    17: {
        "riddle": "What runs around a city but never moves?",
        "answer": "a wall",
        "check": "wall"
    },
    18: {
        "riddle": "Double my number, I'm less than a score. Half of my number is less than four. Add one to my double when bakers are near. Days of the week are still greater, I fear.",
        "answer": "6",
        "check": "6"
    },
    19: {
        "riddle": "A box without hinges, key or lid and yet golden treasure within is hid.",
        "answer": "an egg",
        "check": "egg"
    },
    20: {
        "riddle": "If you feed me, I will live, but if you make me drink, I will die. What am I?",
        "answer": "fire",
        "check": "fire"
    },
    21: {
        "riddle": "The more you leave it behind, the more you take. What am I?",
        "answer": "footsteps",
        "check": "footsteps"
    },
    22: {
        "riddle": "What can go around the wood but can never go inside the wood?",
        "answer": "the tree bark",
        "check": "tree bark"
    },
    23: {
        "riddle": "What falls but never breaks, and what breaks but never falls?",
        "answer": "night and day",
        "check": "night and day"
    },
    24: {
        "riddle": "Clash blade and arrow upon my face, and with my sturdy brow I'll brace, the blows of mighty sword, axe and mace. My brothers in war are weapons of steel, but never a killing blow I'll deal, it's only the strikes of others I feel. My duty is a true protection, so wield me in your foe's direction, and let their blades taste my rejection.",
        "answer": "a shield",
        "check": "shield"
    },
    25: {
        "riddle": "The more there is, the less you see. What is it?",
        "answer": "darkness",
        "check": "darkness"
    },
    26: {
        "riddle": "Red when born, oil makes me dull. Shiny when old, water makes me red. What am I?",
        "answer": "a sword",
        "check": "sword"
    },
    27: {
        "riddle": "The ocean is my mother, and my children lakes and streams. Sometimes clothed in silver and gold, many still insist I'm dull. Though I'm void of any colour, I give life to all that's green.",
        "answer": "clouds",
        "check": "clouds"
    },
    28: {
        "riddle": "Four siblings born together, though more different they could not be. The first runs and never wearies. The second eats and is never full. The third drinks and is always thirsty. The fourth sings a song so shrill.",
        "answer": "water, fire, earth and wind",
        "check": "water, fire, earth and wind"
    },
    29: {
        "riddle": "Brought to the table. Cut and served. Never eaten. What am I?",
        "answer": "a deck of playing cards",
        "check": "deck of playing cards"
    },
    30: {
        "riddle": "You write me with five letters, but seven I contain. I have keys but no locks, keep time but no clocks.",
        "answer": "music",
        "check": "music"
    },
    31: {
        "riddle": "When you don't need me, you hold me close. But when you need me, you have to drop me. What am I?",
        "answer": "a anchor",
        "check": "anchor"
    },
    32: {
        "riddle": "A thousand coloured folds stretch toward the sky, atop a tender strand, rising from the land, 'til killed by maiden's hand, perhaps a token of love, perhaps to say goodbye.",
        "answer": "a flower",
        "check": "flower"
    },
    33: {
        "riddle": "Alive without breath, as cold as death, clad in mail never clinking, never thirsty, ever drinking",
        "answer": "a fish",
        "check": "fish"
    },
    34: {
        "riddle": "Often held but never touched, always wet but never rusts, often bites but seldom bit, to use me well you must have wit.",
        "answer": "your tongue",
        "check": "your tongue"
    },
    35: {
        "riddle": "Three lives have I. Gentle enough to soothe the skin, light enough to caress the sky, hard enough to crack rocks.",
        "answer": "water",
        "check": "water"
    },
    36: {
        "riddle": "A father's child, a mother's child, yet no one's son. Who am I?",
        "answer": "a daughter (Though any childs gender besides 'son' is works)",
        "check": "daughter"
    },
    37: {
        "riddle": "I don't have eyes, but once I did see. I once had thoughts, now white and empty.",
        "answer": "a skull",
        "check": "skull"
    },
    38: {
        "riddle": "The more you take away, the bigger I become.",
        "answer": "a hole",
        "check": "hole"
    },
    39: {
        "riddle": "I am black when bought, red when used, then white when disposed of. What am I?",
        "answer": "coal",#"or charcoal",
        "check": "coal"
    },
    40: {
        "riddle": "What has to be broken before it can be used?",
        "answer": "an egg",
        "check": "egg"
    },
    41: {
        "riddle": "What is full of holes but still holds water?",
        "answer": "a sponge",
        "check": "sponge"
    },
    42: {
        "riddle": "I have branches, but no fruit, trunk or leaves. What am I?",
        "answer": "a bank",
        "check": "bank"
    }
}



if __name__ == '__main__':
    start()
