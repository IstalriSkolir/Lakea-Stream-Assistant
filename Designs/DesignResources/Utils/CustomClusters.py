from diagrams import Cluster
from DesignResources.Utils.CustomAttributes import CLUSTER_ATTRIBUTES

def group(name: str):
    return Cluster(name, graph_attr=CLUSTER_ATTRIBUTES)