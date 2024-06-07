from diagrams import Diagram
from DesignResources.Utils.CustomAttributes import DIAGRAM_ATTRIBUTES
from DesignResources.Utils.CustomClusters import *
from DesignResources.Utils.CustomNodes import *
from DesignResources.Utils.CustomEdges import *

def render(design_name: str):
    with Diagram(design_name, show=False, graph_attr=DIAGRAM_ATTRIBUTES):
        start = obj("Start")
        environment_reset = obj("Environment\nReset")
        character_training = obj("Character\nTraining")
        monster_battles = obj("Monster\nBattles")
        boss_battle = obj("Boss\nBattle")
        encounter = obj("Encounter")

        start >> [environment_reset, character_training, monster_battles, boss_battle]
        [monster_battles, boss_battle] >> edge() >> encounter
        [monster_battles, boss_battle] >> edge() << encounter