from obswebsocket import obsws, requests

IP = "{IP}"
PORT = "{PORT}"
PASSWORD = "{PASSWORD}"

SOCK = {}
INITIALISED = False

def create_connection():
    global INITIALISED
    sock = obsws(IP, PORT, PASSWORD)
    sock.connect()
    INITIALISED = True
    return sock

def _check_if_initialised():
    global SOCK
    if INITIALISED == False:
        SOCK = create_connection()

def get_source_id(source_name, scene):
    _check_if_initialised()
    response = SOCK.call(requests.GetSceneItemId(sourceName=source_name, sceneName=scene))
    source_id = response.datain["sceneItemId"]
    return source_id

def set_source_activity(source_id, scene, visibility):
    _check_if_initialised()  
    SOCK.call(requests.SetSceneItemEnabled(sceneName=scene, sceneItemId=source_id, sceneItemEnabled=visibility))

def set_source_transform(source_id, scene, scene_item_transform):
    _check_if_initialised()
    SOCK.call(requests.SetSceneItemTransform(sceneName=scene, sceneItemId=source_id, sceneItemTransform=scene_item_transform))

def set_source_settings(source_name, source_settings):
    _check_if_initialised()
    SOCK.call(requests.SetInputSettings(inputName=source_name, inputSettings=source_settings))

def create_source(source_name, source_kind, scene_name, input_settings, scene_item_enabled):
    _check_if_initialised()
    SOCK.call(requests.CreateInput(
        sceneName=scene_name, 
        inputName=source_name, 
        inputKind=source_kind, 
        inputSettings=input_settings, 
        sceneItemEnabled=scene_item_enabled
    ))

def remove_source_from_scene(source_id, scene):
    _check_if_initialised()
    SOCK.call(requests.RemoveSceneItem(sceneName=scene, sceneItemId=source_id))

def create_scene(scene):
    _check_if_initialised()
    SOCK.call(requests.CreateScene(sceneName=scene))