using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace JustCopy
{
    public class Main : Mod
    {
        public Main(ModContentPack content) : base(content)
        {
        }

        public override string SettingsCategory()
        {
            return "Just Copy";
        }
    }
}
