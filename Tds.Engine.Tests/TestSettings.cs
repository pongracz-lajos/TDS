﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tds.Engine.Tests
{
    public class TestSettings
    {
        public class Storage
        {
            public static readonly string Location = @"..\..\..\TestData";
            public static readonly string XmlMetadataLocation = Location + @"\Metadata\Xml";
            public static readonly string XmlMetadataManualLocation = XmlMetadataLocation + @"\Manual";
            public static readonly string XmlMetadataGeneratedLocation = XmlMetadataLocation + @"\Generated";
        }
    }
}
