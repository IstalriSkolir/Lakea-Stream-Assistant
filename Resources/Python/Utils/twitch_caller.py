from time import sleep
from Utils.twitch import send_twitch_message, send_twitch_message_looping, create_both_twitch_connections

CONVERSATION_WAIT_MULTIPLIER = 0.05
CONVERSATION_WAIT_MODIFIER = 2.5

def twitch_caller_send_message(value):
    if "|" in value:
        messages = value.split("|")
        for message in messages:
            send_twitch_message(message)
    else:
        send_twitch_message(value)

def twitch_caller_looping_send_message(value):
    if "|" in value:
        messages = value.split("|")
        for message in messages:
            send_twitch_message_looping(message)
    else:
        send_twitch_message_looping(value)

def lakea_and_looping_conversation(value):
    conversation = value.split("|")
    lakeas_turn = True
    if(conversation[0] == "LOOPING"):
        lakeas_turn = False
    conversation.pop(0)
    create_both_twitch_connections()
    for phrase in conversation:
        wait_time = CONVERSATION_WAIT_MODIFIER + (CONVERSATION_WAIT_MULTIPLIER * len(phrase))
        sleep(wait_time)
        if lakeas_turn == True:
            send_twitch_message(phrase)
        else:
            send_twitch_message_looping(phrase)
        lakeas_turn = not lakeas_turn
