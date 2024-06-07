from diagrams import Diagram
from DesignResources.Utils.CustomAttributes import DIAGRAM_ATTRIBUTES
from DesignResources.Utils.CustomClusters import *
from DesignResources.Utils.CustomNodes import *
from DesignResources.Utils.CustomEdges import *

def render(design_name: str):
    with Diagram(design_name, show=False, graph_attr=DIAGRAM_ATTRIBUTES):
        with group("Web"):
            twitch_api = api("Twitch API")
            twitch_stream = web("\n\nTwitch\nStream")
        
        with group("LAN"):
            with group("NAS"):
                nas_storage = files("NAS\nStorage")
            
            with group("Support PC"):
                support_obs = app("OBS")
                rtmp = app("RTMP\nServer")
                lakea = app("Lakea")
                battle_sim = app("Battle\nSimulator")
                woodland = app("woodland")

                lakea >> edge() >> [woodland, battle_sim]
                lakea << edge() << [woodland, battle_sim]
                support_obs >> edge_text("Streams to") >> rtmp

            with group("Main PC"):
                main_obs = app("OBS")
                game = app("Game")

            rtmp >> edge_text("Streams to") >> main_obs
            battle_sim >> edge_text("Game Data\n") >> nas_storage
            battle_sim << edge() << nas_storage

        
        twitch_api >> edge() >> lakea >> edge() >> main_obs
        twitch_api << edge() << lakea << edge() << main_obs
        main_obs >> edge() >> twitch_stream