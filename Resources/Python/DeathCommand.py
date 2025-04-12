import sys
from Utils.twitch import send_twitch_message
from Utils.common_functions import get_from_common_storage, update_common_storage

KEY = "DEATHCOUNT"

def start(args) -> None:
    death_count = get_death_count()
    if len(args) < 2:
        send_twitch_message(f"Materies has died {death_count} times!")
    else:
        match(args[1].upper()):
            case "RESET":
                deaths_reset()
            case "+":
                update_by_one(True, death_count)
            case "-":
                update_by_one(False, death_count)
            case _:
                pass

def deaths_reset() -> None:
    update_common_storage(KEY, "0")
    send_twitch_message("Ok, starting a new counter of Materies deaths!")

def get_death_count() -> int:
    deaths_string = get_from_common_storage(KEY)
    return int(deaths_string)

def update_by_one(increase: bool, death_count: int) -> None:
    if increase: 
        death_count += 1
        if death_count == 1:
            send_twitch_message("That's Materies first death, what is he trying to achieve with that?")
        else:
            send_twitch_message(f"Materies has died again!?! That's {death_count} deaths!")
    else: 
        death_count -= 1
        send_twitch_message(f"Did someone miss count? Materies has died {death_count} times then!")
    update_common_storage(KEY, str(death_count))



if __name__ == '__main__':
    start(sys.argv)