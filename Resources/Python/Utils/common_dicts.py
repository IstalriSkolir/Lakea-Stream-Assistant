from Utils.sensitive_data import STREAMERS_ID

def get_create_channel_redeem_dict(title, prompt, cost, enabled = False, colour = "#FF0000", input_required = False, max_per_stream_enabled = False, max_per_stream_amount = 0, max_per_user_per_stream_enabled = False, max_per_user_per_stream_amount = 0, global_cooldown_enabled = False, global_cooldown_time = 0, skip_requests_queue = True):
    return {
        "Title": title, 
        "Prompt": prompt, 
        "Cost": cost, 
        "IsEnabled": enabled, 
        "BackgroundColor": colour, 
        "IsUserInputRequired": input_required, 
        "IsMaxPerStreamEnabled": max_per_stream_enabled, 
        "MaxPerStream": max_per_stream_amount, 
        "IsMaxPerUserPerStreamEnabled": max_per_user_per_stream_enabled, 
        "MaxPerUserPerStream": max_per_user_per_stream_amount, 
        "IsGlobalCooldownEnabled": global_cooldown_enabled, 
        "GlobalCooldownSeconds": global_cooldown_time, 
        "ShouldRedemptionsSkipRequestQueue": skip_requests_queue       
    }

def get_event_details_dict(name, source, type, id):
    return {
        "Name": name,
        "Source": source,
        "Type": type,
        "ID": id
    }

def get_event_item_dict(key, event_details, event_target):
    return {
        "Key": key,
        "EventItem": {
            "EventDetails": event_details,
            "EventTarget": event_target
        }
    }

def get_event_target_dict(target, goal, args):
    return {
        "Target": target,
        "Goal": goal,
        "Args": args
    }

def get_event_target_args_dict(key, value):
    return {
        "Key": key,
        "Value": value
    }

def get_update_channel_redeem_dict(redeem_id: str, title: str, prompt: str, cost: int, enabled = False, colour = "#FF0000", input_required = False, max_per_stream_enabled = False, max_per_stream_amount = 0, max_per_user_per_stream_enabled = False, max_per_user_per_stream_amount = 0, global_cooldown_enabled = False, global_cooldown_time = 0, skip_requests_queue = True):
    return {
        "RedeemID": redeem_id,
        "RedeemData": {
            "BroadcasterId": STREAMERS_ID,
            "Title": title, 
            "Prompt": prompt, 
            "Cost": cost, 
            "IsEnabled": enabled, 
            "BackgroundColor": colour, 
            "IsUserInputRequired": input_required, 
            "IsMaxPerStreamEnabled": max_per_stream_enabled, 
            "MaxPerStream": max_per_stream_amount, 
            "IsMaxPerUserPerStreamEnabled": max_per_user_per_stream_enabled, 
            "MaxPerUserPerStream": max_per_user_per_stream_amount, 
            "IsGlobalCooldownEnabled": global_cooldown_enabled, 
            "GlobalCooldownSeconds": global_cooldown_time, 
            "ShouldRedemptionsSkipRequestQueue": skip_requests_queue         
        }
    }

def get_update_channel_redeem_dict_from_dict(redeem_id: str, data: dict):
    return {
        "RedeemID": redeem_id,
        "RedeemData": data
    }
