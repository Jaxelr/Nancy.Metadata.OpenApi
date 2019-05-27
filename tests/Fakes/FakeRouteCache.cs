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
            private readonly FakeRouteCache routeCache;

            public FakeRouteCacheConfigurator(FakeRouteCache routeCache)
            {
                this.routeCache = routeCache;
            }
        }
    }
}
