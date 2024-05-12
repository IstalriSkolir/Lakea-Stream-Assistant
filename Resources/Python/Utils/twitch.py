import socket

SERVER = "irc.chat.twitch.tv"
PORT = 6667
NICKNAME = "{NICKNAME}"
TOKEN = "{TOKEN}"
CHANNEL = "{CHANNEL}"

SOCK = {}
INITIALISED = False

def create_connection():
    global INITIALISED, SOCK
    SOCK = socket.socket()    
    SOCK.connect((SERVER, PORT))
    SOCK.send(f"PASS {TOKEN}\n".encode('utf-8'))
    SOCK.send(f"NICK {NICKNAME}\n".encode('utf-8'))
    SOCK.send(f"JOIN {CHANNEL}\n".encode('utf-8'))
    INITIALISED = True
    return SOCK

def get_socket():
    return SOCK

def send_twitch_message(message):
    global SOCK
    if INITIALISED == False:
        SOCK = create_connection()
    SOCK.send(f"PRIVMSG {CHANNEL} :{message}\n".encode('utf-8'))

def get_details_from_message(message):
    parts = message.split(":")
    username = parts[1].split("!")[0]
    parts2 = parts[1].split("#")
    channel = ""
    if len(parts2) > 1:
        channel = parts2[1]
    message = parts[2][:-2]
    details = {
        "username": username,
        "channel": channel,
        "message": message
    }
    return details