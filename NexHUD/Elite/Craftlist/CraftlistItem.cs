using System;
using System.Collections.Generic;
using System.Linq;
using NexHUD.elite.engineers;

namespace NexHUD.elite.craftlist
{
    public class CraftlistItem
    {
        public static CraftlistItem create(BlueprintDatas _bp, BlueprintDatas _exp, int _count)
        {
            CraftlistItem _item = new CraftlistItem();
            if (_bp != null)
            {
                _item.type = _bp.Type;
                _item.name = _bp.Name;
                _item.grade = _bp.Grade;
                if (_exp != null)
                    _item.experimental = _exp.Name;
                _item.count = _count;
                _item.compile();
            }
            return _item;
        }

        //json serialize
        public string type;
        public string name;
        public int grade;
        public string experimental;
        public int count;

        //others
        private BlueprintDatas m_blueprint;
        private BlueprintDatas m_experimentBlueprint;
        private bool m_isValid = false;

        internal BlueprintDatas blueprint { get => m_blueprint; }
        internal BlueprintDatas experimentBlueprint { get => m_experimentBlueprint; }
        internal bool isValid { get => m_isValid; }


        /// <summary>
        /// whe you got everything...
        /// </summary>
        /// <returns>number of craft available</returns>
        public int canCraft()
        {
            Dictionary<string, int> one = new Dictionary<string, int>();
            Dictionary<string, int> cmdr = new Dictionary<string, int>();

            MaterialDatas[] _needed = EngineerHelper.getAllCraftMaterials(type, name, experimental, grade);
            foreach (MaterialDatas m in _needed)
            {
                if (!one.ContainsKey(m.Name))
                    one.Add(m.Name, m.Quantity);
                else
                    one[m.Name] += m.Quantity;

                if (!cmdr.ContainsKey(m.Name))
                    cmdr.Add(m.Name, EngineerHelper.getCmdrMaterials(m.Name));
            }

            int _canCraft = count;

            foreach (string m in one.Keys)
            {
                for (int i = 1; i <= count; i++)
                {
                    if (cmdr[m] < one[m] * i)
                    {
                        _canCraft = Math.Min(i-1, _canCraft);
                        if (_canCraft == 0)
                            return 0;
                    }
                }
            }
            return _canCraft;
        }

        public void compile()
        {
            m_blueprint = EngineerHelper.blueprints.Where(x => x.Type == type && x.Name == name && x.Grade == grade).FirstOrDefault();
            m_experimentBlueprint = EngineerHelper.blueprints.Where(x => x.Type == type && x.Name == experimental && x.IsExperimental).FirstOrDefault();
            m_isValid = blueprint != null;
        }
    }
}
