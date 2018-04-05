﻿using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace JJ.Presentation.Synthesizer.WinForms.Helpers
{
	internal class DelayedControlInvoker : IDisposable
	{
		private const double DELAY_MILLISECONDS = 100;

		// WARNING:
		// I want the next delayed invokation to cancel the previous one.
		// The System Timer uses the thread pool, so each invoke will not the cancel previous one.
		// It will basically do its very best to make sure the invokation goes off.
		// So we need to write logic to specifically prevent that, keeping the multi-threaded nature of this class in mind.
		// 
		// WinForms Timer is no option. The benefit of the WinForms Timer would be that it cancels previous invokations.
		// But starting WinForms Timer should be done on the UI thread,
		// requiring you to invoke the UI thread, which you were specifically trying to avoid.

		private readonly Timer _timer = new Timer(DELAY_MILLISECONDS) { AutoReset = true };
		private readonly Control _control;

		private bool _busy;
		private Action _action;

		public DelayedControlInvoker(Control control)
		{
			_control = control ?? throw new ArgumentNullException(nameof(control));
			_timer.Elapsed += _timer_Elapsed;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}

		public void InvokeWithDelay(Action action)
		{
			// This method is sequentially invoked on the UI thread.
			// (We could guarantee it, by putting a lock around it, but we don't have to.)

			// Prevent starting the timer again as long as Elapsed has not gone off.
			if (_busy)
			{
				return;
			}
			_busy = true;

			_action = action ?? throw new ArgumentNullException(nameof(action));
			_timer.Start();
		}

		private void _timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_timer.Stop();
			_control.Invoke(_action);

			// Accept new invokations only after the current one was processed.
			_busy = false;
		}
	}
}