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
#pragma warning disable RCS1163 // Unused parameter.
#pragma warning disable IDE0060 // Remove unused parameter
            public FakeRouteCacheConfigurator(FakeRouteCache fakeRouteCache)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore RCS1163 // Unused parameter.
            {
            }
        }
    }
}
