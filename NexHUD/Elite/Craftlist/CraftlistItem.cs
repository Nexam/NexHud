using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NexHUD.EDEngineer;

namespace NexHUD.Elite.Craftlist
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
        /// <returns></returns>
        public bool canCraft()
        {
            return false;
        }

        public void compile()
        {
            m_blueprint = EngineerHelper.blueprints.Where(x => x.Type == type && x.Name == name && x.Grade == grade).FirstOrDefault();
            m_experimentBlueprint = EngineerHelper.blueprints.Where(x => x.Type == type && x.Name == experimental && x.IsExperimental).FirstOrDefault();
            m_isValid = blueprint != null;
        }
    }
}
