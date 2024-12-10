using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeScriptableObjectTest
{
    private EnemyTypeScriptableObject enemyTypeSO;

    [SetUp]
    public void SetUp()
    {
        enemyTypeSO = ScriptableObject.CreateInstance<EnemyTypeScriptableObject>();

        enemyTypeSO.enemyType = "Ûrhajó";
        enemyTypeSO.enemyHealth = 100f;
        enemyTypeSO.enemySpeed = 3f;
        enemyTypeSO.enemyShootSpeed = 1.5f;
        enemyTypeSO.enemyDamage = 20;
        enemyTypeSO.enemyBulletSpeed = 10f;
        enemyTypeSO.pointsWorth = 100;
        enemyTypeSO.rotationSpeed = 1.0f;
        enemyTypeSO.specialAttackShootSpeed = 2f;
        enemyTypeSO.specialAttackRotationSpeed = 1.2f;
        enemyTypeSO.twoFirePoints = true;
        enemyTypeSO.enemyCountBasedOnStage = new List<int> { 5, 10, 15 };
    }

    [Test]
    public void EnemyTypeScriptableObjectInitializesCorrectly()
    {
        Assert.AreEqual("Ûrhajó", enemyTypeSO.enemyType);
        Assert.AreEqual(100f, enemyTypeSO.enemyHealth);
        Assert.AreEqual(3f, enemyTypeSO.enemySpeed);
        Assert.AreEqual(10f, enemyTypeSO.enemyBulletSpeed);
        Assert.IsTrue(enemyTypeSO.twoFirePoints);

        Assert.AreEqual(3, enemyTypeSO.enemyCountBasedOnStage.Count);
        Assert.AreEqual(5, enemyTypeSO.enemyCountBasedOnStage[0]);
        Assert.AreEqual(10, enemyTypeSO.enemyCountBasedOnStage[1]);
        Assert.AreEqual(15, enemyTypeSO.enemyCountBasedOnStage[2]);
    }

    [Test]
    public void EnemyTypeScriptableObjectPointsWorthCorrectly()
    {
        Assert.AreEqual(100, enemyTypeSO.pointsWorth);
    }

    [TearDown]
    public void TearDown()
    {
        enemyTypeSO = null;
    }
}
