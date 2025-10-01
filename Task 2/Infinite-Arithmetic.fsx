open Microsoft.FSharp.Core.Result

// Type to pass different error messages
type ErrorMessage =
    | InputMustNotBeEmpty
    | InputMustFollowStructure
    | ElementMustBeInt of int
    // Error no longer needed | TooManyElements

// Prompt the user for the input string
let receiveInput =
    printfn "Enter up to two integers separated by a comma:"
    System.Console.ReadLine()

// Check if input string is empty
let inputNotEmpty (input:string) =
    if input = "" then
        Error InputMustNotBeEmpty
    else Ok input

// Check if input string follow a valid structure
let inputStructureValid (input:string) =
    if input.Contains(",") then
        Ok input
    else 
        match System.Int32.TryParse(input) with
            | false, _ -> Error (ElementMustBeInt 1)
            | true, _ -> Ok input

// Check and validate the input string
let validateInput input =
    input
    |> inputNotEmpty
    |> bind inputStructureValid

// Split the input string by the delimiter
let splitInput (input:Result<string,ErrorMessage>) =
    match input with
        | Ok strInput -> Ok (strInput.Split(","))
        | Error err -> Error err

// Check if every element splitted from the input string represents an integer
let validateStrParts (input:array<string>) =
    let rec checkElement index accum =
        if index >= input.Length then
            Ok accum
        else
            match System.Int32.TryParse(input[index]) with
                | true, value -> checkElement (index+1) (value::accum)
                | false, _ -> Error (ElementMustBeInt (index+1))
    checkElement 0 []

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