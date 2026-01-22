using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsList", menuName = "Scriptable Objects/PrefabsList")]
public class LevelPrefabs : ScriptableObject, IEnumerable<GameObject>
{
    [SerializeField]
    private List<GameObject> _prefabs;

    public IEnumerator<GameObject> GetEnumerator()
    {
        return _prefabs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
