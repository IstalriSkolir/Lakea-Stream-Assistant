from diagrams import Edge
from DesignResources.Utils.CustomAttributes import EDGE_THICKNESS, EDGE_FONT_SIZE, EDGE_FONT_COLOUR, EDGE_SHORT_MIN_LENGTH, EDGE_LONG_MIN_LENGTH

def edge() -> Edge:
    return Edge(penwidth=EDGE_THICKNESS)

def edge_text(text: str) -> Edge:
    return Edge(label=f"{text}\n\n", penwidth=EDGE_THICKNESS, fontsize=EDGE_FONT_SIZE ,fontcolor=EDGE_FONT_COLOUR)

def edge_short() -> Edge:
    return Edge(penwidth=EDGE_THICKNESS, minlen=EDGE_SHORT_MIN_LENGTH)

def edge_long() -> Edge:
    return Edge(penwidth=EDGE_THICKNESS, minlen=EDGE_LONG_MIN_LENGTH)

def edge_custom_length(length: str) -> Edge:
    return Edge(penwidth=EDGE_THICKNESS, minlen=length)