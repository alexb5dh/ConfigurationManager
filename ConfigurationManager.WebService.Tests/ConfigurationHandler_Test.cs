using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using ConfigurationManager.WebService.DataAccess;
using ConfigurationManager.WebService.Formatters;
using ConfigurationManager.WebService.Handlers;
using ConfigurationManager.WebService.Models;
using ConfigurationManager.WebService.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ConfigurationManager.WebService.Tests
{
    [TestClass]
    public sealed class ConfigurationHandler_Test
    {
        private readonly RouteData _routeData = new RouteData();
        private Uri _requestUri;

        private readonly Mock<RequestContext> _requestContextMock= new Mock<RequestContext>();
        private readonly Mock<HttpRequestBase> _requestMock = new Mock<HttpRequestBase>();
        private readonly Mock<HttpContextBase> _contextMock = new Mock<HttpContextBase>();

        private readonly Mock<IFormatter> _formatterMock = new Mock<IFormatter>();
        private readonly Mock<IFormatterResolver> _formatterResolverMock = new Mock<IFormatterResolver>();
        private readonly Mock<IConfigurationStorage> _configurationStorageMock = new Mock<IConfigurationStorage>();

        [TestInitialize]
        public void InitHttpContext()
        {
            _requestUri = new Uri("http://localhost", UriKind.Absolute);

            _requestContextMock.Setup(rc => rc.RouteData)
                              .Returns(_routeData);

            _requestMock.Setup(r => r.RequestContext)
                   .Returns(_requestContextMock.Object);
            _requestMock.Setup(r => r.AppRelativeCurrentExecutionFilePath)
                        .Returns(_requestUri.ToString());

            _contextMock.Setup(context => context.Request)
                       .Returns(_requestMock.Object);

            _formatterResolverMock.Setup(fr => fr.GetFormatter(It.IsAny<HttpContextBase>())).Returns(_formatterMock.Object);

            _configurationStorageMock.Setup(cs => cs.SetAsync(It.IsAny<Key>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            _configurationStorageMock.Setup(cs => cs.DeleteAsync((It.IsAny<Key>()))).Returns(Task.FromResult(true));
        }

        [TestMethod]
        public async Task Set_One_Segment_Key_Should_Pass_Correct_Key_To_Storage_Test()
        {
            _requestMock.Setup(r => r.HttpMethod).Returns("POST");
            _routeData.Values.Add(typeof(ActionType).Name, ActionType.Set);

            var key = "oneSegmentKeyTest";
            var value = "oneSegmentKeyTest - Value";
            _requestMock.SetupGet(r => r[typeof(Key).Name])
                .Returns(key);
            _requestMock.SetupGet(r => r["value"])
                .Returns(value);
            
            var handler = new ConfigurationHandler(_formatterResolverMock.Object, _configurationStorageMock.Object);
            await handler.ProcessRequestAsync(_contextMock.Object);
            _configurationStorageMock.Verify(cs => cs.SetAsync(new Key(key), value));
        }

        [TestMethod]
        public async Task Set_Multi_Segment_Key_Should_Pass_Correct_Key_To_Storage_Test()
        {
            _requestMock.Setup(r => r.HttpMethod).Returns("POST");
            _routeData.Values.Add(typeof(ActionType).Name, ActionType.Set);

            var key = "multi/segment/key/test";
            var value = "MultiSegmentKeyTest - Value";
            _requestMock.SetupGet(r => r[typeof(Key).Name])
                .Returns(key);
            _requestMock.SetupGet(r => r["value"])
                .Returns(value);

            var handler = new ConfigurationHandler(_formatterResolverMock.Object, _configurationStorageMock.Object);
            await handler.ProcessRequestAsync(_contextMock.Object);
            _configurationStorageMock.Verify(cs => cs.SetAsync(new Key(key), value));
        }
    }
}
