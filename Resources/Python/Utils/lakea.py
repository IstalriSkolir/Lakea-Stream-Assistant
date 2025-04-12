import json
from websocket import create_connection
from Utils.sensitive_data import STREAM_ASSISTANT_IP, STREAM_ASSISTANT_PORT

SOCK = {}

def send_to_websocket(data, service: str, to_json = True):
    return_data = {}
    temp_sock = create_connection(f"ws://{STREAM_ASSISTANT_IP}:{STREAM_ASSISTANT_PORT}/{service}")
    init_resp = temp_sock.recv()
    if "Connection" in init_resp and "Confirmed" in init_resp:
        if to_json == True:
            temp_sock.send(json.dumps(data))
        else:
            temp_sock.send(data)
        response = temp_sock.recv()
        temp_sock.close()
        response_parts = response.split(":", 2)
        if len(response_parts) > 2:
            return_data = json.loads(response_parts[2])
    return return_data