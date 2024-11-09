using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whiz.WhizFlow
{
	/// <summary>
	/// A domain configuration stored on the databse
	/// </summary>
	public class WhizFlowConfiguration
	{
		/// <summary>
		/// Unique identifier for the configuration
		/// </summary>
		public Int32 Id { get; set; }
		/// <summary>
		/// Hostname owner of the configuration
		/// </summary>
		public String Hostname { get; set; }
		/// <summary>
		/// Service owner of the configuration
		/// </summary>
		public String Service { get; set; }
		/// <summary>
		/// Domain name of the configuration
		/// </summary>
		public String Domain { get; set; }
		/// <summary>
		/// Configuration
		/// </summary>
		public String Configuration { get; set; }
		/// <summary>
		/// Flag to indicate that the configuration is active (domain will be runt when service starts)
		/// </summary>
		public Boolean Active { get; set; }
	}
}
