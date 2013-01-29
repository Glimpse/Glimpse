using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.BadData;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Framework
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it is only needed for the tests below.")]
    public class ReflectionDiscoverableCollectionTester<T> : ReflectionDiscoverableCollection<T>
    {
        private ReflectionDiscoverableCollectionTester(Mock<ILogger> loggerMock) : base(loggerMock.Object)
        {
            LoggerMock = loggerMock;
        }

        public Mock<ILogger> LoggerMock { get; set; }

        public static ReflectionDiscoverableCollectionTester<T> Create()
        {
            return new ReflectionDiscoverableCollectionTester<T>(new Mock<ILogger>());
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Class is okay because it is only needed for the tests below.")]
    public class ReflectionDiscoverableCollectionTesterRef : MarshalByRefObject
    {
        private readonly ReflectionDiscoverableCollectionTester<IPipelineInspector> collection = ReflectionDiscoverableCollectionTester<IPipelineInspector>.Create();

        public string DiscoveryLocation
        {
            get { return collection.DiscoveryLocation; }
            set { collection.DiscoveryLocation = value; }
        }

        public void Discover()
        {
            collection.Discover();
        }

        public void LoggerVerify()
        {
            collection.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()));
        }
    }

    public class ReflectionDiscoverableCollectionShould : IDisposable
    {
        private ReflectionDiscoverableCollectionTester<IPipelineInspector> tester;

        public ReflectionDiscoverableCollectionTester<IPipelineInspector> Collection
        {
            get { return tester ?? (tester = ReflectionDiscoverableCollectionTester<IPipelineInspector>.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Collection = null;
        }

        [Fact]
        public void Construct()
        {
            Assert.NotNull(Collection);
            Assert.NotNull(Collection.Items);
        }

        [Fact]
        public void ReturnEnumerator()
        {
            var enumerator = Collection.GetEnumerator();
            Assert.NotNull(enumerator);
        }

        [Fact]
        public void AddItems()
        {
            Assert.Empty(Collection);
            Collection.Add(new DummyPipelineInspector1());
            Assert.Equal(1, Collection.Count);
        }

        [Fact]
        public void ReturnCount()
        {
            Assert.Empty(Collection);
        }

        [Fact]
        public void Clear()
        {
            Assert.Equal(0, Collection.Count);
            Collection.Add(new DummyPipelineInspector1());
            Assert.Equal(1, Collection.Count);
            
            Collection.Clear();
            Assert.Empty(Collection);
        }

        [Fact]
        public void Contains()
        {
            var item = new DummyPipelineInspector1();

            Collection.Add(item);

            Assert.True(Collection.Contains(item));
        }

        [Fact]
        public void CopyTo()
        {
            var items = new IPipelineInspector[] { new DummyPipelineInspector1(), new DummyPipelineInspector2() };

            Collection.CopyTo(items, 0);

            Assert.Empty(Collection);
        }

        [Fact]
        public void CopyToWithItems()
        {
            var collection = new ReflectionDiscoverableCollection<DummyObjectContext>(Collection.LoggerMock.Object)
                                 {
                                     new DummyObjectContext(),
                                     new DummyObjectContext()
                                 };

            var items = new[] { new DummyObjectContext(), new DummyObjectContext(), new DummyObjectContext() };

            collection.CopyTo(items, 0);

            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void Remove()
        {
            var item = new DummyObjectContext();
            var collection = new ReflectionDiscoverableCollection<DummyObjectContext>(Collection.LoggerMock.Object)
                                 {
                                     item
                                 };

            Assert.Equal(1, collection.Count);

            collection.Remove(item);

            Assert.Empty(collection);
        }

        [Fact]
        public void NotBeReadOnly()
        {
            Assert.False(Collection.IsReadOnly);
        }

        [Fact]
        public void GetSetAutoDiscover()
        {
            Assert.True(Collection.AutoDiscover);

            Collection.AutoDiscover = false;

            Assert.False(Collection.AutoDiscover);
        }

        [Fact]
        public void IgnoreType()
        {
            Assert.Empty(Collection.IgnoredTypes);

            Collection.IgnoreType(typeof(string));

            Assert.Equal(1, Collection.IgnoredTypes.Count);
        }

        [Fact]
        public void Discover()
        {
            Assert.Empty(Collection);

            Collection.Discover();

            Assert.True(Collection.Count >= 2);
        }

        [Fact]
        public void DiscoverLogsAssemblyLoadExceptions()
        {
            var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };
            var domain = AppDomain.CreateDomain("TestAppDomain", null, setup);
            
            var refType = typeof(ReflectionDiscoverableCollectionTesterRef);
            var collection = domain.CreateInstanceAndUnwrap(refType.Assembly.FullName, refType.FullName) as ReflectionDiscoverableCollectionTesterRef;

            collection.DiscoveryLocation = "../../BadData/";

            collection.Discover();

            collection.LoggerVerify();

            AppDomain.Unload(domain);
        }

        [Fact]
        public void UseBasePathInNonShadowedAppDomains() 
        {
            var setup = new AppDomainSetup { ApplicationBase = AppDomain.CurrentDomain.BaseDirectory };

            var domain = AppDomain.CreateDomain("TestAppDomain", null, setup);

            var refType = typeof(ReflectionDiscoverableCollectionTesterRef);
            var collection = domain.CreateInstanceAndUnwrap(refType.Assembly.FullName, refType.FullName) as ReflectionDiscoverableCollectionTesterRef;

            Assert.Equal(setup.ApplicationBase, collection.DiscoveryLocation);

            AppDomain.Unload(domain);
        }

        [Fact]
        public void UseShadowCopyFolderInShadowedAppDomains() 
        {
            var setup = new AppDomainSetup 
            {
                ShadowCopyFiles = "true",
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                ApplicationName = Guid.NewGuid().ToString(),
                CachePath = Path.GetTempPath()
            };
            var domain = AppDomain.CreateDomain("TestAppDomain", null, setup);

            var refType = typeof(ReflectionDiscoverableCollectionTesterRef);
            var collection = domain.CreateInstanceAndUnwrap(refType.Assembly.FullName, refType.FullName) as ReflectionDiscoverableCollectionTesterRef;

            Assert.Equal(Path.Combine(setup.CachePath, setup.ApplicationName), collection.DiscoveryLocation);

            AppDomain.Unload(domain);
        }

        [Fact]
        public void DiscoverLogsActivatorCreateExceptions()
        {
            var collection = new ReflectionDiscoverableCollection<IBlowup>(Collection.LoggerMock.Object);
            collection.Discover();

            Assert.Equal(0, collection.Count);
            Collection.LoggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>(), It.IsAny<object[]>()));
        }

        [Fact]
        public void UserDefaltDiscoveryLocation()
        {
            Assert.False(string.IsNullOrWhiteSpace(Collection.DiscoveryLocation));
        }

        [Fact]
        public void SetDiscoveryLocationWithRootedPath()
        {
            var path = @"c:\Windows";
            Collection.DiscoveryLocation = path;
            Assert.Equal(path, Collection.DiscoveryLocation);
        }

        [Fact]
        public void SetDiscoveryLocationWithNonRootedPath()
        {
            var path = @"..\..\";
            Collection.DiscoveryLocation = path;
            Assert.Contains(path, Collection.DiscoveryLocation);
        }

        [Fact]
        public void ThrowArgumentExceptionWithBadPath()
        {
            var path = @"c:\I\dont\exist\";
            Assert.Throws<DirectoryNotFoundException>(() => Collection.DiscoveryLocation = path);

            path = @"..\neither\do\I\";
            Assert.Throws<DirectoryNotFoundException>(() => Collection.DiscoveryLocation = path);
        }
    }
}