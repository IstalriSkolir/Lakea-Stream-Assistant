from obswebsocket import obsws, requests
from time import sleep
import socket

EVENT_TIME = 945 # 15 minutes + 45 seconds for setup
CHAT_MESSAGE = "That's it, play time is over rangers!"

IP = "localhost"
PORT = "4455"
PASSWORD = "ZIrQmsOMYQs1AXut"
CURRENT_SCENE = "Woodland"
NEXT_SCENE = "Ranger Rebellion"

IP2 = "192.168.0.32"
PORT2 = "4455"
PASSWORD2 = "N0FbTW94Biy8vwLT"
RETURN_SCENE = "Game Scene - Solo"

SOCK = socket.socket()
SERVER = 'irc.chat.twitch.tv'
SERVER_PORT = 6667
NICKNAME = 'materiescoil'
TOKEN = 'oauth:k3l0ay660z64ehm6cr5ic92ud49un2'
CHANNEL = '#materiescoil'

def start():
    try:
        start_ranger_rebellion()
        sleep(EVENT_TIME)
        end_ranger_rebellion()
    except Exception as error:
        print("Scene Change Failed: " + str(error))

def start_ranger_rebellion():
    print("Change Scene: " + CURRENT_SCENE + " -> " + NEXT_SCENE)
    ws = obsws(IP, PORT, PASSWORD)
    ws.connect()
    ws.call(requests.SetCurrentProgramScene(sceneName=NEXT_SCENE))
    ws.disconnect()
    print("Scene Changed: " + NEXT_SCENE)

def end_ranger_rebellion():
    SOCK.connect((SERVER, SERVER_PORT))
    SOCK.send(f"PASS {TOKEN}\n".encode('utf-8'))
    SOCK.send(f"NICK {NICKNAME}\n".encode('utf-8'))
    SOCK.send(f"JOIN {CHANNEL}\n".encode('utf-8'))
    SOCK.send(f"PRIVMSG {CHANNEL} :{CHAT_MESSAGE}\n".encode('utf-8'))

    sleep(3)

    ws = obsws(IP, PORT, PASSWORD)
    ws.connect()
    ws.call(requests.SetCurrentProgramScene(sceneName=CURRENT_SCENE))
    ws.disconnect()

    ws = obsws(IP2, PORT2, PASSWORD2)
    ws.connect()
    ws.call(requests.SetCurrentProgramScene(sceneName=RETURN_SCENE))
    ws.disconnect()



start()