# Add function definition 
def Add(strIn):
    # 1st: check if input string is empty
    if(len(strIn) == 0): return 0
    # 2nd: check if input string has valid format //[delimiter]\n[numbers...]
    formatStart = "//"
    formatMiddle = "\\n"
    if (formatStart not in strIn) or (formatMiddle not in strIn):
        print("ERROR: input string does not follow valid format")
        return 0
    # 3rd: split input string at the newline (\n) to separate the delimiter and the numbers
    partialStrInParts = strIn.split("\\n")
    # 4th: check for delimiter and set it from the input
    if(len(partialStrInParts[0]) < 3):
        print("ERROR: input string has no delimiter")
        return 0
    delimiter = partialStrInParts[0][2:]
    numbers = partialStrInParts[1]
    strInParts = [partial for partial in numbers.split(delimiter)]
    # 5th: check if the parts of the input string represent integers, and add them if they do
    index = 0
    sum = 0
    negatives = [] # Added an empty list to fill with the negative values if any
    for elem in strInParts:
        try:
            sum += int(elem)
            index += 1
            if(int(elem) < 0):
                negatives.append(int(elem))
        except:
            print("ERROR: element " + str(index+1) + " is not an integer")
            return 0
    # 6th: return the sum of the integers obtained from the input string, or raise an exception if there were any negative numbers
    if(len(negatives) > 0):
        raise Exception(f"negatives not allowed: {negatives}")
    return sum

# General execution of the program
inputString = input("""Enter a string containing a delimiter character, and any number of positive integers separated by the delimiter,
following the format //[delimiter]\\n[numbers...]\n""")
sum = Add(inputString)
print("You entered \"" + inputString + "\"")
result = f"{sum}"
print("The sum is " + result)