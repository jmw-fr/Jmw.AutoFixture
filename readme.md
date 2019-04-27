# Jmw.AutoFixture

Some tools to simplify Fixtures and Mocks in Unit Tests using AutoFixture.

## Why

Creating Fixtures and Mocks is very repetitive and can require many glue to setup tests.

If you like to check Code Coverage like I do, setting up unit tests can be very challenging and require many many code.

To simply this, I create a specific class for testing a class and I use this class to setup everything I need in my tests, including expected results.

## How I setup my tests

So my tests look like :

``` C#
/// <summary>
/// Function nominal test (i.e. normal functioning).
/// </summary>
[Fact]
[Trait(nameof(MyClass.GetResultAsync), nameof(MyClass))]
public async void Constructor_Test_GuardClauses()
{
    // Arrange
    var moq = new MyClassMocks(TestType.Nominal);
    var sut = moq.Fixture.Create<MyClass>();

    // Act
    var r = await sut.GetResultAsync(moq.Param);

    // Assert
    Assert.NotNull(r);
    Assert.Equal(moq.ExpectedResult, r);
    moq.VerifyAll(true);
}

/// <summary>
/// Test if the function correctly thow exceptions.
/// </summary>
/// <param name="testType">Type of test.</param>
/// <param name="expectedException">Type of expected exception.</param>
[Theory]
[Trait(nameof(MyClass.GetResultAsync), nameof(MyClass))]
[InlineData(TestType.ThrowException, typeof(System.InvalidOperationException))]
public async void GetResultAsync_Must_ThrowException(TestType testType, System.Type expectedException)
{
    // Arrange
    var moq = new MyClassMocks(testType);
    var sut = moq.Fixture.Create<MyClass>();

    // Act
    var ex = await Assert.ThrowsAnyAsync<System.Exception>(async () => await sut.GetResultAsync(moq.Param));

    // Assert
    Assert.NotNull(ex);
    Assert.Equal(expectedException, ex.GetType());
}
```

The first test simply checks the nominal function of `GetResultAsync()`. Everything I need is created by MyClassMocks.

In the Assert section, I check the result to be non-null, to be equal to my expected fixtured result and I finally ask my `MyClassMocks` to check that all function injected in `MyClass` are correctly called.

The second test setup the mock to generate an exception and I check the thrown exception is correct.

### Mocks class

From previous examples, `MyClassMocks`inherit from the abstract class `Mocks` from my nuget package. This class simplify the setup of fixtures / moqs.

``` C#
/// <summary>
/// Type of test emulated.
/// </summary>
public enum TestType
{
    /// <summary>
    /// Norminal function.
    /// </summary>
    Nominal,
}

/// <summary>
/// Mocks for <see cref="MyClassUnitTest" />.
/// </summary>
public class MyClassMocks : Mocks<TestType>
{
        /// <summary>
        /// Initializes a new instance of the <see cref="MyClassMocks"/> class.
        /// </summary>
        /// <param name="typeTest">Type of test to emulate.</param>
        public MyClassMocks(TestType typeTest)
            : base(typeTest)
        {
            Param = CreateFixture<string>();
            ExpectedResult = CreateFixture<string>();

            switch (typeTest)
            {
                case TestType.Nominal:
                    {
                        CreateMock<IRepository>(true)
                            .Setup(m => m.GetSomeDataAsync(It.Is<string>(p => p == Param)))
                            .ReturnsAsync(ExpectedResult);

                        break;
                    }

                case TestType.ThrowException:
                    {
                        CreateMock<IRepository>(true)
                            .Setup(m => m.GetSomeDataAsync(It.Is<string>(p => p == Param)))
                            .Throws(new System.InvalidOperationException());

                        break;
                    }

                default:
                    {
                        throw new System.ArgumentException(nameof(typeTest));
                    }
            }
        }
    }
}

```
