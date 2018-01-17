using Repetition.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repetition.Services
{
    public class RealTimeprovider : ITimeProvider
    {
		public DateTime Now { get => DateTime.Now; set => throw new NotImplementedException(); }
	}
}
