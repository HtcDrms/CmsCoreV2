﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsCoreV2.Models
{
    public class SiteMap
    {
        public enum ChangeFrequency
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }

        public class SitemapUrl
        {
            public string Url { get; set; }
            public DateTime? Modified { get; set; }
            public ChangeFrequency? ChangeFrequency { get; set; }
            public double? Priority { get; set; }
        }

    }
}
