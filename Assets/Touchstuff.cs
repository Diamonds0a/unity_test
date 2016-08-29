using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

public class TouchCreator
{
	static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
	static Dictionary<string, FieldInfo> fields;

	object touch;

	public float deltaTime { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
	public int tapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
	public TouchPhase phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
	public Vector2 deltaPosition { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
	public int fingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
	public Vector2 position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
	public Vector2 rawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

	public Touch Create()
	{
		return (Touch)touch;
	}

	public TouchCreator()
	{
		touch = new Touch();
	}

	static TouchCreator()
	{
		fields = new Dictionary<string, FieldInfo>();
		foreach(var f in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
		{
			fields.Add(f.Name, f);
			Debug.Log("name: " + f.Name);
		}
	}
}

public class InputHelper : MonoBehaviour {

	private static TouchCreator lastFakeTouch;

	public static List<Touch> GetTouches()
	{
		List<Touch> touches = new List<Touch>();
		touches.AddRange(Input.touches);
		#if UNITY_EDITOR
		object mouseTouch = _GetMouseTouches();
		if (mouseTouch != null) {
			touches.Add ((Touch) mouseTouch);
		}
		#endif

		return touches;      
	}

	public static Touch GetTouch(int i) {
		List<Touch> touches = GetTouches();
		return touches[i];
	}

	public static int touchCount {
		get {
			int i = Input.touchCount;
			#if UNITY_EDITOR
			if (Input.GetMouseButtonDown (0)) {
				i += 1;
			}
			#endif
			return i;
		}
	}

	#if UNITY_EDITOR
	private static object _GetMouseTouches() {
		if (lastFakeTouch == null) {
			lastFakeTouch = new TouchCreator();
		}
		if (Input.GetMouseButtonDown(0)) {
			lastFakeTouch.phase = TouchPhase.Began;
			lastFakeTouch.deltaPosition = new Vector2(0,0);
			lastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.fingerId = 0;
		}
		else if (Input.GetMouseButtonUp(0)) {
			lastFakeTouch.phase = TouchPhase.Ended;
			Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
			lastFakeTouch.position = newPosition;
			lastFakeTouch.fingerId = 0;
		}
		else if (Input.GetMouseButton(0)) {
			lastFakeTouch.phase = TouchPhase.Moved;
			Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
			lastFakeTouch.phase = lastFakeTouch.deltaPosition.magnitude == 0 ? TouchPhase.Stationary : TouchPhase.Moved;
			lastFakeTouch.fingerId = 0;
		}
		else {
			return null;
		}

		return lastFakeTouch.Create();
	}
	#endif

}

public class Touchstuff : MonoBehaviour {

	int i;

	// Use this for initialization
	void Start () {
		int i = 1;
		i = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (InputHelper.touchCount > 0 && InputHelper.GetTouch(0).phase == TouchPhase.Began) {
			i += 1;
			Debug.Log(i);
		}
	}
}
