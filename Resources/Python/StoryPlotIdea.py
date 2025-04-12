import os, sys
from Utils.common_functions import RESOURCE_FOLDER

SUGGESTIONS_FILE = f"{RESOURCE_FOLDER}SuggestedDnDPlots.txt"
MAX_LINE_LENGTH = 175

def start():
    story_idea = f"User: {sys.argv[1]}\nPlot: {insert_line_breaks(sys.argv[2])}"    
    #print(story_idea)
    if os.path.isfile(SUGGESTIONS_FILE):
        file = open(SUGGESTIONS_FILE, "a")
        file.write(f"\n\n{story_idea}")
        file.close()
    else:
        file = open(SUGGESTIONS_FILE, "w")
        file.write(story_idea)
        file.close()

def insert_line_breaks(message: str) -> str:
    message_length = len(message)
    if message_length <= MAX_LINE_LENGTH:
        return message
    last_line_break_position = 0
    try:
        loops = message_length // MAX_LINE_LENGTH + (message_length % MAX_LINE_LENGTH > 0)
        for x in range(loops):
            current_character = ""
            count_back = 0
            index = 0
            while current_character != " ":
                index = last_line_break_position + MAX_LINE_LENGTH - count_back
                current_character = message[index]
                count_back = count_back + 1
            message = f"{message[:index]}\n{message[index:]}"
            last_line_break_position = last_line_break_position + (index - last_line_break_position)
        return message
    except Exception as error:
        print(f"\n\n{str(error)}\n\n")
        return message



if __name__ == '__main__':
    start()
