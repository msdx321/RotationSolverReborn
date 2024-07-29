namespace RotationSolver.Basic.Rotations.Basic;

partial class DarkKnightRotation
{
    /// <inheritdoc/>
    public override MedicineType MedicineType => MedicineType.Strength;
    private protected sealed override IBaseAction TankStance => GritPvE;

    #region Job Gauge

    /// <summary>
    /// 
    /// </summary>
    public static byte Blood => JobGauge.Blood;

    /// <summary>
    /// 
    /// </summary>
    public static bool HasDarkArts => JobGauge.HasDarkArts;

    static float DarkSideTimeRemainingRaw => JobGauge.DarksideTimeRemaining / 1000f;

    /// <summary>
    /// 
    /// </summary>
    public static float DarkSideTime => DarkSideTimeRemainingRaw - DataCenter.DefaultGCDRemain;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected static bool DarkSideEndAfter(float time) => DarkSideTime <= time;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gctCount"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    protected static bool DarkSideEndAfterGCD(uint gctCount = 0, float offset = 0)
        => DarkSideEndAfter(GCDTime(gctCount, offset));

    static float ShadowTimeRemainingRaw => JobGauge.ShadowTimeRemaining / 1000f;

    /// <summary>
    /// 
    /// </summary>
    public static float ShadowTime => ShadowTimeRemainingRaw - DataCenter.DefaultGCDRemain;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected static bool ShadowTimeEndAfter(float time) => ShadowTimeRemainingRaw <= time;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gctCount"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    protected static bool ShadowTimeEndAfterGCD(uint gctCount = 0, float offset = 0)
        => ShadowTimeEndAfter(GCDTime(gctCount, offset));
    #endregion

    static partial void ModifyHardSlashPvE(ref ActionSetting setting)
    {

    }

    static partial void ModifySyphonStrikePvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.HardSlashPvE];
    }

    static partial void ModifyUnleashPvE(ref ActionSetting setting)
    {
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyGritPvE(ref ActionSetting setting)
    {
 
    }

    static partial void ModifyReleaseGritPvE(ref ActionSetting setting)
    {

    }

    static partial void ModifyUnmendPvE(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MeleeRange;
    }

    static partial void ModifySouleaterPvE(ref ActionSetting setting)
    {
        setting.ComboIds = [ActionID.SyphonStrikePvE];
    }

    static partial void ModifyFloodOfDarknessPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Darkside];
        setting.UnlockedByQuestID = 67590;
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyBloodWeaponPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood <= 70;
        setting.StatusProvide = [StatusID.BloodWeapon];
        setting.CreateConfig = () => new ActionConfig()
        {
            TimeToKill = 10,
        };
        setting.UnlockedByQuestID = 67591;
    }

    static partial void ModifyShadowWallPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
        setting.ActionCheck = Player.IsTargetOnSelf;
    }

    static partial void ModifyStalwartSoulPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood <= 80;
        setting.ComboIds = [ActionID.UnleashPvE];
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyEdgeOfDarknessPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Darkside];
        setting.UnlockedByQuestID = 67592;
    }

    static partial void ModifyDarkMindPvE(ref ActionSetting setting)
    {

    }

    static partial void ModifyLivingDeadPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.LivingDead, StatusID.WalkingDead, StatusID.UndeadRebirth];
        setting.ActionCheck = Player.IsTargetOnSelf;
        setting.UnlockedByQuestID = 67594;
    }

    static partial void ModifySaltedEarthPvE(ref ActionSetting setting)
    {
        setting.UnlockedByQuestID = 67596;
    }

    static partial void ModifyShadowstridePvE(ref ActionSetting setting)
    {
        setting.UnlockedByQuestID = 67597;
        setting.SpecialType = SpecialActionType.MovingForward;
    }

    static partial void ModifyAbyssalDrainPvE(ref ActionSetting setting)
    {
        setting.UnlockedByQuestID = 67598;
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyCarveAndSpitPvE(ref ActionSetting setting)
    {
        setting.UnlockedByQuestID = 67600;
    }

    static partial void ModifyBloodspillerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood >= 50 || !Player.WillStatusEnd(0, true, StatusID.Delirium_1972);
    }

    static partial void ModifyQuietusPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => Blood >= 50 || !Player.WillStatusEnd(0, true, StatusID.Delirium_1972);
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyDeliriumPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Delirium_1972, StatusID.BloodWeapon];
        setting.CreateConfig = () => new ActionConfig()
        {
            TimeToKill = 10,
        };
    }

    static partial void ModifyTheBlackestNightPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.BlackestNight];
        setting.ActionCheck = Player.IsTargetOnSelf;
        setting.UnlockedByQuestID = 68455;
    }

    static partial void ModifyFloodOfShadowPvE(ref ActionSetting setting)
    {
        setting.MPOverride = () => HasDarkArts ? 0 : null;
        setting.StatusProvide = [StatusID.Darkside];
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyEdgeOfShadowPvE(ref ActionSetting setting)
    {
        setting.MPOverride = () => HasDarkArts ? 0 : null;
        setting.StatusProvide = [StatusID.Darkside];
    }

    static partial void ModifyDarkMissionaryPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.DarkMissionary];
        setting.ActionCheck = Player.IsTargetOnSelf;
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 1,
        };
    }

    static partial void ModifyLivingShadowPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Scorn];
    }

    static partial void ModifyOblationPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = [StatusID.Oblation];
        setting.ActionCheck = Player.IsTargetOnSelf;
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 1,
        };
    }

    static partial void ModifySaltAndDarknessPvE(ref ActionSetting setting)
    {
        setting.UnlockedByQuestID = 67596;
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 1,
        };
    }

    static partial void ModifyShadowbringerPvE(ref ActionSetting setting)
    {
        setting.ActionCheck = () => !DarkSideEndAfterGCD();
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 1,
        };
    }

    static partial void ModifyShadowedVigilPvE(ref ActionSetting setting)
    {
        setting.StatusProvide = StatusHelper.RampartStatus;
    }

    static partial void ModifyScarletDeliriumPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Delirium_3836];
    }

    static partial void ModifyComeuppancePvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Delirium_3836];
        setting.ComboIds = [ActionID.ScarletDeliriumPvE];
    }

    static partial void ModifyTorcleaverPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Delirium_3836];
        setting.ComboIds = [ActionID.ComeuppancePvE];
    }

    static partial void ModifyImpalementPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Delirium_1972];
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 3,
        };
    }

    static partial void ModifyDisesteemPvE(ref ActionSetting setting)
    {
        setting.StatusNeed = [StatusID.Scorn];
        setting.CreateConfig = () => new ActionConfig()
        {
            AoeCount = 1,
        };
    }

    /// <inheritdoc/>
    protected override bool EmergencyAbility(IAction nextGCD, out IAction? act)
    {
        if (LivingDeadPvE.CanUse(out act)
            && Player.GetHealthRatio() <= Service.Config.HealthForDyingTanks) return true;
        return base.EmergencyAbility(nextGCD, out act);
    }

    // PvP
    static partial void ModifyPlungePvP(ref ActionSetting setting)
    {
        setting.SpecialType = SpecialActionType.MovingForward;
    }
}
