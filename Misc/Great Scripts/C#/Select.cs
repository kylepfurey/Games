
// Static Select Statement Functions Script
// by Kyle Furey

// Include this heading to use the class
using static Select;

// Select statements.
public static class Select
{
    // Select from a boolean and return the chosen data
    public static DataType select<DataType>(bool condition, DataType trueData, DataType falseData)
    {
        return condition ? trueData : falseData;
    }

    // Select from an byte and return the chosen data
    public static DataType select<DataType>(byte n, params DataType[] nData)
    {
        return nData[n];
    }

    // Select from an integer and return the chosen data
    public static DataType select<DataType>(int n, params DataType[] nData)
    {
        return nData[n];
    }

    // Compare two integers and return the chosen data
    public static DataType GreaterLessEqual<DataType>(int comparer, int compared, DataType greaterThanData, DataType lessThanData, DataType equalToData)
    {
        if (comparer > compared)
        {
            return greaterThanData;
        }
        else if (comparer < compared)
        {
            return lessThanData;
        }
        else
        {
            return equalToData;
        }
    }

    // Compare two floats and return the chosen data
    public static DataType GreaterLessEqual<DataType>(float comparer, float compared, DataType greaterThanData, DataType lessThanData, DataType equalToData)
    {
        if (comparer > compared)
        {
            return greaterThanData;
        }
        else if (comparer < compared)
        {
            return lessThanData;
        }
        else
        {
            return equalToData;
        }
    }
}
