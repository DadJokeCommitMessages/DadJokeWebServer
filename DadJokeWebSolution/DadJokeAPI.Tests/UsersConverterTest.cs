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

public class UsersConverterTests
{
  private Mock<IJokesRepository> _mockJokesRepository;
  private Mock<IUsersRepository> _mockUsersRepository;
  private Mock<IJokeTypesRepository> _mockJokeTypesRepository;
  private UsersConverter _converter;

  public UsersConverterTests()
  {
    _mockJokesRepository = new Mock<IJokesRepository>();
    _mockUsersRepository = new Mock<IUsersRepository>();
    _mockJokeTypesRepository = new Mock<IJokeTypesRepository>();
    _converter = new UsersConverter(_mockJokesRepository.Object, _mockUsersRepository.Object, _mockJokeTypesRepository.Object);
  }

  [Fact]
  public void Convert_ReturnsOkResultWithLoggedInUser_WhenUserExists()
  {
      // Arrange
      var expectedUser = new User { EmailAddress = "test@gmail.com" };
      _mockUsersRepository.Setup(m => m.GetUserByEmail(expectedUser.EmailAddress)).Returns(Result.Ok(expectedUser));

      // Act
      var result = _converter.Convert();

      // Assert
      var okResult = Assert.IsType<Result<User>>(result);
      Assert.True(okResult.IsSuccess);
      var actualUser = okResult.Value;
      Assert.Equal(expectedUser.EmailAddress, actualUser.EmailAddress);
  }

  [Fact]
  public void Convert_ReturnsFailureResult_WhenUserDoesNotExist()
  {
      // Arrange
      var expectedResult = Result.Fail<User>(new ValidationError("User does not exist"));
      _mockUsersRepository.Setup(m => m.GetUserByEmail("test@gmail.com")).Returns(expectedResult);

      // Act
      var result = _converter.Convert();

      // Assert
      var failureResult = Assert.IsType<Result<User>>(result);
      var errorList = Assert.IsType<List<ValidationError>>(failureResult.ValidationErrors);
      Assert.Collection(errorList, error =>
      {
        Assert.Equal("User does not exist", error.ErrorMessage);
      });
  }
}
