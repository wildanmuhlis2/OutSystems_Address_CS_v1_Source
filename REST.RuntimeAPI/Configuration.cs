/* 
 This source code (the "Generated Software") is generated by the OutSystems Platform 
 and is licensed by OutSystems (http://www.outsystems.com) to You solely for testing and evaluation 
 purposes, unless You and OutSystems have executed a specific agreement covering the use terms and 
 conditions of the Generated Software, in which case such agreement shall apply. 
*/

using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.Internal.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace OutSystems.REST
{
    /// <summary>
    /// Stores configuration parameters on how to connect to a REST API. Each Consume REST API has the corresponding associated configuration.
    /// </summary>
    public class Configuration {
            /// <summary>
            /// The URL of the REST web service.
            /// </summary>
            public String Url { get; set; }
            /// <summary>
            /// The username to use on requests.
            /// </summary>
            public String Username { get; set; }
            /// <summary>
            /// User's password for the service.
            /// </summary>
            public String Password { get; set; }
            /// <summary>
            /// Controls whether errors should be traced or not.
            /// </summary>
            public bool TraceErrors { get ; set ; }
            /// <summary>
            /// Controls whether a request should be traced, even if finishes normally.
            /// </summary>
            public bool TraceAll { get ; set ; }
            /// <summary>
            /// Controls whether a request should be traced.
            /// </summary>
            public bool Trace { 
                get {
                    return TraceErrors || TraceAll;
                }
            }

            /// <summary>
            /// Constructs a new, empty Configuration.
            /// </summary>
            public Configuration() : this("","","", false, false) { }

            /// <summary>
            /// Constructs a new Configuration object with the given parameters.
            /// </summary>
            /// <param name="url">URL to connect to.</param>
            /// <param name="username">Username to use.</param>
            /// <param name="password">The user's password.</param>
            /// <param name="traceErrors"><code>bool</code> indicating wether errors should be traced at runtime.</param>
            /// <param name="traceAll"><code>bool</code> indicating wether tracing should enable at runtime, even when errors do not occurr.</param>
            public Configuration(string url, String username, String password, bool traceErrors, bool traceAll) {
                Url = url;
                Username = username;
                Password = password;
                TraceErrors = traceErrors;
                TraceAll = traceAll;
            }

            /// <summary>
            /// Return a previously defined REST API Configuration, based on its Service Studio key and eSpace Id.
            /// </summary>
            /// <param name="restwebrefSSKey">The Service Studio key of the REST API source.</param>
            /// <param name="eSpaceId">The eSpace Id.</param>
            /// <returns>The corresponding Configuration.</returns>
            public static Configuration GetCustomClientConfiguration(string restwebrefSSKey, int eSpaceId) {
                Func<Configuration> fetchFromDb = () => {
                    using (var tran = DatabaseAccess.ForSystemDatabase.GetReadOnlyTransaction()) {
                        using (IDataReader reader = GetCustomClientConfigFields(tran, restwebrefSSKey, eSpaceId)) {
                            if (reader.Read()) {
                                // get effective url, username and password
                                var effectiveUrl = reader.SafeGet<string>("Effective_URL", string.Empty).Trim();
                                var effectiveUsername = reader.SafeGet<string>("Effective_Username", string.Empty).Trim();
                                var effectivePassword = reader.SafeGet<string>("Effective_Password", string.Empty).Trim();
                                var traceErrors = reader.SafeGet<bool>("TraceErrors", false);
                                var traceAll = reader.SafeGet<bool>("TraceAll", false);
                                return new Configuration(effectiveUrl, effectiveUsername, effectivePassword, traceErrors, traceAll);
                            }
                            return new Configuration();
                        }
                    }
                };

                return ConfigurationCache.GetESpaceCachedValue(restwebrefSSKey, "RestConfigCache", eSpaceId, _ => fetchFromDb());
            }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            private static IDataReader GetCustomClientConfigFields(Transaction tran, String webrefSSKey, int espaceId) {
                Command cmd = tran.CreateCommand("SELECT Effective_URL, Effective_Username, Effective_Password, TraceErrors, TraceAll FROM ossys_Rest_Web_Reference WHERE Espace_Id = @espaceId AND SS_Key = @webrefSSKey");
                cmd.CreateParameter("@espaceId", DbType.Int32, espaceId);
                cmd.CreateParameter("@webrefSSKey", DbType.String, webrefSSKey);
                return cmd.ExecuteReader();
            }

            private static class ConfigurationCache {
                private static Dictionary<string, Configuration> configCache = new Dictionary<string, Configuration>();

                    private static bool reentrantCall = false;

                    private static void InsertESpaceCache(string cacheName, int eSpaceId) {
                        System.Web.HttpRuntime.Cache.Insert(
                            cacheName, 
                            eSpaceId, 
                            AppInfo.CalculateCacheDependency(eSpaceId, 0),
                            DateTime.UtcNow.AddDays(1), 
                            TimeSpan.Zero, 
                            System.Web.Caching.CacheItemPriority.NotRemovable, 
                            new System.Web.Caching.CacheItemRemovedCallback(CacheRemovedCallback));
                    }

                    private static void CacheRemovedCallback(string key, object value, System.Web.Caching.CacheItemRemovedReason reason) {
                        lock (configCache) {
                            configCache.Remove(key);
                        }
                    }

                    public static Configuration GetESpaceCachedValue(string key, String cacheName, int eSpaceId, Func<string, Configuration> Getter) {
                        String cacheKey = cacheName + eSpaceId + key;                    
                        return InnerGetCachedValue(cacheKey, cacheName, Getter, InsertESpaceCache, eSpaceId);
                    }

                    private static Configuration InnerGetCachedValue(string key, String cacheName, Func<string, Configuration> Getter, Action<string, int> CacheInsertMethod, int cacheExtraId) {
                        Configuration result;
                        lock (configCache) {
                            if (reentrantCall) {
                                throw new InvalidOperationException("Reentrant call in AppCache for key: " + key);
                            }

                            if (!configCache.TryGetValue(key, out result)) {
                                try {
                                    reentrantCall = true;
                                    configCache.Add(key, result = Getter(key));
                                } catch (ArgumentException e) {
                                    throw new ArgumentException("DuplicateKey was: " + key + ", map contents was: " + configCache.ToString() + ", TryGetValue was: " + configCache.TryGetValue(key, out result), e);
                                } finally {
                                    reentrantCall = false;
                                }
                                CacheInsertMethod(key, cacheExtraId);
                            }
                        }
                        return result;
                    }
                }
    }
}