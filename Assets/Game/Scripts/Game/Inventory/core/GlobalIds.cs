using System;
using System.Collections;
using System.Collections.Generic;

public class GlobalIds
{
	private static System.Random random = new System.Random();
    public int UniqueId;
    public static int GenerateGUID()
    {
        DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        int newId = Convert.ToInt32(timestamp) + random.Next(1000000000);
        return newId;
    }
}

