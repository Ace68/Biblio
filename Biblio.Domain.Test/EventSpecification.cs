using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

using Biblio.Infrastructures.Abstracts;
using Biblio.Infrastructures.Concretes;

namespace Biblio.Domain.Test
{
    /// <summary>
    /// https://github.com/luizdamim/NEventStoreExample/tree/master/NEventStoreExample.Test
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class EventSpecification<TCommand> where TCommand : CommandBase
    {
        protected Exception Caught { get; private set; }

        protected InMemoryEventRepository Repository { get; private set; }

        [Test]
        public void SetUp()
        {
            this.Caught = null;
            this.Repository = new InMemoryEventRepository(this.Given().ToList());
            var handler = this.OnHandler();

            try
            {
                handler.Handle(this.When());
                var expected = this.Expect().ToList();
                var published = this.Repository.Events;
                CompareEvents(expected, published);
            }
            catch (AssertionException)
            {
                throw;
            }
            catch (Exception exception)
            {
                this.Caught = exception;
                throw;
            }
        }

        protected abstract IEnumerable<EventBase> Given();

        protected abstract TCommand When();

        protected abstract ICommandHandler<TCommand> OnHandler();

        protected abstract IEnumerable<EventBase> Expect();

        private static void CompareEvents(ICollection<EventBase> expected, ICollection<EventBase> published)
        {
            Assert.That(published.Count, Is.EqualTo(expected.Count), "Different number of expected/published events.");

            var compareObjects = new CompareLogic();

            var eventPairs = expected.Zip(published, (e, p) => new { Expected = e, Produced = p });
            foreach (var events in eventPairs)
            {
                var result = compareObjects.Compare(events.Expected, events.Produced);
                if (!result.AreEqual)
                {
                    Assert.Fail(result.DifferencesString);
                }
            }
        }
    }
}
