using UnityEngine;
using System.Collections;

public enum CustomFingerPhase
{
	None = 0,
	Down
}

public class FingerDownEvent : FingerEvent 
{
	public CustomFingerPhase Phase = CustomFingerPhase.None;
}

[AddComponentMenu( "FingerGestures/Finger Events/Finger Down Detector" )]
public class FingerDownDetector : FingerEventDetector<FingerDownEvent>
{
    public event FingerEventHandler OnFingerDown;
    public string MessageName = "OnFingerDown";    
    
    protected override void ProcessFinger( FingerGestures.Finger finger )
    {
        if( finger.IsDown && !finger.WasDown )
        {
            FingerDownEvent e = GetEvent( finger.Index );
            e.Name = MessageName;
			e.Phase = CustomFingerPhase.Down;
            UpdateSelection( e );

            if( OnFingerDown != null )
                OnFingerDown( e );

            TrySendMessage( e );
        }
		
		else if( !finger.IsDown && finger.WasDown)
		{
			FingerDownEvent e = GetEvent( finger.Index );
			e.Name = MessageName;
			e.Phase = CustomFingerPhase.None;
			
			UpdateSelection( e );

            if( OnFingerDown != null )
                OnFingerDown( e );

            TrySendMessage( e );
		}
    }
}
