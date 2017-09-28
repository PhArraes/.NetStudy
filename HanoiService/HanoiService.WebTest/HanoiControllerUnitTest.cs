using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HanoiService.Web.Controllers;
using HanoiService.Core;
using HanoiService.Data.Repository;
using HanoiService.Data.Context;
using System.Web.Http;
using System.Web.Http.Results;
using HanoiService.Core.Interfaces;
using HanoiService.Core.Entities;
using Moq;
using System.Net;

namespace HanoiService.WebTest
{
    [TestClass]
    public class HanoiControllerUnitTest
    {
        private int _logCount { get; set; }

        public HanoiControllerUnitTest()
        {
            _logCount = 0;
        }

        private Mock<IHanoiExecutionRepository> GetMock()
        {
            var mockRepository = new Mock<IHanoiExecutionRepository>();
            mockRepository.Setup(x => x.Get(3))
                .Returns(new HanoiExecution { HanoiExecutionId = 3, CreationTime = DateTime.Now, DiscsNumber = 15, EndTime = DateTime.Now });
            mockRepository.Setup(x => x.Add(15))
                .Returns(()=>_logCount++);
            return mockRepository;
        }

        [TestMethod]
        public void GetDeveRetornarStatusOkParaValoresAcimaDe0()
        {

            var controller = new HanoiController(new HanoiManager(
                GetMock().Object));


            var contentResult = controller.Get(3) as OkNegotiatedContentResult<int>;
            
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content > -1);
            Assert.IsInstanceOfType(contentResult, typeof(OkNegotiatedContentResult<int>));
            
        }

        [TestMethod]
        public void GetDeveRetornarErroParaValoresAbaixoDe1()
        {
            var controller = new HanoiController(new HanoiManager(
                GetMock().Object));
            Assert.IsInstanceOfType(controller.Get(-1), typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetDeveRetornarErroParaMaisDe3ExecucoesSimultaneas()
        {
            var controller = new HanoiController(new HanoiManager(
                GetMock().Object));
            controller.Get(15);
            controller.Get(15);
            controller.Get(15);            ;
            var statusResult = controller.Get(15) as StatusCodeResult;
            Assert.IsInstanceOfType(statusResult, typeof(StatusCodeResult));
            Assert.AreEqual(statusResult.StatusCode, HttpStatusCode.ServiceUnavailable);
        }
    }
}
