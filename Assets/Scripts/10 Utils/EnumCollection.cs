using System;

public enum TypeTabShopCharacter
{
    HAT = 0,
    SKIN = 1,
    PET = 2
}

public enum GameState
{
    LOBBY,
    PRE_START_GAME,
    REVIVE,
    IN_GAME,
    END_GAME,
    SHOW_ROLE
}

public enum GameMode
{
    NONE,
    HIDE,
    SEEK
}

public enum IdPack
{
    NONE = -1,
    ONE = 0,
    HEAP = 1,
    PACK = 2,
    CLOTH_SACK = 3,
    IRON_CASE = 4,
    GOLDEN = 5,
    NO_ADS_PREMIUM = 6,
    NO_ADS_BASIC = 7
}

[Flags]
public enum Role
{
    NotSet = 0,
    Hider = 1 << 0,
    Seeker = 1 << 1,
    Manual = 1 << 2
}

public enum CharacterAction
{
    Attack,
    Tremble,
    Victory,
    Run,
    Idle,
    Die,
    Action, 
    Chase
}

public enum LevelResult
{
    NotDecided,
    Win,
    Lose
}

public enum CollectibleType
{
    Gold,
    Speed,
    Key,
    Invisible,
    ReduceDetectRange,
    Stealth
}

[System.Flags]
public enum CollectibleStackType
{
    None = 0 << 0,
    Amount = 1 << 0,
    EffectiveTime = 2 << 0
}

public enum TypeHat
{
    EGG = 0,
    KUNG_FU = 1,
    DESTAP = 2,
    CORONA = 4,
    FLOWER = 5,
    BOX_HAT = 6,
    PULPO = 7,
    HORN = 8,
    AUDIFON = 9

}

public enum TypePet
{
    PET_1 = 0,
    PET_2 = 1,
    PET_3 = 2,
    PET_4 = 3,
    PET_5 = 4,
    PET_6 = 5,
}

public enum TypeUnlockItemShop
{    
    COIN,
    VIDEO,
    SPIN
}

public enum TypeEquipment
{
    HAT,
    SKIN,
    PET, 
    ELEMENT
}

public enum TypeDialogReward
{
    LUCKY_WHEEL,
    END_GAME
}


public enum Skin
{
    COLOR_RED = 0,
    COLOR_BROWN = 1,
    COLOR_CYAN = 2,
    COLOR_GREEN = 3,
    COLOR_GRAY = 4,
    COLOR_LIME = 5,
    COLOR_ORANGE = 6,
    COLOR_PINK = 7,
    COLOR_PURPLE = 8,
    COLOR_BLUE = 9,
    COLOR_WHITE = 10,
    COLOR_YELLOW = 11,
    SKIN_BIG_6 = 12,
    SKIN_DORA = 13,
    SKIN_JASON = 14,
    SKIN_MINION = 15,
    SKIN_NINJA = 16,
    SKIN_SPIDA = 17,
    SKIN_PI_KA_CHU = 18,
    SKIN_VELOM = 19,
    SKIN_MUM_MY = 20
}

public enum TypeGift
{
    GOLD,
    ELEMENT
}

public enum TypeItemAnim
{
    Coin = 0,
    Key = 1,
    Bag = 2
}

public enum TypeSoundIngame
{
    NONE = 0,
    PICK_COIN = 1,
    COLLECT_SPEED = 2
}

public enum Alphabet
{
    A,
    B,
    C,
    D,
    E,
    F,
    G, 
    H,
    I,
    J,
    K,
    L,
    M,
    N,
    O,
    P,
    Q,
    R,
    S, 
    T,
    U, 
    V,
    W,
    X, 
    Y, 
    Z
}

public enum Skill
{
    Fire,
    Water,
    Lightning,
    Cloud,
    Kunai,
    Minercraft,
    Roblox,
    Darkness,
    Holy,
    Paint,
    Candy,
    BlackHole,
    Ice,
    FireBlox,
    Meteor,
    Thunder,
    KiloFruit
}

public enum UnlockType
{
    Alphabet,
    Level
}

public enum SkinCharacter
{
    SKIN_0,
    SKIN_1,
    SKIN_2, 
    SKIN_3, 
    SKIN_4, 
    SKIN_5
}


