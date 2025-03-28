using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combatHelper.Utils;
using ImGuiNET;

namespace combatHelper.Fights
{
    public class M7S : Fight
    {
        private string csv = "M7S.csv";

        public M7S()
        {
            //csv = Path.Combine(InfoManager.Configuration.AssemblyLocation, csv);
            //GenerateLines();
            //TimeManager.Instance.OnFightStart += Reset;
        }

        public void GenerateLines()
        {
            lines = DataFrameManager.ProccessDF(csv);
        }

        public void Reset()
        {

        }

        public override void DrawHelper()
        {
            
        }
    }
}
