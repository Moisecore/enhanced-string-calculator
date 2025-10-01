# Add function definition 
def Add(strIn):
    # 1st: check if input string is empty
    if(len(strIn) == 0): return 0
    # 2nd: split input string at the commas
    strInParts = strIn.split(",")
    # 3rd: check for up to two numbers no longer needed
    # if(len(strInParts) > 2):
    #    print("ERROR: input must have up to two integers separated by a single comma")
    #    return 0
    # 4th: check if the parts of the input string represent integers, and add them if they are
    index = 0
    sum = 0
    for elem in strInParts:
        try:
            sum += int(elem)
            index += 1
        except:
            print("ERROR: element " + str(index+1) + " is not an integer")
            return 0
    # 5th: return the sum of the integers obtained from the input string
    return sum

# General execution of the program
inputString = input("Enter up to two integers separated by a comma:\n")
sum = Add(inputString)
print("You entered \"" + inputString + "\"")
result = f"{sum}"
print("The sum is " + result)