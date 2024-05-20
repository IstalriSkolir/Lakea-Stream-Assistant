from diagrams.custom import Custom, Node
from DesignResources.Utils.CustomAttributes import NODE_ATTRIBUTES, CUSTOM_FONT_SIZE, CUSTOM_FONT_COLOUR

PATH = "DesignResources/Icons/"
THEME = "DARKTHEME"

def text(text: str) -> Node:
    return Node(f"{text}", fontsize=NODE_ATTRIBUTES["fontsize"], fontcolor=NODE_ATTRIBUTES["fontcolor"], peripheries="0")

def api(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"\n\n\n{name}", f"{PATH}Icon66grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"\n\n\n{name}", f"{PATH}Icon66black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    
def app(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"\n\n\n{name}", f"{PATH}Icon12grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"\n\n\n{name}", f"{PATH}Icon12black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)        

def files(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"\n\n{name}", f"{PATH}Icon38grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"\n\n{name}", f"{PATH}Icon38black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)

def obj(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"\n\n\n{name}", f"{PATH}Icon65grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"\n\n\n{name}", f"{PATH}Icon65black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)        

def web(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"{name}", f"{PATH}Icon61grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"{name}", f"{PATH}Icon61black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)

def websocket(name: str) -> Custom:
    if THEME == "DARKTHEME":
        return Custom(f"\n\n{name}", f"{PATH}Icon23grey.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
    else:
        return Custom(f"\n\n{name}", f"{PATH}Icon23black.png", fontsize=CUSTOM_FONT_SIZE, fontcolor=CUSTOM_FONT_COLOUR)
