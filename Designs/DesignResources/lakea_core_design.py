from diagrams import Diagram
from DesignResources.Utils.CustomAttributes import DIAGRAM_ATTRIBUTES
from DesignResources.Utils.CustomClusters import *
from DesignResources.Utils.CustomNodes import *
from DesignResources.Utils.CustomEdges import *

def render(design_name: str):
    with Diagram(design_name, show=False, graph_attr=DIAGRAM_ATTRIBUTES):
        with group("Lakea Stream Assistant"):
            with group("Events Storage"):
                #text("Check if incoming\nevents have matching\noutput events")
                twitch_events = obj("Twitch\nEvents")
                obs_events = obj("OBS\nEvents")
                lakea_events = obj("Lakea\nEvents")

            with group("Events Processing"):
                events_input = obj("Events\nInput")
                events_output = obj("Events\nOutput")
                events_input >> edge_short() >> events_output

            [twitch_events, obs_events, lakea_events] >> edge_short() >> events_input
            [twitch_events, obs_events, lakea_events] << edge_short() << events_input

            lakea_websocket = websocket("WebSocket")
            twitch_interface = obj("Twitch\nInterface")
            obs_interface = obj("OBS\nInterface")
            battle_manager = obj("Battle\nManager")
            python_manager = obj("Python\nManager")
            
            twitch_interface >> edge_long() >> events_input
            obs_interface >> edge_long() >> events_input
            events_output >> edge_short() >> [twitch_interface, obs_interface, battle_manager, python_manager]

            lakea_websocket >> edge_short() >> [events_input, twitch_interface]

        with group("Twitch"):
            helix = api("Helix\nAPI")
            twitch_client = api("Twitch\nClient")
            twitch_pubsub = api("Twitch\nPubSub")
            twitch_chat = websocket("Twitch\nChat")

        obs = websocket("OBS\nWebsocket")

        battle_simulator = app("Monster Battle\nSimulator")
        python_scripts = app("Python\nScripts")

        [twitch_client, twitch_pubsub] >> edge() >> twitch_interface
        twitch_interface >> edge() >> [twitch_chat, helix]

        obs >> edge() >> obs_interface
        obs_interface >> edge() >> obs

        python_manager >> edge() >> python_scripts
        battle_manager >> edge() >> battle_simulator
        battle_simulator >> edge_text("When Battle Sim Ends") >> battle_manager >> edge_long() >> events_input
