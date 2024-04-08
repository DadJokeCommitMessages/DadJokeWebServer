using DadJokeAPI.Controllers;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using DadJokeAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace DadJokeAPI.Tests.Controllers
{
    public class JokesControllerTests
    {
        private readonly Mock<IJokesRepository> _mockJokesRepository;
        private readonly JokesController _controller;

        public JokesControllerTests()
        {
            _mockJokesRepository = new Mock<IJokesRepository>();
            _controller = new JokesController(_mockJokesRepository.Object, null);
        }

        [Fact]
        public void GetRandomJoke_ReturnsOkObjectResult_WithJokeResponse()
        {
            // Arrange
            var joke = new Joke
            {
                JokeID = 1,
                Story = "Why did the scarecrow win an award?",
                JokeType = new JokeType { Description = "Pun" }
            };
            var expectedResult = Result.Ok(joke);
            _mockJokesRepository.Setup(repo => repo.GetRandomJoke(It.IsAny<string>())).Returns(expectedResult);

            // Act
            var result = _controller.GetRandomJoke();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jokeResponse = Assert.IsType<JokeResponse>(okResult.Value);
            Assert.Equal(joke.JokeID, jokeResponse.JokeID);
            Assert.Equal(joke.Story, jokeResponse.Story);
            Assert.Equal(joke.JokeType.Description, jokeResponse.JokeType);

            _mockJokesRepository.Verify(repo => repo.GetRandomJoke(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetRandomJoke_ReturnsNotFound_WhenNoJokeFound()
        {
            // Arrange
            var joke = new Joke
            {
                JokeID = 1,
                Story = "Why did the scarecrow win an award?",
                JokeType = new JokeType { Description = "Pun" }
            };
            var expectedResult = Result.Fail<Joke>(new ValidationError("jokeType", $"No Jokes Found For JokeType '{joke.JokeType.Description}'."));

            _mockJokesRepository.Setup(repo => repo.GetRandomJoke(It.IsAny<string>())).Returns(expectedResult);

            // Act
            var result = _controller.GetRandomJoke();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorList = Assert.IsType<List<ValidationError>>(notFoundResult.Value);
            Assert.Collection(errorList, error =>
            {
                Assert.Equal("jokeType", error.Field);
                Assert.Equal("No Jokes Found For JokeType 'Pun'.", error.ErrorMessage);
            });

            _mockJokesRepository.Verify(repo => repo.GetRandomJoke(It.IsAny<string>()), Times.Once);
        }
    }
}
