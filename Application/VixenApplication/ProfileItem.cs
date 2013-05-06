﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VixenApplication
{
    public class ProfileItem
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _dataFolder;
        public string DataFolder
        {
            get { return _dataFolder; }
            set { _dataFolder = value; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
