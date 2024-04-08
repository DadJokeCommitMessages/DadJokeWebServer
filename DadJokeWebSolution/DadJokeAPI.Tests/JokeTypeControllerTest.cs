using DadJokeAPI.Controllers;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DadJokeAPI.Tests.Controllers
{
    public class JokeTypesControllerTests
    {
        private readonly Mock<IJokeTypesRepository> _mockJokeTypesRepository;
        private readonly JokeTypesController _controller;

        public JokeTypesControllerTests()
        {
            _mockJokeTypesRepository = new Mock<IJokeTypesRepository>();
            _controller = new JokeTypesController(_mockJokeTypesRepository.Object);
        }

        [Fact]
        public void GetAllJokeTypes_ReturnsOkObjectResult_WithJokeTypeDescriptions()
        {
            // Arrange
            var jokeTypes = new List<JokeType>
            {
                new JokeType { JokeTypeID = 1, Description = "Pun" },
                new JokeType { JokeTypeID = 2, Description = "Dad Joke" },
                new JokeType { JokeTypeID = 3, Description = "Knock-Knock" }
            };
            IEnumerable<JokeType> jokeTypesIE = jokeTypes.ToList();
            var expectedResult = Result.Ok(jokeTypesIE);
            _mockJokeTypesRepository.Setup(repo => repo.GetAllJokeTypes()).Returns(expectedResult);

            // Act
            var result = _controller.GetAllJokeTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var descriptions = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Equal(jokeTypes.Select(j => j.Description), descriptions);

            _mockJokeTypesRepository.Verify(repo => repo.GetAllJokeTypes(), Times.Once);
        }
    }
}
