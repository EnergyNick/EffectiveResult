# Effective Result

[![Build and test](https://github.com/EnergyNick/EffectiveResult/actions/workflows/BuildAndTest.yaml/badge.svg)](https://github.com/EnergyNick/EffectiveResult/actions/workflows/BuildAndTest.yaml)
![Code coverage](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/EnergyNick/5ff54bc090435317ddc6be0b72289f04/raw/EffectiveResult-code-coverage.json)


Implementation of the result monad pattern for an alternative way of handling errors. 
Focused on ease of use with Fluent API and support for LINQ-like chaining.
Support (de)serialization by System.Text.Json.

## Roadmap of library
- [x] Base Result model implementation
- [x] Helpful extension methods for result manipulation
- [x] Support channing by `Then` methods
- [x] Add support of async implementations for methods `Then`, `Try`, `Match` and etc
- [x] System.Text.Json (de)serialization implementation
- [ ] Update Readme with extended wiki with all current features: Then methods, Async support, channing usages and etc.
- [ ] Add support of value-tuple result with extensions (ValueResult)
- [ ] Support FluentValidation
- [ ] Support FluentAssertions
- [ ] Support TUnit assertions

## Library usage examples
### Result model
```csharp
// Just result
Result successRes = Result.Ok();

bool isSuccess = successRes.IsSuccess;
bool isFailed = successRes.IsFailed;

// Error from message
Result failedRes1 = Result.Fail("Ooops!"); 
// Error from exception
Result failedRes2 = Result.Fail(new Exception("Bad situation!")); 
```

### Result with value
```csharp
Result<int> successValuedRes = Result.Ok(5);
// Error from message
Result<int> failedValueRes1 = Result.Fail<int>("Ooops!"); 
// Error from exception
Result<int> failedValueRes2 = Result.Fail<int>(new Exception("Bad situation!")); 
```

### Result class methods
```csharp
Result<int> countResult = Result.Ok(1000);

// Throw, if failed
int myValue1 = countResult.Value; 
// Not throw, but can return default value of type
int myValue2 = countResult.ValueOrDefault; 
// Get value or return other value
int myValue3 = countResult.GetValueOrDefault(100); 
// Get value or construct and return other value
int myValue4 = countResult.GetValueOrDefault(() => 2); 
```

### Work with errors
```csharp
// Base type of errors
var errorWithMessage = new ResultError("Bad 1");
var errorWithException = new ResultError(new Exception("Sorry, but..."));
var errorWithMessageAndException = new ResultError("Bad 2", new Exception("Sorry, but..."));
var errorWithCaused = new ResultError("Bad 2", new Exception("Sorry, but..."), [errorWithMessage]);

// Errors using
Result fail = Result.Fail(new ArgumentException("myVariable"));
// Get errors of result
IReadOnlyCollection<ResultError> errors = fail.Errors;

bool hasSpecificError = fail.TryGetException<ArgumentException>(out ArgumentException typedException);

// Get all exception of ResultError, if contains
IEnumerable<Exception> exceptions = fail.GetExceptions(); 
// Get all typed exception of ResultError, if contains
IEnumerable<ArgumentException> exceptionsTyped = fail.GetExceptions<ArgumentException>(); 
// Get all filtered exception of ResultError, if contains
IEnumerable<Exception> exceptionsFiltered = fail.GetExceptions(x => x.Message == "Oops!"); 

// Get first exception of ResultError, if contains
bool contains1 = fail.TryGetException(out Exception? exception); 
// Get first typed exception of ResultError, if contains
bool contains2 = fail.TryGetException(out ArgumentException? argumentEx); 
// Get first filtered exception of ResultError, if contains
bool contains3 = fail.TryGetException(out Exception? filtered, x => x.Message == "Oops!"); 
```

### Useful static methods
```csharp
// Conditional
Result cond1 = Result.OkIf(1 == 2, "Error message");
Result cond2 = Result.FailIf(2 == 1, new Exception("Oh..."));

/// Try and return
// Return failed result with error, variable will be false
Result wasSuccess = Result.Try(() => throw new Exception("No no no")); 
// Return success result with value
Result<int> valuedResult = Result.Try(() => 100); 
```
