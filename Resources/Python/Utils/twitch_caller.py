from Utils.twitch import send_twitch_message

def twitch_caller_send_message(value):
    if "|" in value:
        messages = value.split("|")
        for message in messages:
            send_twitch_message(message)
    else:
        send_twitch_message(value)