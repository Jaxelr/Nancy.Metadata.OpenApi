using System;
using System.Collections.Generic;
using Nancy.Routing;

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
            var configurator = new FakeRouteCacheConfigurator(this);

            closure.Invoke(configurator);
        }

        public bool IsEmpty() => false;

        public class FakeRouteCacheConfigurator
        {
#pragma warning disable IDE0052 // Remove unread private members
            private readonly FakeRouteCache routeCache;
#pragma warning restore IDE0052 // Remove unread private members

            public FakeRouteCacheConfigurator(FakeRouteCache routeCache)
            {
                this.routeCache = routeCache;
            }
        }
    }
}
