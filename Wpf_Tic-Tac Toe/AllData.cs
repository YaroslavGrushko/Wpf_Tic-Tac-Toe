using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wpf_Tic_Tac_Toe
{
    public class AllData
    {
        public int[][] obj;
        public string sourceID;
        public bool IsSourceMain;
        public AllData(int[][] _obj, string _sourceID, bool _IsSourceMain)
        {
            obj = _obj;
            sourceID = _sourceID;
            IsSourceMain = _IsSourceMain;
        }
    }
}