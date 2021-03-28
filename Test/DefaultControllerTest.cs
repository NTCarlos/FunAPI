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

        [Fact(DisplayName = "Successfully Post Must Return Creted(201)")]
        public void SuccessfullyPostMustReturnCreated()
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
            Assert.Equal(201, taskResult.StatusCode);
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
    }
}
