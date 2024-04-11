using DadJokeAPI.Controllers;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using DadJokeAPI.Models.DTO;
using DadJokeAPI.Converters;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

public class JokesConverterTests
{
    private Mock<IJokesRepository> _mockJokesRepository;
    private Mock<IUsersRepository> _mockUsersRepository;
    private Mock<IJokeTypesRepository> _mockJokeTypesRepository;
    private JokesConverter _converter;

    public JokesConverterTests()
    {
      _mockJokesRepository = new Mock<IJokesRepository>();
      _mockUsersRepository = new Mock<IUsersRepository>();
      _mockJokeTypesRepository = new Mock<IJokeTypesRepository>();
      _converter = new JokesConverter(_mockJokesRepository.Object, _mockUsersRepository.Object, _mockJokeTypesRepository.Object);
    }

    // Test for Convert(CreateJokeRequest, User)
    [Fact]
    public void Convert_CreateJokeRequest_ReturnsOkResultWithJoke_WhenUserAndJokeTypeValid()
    {
      // Arrange
      var createJokeRequest = new CreateJokeRequest { Story = "Test Joke", JokeType = "Dad" };
      var loggedInUser = new User { EmailAddress = "test@example.com" };
      var expectedJoke = new Joke { Story = createJokeRequest.Story, User = loggedInUser, JokeType = new JokeType { Description = createJokeRequest.JokeType } };
      _mockUsersRepository.Setup(m => m.GetUserByEmail(loggedInUser.EmailAddress)).Returns(Result.Ok(loggedInUser));
      _mockJokeTypesRepository.Setup(m => m.GetJokeTypeByDescription(createJokeRequest.JokeType)).Returns(Result.Ok(new JokeType { Description = createJokeRequest.JokeType }));

      // Act
      var result = _converter.Convert(createJokeRequest, loggedInUser);

      // Assert
      var okResult = Assert.IsType<Result<Joke>>(result);
      Assert.True(okResult.IsSuccess);
      var actualJoke = okResult.Value;
      Assert.Equal(expectedJoke.Story, actualJoke.Story);
      Assert.Equal(expectedJoke.User, actualJoke.User);
      Assert.Equal(expectedJoke.JokeType.Description, actualJoke.JokeType.Description);
  }

    // Test for Convert(int, UpdateJokeRequest, User)
    [Fact]
    public void Convert_UpdateJokeRequest_ReturnsOkResultWithUpdatedJoke_WhenUserAndJokeTypeValid()
    {
      // Arrange
      var jokeId = 1;
      var updateJokeRequest = new UpdateJokeRequest { Story = "Updated Joke", JokeType = "Knock-Knock" };
      var loggedInUser = new User { EmailAddress = "test@example.com" };
      var expectedJoke = new Joke
      {
        JokeID = jokeId,
        Story = updateJokeRequest.Story,
        User = loggedInUser,
        JokeType = new JokeType { Description = updateJokeRequest.JokeType }
      };
      _mockJokeTypesRepository.Setup(m => m.GetJokeTypeByDescription(updateJokeRequest.JokeType)).Returns(Result.Ok(new JokeType { Description = updateJokeRequest.JokeType }));
      _mockUsersRepository.Setup(m => m.GetUserByEmail(loggedInUser.EmailAddress)).Returns(Result.Ok(loggedInUser));

      // Act
      var result = _converter.Convert(jokeId, updateJokeRequest, loggedInUser);

      // Assert
      var okResult = Assert.IsType<Result<Joke>>(result);
      Assert.True(okResult.IsSuccess);
      var actualJoke = okResult.Value;
      Assert.Equal(expectedJoke.Story, actualJoke.Story);
      Assert.Equal(expectedJoke.User, actualJoke.User);
      Assert.Equal(expectedJoke.JokeType.Description, actualJoke.JokeType.Description);
    }
}
