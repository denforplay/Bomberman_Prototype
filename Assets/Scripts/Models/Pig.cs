using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Models
{
    public class Pig : MonoBehaviour
    {
        [SerializeField] private GameObject _bombsContainer;
        [SerializeField] private Tilemap _gameTilemap;
        [SerializeField] private GameObject _bomb;

        public void SetBomb()
        {
            var cellPosDefault = _gameTilemap.WorldToCell(transform.position);
            var cellPosCenter = _gameTilemap.GetCellCenterWorld(cellPosDefault);
            if (_gameTilemap.GetColliderType(cellPosDefault) == Tile.ColliderType.None)
            {
                Debug.Log("Bomb has been planted");
                var bomb = Instantiate(_bomb, _bombsContainer.transform);
                GameObject.Destroy(bomb, 5f);
                bomb.OnDestroyAsObservable().Subscribe(_ =>
                {
                    _gameTilemap.SetColliderType(cellPosDefault, Tile.ColliderType.None);
                });
                
                bomb.transform.position = cellPosCenter;
                _gameTilemap.SetColliderType(cellPosDefault, Tile.ColliderType.Sprite);
            }
        }
    }
}
