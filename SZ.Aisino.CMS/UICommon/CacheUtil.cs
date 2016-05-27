using Microsoft.ApplicationServer.Caching;
using System.Collections.Generic;
namespace SZ.Aisino.CMS
{
    /// <summary>
    /// Summary description for Cache
    /// </summary>
    public class CacheUtil
    {
        private static DataCacheFactory _factory = null;
        private static DataCache _cache = null;

        public static DataCache GetCache()
        {
            if (_cache != null)
                return _cache;

            //-------------------------
            // Configure Cache Client 
            //-------------------------

            //Define Array for 1 Cache Host
            List<DataCacheServerEndpoint> servers = new List<DataCacheServerEndpoint>(1);

            //Specify Cache Host Details 
            //  Parameter 1 = host name
            //  Parameter 2 = cache port number
            //servers.Add(new DataCacheServerEndpoint("192.168.152.150", 22233));
            servers.Add(new DataCacheServerEndpoint("localhost", 22233));

            //Create cache configuration
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();

            //Set the cache host(s)
            configuration.Servers = servers;

            //Set default properties for local cache (local cache disabled)
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties();

            //Disable tracing to avoid informational/verbose messages on the web page
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);

            //Pass configuration settings to cacheFactory constructor
            _factory = new DataCacheFactory(configuration);

            //Get reference to named cache called "default"
            _cache = _factory.GetCache("default");

            return _cache;
        }
    }

}
