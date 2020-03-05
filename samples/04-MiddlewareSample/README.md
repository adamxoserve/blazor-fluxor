# 01-MiddlewareSample
This sample shows how to write Middleware for Fluxor

## Creating your middleware
1. In the Client project create a new folder `Store\Middlewares\AnExample`
2. Create a new class `AnExampleMiddleware` and descend it from the `Blaxor.Fluxor.Middleware` class

## Consuming your middleware
Fluxor will *not* automatically discover and install Middlewares. These must be registered manually in the services registration code, like so

```c#
...AddFluxor(options => options
  .UseDependencyInjection(typeof(XXX).Assembly)
  .AddMiddleware<Blazor.Fluxor.Routing.RoutingMiddleware>() // So we can see route changes in the console
  .AddMiddleware<AnExampleMiddleware>()
);
```

Where `XXX` is `Startup` in Server-side Blazor apps, or `Program` in Client-side Blazor apps, or any class at all in your store (action / feature / state / whatever).

Features, Reducers, and Effects in the same assembly + namespace (or sub-namespace) as a class implementing IMiddleware will be blacklisted from automatic registration when using `options.UseDependencyInjection`.

When you call `options.AddMiddleware<T>()` then classes in the same assembly + namespace (or sub-namespace) will be whitelisted, and therefore autoatically discovered by dependency injection because you have whitelisted the middleware.

For a more comprehensive example of writing middleware for Fluxor look at the [Redux Devtools Middleware][1] classes.

  [1]: <https://github.com/mrpmorris/blazor-fluxor/blob/master/src/Blazor.Fluxor/ReduxDevTools/ReduxDevToolsMiddleware.cs>
