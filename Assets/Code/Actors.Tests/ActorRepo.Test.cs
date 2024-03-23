using DGJ24.Actors;
using NUnit.Framework;
using UnityEngine;

namespace DCJ24.Actors
{
    public class ActorRepoTests
    {
        private static ActorRepo MakeTestRepo()
        {
            var gameObject = new GameObject();
            return gameObject.AddComponent<ActorRepo>();
        }

        [Test]
        public void ShouldHaveNoActorsOnCreation()
        {
            var repo = MakeTestRepo();

            var actors = repo.Actors;

            Assert.That(actors, Is.Empty);
        }
    }
}
