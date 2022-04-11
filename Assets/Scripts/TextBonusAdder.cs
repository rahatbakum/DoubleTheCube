using UnityEngine;

[DisallowMultipleComponent]
public class TextBonusAdder : MonoBehaviour
{
    [SerializeField] private GameObject _textBonusPrefab;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, 0f);

    public void OnCubeSpawnedAfterMerge(Cube cube)
    {
        GameObject textBonusGameObject = Instantiate(_textBonusPrefab, cube.transform.position + _offset, Quaternion.identity, transform) as GameObject;
        TextBonusAnimation textBonus = textBonusGameObject.GetComponent<TextBonusAnimation>();
        textBonus.InitializeAndPlay(cube.Number);
    }
}
