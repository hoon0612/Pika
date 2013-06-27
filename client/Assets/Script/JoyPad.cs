using UnityEngine;
using System.Collections;

// A simple class for bounding how far the GUITexture will move
public class Boundary 
{
	public Vector2 min = Vector2.zero;	
	public Vector2 max = Vector2.zero;	
}

public class JoyPad : MonoBehaviour {

//	static private Object[] JoyPads;					// A static collection of all JoyPads
	static private bool enumeratedJoyPads = false;
	static private float tapTimeDelta = 0.3f;				// Time allowed between taps
	
	public Vector2 position; 								// [-1, 1] in x,y
	public Vector2 deadZone = Vector2.zero;				// Control when position is output
	public bool normalize = false; 					// Normalize output after the dead-zone?
	public int tapCount;									// Current tap count
	public Vector2 size_paddle;
	
	private int lastFingerId = -1;								// Finger last used for this JoyPad
	private float tapTimeWindow;							// How much time there is left for a tap to occur
	private GUITexture gui;								// JoyPad graphic
	private Rect defaultRect;								// Default position / extents of the JoyPad graphic
	public Boundary guiBoundary = new Boundary();			// Boundary for JoyPad graphic
	private Vector2 guiTouchOffset;						// Offset to apply to touch input
	private Vector2 guiCenter;							// Center of JoyPad graphic
	
	public GUITexture Texture_JoyPad; 
	public GUITexture Texture_JoyPadBG;
	public Texture2D texture2D_JoyPad_Normal;
	public Texture2D texture2D_JoyPadBG_Normal;
	public Texture2D texture2D_JoyPad_Clicked;
	public Texture2D texture2D_JoyPadBG_Clicked;
	

	// Use this for initialization
	void Start () {
		// Cache this component at startup instead of looking up every frame	
		gui = GetComponent( typeof(GUITexture) ) as GUITexture;
		
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;
		
		// This is an offset for touch input to match with the top left
		// corner of the GUI
		guiTouchOffset.x = (float) (defaultRect.width * 0.5);
		guiTouchOffset.y = (float) (defaultRect.height * 0.5);
		
		// Cache the center of the GUI, since it doesn't change
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		
		// Let's build the GUI boundary, so we can clamp JoyPad movement
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	}
	
	
	void Disable()
	{
		gameObject.active = false;
		enumeratedJoyPads = false;
	}
	
	void Reset()
	{
		// Release the finger control and set the JoyPad back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		
		Color textureColor = Texture_JoyPad.color;
		textureColor.a = 68.0f/256.0f;
		Texture_JoyPad.color = textureColor;

		//Texture_JoyPad.texture = texture2D_JoyPad_Normal;
		//Texture_JoyPadBG.texture = texture2D_JoyPadBG_Normal;
	}
	
