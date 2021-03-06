using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Views;
using OmniGui.Geometry;

namespace OmniGui.Android
{
    public class AndroidEventSource : IEventSource
    {

        public AndroidEventSource(OmniGuiView view)
        {
            view.Touchables.Add(view);
            
            Pointer = Observable
                .FromEventPattern<EventHandler<View.TouchEventArgs>, View.TouchEventArgs>(ev => view.Touch += ev, ev => view.Touch -= ev) 
                .Select(ev =>
                {
                    var eventArgsEvent1 = ev.EventArgs.Event;
                    return new PointerInput
                    {
                        Point = new Point(eventArgsEvent1.RawX, eventArgsEvent1.RawY),
                        PrimaryButtonStatus = PointerStatus.Down                        
                    };
                });
            TextInput = view.TextInput.Select(sequence => new TextInputArgs {Text = sequence.ToString()});
            KeyInput = CreateKeyInputObservable(view);
        }
    
        private static IObservable<KeyArgs> CreateKeyInputObservable(View element)
        {
            var fromKeyPress = Observable.FromEventPattern<EventHandler<View.KeyEventArgs>, View.KeyEventArgs>(
                    ev => element.KeyPress += ev,
                    ev => element.KeyPress -= ev)
                .Where(pattern => pattern.EventArgs.Event.Action == KeyEventActions.Down)
                .Select(ep => new KeyArgs(ep.EventArgs.KeyCode.ToOmniGui()));

 
            return fromKeyPress;
        }

        public IObservable<PointerInput> Pointer { get; }
        public IObservable<TextInputArgs> TextInput { get; } = new Subject<TextInputArgs>();
        public IObservable<KeyArgs> KeyInput { get; }
        public IObservable<ScrollWheelArgs> ScrollWheel { get; } = Observable.Never<ScrollWheelArgs>();
    }
}