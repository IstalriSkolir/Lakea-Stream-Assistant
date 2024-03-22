import os
import socket
import threading
from time import sleep
from pynput.keyboard import Key, Controller

keyboard = Controller()

retro_arch_dir = "C:\RetroArch-Win64"
#retro_arch_exe = r'"retroarch.exe -L "C:\RetroArch-Win64\cores\vba_next_libretro.dll" "C:\Users\mcwol\Documents\Resources\Games\PokemonRuby.gba" -f"'
retro_arch_exe = r'"retroarch.exe -L "C:\RetroArch-Win64\cores\vba_next_libretro.dll" "C:\Users\mcwol\Documents\Resources\Games\PokemonRuby.gba""'

server = 'irc.chat.twitch.tv'
port = 6667
nickname = '{nickname}'
token = '{token}'
channel = '{channel}'
sock = socket.socket()
keep_alive = True

def start():
    global retro_arch_dir, retro_arch_exe
    retro_arch_thread = threading.Thread(target=run_retro_arch, args=[retro_arch_dir, retro_arch_exe])
    retro_arch_thread.start()
    load_save_game()
    sock = create_socket()
    socket_loop(sock)

def run_retro_arch(retro_arch_dir, retro_arch_exe):
    os.chdir(retro_arch_dir)
    os.system(retro_arch_exe)

def load_save_game():
    global keyboard
    sleep(5)
    print("Pressing X")
    keyboard.press('x')
    sleep(0.2)
    print("Releasing X")
    keyboard.release('x')
    sleep(2)
    print("Pressing X")
    keyboard.press('x')
    sleep(0.2)
    print("Releasing X")
    keyboard.release('x')
    sleep(2)
    print("Pressing X")
    keyboard.press('x')
    sleep(0.2)
    print("Releasing X")
    keyboard.release('x')
    sleep(2)
    print("Pressing X")
    keyboard.press('x')
    sleep(0.2)
    print("Releasing X")
    keyboard.release('x')

def create_socket():
    global server, port, nickname, token, channel, sock
    sock.connect((server, port))
    sock.send(f"PASS {token}\n".encode('utf-8'))
    sock.send(f"NICK {nickname}\n".encode('utf-8'))
    sock.send(f"JOIN {channel}\n".encode('utf-8'))
    return sock

def socket_loop(sock):
    global keep_alive
    while keep_alive is True:
        resp = sock.recv(2048).decode('utf-8')
        if resp.startswith('PING'):
            sock.send("PONG\n".encode('utf-8'))
        else:
            command = split_message(resp)
            check_list_of_commands(command, resp)

def split_message(message):
    parts = message.split(":")
    command = parts[2][:-2]
    print(command)
    command = command.replace(' ', '')
    return command

def check_list_of_commands(command, resp):
    global keep_alive
    match(command.lower()):
        case "left":
            perform_key_press(Key.left)
        case "left1":
            perform_key_press(Key.left)
        case "left2":
            perform_multiple_key_presses(Key.left, 2)
        case "left3":
            perform_multiple_key_presses(Key.left, 3)
        case "right":
            perform_key_press(Key.right)
        case "right1":
            perform_key_press(Key.right)
        case "right2":
            perform_multiple_key_presses(Key.right, 2)
        case "right3":
            perform_multiple_key_presses(Key.right, 3)
        case "up":
            perform_key_press(Key.up)
        case "up1":
            perform_key_press(Key.up)
        case "up2":
            perform_multiple_key_presses(Key.up, 2)
        case "up3":
            perform_multiple_key_presses(Key.up, 3)
        case "down":
            perform_key_press(Key.down)
        case "down1":
            perform_key_press(Key.down)
        case "down2":
            perform_multiple_key_presses(Key.down, 2)
        case "down3":
            perform_multiple_key_presses(Key.down, 3)
        case "a":
            perform_key_press('x')
        case "a1":
            perform_key_press('x')
        case "a2":
            perform_multiple_key_presses('x', 2)
        case "a3":
            perform_multiple_key_presses('x', 3)
        case "b":
            perform_key_press('z')
        case "b1":
            perform_key_press('z')
        case "b2":
            perform_multiple_key_presses('z', 2)
        case "b3":
            perform_multiple_key_presses('z', 3)
        case "select":
            perform_key_press(Key.shift_r)
        case "start":
            perform_key_press(Key.enter)
        case "!exit":
            keep_alive = False
        case "that'sit,playtimeisoverrangers!":
            end_script(resp)

def perform_multiple_key_presses(key, presses):
    for x in range(presses):
        perform_key_press(key)
        sleep(0.4)

def perform_key_press(key):
    global keyboard
    keyboard.press(key)
    sleep(0.2)
    keyboard.release(key)

def end_script(resp):
    global keep_alive
    print(resp)
    username = resp.split("!", 1)[0][1:]
    if username == "materiescoil" or username == "lakeamoonlight":
       keep_alive = False
       


start()