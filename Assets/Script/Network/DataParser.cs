using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PenguinModel;

class DataParser
{
    public static Object ParseData(String jsString)
    {
        GameInfo gameInfo = new GameInfo();
        return (Object)Convert.ChangeType(gameInfo, typeof(Object));
    }

    public static Type getDataType(DataType dataType)
    {
        return Type.GetType(dataType.ToString());
    }
}