using Blazor.Fluxor;

namespace AzureADB2C.Client.Store.Counter
{
  public class IncrementCounterReducer : Reducer<CounterState, IncrementCounterAction>
  {
    public override CounterState Reduce(CounterState state, IncrementCounterAction action) =>
      new CounterState(state.ClickCount + 1);
  }
}
