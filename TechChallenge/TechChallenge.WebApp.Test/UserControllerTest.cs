using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Shouldly;
using TechChallenge.Core.Calendar.Entities;
using TechChallenge.Core.Calendar.Services;
using TechChallenge.WebApp.Controllers;

namespace TechChallenge.WebApp.Test
{
    public class UserControllerTest
    {

        [Test]
        public async Task should_sucess_insert_new_user()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.FromResult(true));

            var controller = new UserController(userServiceMock.Object);
            var result =
                (ObjectResult) await controller.Post(new User() {Name = "Test", Role = UserRole.Candidate});
            
            result.StatusCode.ShouldBe(201);
        }

        [Test]
        public async Task should_fail_insert_new_user()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(Task.FromResult(false));

            var controller = new UserController(userServiceMock.Object);
            var result = (ObjectResult) await controller.Post(new User() {Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Name' must not be empty.");
        }

        [Test]
        public async Task should_fail_login_username_or_password_incorrect()
        {
            var userServiceMock = new Mock<IUserService>();

            var controller = new UserController(userServiceMock.Object);
            var result = (ObjectResult) await controller.Login(new User()
                {Username = "teste", Password = "teste", Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("Username or password is incorrect");
        }
        
        [Test]
        public async Task should_fail_login_must_insert_username()
        {
            var userServiceMock = new Mock<IUserService>();

            var controller = new UserController(userServiceMock.Object);
            var result = (ObjectResult) await controller.Login(new User() {Password = "teste2", Role = UserRole.Candidate});

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldContain("'Username' must not be empty.");
        }

        [Test]
        public async Task should_sucess_login()
        {
            var user = new User() {Username = "teste", Password = "teste", Role = UserRole.Candidate};

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.FindUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            var controller = new UserController(userServiceMock.Object);
            var result = (ObjectResult) await controller.Login(user);

            result.StatusCode.ShouldBe(201);
            result.Value.ToString().ShouldContain("token = ");
        }
    }
}