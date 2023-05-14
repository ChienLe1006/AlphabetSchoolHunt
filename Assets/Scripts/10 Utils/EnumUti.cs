public static class EnumUti
{
    public static Role ToCharacterRole(this GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.HIDE:
                return Role.Hider;
            case GameMode.SEEK:
                return Role.Seeker;
            case GameMode.NONE:
            default:
                return Role.NotSet;
        }
    }
}
