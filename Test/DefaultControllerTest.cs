using Data;
using Data.Models;
using Data.Repositories;
using FunApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Services.DTO;
using Services.Exceptions.BadRequest;
using Services.Exceptions.NotFound;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class DefaultControllerTest
    {
        private DbContextOptions<ApplicationDBContext> ContextOptions { get; }

        public DefaultControllerTest()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationDBContext>()
                                     .UseInMemoryDatabase("TestDatabase")
                                     .Options;
            SeedAsync().Wait();
        }
        private async Task SeedAsync()
        {
            await using var context = new ApplicationDBContext(ContextOptions);

            if (await context.Settings.AnyAsync() == false)
            {
                await context.Settings.AddAsync(new Setting
                {
                    Id = 1,
                    Key = "0001",
                    Value = "Hello Mate!"
                });

                await context.SaveChangesAsync();
            }

        }

        [Fact(DisplayName = "Successfully Post Must Return Ok(200)")]
        public void SuccessfullyPostMustReturnOk()
        {
            // ARRANGE
            var fakePostRequest = new SettingDto
            {
                Key="0002",
                Value="Some Useful Info"
            };
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (ObjectResult)mockController.Post(fakePostRequest).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Return Bad Request (400) If A Key Already Exist")]
        public void ReturnBadRequestIfAKeyAlreadyExist()
        {
            // ARRANGE
            var fakePostRequest = new SettingDto
            {
                Key = "0001",
                Value = "Hello Mate!"
            };
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (KeyAlreadyExistException)mockController.Post(fakePostRequest).Exception.InnerException;

            // ASSERT
            Assert.Equal(400, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Get All Must Return Ok(200)")]
        public void SuccessfullyGetAllMustReturnOk()
        {
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (ObjectResult)mockController.GetAll().Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Successfully Get by Id Must Return Ok(200)")]
        public void SuccessfullyGetbyIdMustReturnOk()
        {
            // ARRANGE
            int id = 1;

            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (ObjectResult)mockController.Get(id).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Successfully Put Must Return Ok(200)")]
        public void SuccessfullyPutMustReturnOk()
        {
            // ARRANGE
            int id = 1;
            var fakePutRequest = new SettingDto
            {
                Key = "0003",
                Value = "Some Updated Info"
            };
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (ObjectResult)mockController.Put(fakePutRequest, id).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Put With Nonexistent Id Must Return Not Found(404)")]
        public void PutWithInexistentIdMustReturnNotFound()
        {
            // ARRANGE
            int id = 9999;
            var fakePutRequest = new SettingDto
            {
                Key = "0003",
                Value = "Some Updated Info"
            };
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (SettingNotFound)mockController.Put(fakePutRequest, id).Exception.InnerException;

            // ASSERT
            Assert.Equal(404, taskResult.HttpCode);
        }

        [Fact(DisplayName = "Successfully Delete Must Return Ok(200)")]
        public void SuccessfullyDeleteMustReturnOk()
        {
            // ARRANGE
            int id = 1;
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (ObjectResult)mockController.Delete(id).Result;

            // ASSERT
            Assert.Equal(200, taskResult.StatusCode);
        }

        [Fact(DisplayName = "Delete With Nonexistent Id Must Return Not Found(404)")]
        public void DeleteWithNonexistentIdMustReturnNotFound()
        {
            // ARRANGE
            int id = 9999;
            using var context = new ApplicationDBContext(ContextOptions);
            var mockService = new DefaultService(new GenericRepository<Setting>(context), new Mock<ILogger<DefaultService>>().Object);
            var mockController = new DefaultController(mockService);

            // ACT
            var taskResult = (SettingNotFound)mockController.Delete(id).Exception.InnerException;

            // ASSERT
            Assert.Equal(404, taskResult.HttpCode);
        }
    }
}
