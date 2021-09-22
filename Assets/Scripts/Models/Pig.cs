using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Models
{
    public class Pig : MonoBehaviour
    {
        [SerializeField] private GameObject _bombsContainer;
        [SerializeField] private Tilemap _gameTilemap;
        [SerializeField] private GameObject _bomb;
        private Bomb.BombFactory _bombFactory;

        [Inject]
        public void Init(Bomb.BombFactory bombFactory)
        {
            _bombFactory = bombFactory;
        }
        
        public void SetBomb()
        {
            var cellPosDefault = _gameTilemap.WorldToCell(transform.position);
            var cellPosCenter = _gameTilemap.GetCellCenterWorld(cellPosDefault);
            if (_gameTilemap.GetColliderType(cellPosDefault) == Tile.ColliderType.None)
            {
                Debug.Log("Bomb has been planted");
                var bomb = _bombFactory.Create();
                bomb.transform.position = transform.position;
                if (bomb.TryGetComponent(out Bomb bombObject))
                {
                    bombObject.SetTileMap(_gameTilemap);
                }
                
                GameObject.Destroy(bomb.gameObject, 1f);
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
