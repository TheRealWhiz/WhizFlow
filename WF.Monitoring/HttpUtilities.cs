using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Whiz.WhizFlow.Monitoring
{
	/// <summary>
	/// Utility class for Http requests
	/// </summary>
	public static class HttpUtilities
	{
		#region Web Methods
		/// <summary>
		/// This method performs an http get to the specified url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns></returns>
		public static String HttpCall(String url, Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = WebRequestMethods.Http.Get;
			pWebReq.Accept = "application/json";
			try
			{
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		/// <summary>
		/// This method performs an http get to the specified url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="headers"></param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns></returns>
		public static String HttpCall(String url, NameValueCollection headers, Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = WebRequestMethods.Http.Get;
			pWebReq.Accept = "application/json";
			foreach (String t in headers.AllKeys)
			{
				pWebReq.Headers.Add(t, headers[t]);
			}
			try
			{
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		/// <summary>
		/// This method performs an http operation (specified) to the specified url with the specified payload
		/// </summary>
		/// <param name="url"></param>
		/// <param name="payload"></param>
		/// <param name="contentType"></param>
		/// <param name="method"></param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns></returns>
		public static String HttpCall(String url, String method, String payload, String contentType = "application/json", Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = method;
			pWebReq.Accept = "application/json";
			try
			{
				Byte[] byteContent = ASCIIEncoding.UTF8.GetBytes(payload);
				pWebReq.ContentType = contentType;
				pWebReq.ContentLength = byteContent.Length;
				using (Stream pWriter = pWebReq.GetRequestStream())
				{
					pWriter.Write(byteContent, 0, byteContent.Length);
					pWriter.Flush();
					pWriter.Close();
				}
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="headers"></param>
		/// <param name="method"></param>
		/// <param name="payload"></param>
		/// <param name="contentType"></param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns></returns>
		public static String HttpCall(String url, NameValueCollection headers, String method, String payload, String contentType = "application/json", Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = method;
			pWebReq.Accept = "application/json";
			foreach (String t in headers.AllKeys)
			{
				pWebReq.Headers.Add(t, headers[t]);
			}
			try
			{
				Byte[] byteContent = ASCIIEncoding.UTF8.GetBytes(payload);
				pWebReq.ContentType = contentType;
				pWebReq.ContentLength = byteContent.Length;
				using (Stream pWriter = pWebReq.GetRequestStream())
				{
					pWriter.Write(byteContent, 0, byteContent.Length);
					pWriter.Flush();
					pWriter.Close();
				}
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		/// <summary>
		/// Performs an http call with no result
		/// </summary>
		/// <param name="url">Url to call</param>
		/// <param name="method">Http method</param>
		/// <param name="payload">Eventual payload in string format</param>
		/// <param name="contentType">Content type</param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		public static void HttpCallVoid(String url, String method, String payload, String contentType = "application/json", Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = method;
			pWebReq.Accept = "application/json";
			Byte[] byteContent = ASCIIEncoding.UTF8.GetBytes(payload);
			pWebReq.ContentType = contentType;
			pWebReq.ContentLength = byteContent.Length;
			using (Stream pWriter = pWebReq.GetRequestStream())
			{
				pWriter.Write(byteContent, 0, byteContent.Length);
				pWriter.Flush();
				pWriter.Close();
			}
			HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
			String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
			if (pResp != null)
			{
				pResp.GetResponseStream().Close();
			}
		}
		/// <summary>
		/// Performs an http call
		/// </summary>
		/// <param name="url">Url to call</param>
		/// <param name="method">Http method</param>
		/// <param name="payload">Eventual payload in string format</param>
		/// <param name="username">Username for basic authentication</param>
		/// <param name="password">Password for basic authentication</param>
		/// <param name="contentType">Content type</param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns>The result from the Http call</returns>
		public static String HttpCall(String url, String method, String payload, String username, String password, String contentType = "application/json", Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = method;
			String credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
			pWebReq.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
			pWebReq.Accept = "application/json";
			try
			{
				Byte[] byteContent = ASCIIEncoding.UTF8.GetBytes(payload);
				pWebReq.ContentType = contentType;
				pWebReq.ContentLength = byteContent.Length;
				using (Stream pWriter = pWebReq.GetRequestStream())
				{
					pWriter.Write(byteContent, 0, byteContent.Length);
					pWriter.Flush();
					pWriter.Close();
				}
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		/// <summary>
		/// This method performs an http operation towards the specified url
		/// </summary>
		/// <param name="url"></param>
		/// <param name="method"></param>
		/// <param name="maskException">Exception trapping indicator. If true the method will not throw an exception</param>
		/// <returns></returns>
		public static String HttpCall(String url, String method, Boolean maskException = true)
		{
			Uri u = new Uri(url);
			HttpWebRequest pWebReq;
			pWebReq = (HttpWebRequest)HttpWebRequest.Create(u);
			pWebReq.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			pWebReq.Method = method;
			pWebReq.Accept = "application/json";
			try
			{
				HttpWebResponse pResp = (HttpWebResponse)pWebReq.GetResponse();
				String a = (new StreamReader(pResp.GetResponseStream())).ReadToEnd();
				if (pResp != null)
				{
					pResp.GetResponseStream().Close();
				}
				return a;
			}
			catch (WebException ex)
			{
				if (maskException)
				{
					String a = (new StreamReader(ex.Response.GetResponseStream())).ReadToEnd();
					return a;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				if (maskException)
				{
					return null;
				}
				else
				{
					throw (ex);
				}
			}
		}
		#endregion

	}
}
