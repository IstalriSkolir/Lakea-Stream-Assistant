import os, sys
from Utils.monster_battle_classes import character

def start():
    account_id = sys.argv[1]
    xp_increase = int(sys.argv[2])
    char = character(account_id)
    char.increase_xp(xp_increase)
    char.save_character()



if __name__ == "__main__":
    start()
