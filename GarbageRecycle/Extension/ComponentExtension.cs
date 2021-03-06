using UnityEngine;

static public class ComponentExtension
{
	/// <summary>
	/// Gets or add the component.
	/// </summary>
	/// <returns>The or add component.</returns>
	/// <param name="go">Go.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	static public T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T ret = go.GetComponent<T>();
		if (null == ret)
			ret = go.AddComponent<T>();
		return ret;
	}
}

