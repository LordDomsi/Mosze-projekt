using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemyTypeScriptableObjectTests
{
    [Test]
    public void GeneratedCorrectly()
    {
        // Teszteli, hogy az EnemyTypeScriptableObject létrejön-e
        var enemyType = ScriptableObject.CreateInstance<EnemyTypeScriptableObject>();
        Assert.IsNotNull(enemyType);
    }

    [Test]
    public void PropertiesWork()
    {
        // Teszteli, hogy a tulajdonságok jól mûködnek-e
        var enemyType = ScriptableObject.CreateInstance<EnemyTypeScriptableObject>();
        var testPrefab = new GameObject("TestEnemy");

        enemyType.enemyType = "TestEnemy";
        enemyType.enemyPrefab = testPrefab;
        enemyType.enemyHealth = 100f;
        enemyType.enemySpeed = 5f;
        enemyType.enemyShootSpeed = 2f;
        enemyType.enemyDamage = 20;
        enemyType.enemyBulletSpeed = 10f;
        enemyType.pointsWorth = 500;
        enemyType.rotationSpeed = 3f;
        enemyType.specialAttackShootSpeed = 1.5f;
        enemyType.specialAttackRotationSpeed = 2f;
        enemyType.twoFirePoints = true;
        enemyType.enemyCountBasedOnStage = new List<int> { 1, 2, 3, 4 };

        Assert.AreEqual("TestEnemy", enemyType.enemyType);
        Assert.AreEqual(testPrefab, enemyType.enemyPrefab);
        Assert.AreEqual(100f, enemyType.enemyHealth);
        Assert.AreEqual(5f, enemyType.enemySpeed);
        Assert.AreEqual(2f, enemyType.enemyShootSpeed);
        Assert.AreEqual(20, enemyType.enemyDamage);
        Assert.AreEqual(10f, enemyType.enemyBulletSpeed);
        Assert.AreEqual(500, enemyType.pointsWorth);
        Assert.AreEqual(3f, enemyType.rotationSpeed);
        Assert.AreEqual(1.5f, enemyType.specialAttackShootSpeed);
        Assert.AreEqual(2f, enemyType.specialAttackRotationSpeed);
        Assert.IsTrue(enemyType.twoFirePoints);
        Assert.AreEqual(4, enemyType.enemyCountBasedOnStage.Count);
        Assert.AreEqual(2, enemyType.enemyCountBasedOnStage[1]);
    }

    [Test]
    public void CanAddValues()
    {
        // Teszteli, hogy a lista bõvítése mûködik-e
        var enemyType = ScriptableObject.CreateInstance<EnemyTypeScriptableObject>();
        enemyType.enemyCountBasedOnStage = new List<int>();

        enemyType.enemyCountBasedOnStage.Add(5);
        enemyType.enemyCountBasedOnStage.Add(10);

        Assert.AreEqual(2, enemyType.enemyCountBasedOnStage.Count);
        Assert.AreEqual(5, enemyType.enemyCountBasedOnStage[0]);
        Assert.AreEqual(10, enemyType.enemyCountBasedOnStage[1]);
    }
}
