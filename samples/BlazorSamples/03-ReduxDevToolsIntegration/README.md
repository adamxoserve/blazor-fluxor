# 03-ReduxDevToolsIntegration (Integration with Redux Dev Tools)
This sample shows how to integrate with the [Redux dev tools] plugin for Google Chrome. It is recommended that you read both [Tutorial 1][2] and [Tutorial 2][3] first.

## Setting up the project
Once you have your project up and running, adding support for ReduxDevTools is simple. Edit the code that calls `AddFluxor` and add the Routing and Redux Dev Tools middlewares to the options.

#### Server-side Blazor
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddFluxor(options => options
    .UseDependencyInjection(typeof(Startup).Assembly)
    .AddMiddleware<Blazor.Fluxor.ReduxDevTools.ReduxDevToolsMiddleware>()
    .AddMiddleware<Blazor.Fluxor.Routing.RoutingMiddleware>()
  );
}
```

#### Client-side Blazor
```c#
builder.Services.AddFluxor(options => options
  .UseDependencyInjection(typeof(Program).Assembly)
  .AddMiddleware<Blazor.Fluxor.ReduxDevTools.ReduxDevToolsMiddleware>()
  .AddMiddleware<Blazor.Fluxor.Routing.RoutingMiddleware>()
);
```

## Required changes to state classes
Because the [Redux dev tools][1] implementation uses serialization to switch back to historial states it is necessary to create a parameterless constructor on all of your state classes. At this point in time `System.Text.Json` requires the constructor to be public.

```c#
[Obsolete("Used for deserialization only")]
public CounterState() {}
```

In order to deserialize state your properties must have setters, again the setters (currently) must be public.

## Subscribing to state changes
To ensure your component is re-rendered when state is changed in another component simply descend your components from `FluxorComponent`, like this `@inherits Blazor.Fluxor.Components.FluxorComponent`.

If you do not wish to descend from a specific base class you can instruct Fluxor to call your component's `StateHasChanged` method whenever its state changes, like this:

```c#
protected override void OnInitialized()
{
  base.OnInitialized();
  NameOfYourState.Subscribe(this)
}
```

This will register your component's `StateHasChanged` method to be called back whenever the state changes. There is no need to unsubscribe, Fluxor will no longer call back your component once it has been garbage collected.

  [1]: <https://chrome.google.com/webstore/detail/redux-devtools/lmhkpmbekcpmknklioeibfkpmmfibljd>
  [2]: <https://github.com/mrpmorris/blazor-fluxor/tree/master/samples/01-CounterSample>
  [3]: <https://github.com/mrpmorris/blazor-fluxor/tree/master/samples/02-WeatherForecastSample>
