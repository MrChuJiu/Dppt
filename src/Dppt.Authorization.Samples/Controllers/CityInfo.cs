using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dppt.Authorization.Samples.Controllers
{
    class CityInfo : IEqualityComparer<CityInfo>
    {
        public string Name { get; set; }
        public DateTime LastQueryDate { get; set; } = DateTime.Now;
        public decimal Longitude { get; set; } = decimal.MaxValue;
        public decimal Latitude { get; set; } = decimal.MaxValue;
        public int[] RecentHighTemperatures { get; set; } = new int[] { 0 };

        public bool Equals(CityInfo x, CityInfo y)
            => (x.Name, x.Longitude, x.Latitude) ==
               (y.Name, y.Longitude, y.Latitude);

        public int GetHashCode(CityInfo cityInfo) =>
            cityInfo?.Name.GetHashCode() ?? throw new ArgumentNullException(nameof(cityInfo));
    }

}
