import socket
from Utils.sensitive_data import LAKEA_BOT_NICKNAME, LAKEA_BOT_TOKEN, LOOPING_BOT_NICKNAME, LOOPING_BOT_TOKEN, STREAMERS_CHANNEL, STREAMERS_ID

SERVER = "irc.chat.twitch.tv"
PORT = 6667

SOCK = {}
SOCK_LOOPING = {}
INITIALISED = False
INITIALISED_LOOPING = False

def create_twitch_connection():
    global INITIALISED, SOCK
    SOCK = socket.socket()    
    SOCK.connect((SERVER, PORT))
    SOCK.send(f"PASS {LAKEA_BOT_TOKEN}\n".encode('utf-8'))
    SOCK.send(f"NICK {LAKEA_BOT_NICKNAME}\n".encode('utf-8'))
    SOCK.send(f"JOIN {STREAMERS_CHANNEL}\n".encode('utf-8'))
    INITIALISED = True
    return SOCK

def create_looping_twitch_connection():
    global INITIALISED_LOOPING, SOCK_LOOPING
    SOCK_LOOPING = socket.socket()
    SOCK_LOOPING.connect((SERVER, PORT))
    SOCK_LOOPING.send(f"PASS {LOOPING_BOT_TOKEN}\n".encode('utf-8'))
    SOCK_LOOPING.send(f"NICK {LOOPING_BOT_NICKNAME}\n".encode('utf-8'))
    SOCK_LOOPING.send(f"JOIN {STREAMERS_CHANNEL}\n".encode('utf-8'))
    INITIALISED_LOOPING = True
    return SOCK_LOOPING

def create_both_twitch_connections():
    global SOCK, SOCK_LOOPING
    SOCK = create_twitch_connection()
    SOCK_LOOPING = create_looping_twitch_connection()


def get_socket():
    return SOCK

def send_twitch_message(message: str):
    global SOCK
    if INITIALISED == False:
        SOCK = create_twitch_connection()
    SOCK.send(f"PRIVMSG {STREAMERS_CHANNEL} :{message}\n".encode('utf-8'))

def send_twitch_message_looping(message: str):
    global SOCK_LOOPING
    if INITIALISED_LOOPING == False:
        SOCK_LOOPING = create_looping_twitch_connection()
    SOCK_LOOPING.send(f"PRIVMSG {STREAMERS_CHANNEL} :{message}\n".encode('utf-8'))

def get_details_from_message(message: str) -> dict:
    parts = message.split(":")
    username = parts[1].split("!")[0]
    parts2 = parts[1].split("#")
    channel = ""
    if len(parts2) > 1:
        channel = parts2[1].replace(" ", "")
    message = parts[2][:-2]
    details = {
        "username": username,
        "channel": channel,
        "message": message
    }
    return details