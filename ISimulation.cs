using System.Collections.Generic;
using CodeName.EventEngine.GameEvents;
using CodeName.EventEngine.Tasks;

namespace CodeName.EventEngine
{
    public interface ISimulation<TState>
    {
        public TState State { get; }

        public EventTracker<TState> Events { get; }
        public GameEventNode<TState> CurrentNode => Events.CurrentNode;

        public IReadOnlyList<IEventHandler<TState>> EventHandlers { get; }

        /// <summary>
        /// Raise an event to be applied.
        /// <para/>
        /// Note: An event can be Prevented. Raising an event does not guarantee it will be applied.
        /// <para/>
        /// When an event is raised, 3 events will be called: <br/>
        /// 1. OnEventRaised - Use to prevent events from being applied. <br/>
        /// 2. OnEventConfirmed - Use to react to events before they are applied. <br/>
        /// 3. OnEventApplied - Use to react to events after they are applied.
        /// </summary>
        public StateTask RaiseEvent(GameEvent<TState> gameEvent);
    }
}
