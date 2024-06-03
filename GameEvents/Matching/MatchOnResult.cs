namespace CodeName.EventEngine.GameEvents.Matching
{
    public readonly struct MatchOnResult<TGameEvent, TGameState> : INodeMatchResult<TGameState> where TGameEvent : GameEvent<TGameState>
    {
        public MatchOnResult(ISimulation<TGameState> context, EventMatchCondition<TGameEvent, TGameState> condition)
        {
            Tracker = context;
            Node = null;
            Event = null;

            var node = context.Events.CurrentNode;
            if (node.Event is TGameEvent gameEvent && (condition?.Invoke(new NodeMatchContext<TGameEvent, TGameState>(Tracker, node, gameEvent)) ?? true))
            {
                Node = node;
                Event = gameEvent;
            }
        }

        public bool IsSuccess => Event != null;

        public ISimulation<TGameState> Tracker { get; }
        public GameEventNode<TGameState> Node { get; }
        public TGameEvent Event { get; }

        public static implicit operator bool(MatchOnResult<TGameEvent, TGameState> value)
        {
            return value.IsSuccess;
        }
    }
}
