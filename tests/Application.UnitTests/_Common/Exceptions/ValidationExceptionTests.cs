namespace Application.UnitTests.Common.Exceptions;

public class ValidationExceptionTests
{
    [Fact]
    public void DefaultConstructor_CreatesAnEmptyErrorDictionary()
    {
        var sut = new ValidationException().Errors;
        sut.Keys.Should().BeEquivalentTo(Array.Empty<string>());
    }

    [Fact]
    public void SingleValidationFailure_CreatesASingleElementErrorDictionary()
    {
        //Arrange
        var sut = new List<ValidationFailure>
        {
            new ValidationFailure("Age", "must be over 18"),
        };

        //Act
        var actual = new ValidationException(sut).Errors;

        //Assert(s)
        actual.Keys.Should().BeEquivalentTo(new string[] { "Age" });
        actual["Age"].Should().BeEquivalentTo(new string[] { "must be over 18" });
    }


    [Fact]
    public void MulitpleValidationFailureForMultipleProperties_CreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
    {
        //Arrange
        var sut = new List<ValidationFailure>
        {
            new ValidationFailure("Age", "must be 18 or older"),
            new ValidationFailure("Age", "must be 25 or younger"),
            new ValidationFailure("Password", "must contain at least 8 characters"),
            new ValidationFailure("Password", "must contain a digit"),
            new ValidationFailure("Password", "must contain upper case letter"),
            new ValidationFailure("Password", "must contain lower case letter"),
        };

        //Act
        var actual = new ValidationException(sut).Errors;

        //Assert(s)
        actual.Keys.Should().BeEquivalentTo(new string[] { "Password", "Age" });
        actual["Age"].Should().BeEquivalentTo(new string[]
        {
            "must be 25 or younger",
            "must be 18 or older",
        });
        actual["Password"].Should().BeEquivalentTo(new string[]
        {
            "must contain lower case letter",
            "must contain upper case letter",
            "must contain at least 8 characters",
            "must contain a digit",
        });
    }
}