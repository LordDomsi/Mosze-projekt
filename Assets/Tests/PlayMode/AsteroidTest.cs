using NUnit.Framework;
using UnityEngine;

public class AsteroidTest
{
    private GameObject asteroidObject;
    private Asteroid asteroid;

    [SetUp]
    public void SetUp()
    {
        asteroidObject = new GameObject();
        asteroid = asteroidObject.AddComponent<Asteroid>();
    }

    [Test]
    public void AsteroidSetSize_SetsCorrectSize()
    {
        asteroid.SetSize(0.08f);

        Assert.AreEqual(0.08f, asteroid.GetSize());
    }

    [Test]
    public void AsteroidSetSize_SetsDifferentSizes()
    {
        asteroid.SetSize(0.16f);
        Assert.AreEqual(0.16f, asteroid.GetSize());

        asteroid.SetSize(0.04f);
        Assert.AreEqual(0.04f, asteroid.GetSize());
    }

    [Test]
    public void AsteroidInitializesCorrectlyWithSize()
    {
        asteroid.SetSize(0.08f);
        Assert.AreEqual(0.08f, asteroid.GetSize());
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(asteroidObject);
    }
}
