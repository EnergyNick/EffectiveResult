# FunctionalResult

### Simple examples
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

/// Result methods
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
var coreError = new Error("Bad 1");
var exError = new ExceptionalError(new Exception("Sorry, but..."));

// Errors using
Result fail = Result.Fail(new ArgumentException("myVariable"));
// Get errors of result
IReadOnlyCollection<IError> errors = fail.Errors; 

bool hasSpecificError = fail.Errors.HasErrorsOfType<ExceptionalError>(x => x.Exception is ArgumentException);

// Get all exception of ExceptionalError, if contains
IEnumerable<Exception> exceptions = fail.GetExceptions(); 
// Get all typed exception of ExceptionalError, if contains
IEnumerable<ArgumentException> exceptionsTyped = fail.GetExceptions<ArgumentException>(); 
// Get all filtered exception of ExceptionalError, if contains
IEnumerable<Exception> exceptionsFiltered = fail.GetExceptions(x => x.Message == "Oops!"); 

// Get first exception of ExceptionalError, if contains
bool contains1 = fail.TryGetException(out Exception? exception); 
// Get first typed exception of ExceptionalError, if contains
bool contains2 = fail.TryGetException(out ArgumentException? argumentEx); 
// Get first filtered exception of ExceptionalError, if contains
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