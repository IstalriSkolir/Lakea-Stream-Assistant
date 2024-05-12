COMMON_STORAGE_FILE = "X:\\1-APPLICATIONDATA\\LIVEDATA\\Python\\ScriptResources\\CommonStorage.txt"

def get_from_common_storage(key):
    file = open(COMMON_STORAGE_FILE, 'r')
    lines = file.read().splitlines()
    file.close
    value = ""
    for line in lines:
        parts = line.split(':', 1)
        if parts[0] == key:
            value = parts[1]
            break
    return value

def update_common_storage(key, value):
    file = open(COMMON_STORAGE_FILE, 'r')
    lines = file.read().splitlines()
    file.close()
    for index in range(len(lines)):
        if lines[index] != "":
            parts = lines[index].split(':', 1)
            if parts[0] == key:
                lines[index] = f"{key}:{value}"
                break
    file = open(COMMON_STORAGE_FILE, 'w')
    for line in lines:
        if line != "":
            file.write(f"{line}\n")
    file.close()
    