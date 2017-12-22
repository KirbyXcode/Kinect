using UnityEngine;

#region Global delegate 委托
//public delegate void StateChangedEvent (object sender,EnumObjectState newState,EnumObjectState oldState);
	
//public delegate void MessageEvent(Message message);

public delegate void OnTouchEventHandle(GameObject _listener, object _args, object _param);

//public delegate void PropertyChangedHandle(BaseActor actor, int id, object oldValue, object newValue);
#endregion

public class Defines
{
    public const string Tag_Canvas = "Tag_Canvas";
    public const string Tag_Garbage = "Tag_Garbage";
    public const string Tag_GarbageBin = "Tag_GarbageBin";

    public const string Cursor = "Cursor";
    public const string LoadMask = "LoadMask";
    public const string Good = "Good";
    public const string Bad = "Bad";

    public const string Pool_Recyclable = "Pool_Recyclable";
    public const string Pool_Unrecyclable = "Pool_Unrecyclable";
    public const string Pool_Kitchen = "Pool_Kitchen";
    public const string Pool_Other = "Pool_Other";
    public const string Pool_PhizGood = "Pool_Good";
    public const string Pool_PhizBad = "Pool_Bad";

    public const string Sprite_Recyclable = "Recyclable";
    public const string Sprite_Unrecyclable = "Unrecyclable";
    public const string Sprite_Kitchen = "Kitchen";
    public const string Sprite_Other = "Other";

    public const string Sprite_Cursor = "Cursor";

    public const string Audio_Grab = "Grab";
    public const string Audio_Right = "Right";
    public const string Audio_Wrong = "Wrong";
    public const string Audio_CountDown = "CountDown";
}

public enum EnumTouchEventType
{
	OnClick,
	OnDoubleClick,
	OnDown,
	OnUp,
	OnEnter,
	OnExit,
	OnSelect,  
	OnUpdateSelect,  
	OnDeSelect, 
	OnDrag, 
	OnDragEnd,
	OnDrop,
	OnScroll, 
	OnMove,
}

public enum ButtonType
{
    Start,
    Close,
    Over,
    Restart,
    Back,
    Spawn
}

public enum DataType
{
    Score,
    Time
}

public enum GarbageType
{
    Recyclable,
    Unrecyclable,
    Kitchen,
    Other
}

public enum EnumSceneType
{
	None = 0,
	StartGame,
	LoadingScene,
	LoginScene,
	MainScene,
	CopyScene,
	PVPScene,
	PVEScene,
}	


