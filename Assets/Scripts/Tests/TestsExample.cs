using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using Player;
using UnityEngine.TestTools;
using Vector2 = UnityEngine.Vector2;

public class TestsExample
{
    [Test]
    public void TargetSpeed_IsZero_WhenSpeed_IsZero()
    {
        var result = SimpleMovement.TargetSpeed(new Vector2(1231,434343), 0);
        Assert.AreEqual(Vector2.zero, result);
    }

    [Test]
    public void SpeedDifference()
    {
        var result = SimpleMovement.SpeedDifference(new Vector2(15,20), new Vector2(5, 10));
        Assert.AreEqual(new Vector2(10,10), result);
    }
}
