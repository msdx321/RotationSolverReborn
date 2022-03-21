﻿using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XIVComboPlus.Combos
{
    internal class BaseAction
    {
        internal byte Level { get; }
        internal uint ActionID { get; }
        private bool IsAbility { get; }
        internal virtual uint MPNeed { get; }
        /// <summary>
        /// 如果之前是这些ID，那么就不会执行。
        /// </summary>
        internal uint[] OtherIDs { private get; set; } = null;
        /// <summary>
        /// 是不是需要加上所有的Debuff
        /// </summary>
        internal bool NeedAllDebuffs { private get; set; } = false;
        /// <summary>
        /// 给敌人造成的Debuff,如果有这些Debuff，那么不会执行。
        /// </summary>
        internal ushort[] Debuffs { private get; set; } = null;
        /// <summary>
        /// 使用了这个技能会得到的Buff，如果有这些Buff中的一种，那么就不会执行。 
        /// </summary>
        internal ushort[] BuffsProvide { private get; set; } = null;

        /// <summary>
        /// 使用这个技能需要的前置Buff
        /// </summary>
        internal ushort BuffNeed { private get; set; } = 0;

        /// <summary>
        /// 如果有一些别的需要判断的，可以写在这里。True表示可以使用这个技能。
        /// </summary>
        internal Func<bool> OtherCheck { private get; set; } = null;

        internal BaseAction(byte level, uint actionID, uint mpNeed = 0, bool ability = false)
        {
            this.Level = level;
            this.ActionID = actionID;
            this.MPNeed = mpNeed;
            this.IsAbility = ability;
        }

        public bool TryUseAction(byte level, out uint action, uint lastAct = 0)
        {
            action = ActionID;

            //如果有输入上次的数据，那么上次不能和这次一样。
            if(lastAct == ActionID) return false;
            if(OtherIDs != null)
            {
                foreach (var id in OtherIDs)
                {
                    if (lastAct == id) return false;
                }
            }

            //等级不够。
            if (level < this.Level) return false;

            //MP不够
            if (Service.ClientState.LocalPlayer.CurrentMp < this.MPNeed) return false;

            //没有前置Buff
            if(BuffNeed != 0 && !HaveStatus(FindStatusSelfFromSelf(BuffNeed))) return false;

            //已有提供的Buff的任何一种
            if (BuffsProvide != null)
            {
                foreach (var buff in BuffsProvide)
                {
                    if(HaveStatus(FindStatusSelfFromSelf(buff))) return false;
                }
            }

            //敌方已有充足的Debuff
            if (Debuffs != null)
            {
                if (NeedAllDebuffs)
                {
                    bool haveAll = true;
                    foreach (var debuff in Debuffs)
                    {
                        if (!EnoughStatus(FindStatusTargetFromSelf(debuff)))
                        {
                            haveAll = false;
                            break;
                        }
                    }
                    if (haveAll) return false;
                }
                else
                {
                    foreach (var debuff in Debuffs)
                    {
                        if (EnoughStatus(FindStatusTargetFromSelf(debuff))) return false;
                    }
                }
            }

            //如果是能力技能，还在冷却。
            if (IsAbility && Service.IconReplacer.GetCooldown(ActionID).IsCooldown) return false;

            //用于自定义的要求没达到
            if (OtherCheck!= null && !OtherCheck()) return false;

            return true;
        }

        internal static bool EnoughStatus(Status status)
        {
            return StatusRemainTime(status) > 3f; ;
        }

        internal static bool HaveStatus(Status status)
        {
            return StatusRemainTime(status) != 0f;
        }
        internal static float StatusRemainTime(Status status)
        {
            return status?.RemainingTime ?? 0f;
        }

        /// <summary>
        /// 找到任何对象附加到自己敌人的状态。
        /// </summary>
        /// <param name="effectID"></param>
        /// <returns></returns>
        internal static Status FindStatusTarget(ushort effectID)
        {
            return FindStatus(effectID, Service.TargetManager.Target, null);
        }

        /// <summary>
        /// 找到任何对象附加到自己身上的状态。
        /// </summary>
        /// <param name="effectID"></param>
        /// <returns></returns>
        internal static Status FindStatusSelf(ushort effectID)
        {
            return FindStatus(effectID, Service.ClientState.LocalPlayer, null);
        }

        /// <summary>
        /// 找到玩家附加到敌人身上的状态。
        /// </summary>
        /// <param name="effectID"></param>
        /// <returns></returns>
        internal static Status FindStatusTargetFromSelf(ushort effectID)
        {
            GameObject currentTarget = Service.TargetManager.Target;
            PlayerCharacter localPlayer = Service.ClientState.LocalPlayer;
            return FindStatus(effectID, currentTarget, localPlayer != null ? new uint?(localPlayer.ObjectId) : null);
        }

        /// <summary>
        /// 找到自己附加到自己身上的状态。
        /// </summary>
        /// <param name="effectID"></param>
        /// <returns></returns>
        internal static Status FindStatusSelfFromSelf(ushort effectID)
        {
            PlayerCharacter localPlayer = Service.ClientState.LocalPlayer,
                            localPlayer2 = Service.ClientState.LocalPlayer;
            return FindStatus(effectID, localPlayer, localPlayer2 != null ? new uint?(localPlayer2.ObjectId) : null);
        }

        private static Status FindStatus(ushort effectID, GameObject obj, uint? sourceID)
        {
            if (obj == null)
            {
                return null;
            }
            BattleChara val = (BattleChara)obj;
            if (val == null)
            {
                return null;
            }
            foreach (Status status in val.StatusList)
            {
                if (status.StatusId == effectID && (!sourceID.HasValue || status.SourceID == 0 || status.SourceID == 3758096384u || status.SourceID == sourceID))
                {
                    return status;
                }
            }
            return null;
        }
    }
}
