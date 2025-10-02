open Microsoft.FSharp.Core.Result

// Type to pass different error messages
type ErrorMessage =
    | InputMustNotBeEmpty
    | InputMustFollowStructure
    | ElementMustBeInt of int
    // Error no longer needed | TooManyElements
    | NoDelimiterFound

// Prompt the user for the input string
let receiveInput =
    printfn @"Enter a string containing one or more delimiter characters, each one enclosed by square brackets,
    and any number of positive integers separated by the delimiter, following the format //[delimiter]\n[numbers...]
    Values over 1000 are ignored."
    System.Console.ReadLine()

// Check if input string is empty
let inputNotEmpty (input:string) =
    if input = "" then
        Error InputMustNotBeEmpty
    else Ok input

// Check if input string follows a valid structure
let inputStructureValid (input:string) =
    if input.Contains("//") && input.Contains("\\n") then
        Ok input
    else Error InputMustFollowStructure

// Check and validate the input string
let validateInput input =
    input
    |> inputNotEmpty
    |> bind inputStructureValid

// Get the delimiters from the input string
let getDelimiters (input:string) = 
    let partialStrInParts = input.Split("\\n")
    if partialStrInParts[0].Length < 5 then 
        [||]
    else 
        (partialStrInParts[0][3..partialStrInParts[0].Length-2]).Split("][") // Here it gets splitted by the ][

// Split the input string by the delimiters
let splitInput (input:Result<string,ErrorMessage>) =
    match input with
        | Ok strInput -> 
            let delimiters = getDelimiters strInput
            if delimiters = [||] then
                Error NoDelimiterFound
            else
                let numbersPartStr = strInput.Split("\\n")[1]
                Ok (numbersPartStr.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries))
        | Error err -> Error err

// Check if every element splitted from the input string represents an integer, and raise an exception if there are negative numbers
let validateStrParts (input:array<string>) =
    let rec checkElement index accum (negatives: int list) =
        if index >= input.Length then
            if negatives.Length > 0 then
                raise (System.Exception($"negatives not allowed: {List.rev negatives}"))
            else Ok accum
        else
            match System.Int32.TryParse(input[index]) with
                | true, value -> 
                    if value < 0 then // Added a check to ignore values over 1000
                        checkElement (index+1) (value::accum) (value::negatives)
                    elif value > 1000 then
                        checkElement (index+1) accum negatives
                    else 
                        checkElement (index+1) (value::accum) negatives
                | false, _ -> Error (ElementMustBeInt (index+1))
    checkElement 0 [] []

// Check and validate the values splitted from the input string
let validateStrArray (input:Result<array<string>,ErrorMessage>) =
    match input with
        | Ok strInput -> validateStrParts strInput
            // Check for up to two numbers no longer needed
            // if strInput.Length > 2 then
            //    Error TooManyElements
            // else validateStrParts strInput
        | Error err -> Error err

// Sum the numbers parsed from the input string if valid
let sumNumbers (input:Result<list<int>,ErrorMessage>) : int =
    match input with
        | Error err -> 0
        | Ok numbers -> List.sum numbers

// Returns an int value in accordance with the result of all previous checks and validations
let returnResult (result:Result<list<int>,ErrorMessage>) : int =
    match result with
        | Ok input -> sumNumbers result
        | Error err ->
            match err with
                | InputMustNotBeEmpty -> 0
                | InputMustFollowStructure -> printfn "Error: the input string does not follow the correct syntax"; 0
                | ElementMustBeInt index-> printfn "Error: element %i is not an integer" index; 0
                // No longer an error | TooManyElements -> printfn "ERROR: input must have up to two integers separated by a single comma"; 0
                | NoDelimiterFound -> printfn "Error: no delimiter found"; 0

// Add function definition
let add (input:string) : int =
    input 
    |> validateInput
    |> splitInput
    |> validateStrArray
    |> returnResult

// General execution of the program
let inputString = receiveInput

let sum = add inputString

printfn "You entered \"%s\"" inputString
printfn "The sum is %i" sum