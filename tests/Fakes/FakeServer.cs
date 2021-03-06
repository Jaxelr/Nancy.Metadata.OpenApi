﻿using System.Collections.Generic;
using Nancy.Metadata.OpenApi.Model;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
#pragma warning disable RCS1102 // Make class static.
    public class FakeServer
#pragma warning restore RCS1102 // Make class static.
    {
        private static readonly string[] Enums = { "8080", "8088" };
        internal const string FakeKey = "port";
        internal static readonly ServerVariable ServerVariable = new ServerVariable() { Enum = Enums, Default = "demo", Description = "This is a sample description" };
        private static readonly Dictionary<string, ServerVariable> InternalDict = new Dictionary<string, ServerVariable>() { { FakeKey, ServerVariable } };

        public static Server Server = new Server() { Url = "http://localhost:{port}/", Description = "Sample expanded server", Variables = InternalDict };
    }
}
