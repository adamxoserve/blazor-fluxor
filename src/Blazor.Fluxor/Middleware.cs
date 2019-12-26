﻿using System;
using System.Threading.Tasks;

namespace Blazor.Fluxor
{
	/// <see cref="IMiddleware"/>
	public abstract class Middleware : IMiddleware
	{
		/// <summary>
		/// A reference to the store instance this middleware was added to
		/// </summary>
		protected IStore Store { get; private set; }

		/// <summary>
		/// Number of times <see cref="IMiddleware.BeginInternalMiddlewareChange"/> has been called
		/// </summary>
		protected int BeginMiddlewareChangeCount { get; private set; }

		/// <summary>
		/// True if <see cref="BeginMiddlewareChangeCount"/> is greater than zero
		/// </summary>
		protected bool IsInsideMiddlewareChange => BeginMiddlewareChangeCount > 0;

		/// <see cref="IMiddleware.GetClientScripts"/>
		public virtual string GetClientScripts() => null;

		/// <see cref="IMiddleware.InitializeAsync(IStore)"/>
		public virtual ValueTask InitializeAsync(IStore store)
		{
			Store = store;
			return new ValueTask(Task.CompletedTask);
		}

		/// <see cref="IMiddleware.AfterInitializeAllMiddlewares"/>
		public virtual void AfterInitializeAllMiddlewares() { }

		/// <see cref="IMiddleware.MayDispatchAction(object)"/>
		public virtual bool MayDispatchAction(object action) => true;

		/// <see cref="IMiddleware.BeforeDispatch(object)"/>
		public virtual void BeforeDispatch(object action) { }

		/// <see cref="IMiddleware.AfterDispatch(object)"/>
		public virtual void AfterDispatch(object action) { }

		/// <summary>
		/// Executed when <see cref="BeginMiddlewareChangeCount"/> becomes zero
		/// </summary>
		protected virtual void OnInternalMiddlewareChangeEnding() { }

		IDisposable IMiddleware.BeginInternalMiddlewareChange()
		{
			BeginMiddlewareChangeCount++;
			return new DisposableCallback(() =>
			{
				if (BeginMiddlewareChangeCount == 1)
					OnInternalMiddlewareChangeEnding();
				BeginMiddlewareChangeCount--;
			});
		}
	}
}
