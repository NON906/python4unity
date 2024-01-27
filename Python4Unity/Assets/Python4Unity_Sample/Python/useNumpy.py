import numpy as np

def getRand(range):    
    res = np.random.randint(range)
    return res
    
def getMaxAndMin(dataList):    
    maxIndex = np.argmax(dataList)
    maxVal = dataList[maxIndex]
    minIndex = np.argmin(dataList)    
    minVal = dataList[minIndex]
    res = [maxVal, minVal]
    return res