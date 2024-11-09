using Whiz.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhizFlowTestPlugins
{
	public class HttpTest
	{
		public static String Test(GenericConfiguration domainConfiguration, GenericConfiguration configuration, NameValueCollection queryString, String payload, String method)
		{
			return "This is my module";
		}
	}
}
