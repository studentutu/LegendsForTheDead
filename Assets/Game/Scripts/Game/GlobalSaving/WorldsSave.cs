using System.Collections;
using System.Collections.Generic;
using Game;
public static class WorldsSave
{
    public static void SaveToPath<T>(T anything, string nameOfFile)
    {
        var serializer = new BinarySerializer<T>();

        serializer.Save(anything, nameOfFile);

    }

    public static T LoadFrom<T>(string nameOfFile)
    {
        var serializer = new BinarySerializer<T>();

        return serializer.Load(nameOfFile);

    }

    public static void ClearAllData()
    {
        var serializer = new BinarySerializer<int>();
        serializer.DeleteAllData();
    }

}
