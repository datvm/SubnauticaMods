using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterGrowth
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float DurationMultiplier { get; set; } = .1f;

    }
}
