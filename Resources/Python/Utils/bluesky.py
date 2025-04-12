from Utils.sensitive_data import BLUESKY_LAKEA_USERNAME, BLUESKY_LAKEA_PASSWORD, BLUESKY_MATERIES_USERNAME, BLUESKY_MATERIES_PASSWORD
from atproto import Client

def get_bluesky_client_materies() -> object:
    client = Client()
    client.login(BLUESKY_MATERIES_USERNAME, BLUESKY_MATERIES_PASSWORD)
    return client

def get_bluesky_client_lakea() -> object:
    client = Client()
    client.login(BLUESKY_LAKEA_USERNAME, BLUESKY_LAKEA_PASSWORD)
    return client