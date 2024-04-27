using System;
using CodeName.EventSystem.Tasks;
using CodeName.Serialization;

namespace CodeName.EventSystem.GameEvents
{
    public class ConstGameStateTracker<TGameState> : GameStateTracker<TGameState>
    {
        public ConstGameStateTracker(TGameState state, GameStateTrackerConfig<TGameState> config) : base(state, UseNullOpSerializer(config)) {}

        public override async StateTask RaiseEvent(GameEvent<TGameState> gameEvent)
        {
            var currentNode = Events.Push(State, gameEvent);
            {
                await GameStateTrackerUtility.OnEventRaised(this, Config.GameEventHandlers);
                currentNode.Lock();
                await GameStateTrackerUtility.OnEventConfirmed(this, Config.GameEventHandlers);
                await currentNode.Event.Apply(this);
                await GameStateTrackerUtility.OnEventApplied(this, Config.GameEventHandlers);
            }
            Events.Pop();
        }

        private static GameStateTrackerConfig<TGameState> UseNullOpSerializer(GameStateTrackerConfig<TGameState> config)
        {
            var result = config.ShallowCopy();
            result.Serializer = new NullOpSerializer();

            return result;
        }

        private class NullOpSerializer : ISerializer
        {
            public string Serialize(object value)
            {
                throw new NotSupportedException();
            }

            public T Deserialize<T>(string data)
            {
                throw new NotSupportedException();
            }

            public T Clone<T>(T value)
            {
                return value;
            }
        }
    }
}
