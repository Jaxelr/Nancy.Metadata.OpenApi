using Nancy.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRouteCache : Dictionary<Type, List<Tuple<int, RouteDescription>>>, IRouteCache
    {
        public static FakeRouteCache Empty = new FakeRouteCache();

        public FakeRouteCache()
        {
        }

        public FakeRouteCache(Action<FakeRouteCacheConfigurator> closure)
        {
            var configurator =
                new FakeRouteCacheConfigurator(this);

            closure.Invoke(configurator);
        }

        public bool IsEmpty() => false;

        public class FakeRouteCacheConfigurator
        {
            private readonly FakeRouteCache routeCache;

            public FakeRouteCacheConfigurator(FakeRouteCache routeCache)
            {
                this.routeCache = routeCache;
            }

            private void AddRoutesToCache(IEnumerable<RouteDescription> routes, Type moduleType)
            {
                if (!routeCache.ContainsKey(moduleType))
                {
                    routeCache[moduleType] = new List<Tuple<int, RouteDescription>>();
                }

                routeCache[moduleType].AddRange(routes.Select((r, i) => new Tuple<int, RouteDescription>(i, r)));
            }

            public FakeRouteCacheConfigurator AddGetRoute(string path, Type moduleType) => this;

            public FakeRouteCacheConfigurator AddGetRoute(string path, Type moduleType, Func<NancyContext, bool> condition) => this;
        }
    }
}
