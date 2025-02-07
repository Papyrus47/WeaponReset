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
                string name = value.GetType().Name;
                int i = 0;
                while (true)
                {
                    i++;
                    if (!SkillsParis.ContainsKey(name))
                    {
                        name += i.ToString();
                        if (IDParis.ContainsValue(name))
                        {
                            name += "_" + IDParis.Count.ToString();
                            continue;
                        }
                        break;
                    }
                }
                IDParis.Add(value, name);
                SkillsParis.Add(name, value);
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
        public string ID { get; set; }
        public Dictionary<ProjSkill_Instantiation, string> IDParis { get; set; }
        public Dictionary<string, ProjSkill_Instantiation> SkillsParis { get; set; }
        public void SendData(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(ID);
        }
        public void ReceiveData(BinaryReader binaryReader)
        {
            ID = binaryReader.ReadString();
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