	void LatchedFinger( int fingerId )
	{
		// If another JoyPad has latched this finger, then we must release it
		if ( lastFingerId == fingerId )
			Reset();
	}
	
		
	// Update is called once per frame
	void Update () {
		
		if ( !enumeratedJoyPads )
		{
			
//			JoyPads = FindObjectsOfType(typeof(JoyPad));
			
			enumeratedJoyPads = true;
		}	
			
		int count = Input.touchCount;
		
		// Adjust the tap time window while it still available
		if ( tapTimeWindow > 0 )
			tapTimeWindow -= Time.deltaTime;
		else
			tapCount = 0;
		
		if ( count == 0 )
			Reset();
		else
		{
			for(int i = 0;i < count; i++)
			{
				Touch touch = Input.GetTouch(i);			
				Vector2 guiTouchPos = touch.position - guiTouchOffset;
		
				// Latch the finger if this is a new touch
				if ( gui.HitTest( touch.position ) && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
				{
					
					Color textureColor = Texture_JoyPad.color;
    				textureColor.a = 1.0f;
    				Texture_JoyPad.color = textureColor;
					
					//Texture_JoyPad.texture = texture2D_JoyPad_Clicked;
					//Texture_JoyPadBG.texture = texture2D_JoyPadBG_Clicked;
					
					lastFingerId = touch.fingerId;
					
					// Accumulate taps if it is within the time window
					if ( tapTimeWindow > 0 )
						tapCount++;
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
								
					
					/*for ( var j : JoyPad in JoyPads )
					{
						if ( j != this )
							j.LatchedFinger( touch.fingerId );
					}*/
								
					// Tell other JoyPads we've latched this finger
					//for (int j = 0; j < JoyPads.Length; ++j)
					//{
					//	if( JoyPads[i] != this)
					//		LatchedFinger( touch.fingerId );
					//}
				}				
		
				if ( lastFingerId == touch.fingerId )
				{	
					// Override the tap count with what the iPhone SDK reports if it is greater
					// This is a workaround, since the iPhone SDK does not currently track taps
					// for multiple touches
					if ( touch.tapCount > tapCount )
						tapCount = touch.tapCount;
						
	
					// Change the location of the JoyPad graphic to match where the touch is
					int x = Mathf.Clamp( (int)guiTouchPos.x, (int)guiBoundary.min.x, (int)guiBoundary.max.x );
					int y = Mathf.Clamp( (int)guiTouchPos.y, (int)guiBoundary.min.y, (int)guiBoundary.max.y );		
					
					//Rect pixelInsetRect = new Rect(x,y,64,64);
					Rect pixelInsetRect = new Rect(x,y,size_paddle.x,size_paddle.y);
					gui.pixelInset = pixelInsetRect;
					
					if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
						Reset();					
				}			
			}
		}
		
		// Get a value between -1 and 1
		position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
		position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		
		// Adjust for dead zone	
		var absoluteX = Mathf.Abs( position.x );
		var absoluteY = Mathf.Abs( position.y );
		
		if ( absoluteX < deadZone.x )
		{
			// Report the JoyPad as being at the center if it is within the dead zone
			position.x = 0;
		}
		else if ( normalize )
		{
			// Rescale the output after taking the dead zone into account
			position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
		}
			
		if ( absoluteY < deadZone.y )
		{
			// Report the JoyPad as being at the center if it is within the dead zone
			position.y = 0;
		}
		else if ( normalize )
		{
			// Rescale the output after taking the dead zone into account
			position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
		}
		
	}
}

#if (_123_)

@script RequireComponent( GUITexture )

// A simple class for bounding how far the GUITexture will move
class Boundary 
{
	var min : Vector2 = Vector2.zero;
	var max : Vector2 = Vector2.zero;
}

static private var JoyPads : JoyPad[];					// A static collection of all JoyPads
static private var enumeratedJoyPads : boolean = false;
static private var tapTimeDelta : float = 0.3;				// Time allowed between taps

public var position : Vector2; 								// [-1, 1] in x,y
public var deadZone : Vector2 = Vector2.zero;				// Control when position is output
public var normalize : boolean = false; 					// Normalize output after the dead-zone?
public var tapCount : int;									// Current tap count

private var lastFingerId = -1;								// Finger last used for this JoyPad
private var tapTimeWindow : float;							// How much time there is left for a tap to occur
private var gui : GUITexture;								// JoyPad graphic
private var defaultRect : Rect;								// Default position / extents of the JoyPad graphic
private var guiBoundary : Boundary = Boundary();			// Boundary for JoyPad graphic
private var guiTouchOffset : Vector2;						// Offset to apply to touch input
private var guiCenter : Vector2;							// Center of JoyPad graphic

function Start()
{
	// Cache this component at startup instead of looking up every frame	
	gui = GetComponent( GUITexture );
	
	// Store the default rect for the gui, so we can snap back to it
	defaultRect = gui.pixelInset;
	
	// This is an offset for touch input to match with the top left
	// corner of the GUI
	guiTouchOffset.x = defaultRect.width * 0.5;
	guiTouchOffset.y = defaultRect.height * 0.5;
	
	// Cache the center of the GUI, since it doesn't change
	guiCenter.x = defaultRect.x + guiTouchOffset.x;
	guiCenter.y = defaultRect.y + guiTouchOffset.y;
	
	// Let's build the GUI boundary, so we can clamp JoyPad movement
	guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
	guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
	guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
	guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
}

function Disable()
{
	gameObject.active = false;
	enumeratedJoyPads = false;
}

function Reset()
{
	// Release the finger control and set the JoyPad back to the default position
	gui.pixelInset = defaultRect;
	lastFingerId = -1;
}
	
function LatchedFinger( fingerId : int )
{
	// If another JoyPad has latched this finger, then we must release it
	if ( lastFingerId == fingerId )
		Reset();
}

function Update()
{	
	if ( !enumeratedJoyPads )
	{
		// Collect all JoyPads in the game, so we can relay finger latching messages
		JoyPads = FindObjectsOfType( JoyPad );
		enumeratedJoyPads = true;
	}	
		
	var count = iPhoneInput.touchCount;
	
	// Adjust the tap time window while it still available
	if ( tapTimeWindow > 0 )
		tapTimeWindow -= Time.deltaTime;
	else
		tapCount = 0;
	
	if ( count == 0 )
		Reset();
	else
	{
		for(var i : int = 0;i < count; i++)
		{
			var touch : iPhoneTouch = iPhoneInput.GetTouch(i);			
			var guiTouchPos : Vector2 = touch.position - guiTouchOffset;
	
			// Latch the finger if this is a new touch
			if ( gui.HitTest( touch.position ) && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
			{
				lastFingerId = touch.fingerId;
				
				// Accumulate taps if it is within the time window
				if ( tapTimeWindow > 0 )
					tapCount++;
				else
				{
					tapCount = 1;
					tapTimeWindow = tapTimeDelta;
				}
											
				// Tell other JoyPads we've latched this finger
				for ( var j : JoyPad in JoyPads )
				{
					if ( j != this )
						j.LatchedFinger( touch.fingerId );
				}						
			}				
	
			if ( lastFingerId == touch.fingerId )
			{	
				// Override the tap count with what the iPhone SDK reports if it is greater
				// This is a workaround, since the iPhone SDK does not currently track taps
				// for multiple touches
				if ( touch.tapCount > tapCount )
					tapCount = touch.tapCount;
					
				// Change the location of the JoyPad graphic to match where the touch is
				gui.pixelInset.x = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
				gui.pixelInset.y = Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
				
				if ( touch.phase == iPhoneTouchPhase.Ended || touch.phase == iPhoneTouchPhase.Canceled )
					Reset();					
			}			
		}
	}
	
	// Get a value between -1 and 1
	position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
	position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
	
	// Adjust for dead zone	
	var absoluteX = Mathf.Abs( position.x );
	var absoluteY = Mathf.Abs( position.y );
	
	if ( absoluteX < deadZone.x )
	{
		// Report the JoyPad as being at the center if it is within the dead zone
		position.x = 0;
	}
	else if ( normalize )
	{
		// Rescale the output after taking the dead zone into account
		position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
	}
		
	if ( absoluteY < deadZone.y )
	{
		// Report the JoyPad as being at the center if it is within the dead zone
		position.y = 0;
	}
	else if ( normalize )
	{
		// Rescale the output after taking the dead zone into account
		position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
	}
}

#endif