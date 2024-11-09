using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Collections;

namespace Whiz.WhizFlow.Engine.Monitoring.Utilities.WMI.BE
{
	/// <summary>
	/// Exposes methods to access information exposed by WhizFlow on WMI
	/// </summary>
	public static class WMI
	{
		/// <summary>
		/// Retrieves all the information for the specified Machine/WhizFlow/Entity
		/// </summary>
		/// <param name="entity">The WhizFlow entity to query</param>
		/// <param name="whizFlow">The WhizFlow instance</param>
		/// <param name="machine">The machine on which WhizFlow instance run</param>
		/// <returns>All the objects with their properties</returns>
		public static Dictionary<Tuple<String, String, String>, Dictionary<String, String>> GetObjects(Entities entity, String whizFlow, String machine)
		{
			Dictionary<Tuple<String, String, String>, Dictionary<String, String>> result = new Dictionary<Tuple<String, String, String>, Dictionary<String, String>>();
			String queryText = "";
			switch (entity)
			{
				case Entities.QueueHandlerThread:
					queryText = "select * from QueueHandlerThread where WhizFlow=\"" + whizFlow + "\"";
					break;
				case Entities.QueuesHandler:
					queryText = "select * from QueuesHandler where WhizFlow=\"" + whizFlow + "\"";
					break;
				case Entities.SchedulersHandler:
					queryText = "select * from SchedulersHandler where WhizFlow=\"" + whizFlow + "\"";
					break;
				case Entities.SchedulerHandlerThread:
					queryText = "select * from SchedulerHandlerThread where WhizFlow=\"" + whizFlow + "\"";
					break;
				case Entities.WorkersHandler:
					queryText = "select * from WorkersHandler where WhizFlow=\"" + whizFlow + "\"";
					break;
				case Entities.WorkerHandlerThread:
					queryText = "select * from WorkerHandlerThread where WhizFlow=\"" + whizFlow + "\"";
					break;
			}
			SelectQuery query = new SelectQuery(queryText);
			ManagementObjectSearcher search = new ManagementObjectSearcher(GetManagementScope(machine, @"whiz\whizflow"), query);
			if (!search.Scope.IsConnected) search.Scope.Connect();
			ManagementObjectCollection col = search.Get();
			foreach (ManagementObject obj in col)
			{
				Dictionary<String, String> pTemp = new Dictionary<String, String>();
				foreach (PropertyData property in obj.Properties)
				{
					pTemp.Add(property.Name, property.Value == null ? "" : property.Value.ToString());
				}

				switch (entity)
				{
					case Entities.QueueHandlerThread:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], pTemp["Queue"]), pTemp);
						break;
					case Entities.QueuesHandler:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], "QueuesHandler"), pTemp);
						break;
					case Entities.SchedulersHandler:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], "SchedulersHandler"), pTemp);
						break;
					case Entities.SchedulerHandlerThread:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], pTemp["SchedulerName"]), pTemp);
						break;
					case Entities.WorkersHandler:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], "WorkersHandler"), pTemp);
						break;
					case Entities.WorkerHandlerThread:
						result.Add(new Tuple<String, String, String>(pTemp["WhizFlow"], pTemp["WhizFlowDomain"], pTemp["WorkerName"]), pTemp);
						break;
				}
			}
			return result;
		}
		/// <summary>
		/// Retrieves all the information for the specified Machine/WhizFlow/Entity/ObjectName. Only QueueHandlerThread and SchedulerHandlerThread support this interrogation
		/// </summary>
		/// <param name="entity">The WhizFlow entity to query, note that only QueueHandlerThread and SchedulerHandlerThread support this interrogation</param>
		/// <param name="whizFlow">The WhizFlow instance</param>
		/// <param name="machine">The machine on which WhizFlow instance run</param>
		/// <param name="objectName">The object name</param>
		/// <returns>Object properties</returns>
		public static Dictionary<String, String> GetObject(Entities entity, String whizFlow, String machine, String objectName)
		{
			String query = "";
			switch (entity)
			{
				case Entities.QueueHandlerThread:
					query = "select * from QueueHandlerThread where WhizFlow=\"" + whizFlow + "\" and Queue=\"" + objectName + "\"";
					break;
				case Entities.SchedulerHandlerThread:
					query = "select * from SchedulerHandlerThread where WhizFlow=\"" + whizFlow + "\" and SchedulerName=\"" + objectName + "\"";
					break;
				case Entities.WorkerHandlerThread:
					query = "select * from WorkerHandlerThread where WhizFlow=\"" + whizFlow + "\" and WorkerName=\"" + objectName + "\"";
					break;
				default:
					return null;
			}
			SelectQuery q = new SelectQuery(query);
			ManagementObjectSearcher search = new ManagementObjectSearcher(GetManagementScope(machine, @"whiz\whizflow"), q);
			if (!search.Scope.IsConnected) search.Scope.Connect();
			ManagementObjectCollection col = search.Get();
			foreach (ManagementObject obj in col)
			{
				Dictionary<String, String> res = new Dictionary<String, String>();
				foreach (PropertyData prop in obj.Properties)
				{
					res.Add(prop.Name, prop.Value.ToString());
				}
				return res;
			}
			return null;
		}
		/// <summary>
		/// Retrieves all the WhizFlow Service instances present on the specified machine
		/// </summary>
		/// <param name="machine">Machine</param>
		/// <returns>The list of WhizFlow instances</returns>
		public static String[] GetWhizFlowInstances(String machine)
		{
			List<String> wfs = new List<String>();
			String query = "select * from Win32_Service where Description=\"WhizFlow Service\"";
			SelectQuery q = new SelectQuery(query);
			ManagementObjectSearcher search = new ManagementObjectSearcher(GetManagementScope(machine, "cimv2"), q);
			if (!search.Scope.IsConnected) search.Scope.Connect();
			ManagementObjectCollection col = search.Get();
			foreach (ManagementObject wf in col)
			{
				wfs.Add(wf.GetPropertyValue("DisplayName").ToString());
			}
			return wfs.ToArray();
		}
		/// <summary>
		/// Internal function to create a management scope for the queries
		/// </summary>
		/// <param name="machine">Machine</param>
		/// <param name="path">WMI path</param>
		/// <returns>The management scope</returns>
		private static ManagementScope GetManagementScope(String machine, String path)
		{
			ManagementScope scope = new ManagementScope();
			scope.Options.EnablePrivileges = true;
			scope.Options.Impersonation = ImpersonationLevel.Impersonate;
			scope.Path = new ManagementPath(@"\\" + machine + @"\root\" + path);
			return scope;
		}

	}
}
