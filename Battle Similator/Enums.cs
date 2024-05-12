public enum ExitCode
{
    Character_Training = 1,
    Monster_Battle = 2,
    Boss_Battle = 3,
    Boss_Healthbar_Update = 4,
    Character_Prestige = 8,
    No_Args_Given = 11,
    Not_Enough_Args = 12,
    Invalid_Args = 13,
    IO_Save_Error = 21,
    IO_Load_Error = 22,
    Encounter_Error = 31
}

public enum TwitchSubTier : byte
{
    None,
    Tier_1,
    Tier_2,
    Tier_3
}