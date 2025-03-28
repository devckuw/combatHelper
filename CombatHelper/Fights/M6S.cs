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
    public class M6S : Fight
    {
        private string csv = "M6S.csv";

        public M6S()
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
