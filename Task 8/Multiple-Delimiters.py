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
    # 3rd: split input string at the newline (\n) to separate the delimiters and the numbers
    partialStrInParts = strIn.split("\\n")
    # 4th: check for delimiters and set them from the input. 
    if(len(partialStrInParts[0]) < 5):
        print("ERROR: input string has no delimiters")
        return 0
    untrimmedDelimiters = partialStrInParts[0][2:]
    unsplittedDelimiters = untrimmedDelimiters.strip("[]")
    delimiters = unsplittedDelimiters.split("][") # Added a splitting of the delimiters by the brackets inbetween them
    numbers = partialStrInParts[1]
    # Added a loop to split by every delimiter and end up with just one list of splitted string numbers
    strInParts = [numbers]
    for delim in delimiters:
        newParts = []
        for part in strInParts:
            newParts.extend(part.split(delim))
        strInParts = newParts
    # 5th: check if the parts of the input string represent integers, and add them if they do
    index = 0
    sum = 0
    negatives = [] # Added an empty list to fill with the negative values if any
    for elem in strInParts:
        try:
            if(0 <= int(elem) <= 1000): # Added a check for how big is the number, only sum if not greater than 1000
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
inputString = input("""Enter a string containing one or more delimiter characters, each one enclosed by square brackets, 
and any number of positive integers separated by the delimiter, following the format //[delimiter]\\n[numbers...]
Values over 1000 are ignored.\n""")
sum = Add(inputString)
print("You entered \"" + inputString + "\"")
result = f"{sum}"
print("The sum is " + result)