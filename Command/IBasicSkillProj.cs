using System.Collections.Generic;
using System.IO;

namespace WeaponReset.Command
{
    public interface IBasicSkillProj
    {
        public List<ProjSkill_Instantiation> OldSkills { get; set; }
        public ProjSkill_Instantiation CurrentSkill
        {
            get
            {
                return SkillsParis[ID];
            }
            set
            {
                Register(value);
                ID = IDParis[value];
            }
        }

        public void Register(ProjSkill_Instantiation value)
        {
            if (!IDParis.ContainsKey(value))
            {
                int i = 0;
                while (true)
                {
                    i++;
                    if (!SkillsParis.ContainsKey(i))
                    {
                        if (IDParis.ContainsValue(i))
                            continue;
                        
                        IDParis.Add(value, i);
                        SkillsParis.Add(i, value);
                        break;
                    }
                }
            }
        }
        public void Register(params ProjSkill_Instantiation[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                Register(value[i]);
                //if (!IDParis.ContainsKey(value[i]))
                //{
                //    string name = value.GetType().Name;
                //    int j = 0;
                //    while (true)
                //    {
                //        j++;
                //        if (!IDParis.ContainsValue(name) || !SkillsParis.ContainsKey(name))
                //        {
                //            name += j.ToString();
                //            break;
                //        }
                //    }
                //    IDParis.Add(value[i], name);
                //    SkillsParis.Add(name, value[i]);
                //}
            }
        }
        public int ID { get; set; }
        public Dictionary<ProjSkill_Instantiation, int> IDParis { get; set; }
        public Dictionary<int, ProjSkill_Instantiation> SkillsParis { get; set; }
        public void SendData(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(ID);
        }
        public void ReceiveData(BinaryReader binaryReader)
        {
            ID = binaryReader.ReadInt32();
        }
        public void Init();
        public void SwitchSkill()
        {
            if (CurrentSkill.SkillTimeOut)
            {
                if (PreSkillTimeOut())
                {
                    var targetSkill = OldSkills[0];
                    CurrentSkill.OnSkillDeactivate();
                    targetSkill.OnSkillActive();
                    CurrentSkill = targetSkill;
                    OldSkills.Clear();
                }
                return;
            }
            foreach (var targetSkill in CurrentSkill.switchToSkill)
            {
                if (CurrentSkill.SwitchCondition() && targetSkill.ActivationCondition() || targetSkill.CompulsionSwitchSkill(CurrentSkill))
                {
                    CurrentSkill.OnSkillDeactivate();
                    targetSkill.OnSkillActive();
                    OldSkills.Add(CurrentSkill);
                    CurrentSkill = targetSkill;
                    return;
                }
            }
        }
        /// <summary>
        /// 返回false阻止直接调用回起点
        /// </summary>
        /// <returns></returns>
        public bool PreSkillTimeOut() => true;
    }
}
